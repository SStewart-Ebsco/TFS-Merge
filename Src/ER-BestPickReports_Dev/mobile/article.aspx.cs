using System;
using System.Data.SqlClient;
using System.Web;

namespace ER_BestPickReports_Dev.mobile
{
    public partial class article : BasePage
    {
        private string articleid = "";
        private string catid = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get value from route handler
            articleid = (HttpContext.Current.Items["articleid"].ToString() == "0") ? "" : HttpContext.Current.Items["articleid"].ToString();
            catid = (HttpContext.Current.Items["categoryid"].ToString() == "0") ? "" : HttpContext.Current.Items["categoryid"].ToString();

            ((ER_BestPickReports_Dev.mobile.SiteMobileMaster)Master).catid = catid;

            string sql = "";

            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                conn.Open();

                //Show category list
                sql = "SELECT * FROM TipArticle WHERE (ArticleID = @AID)";

                SqlDataReader rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                new SqlParameter("@AID", articleid));

                if (rdr.Read())
                {
                    PageHeading.Text = rdr["Title"].ToString();
                    Body.Text = rdr["Body"].ToString();

                    //Get meta data
                    string strMetaTitle = "";
                    //string strMetaKey = "";
                    //string strMetaDesc = "";

                    strMetaTitle = rdr["Title"].ToString();
                    //strMetaKey = rdr["PageName"].ToString();
                    //strMetaDesc = rdr["PageName"].ToString();

                    //Set the page title and meta tags
                    Page.Title = strMetaTitle;

                    //HtmlMeta meta1;
                    //HtmlMeta meta2;
                    //meta1 = new HtmlMeta();
                    //meta1.Name = "Description";
                    //meta1.Content = strMetaDesc;
                    //Page.Header.Controls.Add(meta1);
                    //Page.Header.Controls.Add(new LiteralControl("\n"));

                    //meta2 = new HtmlMeta();
                    //meta2.Name = "keywords";
                    //meta2.Content = strMetaKey;
                    //Page.Header.Controls.Add(meta2);
                    //Page.Header.Controls.Add(new LiteralControl("\n"));
                }
                rdr.Close();

                conn.Close();
            }
        }
    }
}