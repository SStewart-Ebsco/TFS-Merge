using System;
using System.Linq;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

namespace ER_BestPickReports_Dev
{
    public partial class mobileredirect : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Add meta info
            HtmlMeta meta1;

            meta1 = new HtmlMeta();
            meta1.Name = "robots";
            meta1.Content = "noindex, nofollow";
            Page.Header.Controls.Add(meta1);
            Page.Header.Controls.Add(new LiteralControl("\n"));

            //Show test label if organic link referer
            //Set session variable based on referer
            string useragentstring = "";
            if (Request.ServerVariables["HTTP_USER_AGENT"] != null)
                useragentstring = Request.ServerVariables["HTTP_USER_AGENT"].ToString().ToLower();

            Regex regEx_apple = new Regex("ipod|iphone|ipad");
            Regex regEx_android = new Regex("android");
            if (regEx_apple.Match(useragentstring).Success)
            {
                Response.Redirect("http://itunes.apple.com/us/app/best-pick-reports/id494057962?mt=8");
            }

            if (regEx_android.Match(useragentstring).Success)
            {
                Response.Redirect("https://play.google.com/store/apps/details?id=com.brotherfish");
            }
        }
    }
}