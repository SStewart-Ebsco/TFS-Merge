using System;
using System.Data;
using System.Data.SqlClient;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev.App_Code
{
    public class PageContentDL
    {
        public static PageContentModel GetByPageId(string pageId)
        {
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr = null;

            string queryString = @"SELECT * FROM PageContent WHERE (PageID = @PAGEID)";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("PAGEID", SqlDbType.Int).Value = pageId;

            PageContentModel content = null;

            try
            {
                conn.Open();

                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        int pageIdr = Convert.ToInt32(rdr["PageID"].ToString());
                        string pageName = rdr["PageName"].ToString();
                        string urlTitle = rdr["UrlTitle"].ToString();
                        string pageContent = rdr["PageContent"].ToString();
                        string metaTitle = rdr["MetaTitle"].ToString();
                        string metaKeywords = rdr["MetaKeywords"].ToString();
                        string metaDesc = rdr["MetaDesc"].ToString();
                        bool isMobileApp = (bool) rdr["IsMobileApp"];
                        string pageContentMobile = rdr["PageContentMobile"].ToString();

                        content = new PageContentModel(pageIdr, pageName, urlTitle, pageContent, metaTitle, metaKeywords,
                                                       metaDesc, isMobileApp, pageContentMobile);
                    }
                }
            }
            finally
            {
                if (rdr != null)
                {
                    if (!rdr.IsClosed)
                        rdr.Close();
                    rdr.Dispose();
                }
                rdr = null;

                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }
            return content;
        }

        public static int GetPageIdByUrl(string urlTitle)
        {
            int pageId = 0;
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr = null;

            string queryString = @"SELECT PageID FROM PageContent WHERE (UrlTitle = @urlTitle)";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("urlTitle", SqlDbType.VarChar).Value = urlTitle;

            try
            {
                conn.Open();
                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        pageId = Convert.ToInt32(rdr["PageID"]);
                    }
                }
            }
            finally
            {
                if (rdr != null)
                {
                    if (!rdr.IsClosed)
                        rdr.Close();
                    rdr.Dispose();
                }

                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    conn.Dispose();
                }
            }
            return pageId;
        }
    }
}