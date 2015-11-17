using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using ER_BestPickReports_Dev.App_Code;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev.mobile
{
    public partial class pagecontent : System.Web.UI.Page
    {
        string pageid = "";
        string basedomain = "";

        private AppCookies bprPreferences = AppCookies.CreateInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get value from route handler
            pageid = (HttpContext.Current.Items["pageid"].ToString() == "0") ? "" : HttpContext.Current.Items["pageid"].ToString();

            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"] != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            var pageContent = PageContentDL.GetByPageId(pageid);

            //Check for cookie and redirect if this is the contact us page
            if (!String.IsNullOrEmpty(bprPreferences.CityUrlName) && pageContent != null && pageContent.UrlTitle == "contact-us")
                Response.Redirect(basedomain + "/content/contact-us-" + bprPreferences.CityUrlName);

            //Check for cookie and redirect if this is the faq page page for boston or philly
            if (pageContent != null && pageContent.UrlTitle == "frequently-asked-questions" && (bprPreferences.CityUrlName == "boston" || bprPreferences.CityUrlName == "philadelphia"))
                Response.Redirect(basedomain + "/content/frequently-asked-questions-online");

            //Get meta data
            string strMetaTitle = "";
            string strMetaKey = "";
            string strMetaDesc = "";

            if (pageContent != null)
            {
                PageHeading.Text = pageContent.PageName;
                Body.Text = pageContent.PageContentMobile;

                if (pageContent.MetaTitle != "")
                    strMetaTitle = pageContent.MetaTitle;
                else
                    strMetaTitle = pageContent.PageName;

                if (pageContent.MetaKeywords != "")
                    strMetaKey = pageContent.MetaKeywords;
                else
                    strMetaKey = pageContent.PageName;

                if (pageContent.MetaDesc != "")
                    strMetaDesc = pageContent.MetaDesc;
                else
                    strMetaDesc = pageContent.PageName;
            }

            //Add meta info
            HtmlMeta meta1;
            HtmlMeta meta2;

            meta2 = new HtmlMeta();
            meta2.Name = "keywords";
            meta2.Content = strMetaKey;
            Page.Header.Controls.Add(meta2);
            Page.Header.Controls.Add(new LiteralControl("\n"));

            meta1 = new HtmlMeta();
            meta1.Name = "description";
            meta1.Content = strMetaDesc;
            Page.Header.Controls.Add(meta1);
            Page.Header.Controls.Add(new LiteralControl("\n"));

            Page.Title = strMetaTitle;
        }
    }
}