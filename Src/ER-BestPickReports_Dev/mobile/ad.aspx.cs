using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ER_BestPickReports_Dev.mobile
{
    public partial class ad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DownloadButton_Click(object sender, EventArgs e)
        {
            var userAgent = Request.UserAgent.ToLower();
            var ios = userAgent.Contains("iphone") || userAgent.Contains("ipad");
            var android = userAgent.Contains("android");

            if (ios)
            {
                Response.Redirect("http://itunes.apple.com/us/app/best-pick-reports/id494057962?mt=8", true);
            }
            else if (android)
            {
                Response.Redirect("https://play.google.com/store/apps/details?id=com.brotherfish", true);
            }
        }

        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            Response.Cookies.Add(new HttpCookie("noads", "true"));
            Response.Redirect("~/");
        }

    }
}