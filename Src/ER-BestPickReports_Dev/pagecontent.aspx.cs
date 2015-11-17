using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev
{
    public partial class pagecontent : BasePage
    {
        string pageid = "";
        string basedomain = "";

        private AppCookies bprPreferences = AppCookies.CreateInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get value from route handler
            pageid = (HttpContext.Current.Items["pageid"].ToString() == "0") ? "" : HttpContext.Current.Items["pageid"].ToString();

            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            string sql = "";

            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                conn.Open();

                //Show page content
                sql = "SELECT * FROM PageContent WHERE (PageID = @PAGEID)";

                SqlDataReader rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                new SqlParameter("@PAGEID", pageid));

                if (rdr.Read())
                {
                    //Check for cookie and redirect if this is the contact us page
                    if (rdr["UrlTitle"].ToString() == "contact-us" && !String.IsNullOrEmpty(bprPreferences.CityUrlName))
                        Response.Redirect(basedomain + "/content/contact-us-" + bprPreferences.CityUrlName);

                    //Check for cookie and redirect if this is the faq page page for boston or philly
                    if (rdr["UrlTitle"].ToString() == "frequently-asked-questions" && (bprPreferences.CityUrlName == "boston" || bprPreferences.CityUrlName == "philadelphia"))
                        Response.Redirect(basedomain + "/content/frequently-asked-questions-online");

                    PageHeading.Text = rdr["PageName"].ToString();
                    Body.Text = rdr["PageContent"].ToString();

                    //Get meta data
                    string strMetaTitle = "";
                    string strMetaKey = "";
                    string strMetaDesc = "";

                    if (rdr["MetaTitle"].ToString() != "")
                        strMetaTitle = rdr["MetaTitle"].ToString();
                    else
                        strMetaTitle = rdr["PageName"].ToString();

                    if (rdr["MetaKeywords"].ToString() != "")
                        strMetaKey = rdr["MetaKeywords"].ToString();
                    else
                        strMetaKey = rdr["PageName"].ToString();

                    if (rdr["MetaDesc"].ToString() != "")
                        strMetaDesc = rdr["MetaDesc"].ToString();
                    else
                        strMetaDesc = rdr["PageName"].ToString();

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
                rdr.Close();

                conn.Close();
            }
        }
    }
}