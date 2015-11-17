using System;
using System.Web;
using System.Web.UI.WebControls;
using ER_BestPickReports_Dev.Helpers;

namespace ER_BestPickReports_Dev.blogfiles
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string basedomain = string.Empty;
            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"] != "localhost")
            {
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            }

            bool isUserInMarket = LocationHelper.CheckInMarketPosition(Request.Cookies.Get("bprpreferences"));
            TopAdds.Visible = !isUserInMarket;
            SideBarAdds.Visible = !isUserInMarket;
            BottomAdds.Visible = !isUserInMarket;
            if (isUserInMarket)
            {
                Mainbar.Width = new Unit(100, UnitType.Percentage);
            }

            LogoLinkBlog.NavigateUrl = "/blog?redirect=false";
            LogoLinkGlobal.NavigateUrl = basedomain + "/";
        }
    }
}