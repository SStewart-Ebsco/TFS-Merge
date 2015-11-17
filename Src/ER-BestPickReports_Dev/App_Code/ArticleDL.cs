using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace ER_BestPickReports_Dev.App_Code
{
    public class ArticleDL
    {
        private string _title;
        private string _urlTitle;
        private string _urlName;

        public string Title
        {
            get { return _title; }
            private set { _title = value; }
        }

        public string UrlTitle
        {
            get { return _urlTitle; }
            private set { _urlTitle = value; }
        }

        public string UrlName
        {
            get { return _urlName; }
            private set { _urlName = value; }
        }

        public ArticleDL(string title, string urlTitle, string urlName)
        {
            _title = title;
            _urlTitle = urlTitle;
            _urlName = urlName;
        }


        public static List<ArticleDL> GetByAreaIDCategoryID(int areaID, int categoryID)
        {
            List<ArticleDL> articleList = new List<ArticleDL>();

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr = null;

            string queryString = "SELECT TipArticle.Title, TipArticle.UrlTitle, CategoryInfo.UrlName AS CatName FROM TipArticle INNER JOIN TipArticleArea ON " +
                                "TipArticle.ArticleID = TipArticleArea.ArticleID INNER JOIN CategoryInfo ON TipArticle.CategoryID = CategoryInfo.CategoryID " +
                                "WHERE (TipArticle.CategoryID = @CATID) AND (TipArticleArea.AreaID = @AREAID) ORDER BY TipArticle.PublishDate DESC";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("CATID", SqlDbType.Int).Value = categoryID;
            command.Parameters.Add("AREAID", SqlDbType.Int).Value = areaID;

            try
            {
                conn.Open();

                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        string title = rdr["Title"].ToString();
                        string urlTitle = rdr["UrlTitle"].ToString();
                        string urlName = rdr["CatName"].ToString();
                        ArticleDL a = new ArticleDL(title, urlTitle, urlName);
                        articleList.Add(a);
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

            return articleList;
        }

    }
}