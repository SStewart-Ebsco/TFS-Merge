using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace ER_BestPickReports_Dev.Helpers
{
    public static class MobileBlogHelper
    {
        public static bool isMobileVersionRequested(RequestContext context)
        {
            var urlParam = (context.HttpContext).Request["mobile"];
            if (!String.IsNullOrEmpty(urlParam) && urlParam == "true")
            {
                (context.HttpContext).Response.Cookies.Add(new HttpCookie("nomobile", "false") { Expires = DateTime.Now.AddDays(1), Path = "/" });
                return true;
            }

            var noMobileCookie = (context.HttpContext).Request.Cookies["nomobile"];
            if (noMobileCookie != null)
            {
                if (noMobileCookie.Value == "true")
                    return false;
                if (noMobileCookie.Value == "false")
                    return true;
            }

            var isMobileDevice = context.HttpContext.Request.Browser.IsMobileDevice && !IsTablet(context.HttpContext.Request);
            if (isMobileDevice)
            {
                return true;
            }

            return false;
        }

        public static bool isAdPageRequired(RequestContext context)
        {
            // temporarily disable splash screen
            return false;

            if ((context.HttpContext).Request.Cookies.AllKeys.Contains("noads"))
            {
                var noAdsCookie = (context.HttpContext).Request.Cookies.Get("noads");
                if (noAdsCookie != null && noAdsCookie.Value == "true")
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsTablet(HttpRequestBase request)
        {
            Regex r = new Regex("Tablet|iPad|PlayBook|BB10|Z30|Nexus 10|Nexus 7|GT-P|SCH-I800|Xoom|Kindle|Silk|KFAPWI", RegexOptions.IgnoreCase);
            bool isTablet = r.IsMatch(request.UserAgent) && request.Browser.IsMobileDevice;
            return isTablet;
        }
    }
}