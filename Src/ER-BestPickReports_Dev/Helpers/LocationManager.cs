using System;
using System.Data.SqlClient;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev.Helpers
{
    public class LocationManager
    {
        private readonly DataAccessHelper _dataAccessHelper = new DataAccessHelper();
        private readonly AppCookies _bprPreferences = AppCookies.CreateInstance();

        /// <summary>
        /// Allow to try change locarion.
        /// </summary>
        /// <param name="zipCode">Zip code of a new location</param>
        /// <returns>Was location changed</returns>
        public bool TryChangeLocationByZip(string zipCode)
        {
            if (zipCode != "")
            {
                using (var conn = new SqlConnection(_dataAccessHelper.ConnString))
                {
                    conn.Open();

                    const string sql = @"SELECT AreaInfo.AreaID, AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaDisplayName,
                            CityInfo.CityID, CityInfo.UrlName AS CityUrlName, CityInfo.DisplayName AS CityDisplayName
                            FROM Zip, CityInfo, AreaInfo
                            WHERE (AreaInfo.AreaID = Zip.AreaID) AND (CityInfo.CityID = AreaInfo.CityID) AND (Zip.ZipCode = @ZIP)";

                    using (SqlDataReader rdr = _dataAccessHelper.Data.ExecuteDatareader(conn, sql, new SqlParameter("@ZIP", zipCode)))
                    {
                        if (rdr.Read())
                        {
                            //Set cookie for city/area preference based on url values
                            _bprPreferences.CityId = int.Parse(rdr["CityID"].ToString());
                            _bprPreferences.CityName = rdr["CityDisplayName"].ToString();
                            _bprPreferences.CityUrlName = rdr["CityUrlName"].ToString();
                            _bprPreferences.AreaId = int.Parse(rdr["AreaID"].ToString());
                            _bprPreferences.AreaName = rdr["AreaDisplayName"].ToString();
                            _bprPreferences.AreaUrlName = rdr["AreaUrlName"].ToString();
                            _bprPreferences.SetExpiration(DateTime.Now.AddDays(365));

                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}