using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ER_BestPickReports_Dev.App_Code
{
    public class MobileCategoriesDL
    {
        private int _id;
        private string _name;
        private string _categoryUrlName;
        private string _areaUrlName;
        private string _areaDisplayName;
        private string _cityUrlName;
        private string _cityDisplayName;
        private string _categoryIcon;
        private bool _isPrimary;

        public int Id
        {
            get { return _id; }
            private set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        public string AreaUrlName
        {
            get { return _areaUrlName; }
            private set { _areaUrlName = value; }
        }

        public string AreaDisplayName
        {
            get { return _areaDisplayName; }
            private set { _areaDisplayName = value; }
        }

        public string CategoryUrlName
        {
            get { return _categoryUrlName; }
            private set { _categoryUrlName = value; }
        }

        public string CityUrlName
        {
            get { return _cityUrlName; }
            private set { _cityUrlName = value; }
        }

        public string CityDisplayName
        {
            get { return _cityDisplayName; }
            private set { _cityDisplayName = value; }
        }

        public string CategoryIcon
        {
            get { return _categoryIcon; }
            private set { _categoryIcon = value; }
        }

        public bool IsPrimary
        {
            get { return _isPrimary; }
            private set { _isPrimary = value; }
        }

        public MobileCategoriesDL(int catId, string catName, string categoryUrlName, string areaUrlName, string areaDisplayName, string cityUrlName, string cityDisplayName, string iconPath, bool isPrimary)
        {
            _id = catId;
            _name = catName;
            _categoryUrlName = categoryUrlName;
            _areaUrlName = areaUrlName;
            _areaDisplayName = areaDisplayName;
            _cityUrlName = cityUrlName;
            _cityDisplayName = cityDisplayName;
            _categoryIcon = iconPath;
            _isPrimary = isPrimary;
        }

        public static List<MobileCategoriesDL> GetByCityIdAreaId(int cityId, int areaId)
        {
            List<MobileCategoriesDL> categoriesList = new List<MobileCategoriesDL>();
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);

            string queryString = "SELECT CityInfo.UrlName AS CityUrlName, CategoryInfo.CategoryID, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName, " +
                        "AreaInfo.UrlName AS AreaUrlName, Info.Website AS InfoWebsite, CategoryArea.IsPrimary, Cityinfo.DisplayName AS CityDisplayName, AreaInfo.DisplayName AS AreaDisplayName " +
                        "FROM CategoryArea INNER JOIN CategoryInfo ON CategoryArea.CategoryID = CategoryInfo.CategoryID INNER JOIN " +
                        "AreaInfo ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CityInfo ON AreaInfo.CityID = CityInfo.CityID " +
                        "INNER JOIN Info ON CategoryArea.CategoryID = Info.ReferenceID AND Info.InfoType = 1 " +
                        "WHERE (CategoryArea.AreaID = @AREAID) AND (AreaInfo.CityID = @CITYID) ORDER BY CategoryInfo.DisplayName";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("@CITYID", SqlDbType.Int).Value = cityId;
            command.Parameters.Add("@AREAID", SqlDbType.Int).Value = areaId;

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
                        string catName = rdr["CatName"].ToString();
                        string categoryUrlName = rdr["CatUrlName"].ToString();
                        string areaUrlName = rdr["AreaUrlName"].ToString();
                        string areaDisplayName = rdr["AreaDisplayName"].ToString();
                        string cityUrlName = rdr["CityUrlName"].ToString();
                        string cityDisplayName = rdr["CityDisplayName"].ToString();
                        string iconPath = rdr["InfoWebsite"].ToString();
                        bool isPrimary = Boolean.Parse(rdr["IsPrimary"].ToString());
                        MobileCategoriesDL categorie = new MobileCategoriesDL(catID, catName, categoryUrlName, areaUrlName, areaDisplayName, cityUrlName, cityDisplayName, iconPath, isPrimary);
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

        public static MobileCategoriesDL GetByCategoryIdCityIdAreaId(int categoryId, int cityId, int areaId)
        {
            MobileCategoriesDL category = null;
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            string queryString = "SELECT CityInfo.UrlName AS CityUrlName, CategoryInfo.CategoryID, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName, " +
                        "AreaInfo.UrlName AS AreaUrlName, Info.Website AS InfoWebsite, CategoryArea.IsPrimary, Cityinfo.DisplayName AS CityDisplayName, AreaInfo.DisplayName AS AreaDisplayName " +
                        "FROM CategoryArea INNER JOIN CategoryInfo ON CategoryArea.CategoryID = CategoryInfo.CategoryID INNER JOIN " +
                        "AreaInfo ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CityInfo ON AreaInfo.CityID = CityInfo.CityID " +
                        "INNER JOIN Info ON CategoryArea.CategoryID = Info.ReferenceID AND Info.InfoType = 1 " +
                        "WHERE (CategoryArea.AreaID = @AREAID) AND (AreaInfo.CityID = @CITYID) AND (CategoryInfo.CategoryID = @CATEGORYID) ORDER BY CategoryInfo.DisplayName";
            
            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("@CATEGORYID", SqlDbType.Int).Value = categoryId;
            command.Parameters.Add("@CITYID", SqlDbType.Int).Value = cityId;
            command.Parameters.Add("@AREAID", SqlDbType.Int).Value = areaId;

            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                rdr = command.ExecuteReader();
                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        int catID = Convert.ToInt32(rdr["CategoryID"]);
                        string catName = rdr["CatName"].ToString();
                        string categoryUrlName = rdr["CatUrlName"].ToString();
                        string areaUrlName = rdr["AreaUrlName"].ToString();
                        string areaDisplayName = rdr["AreaDisplayName"].ToString();
                        string cityUrlName = rdr["CityUrlName"].ToString();
                        string cityDisplayName = rdr["CityDisplayName"].ToString();
                        string iconPath = rdr["InfoWebsite"].ToString();
                        bool isPrimary = Boolean.Parse(rdr["IsPrimary"].ToString());
                        MobileCategoriesDL categorie = new MobileCategoriesDL(catID, catName, categoryUrlName, areaUrlName, areaDisplayName, cityUrlName, cityDisplayName, iconPath, isPrimary);
                        category = categorie;
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

            return category;
        }

    }
}
