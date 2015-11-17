using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ER_BestPickReports_Dev.App_Code
{
    public class BlogCategoryDL
    {
        public static int GetBlogCategoryIdByCatUrlName(string catUrlName)
        {
            int blogCategoryId = 0;

            using (var conn = new SqlConnection(BWConfig.ConnectionString))
            {
                const string queryString =
                    @"SELECT TOP (1) BlogCatID FROM [dbo].[BlogCategories] WHERE BlogCatUrlName = @blogCatUrlName";

                SqlCommand command = new SqlCommand(queryString, conn);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("blogCatUrlName", catUrlName);

                conn.Open();

                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            blogCategoryId = Convert.ToInt32(rdr["BlogCatID"]);
                        }
                    }
                }
            }

            return blogCategoryId;
        }

        public static int GetBlogCategoryIdByCatId(int catId)
        {
            int blogCategoryId = 0;

            using (var conn = new SqlConnection(BWConfig.ConnectionString))
            {
                const string queryString =
                    @"SELECT TOP (1) blogCategoryID FROM [dbo].[Category] WHERE CategoryID = @catId";

                SqlCommand command = new SqlCommand(queryString, conn);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("catId", catId);

                conn.Open();

                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            blogCategoryId = Convert.ToInt32(rdr["blogCategoryID"]);
                        }
                    }
                }
            }

            return blogCategoryId;
        }

        public static List<int> GetCategoryIdsByBlogCatId(int blogCatId)
        {
            var categoryIds = new List<int>();

            using (var conn = new SqlConnection(BWConfig.ConnectionString))
            {
                const string queryString =
                    @"SELECT CategoryID FROM [dbo].[Category] WHERE blogCategoryID = @blogCatId";

                SqlCommand command = new SqlCommand(queryString, conn);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("blogCatId", blogCatId);

                conn.Open();

                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            int categoryId = Convert.ToInt32(rdr["CategoryID"]);
                            categoryIds.Add(categoryId);
                        }
                    }
                }
            }

            return categoryIds;
        }
    }
}