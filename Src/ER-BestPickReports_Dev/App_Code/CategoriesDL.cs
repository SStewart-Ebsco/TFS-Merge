using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ER_BestPickReports_Dev.App_Code
{
    public class CategoriesDL
    {
        private int _id;
        private int _blogCatId;
        private string _name;
        private string _catUrlName;
        private int _contractorsAmount;

        public int Id
        {
            get { return _id; }
            private set { _id = value; }
        }

        public int BlogCatId
        {
            get { return _blogCatId; }
            private set { _blogCatId = value; }
        }

        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        public string CatUrlName
        {
            get { return _catUrlName; }
            private set { _catUrlName = value; }
        }

        public int ContractorsAmount
        {
            get { return _contractorsAmount; }
            private set { _contractorsAmount = value; }
        }

        public CategoriesDL(int catId, int blogCatId, string catName, string catUrlName, int contractorsAmount)
        {
            _id = catId;
            _name = catName;
            _catUrlName = catUrlName;
            _blogCatId = blogCatId;
            _contractorsAmount = contractorsAmount;
        }

        public static List<CategoriesDL> GetAll()
        {
            List<CategoriesDL> categoriesList = new List<CategoriesDL>();
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);

            string queryString = @"SELECT DISTINCT CI.CategoryID, CI.blogCategoryID, CI.DisplayName, CI.UrlName, COUNT(DISTINCT CACR.ContractorID) as ContractorsAmount
                                    FROM CategoryInfo CI
                                    FULL OUTER JOIN 
                                    (SELECT DISTINCT CategoryID, ContractorID, AreaID 
                                        FROM ContractorAreaCategoryRel ) CACR
                                    ON CACR.CategoryID = CI.CategoryID
                                   WHERE CI.blogCategoryID IS NOT NULL
                                   GROUP BY CI.CategoryID, CI.blogCategoryID, CI.DisplayName, CI.UrlName";

            SqlCommand command = new SqlCommand(queryString, conn);
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
                        int catID = Convert.ToInt32(rdr["CategoryID"]);
                        int blogCatId = Convert.ToInt32(rdr["blogCategoryID"]);
                        string catName = rdr["DisplayName"].ToString();
                        string catUrlName = rdr["UrlName"].ToString();
                        int contractorsAmount = Convert.ToInt32(rdr["ContractorsAmount"]);
                        CategoriesDL categorie = new CategoriesDL(catID, blogCatId, catName, catUrlName, contractorsAmount);
                        categoriesList.Add(categorie);
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

            return categoriesList;
        }

        public static List<CategoriesDL> GetCategoriesByZipCode(string zipCode)
        {
            List<CategoriesDL> categoriesList = new List<CategoriesDL>();
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);

            string queryString = @"SELECT DISTINCT CI.CategoryID, CI.blogCategoryID, CI.DisplayName, CI.UrlName, COUNT(DISTINCT CACR.ContractorID) as ContractorsAmount
                                    FROM Zip Z
                                        JOIN Area A ON (Z.AreaID = A.AreaID)
                                        JOIN City C ON (A.CityID = C.CityID)
                                        JOIN BlogPostsToCities BPCity ON (C.CityID = BPCity.CityID)
                                        JOIN BlogPosts BP ON (BPCity.PostID = BP.PostID)
                                        JOIN BlogPostsToCategories BPCat ON (BP.PostID = BPCat.PostID)
                                        JOIN CategoryInfo CI ON (BPCat.BlogCatID = CI.blogCategoryID)
                                        FULL OUTER JOIN 
                                            (SELECT DISTINCT CategoryID, ContractorID, AreaID 
                                                FROM ContractorAreaCategoryRel ) CACR
                                        ON CACR.CategoryID = CI.CategoryID AND CACR.AreaID = Z.AreaId
                                    WHERE Z.ZipCode = @zipCode OR Coalesce(@zipCode,'0') = ''
                                    GROUP BY CI.CategoryID, CI.blogCategoryID, CI.DisplayName, CI.UrlName";
            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("zipCode", SqlDbType.Int).Value = zipCode;

            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                rdr = command.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        int catID = Convert.ToInt32(rdr["CategoryID"]);
                        int blogCatId = Convert.ToInt32(rdr["blogCategoryID"]);
                        string catName = rdr["DisplayName"].ToString();
                        string catUrlName = rdr["UrlName"].ToString();
                        int contractorsAmount = Convert.ToInt32(rdr["ContractorsAmount"]);
                        CategoriesDL category = new CategoriesDL(catID, blogCatId, catName, catUrlName, contractorsAmount);
                        categoriesList.Add(category);
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

            return categoriesList;
        }

        public static List<CategoriesDL> GetTopCategoriesByZipCode(string zipCode)
        {
            List<CategoriesDL> topCategoriesList = new List<CategoriesDL>();
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);

            string queryString = @"SELECT DISTINCT CI.CategoryID, CI.blogCategoryID, CI.DisplayName, CI.UrlName, COUNT(DISTINCT CACR.ContractorID) as ContractorsAmount
                            FROM Zip Z
                                JOIN Area A ON (Z.AreaID = A.AreaID)
                                JOIN City C ON (A.CityID = C.CityID)
                                JOIN BlogPostsToCities BPCity ON (C.CityID = BPCity.CityID)
                                JOIN BlogPosts BP ON (BPCity.PostID = BP.PostID)
                                JOIN BlogPostsToCategories BPCat ON (BP.PostID = BPCat.PostID)
                                JOIN CategoryInfo CI ON (BPCat.BlogCatID = CI.blogCategoryID)
                                FULL OUTER JOIN 
                                    (SELECT DISTINCT CategoryID, ContractorID, AreaID 
                                        FROM ContractorAreaCategoryRel ) CACR
                                ON CACR.CategoryID = CI.CategoryID AND CACR.AreaID = Z.AreaId
                            WHERE Z.ZipCode = @zipCode AND BP.isTopBlog = 1
                            GROUP BY CI.CategoryID, CI.blogCategoryID, CI.DisplayName, CI.UrlName";
            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("zipCode", SqlDbType.Int).Value = zipCode;

            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                rdr = command.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        int catID = Convert.ToInt32(rdr["CategoryID"]);
                        int blogCatId = Convert.ToInt32(rdr["blogCategoryID"]);
                        string catName = rdr["DisplayName"].ToString();
                        string catUrlName = rdr["UrlName"].ToString();
                        int contractorsAmount = Convert.ToInt32(rdr["ContractorsAmount"]);
                        CategoriesDL category = new CategoriesDL(catID, blogCatId, catName, catUrlName, contractorsAmount);
                        topCategoriesList.Add(category);
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

            return topCategoriesList;
        }

        public static List<CategoriesDL> GetTopCategoriesByAreaId(int areaId)
        {
            List<CategoriesDL> topCategoriesList = new List<CategoriesDL>();
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);

            string queryString = @"SELECT CI.CategoryID, CI.blogCategoryID, CI.DisplayName, CI.UrlName, COUNT(DISTINCT CC.ContractorID) as ContractorsAmount
							FROM CategoryArea CA
								JOIN Area A ON (CA.AreaID = A.AreaID)
								JOIN CategoryInfo CI ON (CA.CategoryID = CI.CategoryID)
								JOIN ContractorCategory CC ON (CC.CategoryID = CI.CategoryID)
							WHERE A.AreaID = @areaId AND isTopCategory = 1
							GROUP BY CI.CategoryID, CI.blogCategoryID, CI.DisplayName, CI.UrlName
							ORDER BY CI.DisplayName";
            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("areaId", SqlDbType.Int).Value = areaId;

            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                rdr = command.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        int catID = Convert.ToInt32(rdr["CategoryID"]);
                        int blogCatId = Convert.ToInt32(rdr["blogCategoryID"]);
                        string catName = rdr["DisplayName"].ToString();
                        string catUrlName = rdr["UrlName"].ToString();
                        int contractorsAmount = Convert.ToInt32(rdr["ContractorsAmount"]);
                        CategoriesDL category = new CategoriesDL(catID, blogCatId, catName, catUrlName, contractorsAmount);
                        topCategoriesList.Add(category);
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

            return topCategoriesList;
        }

        public static int GetCategoryIdByUrl(string catUrl)
        {
            int catId = 0;
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr = null;

            string queryString = @"SELECT CategoryID FROM CategoryInfo WHERE (UrlName = @url)";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("url", SqlDbType.VarChar).Value = catUrl;

            try
            {
                conn.Open();
                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        catId = Convert.ToInt32(rdr["CategoryID"]);
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
            return catId;
        }

        public static string GetBlogCategoryIdById(string categoryId)
        {
            string catId = String.Empty;
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr = null;

            string queryString = @"SELECT blogCategoryID FROM CategoryInfo WHERE (CategoryID = @id)";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("id", SqlDbType.VarChar).Value = categoryId;

            try
            {
                conn.Open();
                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        catId = rdr["blogCategoryID"].ToString();
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
            return catId;
        }

        public static CategoriesDL GetCategoryInfo(string catId, string catUrl, bool isImageUrl = false)
        {
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr = null;

            string queryString = @"SELECT CategoryID, blogCategoryID, DisplayName, UrlName, Website
                            FROM CategoryInfo
                            WHERE CategoryID = @id OR UrlName = @catUrl";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("id", SqlDbType.VarChar).Value = catId;
            command.Parameters.Add("catUrl", SqlDbType.VarChar).Value = catUrl;
            CategoriesDL category = new CategoriesDL(-1, -1, String.Empty, String.Empty, 0);
            try
            {
                conn.Open();
                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        category.Id = Convert.ToInt32(rdr["CategoryID"]);
                        category.BlogCatId = Convert.ToInt32(rdr["blogCategoryID"]);
                        category.Name = rdr["DisplayName"].ToString();
                        if (isImageUrl)
                        {
                            category.CatUrlName = rdr["Website"].ToString();
                        }
                        else
                        {
                            category.CatUrlName = rdr["UrlName"].ToString();
                        }
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
            return category;
        }

        public static CategoriesDL GetCategoryInfo(int areaId, int cityId, int categoryId)
        {
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr = null;

                //Get breadcrumb info
            var queryString = "SELECT CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName, AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaDisplayName, CityInfo.UrlName AS CityUrlName, CityInfo.DisplayName AS CityDisplayName " +
                    "FROM CategoryArea INNER JOIN CategoryInfo ON CategoryArea.CategoryID = CategoryInfo.CategoryID INNER JOIN " +
                    "AreaInfo ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CityInfo ON AreaInfo.CityID = CityInfo.CityID WHERE (AreaInfo.AreaID = @AREAID) AND " +
                    "(CityInfo.CityID = @CITYID) AND (CategoryInfo.CategoryID = @CATID)";

                SqlCommand command = new SqlCommand(queryString, conn);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("CATID", SqlDbType.VarChar).Value = categoryId;
                command.Parameters.Add("CITYID", SqlDbType.VarChar).Value = cityId;
                command.Parameters.Add("AREAID", SqlDbType.VarChar).Value = areaId;
                CategoriesDL category = new CategoriesDL(-1, -1, String.Empty, String.Empty, 0);

                try
                {
                    conn.Open();
                    rdr = command.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        if (rdr.Read())
                        {
                            category.CatUrlName = String.Concat("/", rdr["CatUrlName"], "/", rdr["CityURLName"], "/", rdr["AreaURLName"]);
                            category.Name = rdr["CatName"].ToString();
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
                return category;
        }

        public static List<CategoriesDL> GetCategoriesForBlogPost(int postId)
        {
            var cats = new List<CategoriesDL>();
            const string query = @"SELECT * FROM BlogPostsToCategories BPC
                    INNER JOIN CategoryInfo C ON C.blogCategoryID = BPC.BlogCatId WHERE PostID = @PID";
            var conn = new SqlConnection(BWConfig.ConnectionString);
            var command = new SqlCommand(query, conn);
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
                        cats.Add(new CategoriesDL(
                            (int)rdr["CategoryID"],
                            (int)rdr["blogCategoryID"],
                            rdr["DisplayName"].ToString(),
                            rdr["UrlName"].ToString(),
                            0));
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
            return cats;
        }
    }
}
