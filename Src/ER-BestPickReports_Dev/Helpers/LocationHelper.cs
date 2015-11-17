using System;
using System.Data.SqlClient;
using System.Web;
using ER_BestPickReports_Dev.App_Code;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev.Helpers
{
    public static class LocationHelper
    {
        /// <summary>
        /// Allow to try change locarion.
        /// </summary>
        /// <param name="zipCode">Zip code of a new location</param>
        /// <returns>Was location changed</returns>
        public static bool TryChangeLocationByZip(string zipCode)
        {
            var dataAccessHelper = new DataAccessHelper();
            var bprPreferences = AppCookies.CreateInstance();

            if (zipCode != "")
            {
                using (var conn = new SqlConnection(dataAccessHelper.ConnString))
                {
                    conn.Open();

                    const string sql = @"SELECT AreaInfo.AreaID, AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaDisplayName,
                            CityInfo.CityID, CityInfo.UrlName AS CityUrlName, CityInfo.DisplayName AS CityDisplayName
                            FROM Zip, CityInfo, AreaInfo
                            WHERE (AreaInfo.AreaID = Zip.AreaID) AND (CityInfo.CityID = AreaInfo.CityID) AND (Zip.ZipCode = @ZIP)";

                    using (SqlDataReader rdr = dataAccessHelper.Data.ExecuteDatareader(conn, sql, new SqlParameter("@ZIP", zipCode)))
                    {
                        if (rdr.Read())
                        {
                            //Set cookie for city/area preference based on url values
                            bprPreferences.CityId = int.Parse(rdr["CityID"].ToString());
                            bprPreferences.CityName = rdr["CityDisplayName"].ToString();
                            bprPreferences.CityUrlName = rdr["CityUrlName"].ToString();
                            bprPreferences.AreaId = int.Parse(rdr["AreaID"].ToString());
                            bprPreferences.AreaName = rdr["AreaDisplayName"].ToString();
                            bprPreferences.AreaUrlName = rdr["AreaUrlName"].ToString();
                            bprPreferences.SetExpiration(DateTime.Now.AddDays(365));

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool CheckInMarketPosition(HttpCookie preferences)
        {
            var bprPreferences = AppCookies.CreateInstance();
            //return preferences != null && !String.IsNullOrEmpty(preferences.Values.Get("areaid"))
            //    && String.IsNullOrEmpty(preferences.Values.Get("outOfMarketZip"));
            return bprPreferences.AreaId > 0
                && String.IsNullOrEmpty(bprPreferences.OutOfMarketZip);
        }

        public static string GetZip(HttpRequest Request, HttpResponse Response, System.Web.SessionState.HttpSessionState Session)
        {
            var bprPreferences = AppCookies.CreateInstance();

            string zip = String.Empty;
            if (IsGeocodingRequired())
            {
                zip = GetZipByGeocoding(Request, Response, Session);
            }
            else
            {
                if (bprPreferences.AreaId > 0)
                {
                    zip = ZipDL.GetZipByAreaId(bprPreferences.AreaId.ToString());
                }
                else
                {
                    if (bprPreferences.CityId > 0)
                    {
                        int areaId = AreaDL.GetAreaIdByCityId(bprPreferences.CityId.ToString());
                        zip = ZipDL.GetZipByAreaId(areaId.ToString());
                    }
                }
            }
            return zip;
        }

        private static string GetZipByGeocoding(HttpRequest Request, HttpResponse Response, System.Web.SessionState.HttpSessionState Session)
        {
            var bprPreferences = AppCookies.CreateInstance();

            string state = String.Empty;
            string country = String.Empty;
            string city = String.Empty;
            string zip = String.Empty;

            if (IsOutOfMarketMode())
            {
                GeocodingResponse location =
                    HttpRequestHelper.GetJson<GeocodingResponse>(String.Format(BWConfig.GetClientZipInfoUrl, bprPreferences.OutOfMarketZip));
                zip = bprPreferences.OutOfMarketZip;
                state = location.State;
                country = location.Country;
                city = location.City;
                bprPreferences.OutOfMarketZip = zip;
            }
            else
            {
                string clientIp = HttpRequestHelper.GetClientIp(Request);

                //debug action for localhost
                if (clientIp.Equals("::1") || clientIp.StartsWith("192.168.") || clientIp.Equals("localhost") || clientIp.Equals("127.0.0.1"))
                {
                    clientIp = String.Empty;
                }

                //Required to check Status Code according to API: When incorrect user input is entered,
                // the server returns an HTTP 400 Error (Bad Request), along with a JSON-encoded error message.
                try
                {
                    GeocodingResponse location =
                        HttpRequestHelper.GetJson<GeocodingResponse>(String.Format(BWConfig.GetClientInfoUrl, clientIp));
                    zip = location.PostalCode;
                    state = location.Region;
                    country = location.Country;
                }
                catch (Exception e)
                {

                }

                if (!String.IsNullOrEmpty(zip))
                {
                    CookiesInfo cookies = InfoDL.GetCookiesInfoByZipCode(zip);

                    if (cookies.AreaId != null && cookies.CityId != null)
                    {
                        bprPreferences.CityId = int.Parse(cookies.CityId);
                        bprPreferences.CityName = cookies.CityDisplayName;
                        bprPreferences.CityUrlName = cookies.CityUrl;
                        bprPreferences.AreaId = int.Parse(cookies.AreaId);
                        bprPreferences.AreaName = cookies.AreaDisplayName;
                        bprPreferences.AreaUrlName = cookies.AreaUrl;
                        bprPreferences.SetExpiration(DateTime.Now.AddDays(365));
                        bprPreferences.Remove(AppCookies.OUT_OF_MARKET_ZIP);

                        Response.Redirect(Request.Url.AbsoluteUri);
                    }
                }
            }

            bprPreferences.SetExpiration(DateTime.Now.AddDays(365));
            bprPreferences.Remove(AppCookies.CITY_ID);
            bprPreferences.Remove(AppCookies.CITY_NAME);
            bprPreferences.Remove(AppCookies.CITY_URL_NAME);
            bprPreferences.Remove(AppCookies.AREA_ID);
            bprPreferences.Remove(AppCookies.AREA_NAME);
            bprPreferences.Remove(AppCookies.AREA_URL_NAME);

            string areaname = !String.IsNullOrEmpty(city)
                                  ? city.ToUppercaseFirst()
                                  : !String.IsNullOrEmpty(state)
                                        ? state
                                        : !String.IsNullOrEmpty(country) ? country : null;

            if (!String.IsNullOrEmpty(areaname))
            {
                Session.Add("areaname", areaname);
            }
            else
            {
                Response.Cookies.Remove("bprpreferences");
                Session.Remove("areaname");
            }

            return zip;
        }

        private static string ToUppercaseFirst(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            return char.ToUpper(text[0]) + text.Substring(1).ToLower();
        }

        private static bool IsGeocodingRequired()
        {
            var bprPreferences = AppCookies.CreateInstance();

            return !bprPreferences.Exists() ||
                bprPreferences.AreaId == 0 ||
                bprPreferences.CityId == 0 ||
                IsOutOfMarketMode();
        }

        private static bool IsOutOfMarketMode()
        {
            var bprPreferences = AppCookies.CreateInstance();

            return bprPreferences.Exists() &&
                !String.IsNullOrEmpty(bprPreferences.OutOfMarketZip);
        }
    }
}