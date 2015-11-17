using System.Web;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev.Helpers
{
    public class SiteMasterHelper
    {
        private readonly AppCookies _bprPreferences;
        private readonly DataAccessHelper _dataAccessHelper;

        public SiteMasterHelper(AppCookies bprPreferences, DataAccessHelper dataAccessHelper)
        {
            _bprPreferences = bprPreferences;
            _dataAccessHelper = dataAccessHelper;
        }

        public string GetBaseDomain()
        {
            string baseDomain = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
            {
                baseDomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            }
            return baseDomain;
        }

        /// <summary>
        /// Try change location by changing zip code
        /// </summary>
        /// <param name="zipCode">Zip code</param>
        /// <param name="basedomain">Base domain url</param>
        /// <param name="catid">Current category id</param>
        /// <param name="contractorid">Current contractor id</param>
        /// <returns>Page url redirect to</returns>
        public string TryChangeZipCode(string zipCode, string basedomain, string catid, string contractorid)
        {
            int currentLocationCityId = _bprPreferences.CityId;
            bool wasLocationChanged = LocationHelper.TryChangeLocationByZip(zipCode);

            string redirectPage = string.Empty;
            if (wasLocationChanged)
            {
                string cityUrlName = _bprPreferences.CityUrlName;
                string areaUrlName = _bprPreferences.AreaUrlName;

                redirectPage = basedomain + "/" + cityUrlName + "/" + areaUrlName;

                //See if new zip is within the same metro as the current area cookie if this is a category page
                if (catid != null && contractorid != null)
                {
                    if (catid != "0" && contractorid == "0")
                    {
                        if (currentLocationCityId == _bprPreferences.CityId)
                        {
                            redirectPage = basedomain + "/" + _dataAccessHelper.GetCategoryUrlFromID(catid) + "/" + cityUrlName + "/" + areaUrlName;
                        }
                    } 
                }
            }

            return redirectPage;
        }
    }
}