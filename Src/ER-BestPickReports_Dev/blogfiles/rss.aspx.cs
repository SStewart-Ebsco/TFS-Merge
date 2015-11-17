using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using ER_BestPickReports_Dev.Helpers;

namespace ER_BestPickReports_Dev.blogfiles
{
    public partial class rss : BasePage
    {
        string cityid = "";
        public string strcity = "";
        public string basedomain = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get value from route handler
            cityid = (HttpContext.Current.Items["cityid"] == null) ? "" : HttpContext.Current.Items["cityid"].ToString();
            strcity = (HttpContext.Current.Items["city"] == null) ? "" : HttpContext.Current.Items["city"].ToString();

            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            string cname = "";

            string sql = "SELECT DisplayName FROM CityInfo WHERE (CityID = @CITYID)";
            object o = DataAccessHelper.Data.ExecuteScalar(sql,
                new SqlParameter("@CITYID", cityid));
            if (o != null)
            {
                cname = o.ToString();
            }

            RSSTitle.Text = "Best Pick Reports " + cname;

            RSSDesc.Text = "Providing homeowners like you valuable instruction and insider information about how to best care for your home - advice straight from the professionals.";
            RSSLink.Text = basedomain + "/blog/rss/" + strcity;

            sql = "SELECT BlogPosts.PostID, BlogPosts.Title, BlogPosts.UrlTitle, BlogPosts.Summary, BlogPosts.PublishDate, BlogPosts.ImagePath, BlogPosts.ImageSummary FROM BlogPosts INNER JOIN BlogPostsToCities ON BlogPosts.PostID = BlogPostsToCities.PostID" +
                " WHERE (BlogPosts.PublishDate <= @DATE) AND (BlogPostsToCities.CityID = @CITYID)";

            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                conn.Open();

                // Bind to the RSS list
                SqlDataReader rssrdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@CITYID", cityid),
                    new SqlParameter("@DATE", DateTime.Now));
                rptRSS.DataSource = rssrdr;
                rptRSS.DataBind();
                rssrdr.Close();

                conn.Close();
            }
        }

        protected string FormatForXML(object input)
        {
            string data = input.ToString();
            //Replace characters not allowed in XML
            data = data.Replace("&", "&amp;");
            data = data.Replace("'", "&quot;");
            data = data.Replace("'", "&apos;");
            data = data.Replace("<", "&lt;");
            data = data.Replace(">", "&gt;");
            data = data.Replace("&mdash;", "-");

            return data;
        }

        protected void rptRSS_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            SqlDataReader rdr = (SqlDataReader)rptRSS.DataSource;

            Literal itemlink = (Literal)e.Item.FindControl("ItemLink");
            Literal glink = (Literal)e.Item.FindControl("GLink");
            Literal itemteaser = (Literal)e.Item.FindControl("ItemTeaser");

            if (rdr["Summary"].ToString().Trim() != "")
                itemteaser.Text = FormatForXML(rdr["Summary"].ToString());
            else
                itemteaser.Text = "";

            itemlink.Text = basedomain + "/blog/post/" + rdr["UrlTitle"].ToString();
            glink.Text = basedomain + "/blog/post/" + rdr["UrlTitle"].ToString();
        }
    }
}