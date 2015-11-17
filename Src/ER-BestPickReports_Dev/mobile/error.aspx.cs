using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ER_BestPickReports_Dev.mobile
{
    public partial class error : BasePage
    {
        public string basedomain = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            //Add meta info
            HtmlMeta meta1;
            HtmlMeta meta2;

            meta1 = new HtmlMeta();
            meta1.Name = "robots";
            meta1.Content = "noindex, nofollow";
            Page.Header.Controls.Add(meta1);
            Page.Header.Controls.Add(new LiteralControl("\n"));

            meta2 = new HtmlMeta();
            meta2.HttpEquiv = "refresh";
            meta2.Content = "5;URL=" + basedomain + "/";
            Page.Header.Controls.Add(meta2);
            Page.Header.Controls.Add(new LiteralControl("\n"));
        }
    }
}