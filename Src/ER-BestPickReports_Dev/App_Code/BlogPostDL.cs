using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev.App_Code
{
    public class BlogPostDL
    {
        #region Private fields

        private readonly string _postId;
        private readonly string _title;
        private readonly string _urlTitle;
        private readonly string _createDate;
        private readonly string _publishDate;
        private readonly string _summary;
        private readonly string _body;
        private readonly string _metaKeywords;
        private readonly string _metaDesc;
        private readonly string _atlantaLinks;
        private readonly string _chicagoLinks;
        private readonly string _dallasLinks;
        private readonly string _novaLinks;
        private readonly string _houstonLinks;
        private readonly string _marylandLinks;
        private readonly string _dcLinks;
        private readonly string _bhamLinks;
        private readonly string _bostonLinks;
        private readonly string _philiLinks;
        private readonly string _imageSummary;
        private readonly string _imageBody;
        private readonly string _imagePath;
        private readonly string _isTopBlog;
        private readonly string _authorNames;
        private readonly string _authorTitles;
        private readonly string _authorDescriptions;

        public BlogPostDL(string postId, string title, string urlTitle, string createDate, string publishDate,
                          string summary, string body, string metaKeywords, string metaDesc, string atlantaLinks,
                          string chicagoLinks, string dallasLinks, string novaLinks, string houstonLinks,
                          string marylandLinks, string dcLinks, string bhamLinks, string bostonLinks, string philiLinks,
                          string imageSummary, string imageBody, string imagePath, string isTopBlog, string authorNames,
                          string authorTitles, string authorDescription)
        {
            _postId = postId;
            _title = title;
            _urlTitle = urlTitle;
            _createDate = createDate;
            _publishDate = publishDate;
            _summary = summary;
            _body = body;
            _metaKeywords = metaKeywords;
            _metaDesc = metaDesc;
            _atlantaLinks = atlantaLinks;
            _chicagoLinks = chicagoLinks;
            _dallasLinks = dallasLinks;
            _novaLinks = novaLinks;
            _houstonLinks = houstonLinks;
            _marylandLinks = marylandLinks;
            _dcLinks = dcLinks;
            _bhamLinks = bhamLinks;
            _bostonLinks = bostonLinks;
            _philiLinks = philiLinks;
            _imageSummary = imageSummary;
            _imageBody = imageBody;
            _imagePath = imagePath;
            _isTopBlog = isTopBlog;
            _authorNames = authorNames;
            _authorTitles = authorTitles;
            _authorDescriptions = authorDescription;
        }

        #endregion

        #region Public fields

        public string PostId
        {
            get { return _postId; }
        }

        public string Title
        {
            get { return _title; }
        }

        public string UrlTitle
        {
            get { return _urlTitle; }
        }

        public string CreateDate
        {
            get { return _createDate; }
        }

        public string PublishDate
        {
            get { return _publishDate; }
        }

        public string Summary
        {
            get { return _summary; }
        }

        public string Body
        {
            get { return _body; }
        }

        public string MetaKeywords
        {
            get { return _metaKeywords; }
        }

        public string MetaDesc
        {
            get { return _metaDesc; }
        }

        public string AtlantaLinks
        {
            get { return _atlantaLinks; }
        }

        public string ChicagoLinks
        {
            get { return _chicagoLinks; }
        }

        public string DallasLinks
        {
            get { return _dallasLinks; }
        }

        public string NovaLinks
        {
            get { return _novaLinks; }
        }

        public string HoustonLinks
        {
            get { return _houstonLinks; }
        }

        public string MarylandLinks
        {
            get { return _marylandLinks; }
        }

        public string DCLinks
        {
            get { return _dcLinks; }
        }

        public string BhamLinks
        {
            get { return _bhamLinks; }
        }

        public string BostonLinks
        {
            get { return _bostonLinks; }
        }

        public string PhiliLinks
        {
            get { return _philiLinks; }
        }

        public string ImageSummary
        {
            get { return _imageSummary; }
        }

        public string ImageBody
        {
            get { return _imageBody; }
        }

        public string ImagePath
        {
            get { return _imagePath; }
        }

        public string IsTopBlog
        {
            get { return _isTopBlog; }
        }

        public string AuthorNames
        {
            get { return _authorNames; }
        }

        public string AuthorTitles
        {
            get { return _authorTitles; }
        }

        public string AuthorDescriptions
        {
            get { return _authorDescriptions; }
        }

        #endregion

        private static BlogPostDL ConvertToBlogPost(SqlDataReader rdr)
        {
            string postId = rdr["postId"].ToString();
            string title = rdr["title"].ToString();
            string urlTitle = rdr["urlTitle"].ToString();
            string createDate = rdr["createDate"].ToString();
            string publishDate = rdr["publishDate"].ToString();
            string summary = rdr["summary"].ToString();
            string body = rdr["body"].ToString();
            string metaKeywords = rdr["metaKeywords"].ToString();
            string metaDesc = rdr["metaDesc"].ToString();
            string atlantaLinks = rdr["atlantaLinks"].ToString();
            string chicagoLinks = rdr["chicagoLinks"].ToString();
            string dallasLinks = rdr["dallasLinks"].ToString();
            string novaLinks = rdr["novaLinks"].ToString();
            string houstonLinks = rdr["houstonLinks"].ToString();
            string marylandLinks = rdr["marylandLinks"].ToString();
            string dcLinks = rdr["dcLinks"].ToString();
            string bhamLinks = rdr["bhamLinks"].ToString();
            string bostonLinks = rdr["bostonLinks"].ToString();
            string philiLinks = rdr["philiLinks"].ToString();
            string imageSummary = rdr["imageSummary"].ToString();
            string imageBody = rdr["imageBody"].ToString();
            string imagePath = rdr["imagePath"].ToString();
            string isTopBlog = rdr["isTopBlog"].ToString();
            string authorNames = rdr["authorNames"].ToString();
            string authorTitles = " ";//rdr["authorTitles"].ToString();
            string authorDescriptions = rdr["authorDescriptions"].ToString();

            return new BlogPostDL(postId, title, urlTitle, createDate, publishDate, summary, body,
                                                metaKeywords, metaDesc, atlantaLinks, chicagoLinks, dallasLinks,
                                                novaLinks, houstonLinks, marylandLinks, dcLinks, bhamLinks,
                                                bostonLinks, philiLinks, imageSummary, imageBody, imagePath, isTopBlog,
                                                authorNames, authorTitles, authorDescriptions);
        }

        public static BlogPostDL GetById(int id)
        {
            BlogPostDL post = null;
            var query = "SELECT * FROM BlogPosts WHERE (PostID = @PID)";
            using (SqlConnection conn = new SqlConnection(BWConfig.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@PID", id));
                conn.Open();
                SqlDataReader rdr = command.ExecuteReader();
                if (rdr.Read())
                {
                    post = ConvertToBlogPost(rdr);
                }
                rdr.Close();
            }
            return post;
        }

        public static string GetPostUrlTitleById(string id)
        {
            string title = null;
            var query = "SELECT UrlTitle FROM BlogPosts WHERE (PostID = @PID)";
            using (SqlConnection conn = new SqlConnection(BWConfig.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@PID", id));
                conn.Open();
                SqlDataReader rdr = command.ExecuteReader();
                if (rdr.Read())
                {
                    title = rdr["UrlTitle"].ToString();
                }
                rdr.Close();
            }
            return title;
        }

        public static List<BlogPostDL> GetFilteredBlogPosts(string zipCode, string month = "", string year = "")
        {
            if (month.Equals("0"))
            {
                month = String.Empty;
            }
            if (year.Equals("0"))
            {
                year = String.Empty;
            }
            if (String.IsNullOrEmpty(zipCode))
            {
                zipCode = "0";
            }

            bool needPostsAfterCurrentDate = !string.IsNullOrEmpty(month) && !string.IsNullOrEmpty(year);
            const string querySelectPart = @"SELECT BP.*
                                FROM Zip Z
	                                JOIN Area A ON (Z.AreaID = A.AreaID)
	                                JOIN City C ON (A.CityID = C.CityID)
	                                JOIN BlogPostsToCities BPCity ON (C.CityID = BPCity.CityID)
	                                JOIN BlogPosts BP ON (BPCity.PostID = BP.PostID)
	                                JOIN BlogPostsToCategories BPCat ON (BP.PostID = BPCat.PostID)
	                                JOIN BlogCategories BC ON (BPCat.BlogCatID = BC.BlogCatID)";
            const string queryMainConditionsPart = @" WHERE (Z.ZipCode = @zipCode  OR Coalesce(@zipCode,'0') = '')
                                      AND (((DATEPART(yy, BP.PublishDate) = @year OR Coalesce(@year,'') = '')
                                      AND (DATEPART(mm, BP.PublishDate) = @month OR Coalesce(@month,'') = '')))";
            string queryAdditionalConditionsPart = needPostsAfterCurrentDate
                ? string.Empty
                : @" AND CAST(BP.PublishDate AS DATETIME) <= GETDATE()";
            const string queryOrderByPart = @" ORDER BY BP.PublishDate DESC";

            string query = string.Format("{0}{1}{2}{3}", querySelectPart, queryMainConditionsPart,
                queryAdditionalConditionsPart, queryOrderByPart);

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("zipCode", SqlDbType.Int).Value = zipCode;
            command.Parameters.Add("month", SqlDbType.VarChar).Value = month;
            command.Parameters.Add("year", SqlDbType.VarChar).Value = year;

            List<BlogPostDL> posts = new List<BlogPostDL>();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                rdr = command.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        var post = ConvertToBlogPost(rdr);
                        posts.Add(post);
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
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }
            posts = posts.GroupBy(p => p.PostId).Select(p => p.First()).ToList();
            return posts;
        }

        public static List<BlogPostDL> GetFilteredBlogPosts(string categoryId, string zipCode, string month = "", string year = "")
        {
            if (month.Equals("0"))
            {
                month = String.Empty;
            }
            if (year.Equals("0"))
            {
                year = String.Empty;
            }
            if (String.IsNullOrEmpty(zipCode))
            {
                zipCode = "0";
            }

            bool needPostsAfterCurrentDate = !string.IsNullOrEmpty(month) && !string.IsNullOrEmpty(year);
            const string querySelectPart = @"SELECT BP.*
                                FROM Zip Z
	                                JOIN Area A ON (Z.AreaID = A.AreaID)
	                                JOIN City C ON (A.CityID = C.CityID)
	                                JOIN BlogPostsToCities BPCity ON (C.CityID = BPCity.CityID)
	                                JOIN BlogPosts BP ON (BPCity.PostID = BP.PostID)
	                                JOIN BlogPostsToCategories BPCat ON (BP.PostID = BPCat.PostID)
	                                JOIN BlogCategories BC ON (BPCat.BlogCatID = BC.BlogCatID)";
            const string queryMainConditionsPart = @" WHERE (Z.ZipCode = @zipCode  OR Coalesce(@zipCode,'0') = '')
	                                  AND BPCat.BlogCatID = @categoryID
                                      AND (((DATEPART(yy, BP.PublishDate) = @year OR Coalesce(@year,'') = '')
                                      AND (DATEPART(mm, BP.PublishDate) = @month OR Coalesce(@month,'') = '')))";
            string queryAdditionalConditionsPart = needPostsAfterCurrentDate
                ? string.Empty
                : @" AND CAST(BP.PublishDate AS DATETIME) <= GETDATE()";
            const string queryOrderByPart = @" ORDER BY BP.PublishDate DESC";

            string query = string.Format("{0}{1}{2}{3}", querySelectPart, queryMainConditionsPart,
                queryAdditionalConditionsPart, queryOrderByPart);

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("zipCode", SqlDbType.Int).Value = zipCode;
            command.Parameters.Add("categoryID", SqlDbType.Int).Value = categoryId;
            command.Parameters.Add("month", SqlDbType.VarChar).Value = month;
            command.Parameters.Add("year", SqlDbType.VarChar).Value = year;

            List<BlogPostDL> posts = new List<BlogPostDL>();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                rdr = command.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        var post = ConvertToBlogPost(rdr);
                        posts.Add(post);
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
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }
            posts = posts.GroupBy(p => p.PostId).Select(p => p.First()).ToList();
            return posts;
        }

        public static Dictionary<string, BlogPostDL> GetNextAndPreviousArticleLinks(int currentPostId, string categoryId = null, string zipCode = "", string month = "", string year = "")
        {
            var links = new Dictionary<string, BlogPostDL>();

            List<BlogPostDL> posts = (!String.IsNullOrEmpty(categoryId))
                                         ? GetFilteredBlogPosts(categoryId, zipCode, month, year)
                                         : (!String.IsNullOrEmpty(zipCode))
                                               ? GetFilteredBlogPosts(zipCode, month, year)
                                               : GetAll(categoryId, month, year);
			
            int currentPostIndex = posts.FindIndex(0, p => p.PostId.Equals(currentPostId.ToString()));

            if (posts.ElementAtOrDefault(currentPostIndex - 1) != null)
            {
                links.Add("next", posts.ElementAt(currentPostIndex - 1));
            }

            if (posts.ElementAtOrDefault(currentPostIndex + 1) != null)
            {
                links.Add("previous", posts.ElementAt(currentPostIndex + 1));
            }

            return links;
        }

        public static List<BlogPostDL> GetAll(string categoryId, string month, string year)
        {
            categoryId = String.IsNullOrEmpty(categoryId) ? String.Empty : categoryId;
            List<BlogPostDL> posts = new List<BlogPostDL>();
            const string query = @"SELECT BP.* FROM BlogPosts BP
	                                JOIN BlogPostsToCategories BPCat ON (BP.PostID = BPCat.PostID)
	                                JOIN BlogCategories BC ON (BPCat.BlogCatID = BC.BlogCatID)
                                   WHERE (DATEPART(yy, PublishDate) = @year OR Coalesce(@year,'') = '')
                                      AND (DATEPART(mm, PublishDate) = @month OR Coalesce(@month,'') = '')
                                      AND (BC.BlogCatID = @catId OR Coalesce(@catId,'') = '')
                                      AND CAST(BP.PublishDate AS DATETIME) <= GETDATE()
                                   ORDER BY PublishDate DESC";

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("month", SqlDbType.VarChar).Value = month;
            command.Parameters.Add("year", SqlDbType.VarChar).Value = year;
            command.Parameters.Add("catId", SqlDbType.VarChar).Value = categoryId;

            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                rdr = command.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        var post = ConvertToBlogPost(rdr);
                        posts.Add(post);
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

                if (conn.State == ConnectionState.Open)
                    conn.Close();
                conn.Dispose();
            }
            posts = posts.GroupBy(p => p.PostId).Select(p => p.First()).ToList();
            return posts;
        }

        public static int GetPostIdByUrlTitle(string urlTitle)
        {
            int postId = 0;
            var conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr;

            const string queryString = @"SELECT PostID FROM BlogPosts WHERE UrlTitle = @url";

            var command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("url", SqlDbType.VarChar).Value = urlTitle;

            try
            {
                conn.Open();
                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        postId = Convert.ToInt32(rdr["PostID"]);
                    }
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                conn.Dispose();
            }
            return postId;
        }

        public static string GetPlogPostCategoryUrl(string postId)
        {
            string categoryUrl = String.Empty;

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr;

            const string queryString = @"SELECT BC.BlogCatUrlName FROM [BlogCategories] BC
			INNER JOIN [BlogPostsToCategories] BPC ON BC.BlogCatID = BPC.BlogCatID WHERE BPC.PostID = @postid";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("postid", SqlDbType.Int).Value = postId;

            try
            {
                conn.Open();
                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        categoryUrl = rdr["BlogCatUrlName"].ToString();
                    }
                }
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    conn.Dispose();
                }
            }

            return categoryUrl;
        }

        public static List<BlogPostDL> GetTopPosts()
        {
            List<BlogPostDL> posts = new List<BlogPostDL>();
            const string query = @"SELECT TOP 3 * FROM BlogPosts WHERE isTopBlog = 1 AND CAST(PublishDate AS DATE) <= GETDATE() ORDER BY PublishDate DESC";

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);
            command.CommandType = CommandType.Text;

            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                rdr = command.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        var post = ConvertToBlogPost(rdr);
                        posts.Add(post);
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
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }
            return posts;
        }

        public static Dictionary<string, Dictionary<string, string>> GetArchiveList(DateTime date, int cityId)
        {
            Dictionary<string, Dictionary<string, string>> archiveInfo = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> monthInfo = new Dictionary<string, string>();

            const string query = @"SELECT  COUNT(*) as TotalPosts, tbl.PostYear, tbl.PostMonth
                                    FROM 
                                    (SELECT distinct PublishDate, YEAR(PublishDate) as PostYear, MONTH(PublishDate) as PostMonth FROM BlogPosts
                                     INNER JOIN BlogPostsToCities ON BlogPosts.PostID = BlogPostsToCities.PostID
                                     WHERE PublishDate IS NOT NULL AND (BlogPostsToCities.CityID = @cityId OR Coalesce(@cityId,'0') = '')
                                    ) tbl
                                    WHERE tbl.PublishDate <= @date
                                    GROUP BY tbl.PostYear, tbl.PostMonth
                                    ORDER BY tbl.PostYear DESC";

            var conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr;

            var command = new SqlCommand(query, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("date", SqlDbType.DateTime).Value = date;
            command.Parameters.Add("cityId", SqlDbType.Int).Value = cityId;

            string previouseYearValue = String.Empty;

            try
            {
                conn.Open();
                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        string year = rdr["PostYear"].ToString();
                        string month = rdr["PostMonth"].ToString();
                        string count = rdr["TotalPosts"].ToString();
                        if (!String.IsNullOrEmpty(previouseYearValue) && String.CompareOrdinal(previouseYearValue, year) != 0 && !archiveInfo.ContainsKey(previouseYearValue))
                        {
                            archiveInfo.Add(previouseYearValue, monthInfo);
                            monthInfo = new Dictionary<string, string>();
                            monthInfo.Add(month, count);
                        }
                        else
                        {
                            previouseYearValue = year;
                            monthInfo.Add(month, count);
                        }
                    }
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                conn.Dispose();
            }
            return archiveInfo;
        }

        public static List<BlogArticleListingItem> GetBlogArticleListing()
        {
            var posts = new List<BlogArticleListingItem>();
            const string query = @"SELECT DISTINCT BP.PostID, BP.Title, BP.PublishDate, BP.isTopBlog,
                                    substring(
                                        (
                                            Select  ', ' + BC.BlogCatName  AS [text()]
				                                FROM BlogCategories BC
					                                JOIN BlogPostsToCategories BPCat ON (BP.PostID = BPCat.PostID)
				                                where BPCat.BlogCatID = BC.BlogCatID
                                            ORDER BY BP.PostID
                                            For XML PATH ('')
                                        ), 2, 1000) Categories
                                FROM dbo.BlogPosts BP
                                ORDER BY BP.PublishDate DESC";

            var conn = new SqlConnection(BWConfig.ConnectionString);
            var command = new SqlCommand(query, conn);
            command.CommandType = CommandType.Text;

            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                rdr = command.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        int postId = (int)rdr["postId"];
                        string title = rdr["title"].ToString();
                        string publishDate = rdr["publishDate"].ToString();
                        string categories = rdr["Categories"].ToString();
                        bool isTopBlog = (bool)rdr["isTopBlog"];

                        var post = new BlogArticleListingItem(postId, title, categories, isTopBlog, publishDate);
                        posts.Add(post);
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

                if (conn.State == ConnectionState.Open)
                    conn.Close();
                conn.Dispose();
            }
            return posts;
        }

        public static void UpdateBlogPost(int postId, string title = null, string urlTitle = null, DateTime? publishDate = null,
            string summary = null, string body = null, string metaKeywords = null, string metaDesc = null, string imagePath = null,
            bool? imageSummary = null, bool? imageBody = null, string atlantaLinks = null, string chicagoLinks = null,
            string dallasLinks = null, string novaLinks = null, string houstonLinks = null, string marylandLinks = null,
            string dCLinks = null, string bhamLinks = null, string bostonLinks = null, string philLinks = null, List<string> tags = null, List<string> cityIds = null,
            List<string> blogCatIds = null, bool? isTopBlog = null, string authorNames = null, string authorTitles = null, string authorDescriptions = null)
        {
            using (SqlConnection conn = new SqlConnection(BWConfig.ConnectionString))
            {
                // Update post
                var updateQueries = new List<string>();
                var queryParameters = new List<SqlParameter>();
                if (!String.IsNullOrEmpty(title))
                {
                    updateQueries.Add("title = @TITLE");
                    queryParameters.Add(new SqlParameter("@TITLE", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "Title", DataRowVersion.Default, title));
                }
                if (!String.IsNullOrEmpty(urlTitle))
                {
                    updateQueries.Add("urlTitle = @URLTITLE");
                    queryParameters.Add(new SqlParameter("@URLTITLE", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "UrlTitle", DataRowVersion.Default, urlTitle));
                }
                if (publishDate.HasValue)
                {
                    updateQueries.Add("publishdate = @PUBLISH");
                    queryParameters.Add(new SqlParameter("@PUBLISH", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, false, 0, 0, "PublishDate", DataRowVersion.Default, publishDate.Value));
                }
                if (!String.IsNullOrEmpty(summary))
                {
                    updateQueries.Add("summary = @SUMMARY");
                    queryParameters.Add(new SqlParameter("@SUMMARY", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "Summary", DataRowVersion.Default, summary));
                }
                if (!String.IsNullOrEmpty(body))
                {
                    updateQueries.Add("body = @BODY");
                    queryParameters.Add(new SqlParameter("@BODY", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "Body", DataRowVersion.Default, body));
                }
                if (!String.IsNullOrEmpty(metaKeywords))
                {
                    updateQueries.Add("metakeywords = @KEY");
                    queryParameters.Add(new SqlParameter("@KEY", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "MetaKeywords", DataRowVersion.Default, metaKeywords));
                }
                if (!String.IsNullOrEmpty(metaDesc))
                {
                    updateQueries.Add("metadesc = @DESC");
                    queryParameters.Add(new SqlParameter("@DESC", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "MetaDesc", DataRowVersion.Default, metaDesc));
                }
                if (imageSummary.HasValue)
                {
                    updateQueries.Add("imagesummary = @IMGSUM");
                    queryParameters.Add(new SqlParameter("@IMGSUM", SqlDbType.Bit, 0, ParameterDirection.Input, false, 0, 0, "ImageSummary", DataRowVersion.Default, imageSummary.Value));
                }
                if (imageBody.HasValue)
                {
                    updateQueries.Add("imagebody = @IMGBODY");
                    queryParameters.Add(new SqlParameter("@IMGBODY", SqlDbType.Bit, 0, ParameterDirection.Input, false, 0, 0, "ImageBody", DataRowVersion.Default, imageBody.Value));
                }
                if (!String.IsNullOrEmpty(imagePath))
                {
                    updateQueries.Add("imagepath = @IMG");
                    queryParameters.Add(new SqlParameter("@IMG", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "ImagePath", DataRowVersion.Default, imagePath));
                }
                if (!String.IsNullOrEmpty(atlantaLinks))
                {
                    updateQueries.Add("atlantalinks = @ATL");
                    queryParameters.Add(new SqlParameter("@ATL", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "AtlantaLinks", DataRowVersion.Default, atlantaLinks));
                }
                if (!String.IsNullOrEmpty(chicagoLinks))
                {
                    updateQueries.Add("chicagolinks = @CHI");
                    queryParameters.Add(new SqlParameter("@CHI", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "ChicagoLinks", DataRowVersion.Default, chicagoLinks));
                }
                if (!String.IsNullOrEmpty(dallasLinks))
                {
                    updateQueries.Add("dallaslinks = @DAL");
                    queryParameters.Add(new SqlParameter("@DAL", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "DallasLinks", DataRowVersion.Default, dallasLinks));
                }
                if (!String.IsNullOrEmpty(novaLinks))
                {
                    updateQueries.Add("novalinks = @NOVA");
                    queryParameters.Add(new SqlParameter("@NOVA", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "NovaLinks", DataRowVersion.Default, novaLinks));
                }
                if (!String.IsNullOrEmpty(houstonLinks))
                {
                    updateQueries.Add("houstonlinks = @HOU");
                    queryParameters.Add(new SqlParameter("@HOU", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "HoustonLinks", DataRowVersion.Default, houstonLinks));
                }
                if (!String.IsNullOrEmpty(marylandLinks))
                {
                    updateQueries.Add("marylandlinks = @MARY");
                    queryParameters.Add(new SqlParameter("@MARY", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "MarylandLinks", DataRowVersion.Default, marylandLinks));
                }
                if (!String.IsNullOrEmpty(dCLinks))
                {
                    updateQueries.Add("dclinks = @DC");
                    queryParameters.Add(new SqlParameter("@DC", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "DCLinks", DataRowVersion.Default, dCLinks));
                }
                if (!String.IsNullOrEmpty(bhamLinks))
                {
                    updateQueries.Add("bhamlinks = @BHAM");
                    queryParameters.Add(new SqlParameter("@BHAM", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "BhamLinks", DataRowVersion.Default, bhamLinks));
                }
                if (!String.IsNullOrEmpty(bostonLinks))
                {
                    updateQueries.Add("bostonLinks = @BOSTON");
                    queryParameters.Add(new SqlParameter("@BOSTON", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "bostonLinks", DataRowVersion.Default, bostonLinks));
                }
                if (!String.IsNullOrEmpty(philLinks))
                {
                    updateQueries.Add("philiLinks = @PHILI");
                    queryParameters.Add(new SqlParameter("@PHILI", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "philiLinks", DataRowVersion.Default, philLinks));
                }
                if (isTopBlog.HasValue)
                {
                    updateQueries.Add("isTopBlog = @ISTOPBLOG");
                    queryParameters.Add(new SqlParameter("@ISTOPBLOG", SqlDbType.Bit, 0, ParameterDirection.Input, false, 0, 0, "isTopBlog", DataRowVersion.Default, isTopBlog.Value));
                }
                if (!String.IsNullOrEmpty(authorNames))
                {
                    updateQueries.Add("authorNames = @authorNames");
                    queryParameters.Add(new SqlParameter("@authorNames", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "authorNames", DataRowVersion.Default, authorNames));
                }
                if (!String.IsNullOrEmpty(authorTitles))
                {
                    updateQueries.Add("authorTitles = @authorTitles");
                    queryParameters.Add(new SqlParameter("@authorTitles", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "authorTitles", DataRowVersion.Default, authorTitles));
                }
                if (!String.IsNullOrEmpty(authorDescriptions))
                {
                    updateQueries.Add("authorDescriptions = @authorDescriptions");
                    queryParameters.Add(new SqlParameter("@authorDescriptions", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "authorDescriptions", DataRowVersion.Default, authorDescriptions));
                }
                if (updateQueries.Count > 0)
                {

                    queryParameters.Add(new SqlParameter("@PID", SqlDbType.Int, 0, ParameterDirection.Input, false, 0, 0, "PostID", DataRowVersion.Default, postId));
                    var query = "UPDATE BlogPosts SET " + string.Join(", ", updateQueries) + " WHERE PostID = @PID";

                    try
                    {
                        var command = new SqlCommand(query, conn);
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddRange(queryParameters.ToArray());
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        throw;
                    }

                }


                //Insert tags
                if (tags != null && tags.Count > 0)
                {
                    //Remove values from blogtags
                    string query = "DELETE FROM BlogTags WHERE (PostID=@PID)";
                    SqlCommand commandRemoveTags = new SqlCommand(query, conn);
                    commandRemoveTags.Parameters.Add(new SqlParameter("@PID", SqlDbType.Int, 0, ParameterDirection.Input, false, 0, 0, "PostID", DataRowVersion.Default, postId));
                    commandRemoveTags.ExecuteNonQuery();

                    // Insert to blogtags
                    var tagsValues = new List<string>();
                    var tagsParams = new List<SqlParameter>();
                    for (var i = 0; i < tags.Count; i++)
                    {
                        tagsValues.Add("(@ID, @TAG" + i + ")");
                        tagsParams.Add(new SqlParameter("@TAG" + i, tags[i]));
                    }
                    var tagsQuery = "INSERT INTO BlogTags (postid, tagname) VALUES " + string.Join(", ", tagsValues);
                    tagsParams.Add(new SqlParameter("@ID", postId));
                    SqlCommand commandAddTag = new SqlCommand(tagsQuery, conn);
                    commandAddTag.Parameters.AddRange(tagsParams.ToArray());
                    commandAddTag.ExecuteNonQuery();
                }


                //Insert into blogpoststocities
                if (cityIds != null && cityIds.Count > 0)
                {
                    //Remove values from blogpoststocities
                    string query = "DELETE FROM BlogPostsToCities WHERE (PostID=@PID)";
                    SqlCommand commandRemoveCity = new SqlCommand(query, conn);
                    commandRemoveCity.Parameters.Add(new SqlParameter("@PID", SqlDbType.Int, 0, ParameterDirection.Input, false, 0, 0, "PostID", DataRowVersion.Default, postId));
                    commandRemoveCity.ExecuteNonQuery();

                    //Insert to blogpoststocities
                    var cityValues = new List<string>();
                    var cityParams = new List<SqlParameter>();
                    for (var i = 0; i < cityIds.Count; i++)
                    {
                        cityValues.Add("(@ID, @CATID" + i + ")");
                        cityParams.Add(new SqlParameter("@CATID" + i, cityIds[i]));
                    }
                    var cityQuery = "INSERT INTO BlogPostsToCities (postid, cityid) VALUES " + string.Join(", ", cityValues);
                    cityParams.Add(new SqlParameter("@ID", postId));
                    SqlCommand commandAddCityIds = new SqlCommand(cityQuery, conn);
                    commandAddCityIds.Parameters.AddRange(cityParams.ToArray());
                    commandAddCityIds.ExecuteNonQuery();
                }


                if (blogCatIds != null && blogCatIds.Count > 0)
                {
                    //Remove values from blogpoststocategories
                    string query = "DELETE FROM BlogPostsToCategories WHERE (PostID=@PID)";
                    SqlCommand commandRemoveCat = new SqlCommand(query, conn);
                    commandRemoveCat.Parameters.Add(new SqlParameter("@PID", SqlDbType.Int, 0, ParameterDirection.Input, false, 0, 0, "PostID", DataRowVersion.Default, postId));
                    commandRemoveCat.ExecuteNonQuery();

                    // Insert to blogpoststocategories
                    var blogCatValues = new List<string>();
                    var blogCatParams = new List<SqlParameter>();
                    for (var i = 0; i < blogCatIds.Count; i++)
                    {
                        blogCatValues.Add("(@ID, @CATID" + i + ")");
                        blogCatParams.Add(new SqlParameter("@CATID" + i, blogCatIds[i]));
                    }
                    var blogCatQuery = "INSERT INTO BlogPostsToCategories (postid, blogcatid) VALUES " + string.Join(", ", blogCatValues);
                    blogCatParams.Add(new SqlParameter("@ID", postId));
                    SqlCommand commandAddBlogCatId = new SqlCommand(blogCatQuery, conn);
                    commandAddBlogCatId.Parameters.AddRange(blogCatParams.ToArray());
                    commandAddBlogCatId.ExecuteNonQuery();
                }

                conn.Close();

            }
        }

        public static void AddBlogPost(string title, string urlTitle, DateTime publishDate,
           string summary = null, string body = null, string metaKeywords = null, string metaDesc = null, string imagePath = null,
           bool? imageSummary = null, bool? imageBody = null, string atlantaLinks = null, string chicagoLinks = null,
           string dallasLinks = null, string novaLinks = null, string houstonLinks = null, string marylandLinks = null,
           string dCLinks = null, string bhamLinks = null, string bostonLinks = null, string philLinks = null, List<string> tags = null, List<string> cityIds = null,
           List<string> blogCatIds = null, string authorNames = null, string authorTitles = null, string authorDescriptions = null)
        {
            if (title == null || urlTitle == null || imageSummary == null || imageBody == null)
            {
                throw new ArgumentNullException("title, urlTitle, imageSummary, imageBody, isTopBlog can't be null");
            }

            using (SqlConnection conn = new SqlConnection(BWConfig.ConnectionString))
            {
                // Update post
                var updateQueries = new List<string>();
                var queryParameters = new List<SqlParameter>();
                if (!String.IsNullOrEmpty(title))
                {
                    updateQueries.Add("title");
                    queryParameters.Add(new SqlParameter("@title", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "Title", DataRowVersion.Default, title));
                }
                if (!String.IsNullOrEmpty(urlTitle))
                {
                    updateQueries.Add("urlTitle");
                    queryParameters.Add(new SqlParameter("@urlTitle", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "UrlTitle", DataRowVersion.Default, urlTitle));
                }

                updateQueries.Add("publishdate");
                queryParameters.Add(new SqlParameter("@publishdate", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, false, 0, 0, "PublishDate", DataRowVersion.Default, publishDate));

                if (!String.IsNullOrEmpty(summary))
                {
                    updateQueries.Add("summary");
                    queryParameters.Add(new SqlParameter("@summary", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "Summary", DataRowVersion.Default, summary));
                }
                if (!String.IsNullOrEmpty(body))
                {
                    updateQueries.Add("body");
                    queryParameters.Add(new SqlParameter("@body", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "Body", DataRowVersion.Default, body));
                }
                if (!String.IsNullOrEmpty(metaKeywords))
                {
                    updateQueries.Add("metakeywords");
                    queryParameters.Add(new SqlParameter("@metakeywords", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "MetaKeywords", DataRowVersion.Default, metaKeywords));
                }
                if (!String.IsNullOrEmpty(metaDesc))
                {
                    updateQueries.Add("metadesc");
                    queryParameters.Add(new SqlParameter("@metadesc", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "MetaDesc", DataRowVersion.Default, metaDesc));
                }
                if (imageSummary.HasValue)
                {
                    updateQueries.Add("imagesummary");
                    queryParameters.Add(new SqlParameter("@imagesummary", SqlDbType.Bit, 0, ParameterDirection.Input, false, 0, 0, "ImageSummary", DataRowVersion.Default, imageSummary.Value));
                }
                if (imageBody.HasValue)
                {
                    updateQueries.Add("imagebody");
                    queryParameters.Add(new SqlParameter("@imagebody", SqlDbType.Bit, 0, ParameterDirection.Input, false, 0, 0, "ImageBody", DataRowVersion.Default, imageBody.Value));
                }
                if (!String.IsNullOrEmpty(imagePath))
                {
                    updateQueries.Add("imagepath");
                    queryParameters.Add(new SqlParameter("@imagepath", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "ImagePath", DataRowVersion.Default, imagePath));
                }
                if (!String.IsNullOrEmpty(atlantaLinks))
                {
                    updateQueries.Add("atlantalinks");
                    queryParameters.Add(new SqlParameter("@atlantalinks", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "AtlantaLinks", DataRowVersion.Default, atlantaLinks));
                }
                if (!String.IsNullOrEmpty(chicagoLinks))
                {
                    updateQueries.Add("chicagolinks");
                    queryParameters.Add(new SqlParameter("@chicagolinks", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "ChicagoLinks", DataRowVersion.Default, chicagoLinks));
                }
                if (!String.IsNullOrEmpty(dallasLinks))
                {
                    updateQueries.Add("dallaslinks");
                    queryParameters.Add(new SqlParameter("@dallaslinks", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "DallasLinks", DataRowVersion.Default, dallasLinks));
                }
                if (!String.IsNullOrEmpty(novaLinks))
                {
                    updateQueries.Add("novalinks");
                    queryParameters.Add(new SqlParameter("@novalinks", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "NovaLinks", DataRowVersion.Default, novaLinks));
                }
                if (!String.IsNullOrEmpty(houstonLinks))
                {
                    updateQueries.Add("houstonlinks");
                    queryParameters.Add(new SqlParameter("@houstonlinks", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "HoustonLinks", DataRowVersion.Default, houstonLinks));
                }
                if (!String.IsNullOrEmpty(marylandLinks))
                {
                    updateQueries.Add("marylandlinks");
                    queryParameters.Add(new SqlParameter("@marylandlinks", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "MarylandLinks", DataRowVersion.Default, marylandLinks));
                }
                if (!String.IsNullOrEmpty(dCLinks))
                {
                    updateQueries.Add("dclinks");
                    queryParameters.Add(new SqlParameter("@dclinks", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "DCLinks", DataRowVersion.Default, dCLinks));
                }
                if (!String.IsNullOrEmpty(bhamLinks))
                {
                    updateQueries.Add("bhamlinks");
                    queryParameters.Add(new SqlParameter("@bhamlinks", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "BhamLinks", DataRowVersion.Default, bhamLinks));
                }
                if (!String.IsNullOrEmpty(bostonLinks))
                {
                    updateQueries.Add("bostonLinks");
                    queryParameters.Add(new SqlParameter("@bostonLinks", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "bostonLinks", DataRowVersion.Default, bostonLinks));
                }
                if (!String.IsNullOrEmpty(philLinks))
                {
                    updateQueries.Add("philiLinks");
                    queryParameters.Add(new SqlParameter("@philiLinks", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "philiLinks", DataRowVersion.Default, philLinks));
                }
                if (!String.IsNullOrEmpty(authorNames))
                {
                    updateQueries.Add("authorNames");
                    queryParameters.Add(new SqlParameter("@authorNames", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "authorNames", DataRowVersion.Default, authorNames));
                }
                if (!String.IsNullOrEmpty(authorTitles))
                {
                    updateQueries.Add("authorTitles");
                    queryParameters.Add(new SqlParameter("@authorTitles", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "authorTitles", DataRowVersion.Default, authorTitles));
                }
                if (!String.IsNullOrEmpty(authorDescriptions))
                {
                    updateQueries.Add("authorDescriptions");
                    queryParameters.Add(new SqlParameter("@authorDescriptions", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "authorDescriptions", DataRowVersion.Default, authorDescriptions));
                }
                if (updateQueries.Count > 0)
                {

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@NEWID";
                    param.SqlDbType = SqlDbType.Int;
                    param.Direction = ParameterDirection.Output;

                    queryParameters.Add(param);
                    var query = "INSERT INTO BlogPosts (" + string.Join(", ", updateQueries) + ") VALUES (" + string.Join(", ", updateQueries.Select(q => '@' + q)) + ") SET @NEWID = SCOPE_IDENTITY()";

                    try
                    {
                        SqlCommand command = new SqlCommand(query, conn);
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddRange(queryParameters.ToArray());
                        conn.Open();
                        command.ExecuteNonQuery();


                        int newpostid = int.Parse(param.Value.ToString());


                        //Insert tags
                        if (tags != null && tags.Count > 0)
                        {
                            var tagsValues = new List<string>();
                            var tagsParams = new List<SqlParameter>();
                            for (var i = 0; i < tags.Count; i++)
                            {
                                tagsValues.Add("(@ID, @TAG" + i + ")");
                                tagsParams.Add(new SqlParameter("@TAG" + i, tags[i]));
                            }
                            var tagsQuery = "INSERT INTO BlogTags (postid, tagname) VALUES " + string.Join(", ", tagsValues);
                            tagsParams.Add(new SqlParameter("@ID", newpostid));
                            SqlCommand commandAddTag = new SqlCommand(tagsQuery, conn);
                            commandAddTag.Parameters.AddRange(tagsParams.ToArray());
                            commandAddTag.ExecuteNonQuery();
                        }


                        //Insert into blogpoststocities
                        if (cityIds != null && cityIds.Count > 0)
                        {
                            var cityValues = new List<string>();
                            var cityParams = new List<SqlParameter>();
                            for (var i = 0; i < cityIds.Count; i++)
                            {
                                cityValues.Add("(@ID, @CATID" + i + ")");
                                cityParams.Add(new SqlParameter("@CATID" + i, cityIds[i]));
                            }
                            var cityQuery = "INSERT INTO BlogPostsToCities (postid, cityid) VALUES" + string.Join(", ", cityValues);
                            cityParams.Add(new SqlParameter("@ID", newpostid));
                            SqlCommand commandAddCityIds = new SqlCommand(cityQuery, conn);
                            commandAddCityIds.Parameters.AddRange(cityParams.ToArray());
                            commandAddCityIds.ExecuteNonQuery();
                        }


                        if (blogCatIds != null && blogCatIds.Count > 0)
                        {
                            // Insert to blogpoststocategories
                            var blogCatValues = new List<string>();
                            var blogCatParams = new List<SqlParameter>();
                            for (var i = 0; i < blogCatIds.Count; i++)
                            {
                                blogCatValues.Add("(@ID, @CATID" + i + ")");
                                blogCatParams.Add(new SqlParameter("@CATID" + i, blogCatIds[i]));
                            }
                            var blogCatQuery = "INSERT INTO BlogPostsToCategories (postid, blogcatid) VALUES" + string.Join(", ", blogCatValues);
                            blogCatParams.Add(new SqlParameter("@ID", newpostid));
                            SqlCommand commandAddBlogCatId = new SqlCommand(blogCatQuery, conn);
                            commandAddBlogCatId.Parameters.AddRange(blogCatParams.ToArray());
                            commandAddBlogCatId.ExecuteNonQuery();
                        }


                    }
                    catch (Exception e)
                    {
                        throw;
                    }

                }
                conn.Close();

            }
        }

        public static List<string> GetCityIdsForBlogPost(int postId)
        {
            var cityIds = new List<string>();
            const string query = @"SELECT CityID FROM BlogPostsToCities WHERE PostID = @PID";
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add(new SqlParameter("@PID", postId));
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                rdr = command.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        string cityID = rdr["cityID"].ToString();
                        cityIds.Add(cityID);
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

                if (conn.State == ConnectionState.Open)
                    conn.Close();
                conn.Dispose();
            }
            return cityIds;
        }

        public static List<string> GetTagsForBlogPost(int postId)
        {
            var tags = new List<string>();
            const string query = @"SELECT * FROM BlogTags WHERE (PostID = @PID)";
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add(new SqlParameter("@PID", postId));
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                rdr = command.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        string tag = rdr["TagName"].ToString();
                        tags.Add(tag);
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
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }
            return tags;
        }

        public static List<BlogPostDL> SearchBlogPosts(string cityid, string keyword, bool filterByPublishDate)
        {
            if (String.IsNullOrEmpty(keyword))
            {
                return new List<BlogPostDL>();
            }
            string date = (filterByPublishDate) ? DateTime.Now.ToString() : String.Empty;
            
            if (String.IsNullOrEmpty(cityid))
            {
                cityid = "0";
            }

            var keywords = keyword.Split().Select(s => "\"" + s + "\"");
            var keywordQuery = String.Join(" AND ", keywords);

            List<BlogPostDL> posts = new List<BlogPostDL>();
            const string query = @"SELECT BP.*
                                FROM BlogPosts BP
	                                JOIN BlogPostsToCities BPCity ON (BP.PostID = BPCity.PostID)
	                                JOIN BlogPostsToCategories BPCat ON (BP.PostID = BPCat.PostID)
                                WHERE (BPCity.CityID = @cityid OR Coalesce(@cityid,'0') = '')
                                      AND (BP.PublishDate <= @date OR Coalesce(@date,'') = '')
                                      AND CONTAINS(BP.*, @KEYWORD)
                                      AND BP.Title IS NOT NULL
                                      AND CAST(BP.PublishDate AS DATETIME) <= GETDATE()
                                ORDER BY BP.PublishDate DESC";

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("cityid", SqlDbType.Int).Value = cityid;
            command.Parameters.Add("date", SqlDbType.NVarChar).Value = date;
            command.Parameters.Add("KEYWORD", SqlDbType.VarChar).Value = keywordQuery;

            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                rdr = command.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        var post = ConvertToBlogPost(rdr); posts.Add(post);
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
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }
            posts = posts.GroupBy(p => p.PostId).Select(p => p.First()).ToList();
            return posts;
        }
    }
}
