using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Configuration;
using ER_BestPickReports_Dev.App_Code;

namespace ER_BestPickReports_Dev
{
    public class BWConfig
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

        public static string testEnvironmentEmail = "ebscoresearchtesting@gmail.com";

        //Url to get client location information.
        public static string GetClientInfoUrl = "http://www.telize.com/geoip/{0}";
        public static string GetClientZipInfoUrl = "http://ZiptasticAPI.com/{0}";

        public static bool IsProduction()
        {
            return true;
        }

        public static bool IsPreStaging()
        {
            bool isPreStaging = false;

            if (WebConfigurationManager.AppSettings.AllKeys.Contains("IsPreStaging"))
            {
                string value = WebConfigurationManager.AppSettings["IsPreStaging"].ToString();
                Boolean.TryParse(value, out isPreStaging);
            }

            return isPreStaging;
        }
    }


}
