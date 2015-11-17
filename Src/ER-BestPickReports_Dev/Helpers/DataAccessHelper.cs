using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ER_BestPickReports_Dev.App_Code;
using PinnexLib.Data;

namespace ER_BestPickReports_Dev.Helpers
{
    public class DataAccessHelper
    {
        private DataAccess _dataAccess = null;

        public string ConnString
        {
            get { return ConfigurationManager.ConnectionStrings["ConnString"].ToString(); }
        }

        public DataAccess Data
        {
            get
            {
                if (_dataAccess == null)
                {
                    _dataAccess = new DataAccess(ConnString);
                }

                return _dataAccess;
            }
        }

        /// <summary>
        /// Attempts to find the info record associated with the given InfoType and object ID.  The first available match will
        /// be used... area, city, global
        /// </summary>
        /// <returns>
        /// Returns the Info table's DataRow object if an Info record is found
        /// </returns>
        public DataRow FindInfoRecord(InfoType infoType, int objectID, ref InfoLevel infoLevel, ref bool isPrimary)
        {
            InfoType n = infoType;
            string field = "";
            string sql = "";

            // See if this is the primary area
            if (n == InfoType.CategoryArea)
            {
                sql = "SELECT isprimary FROM CategoryArea WHERE (categoryareaid = @ID)";
                object o = Data.ExecuteScalar(sql, new SqlParameter("@ID", objectID));
                if (o != null && o.ToString() != "")
                    isPrimary = (o.ToString().ToLower() == "true");
            }
            else if (n == InfoType.ContractorCategoryArea)
            {
                sql = "SELECT isprimary FROM ContractorCategoryArea WHERE (contractorcategoryareaid = @ID)";
                object o = Data.ExecuteScalar(sql, new SqlParameter("@ID", objectID));
                if (o != null && o.ToString() != "")
                    isPrimary = (o.ToString().ToLower() == "true");
            }

            // Initialize table and field values
            SetField(infoType, ref field);
            while (n != InfoType.None)
            {
                sql = "SELECT * FROM Info, " + n.ToString() + " WHERE (" + field + " = @ID) AND " +
                    "(Info.InfoID = " + n.ToString() + ".InfoID)";
                // There will be no Info record if this is an add
                DataTable dt = Data.ExecuteDataset(sql, new SqlParameter("@ID", objectID)).Tables[0];
                if (dt.Rows.Count == 1)
                    return dt.Rows[0];

                n = RevertInfoType(n, ref objectID, ref infoLevel, ref field);
            }

            return null;
        }

        /// <summary>
        /// Sets the table and field based on the InfoType
        /// </summary>
        private void SetField(InfoType t, ref string field)
        {
            switch (t)
            {
                case InfoType.Category:
                    field = "CategoryID";
                    break;
                case InfoType.CategoryCity:
                    field = "CategoryCityID";
                    break;
                case InfoType.CategoryArea:
                    field = "CategoryAreaID";
                    break;
                case InfoType.ContractorCategory:
                    field = "ContractorCategoryID";
                    break;
                case InfoType.ContractorCategoryCity:
                    field = "ContractorCategoryCityID";
                    break;
                case InfoType.ContractorCategoryArea:
                    field = "ContractorCategoryAreaID";
                    break;
            }
        }

        /// <summary>
        /// Revert the specified InfoType to a higher level and adjust table and field accordingly
        /// </summary>
        private InfoType RevertInfoType(InfoType t, ref int refID, ref InfoLevel infoLevel, ref string field)
        {
            InfoType n = t;

            switch (t)
            {
                case InfoType.Category:
                    n = InfoType.None;
                    break;
                case InfoType.CategoryCity:
                    n = InfoType.Category;
                    infoLevel = InfoLevel.Global;
                    refID = FindRevertedRefID(t, refID);
                    field = "CategoryID";
                    break;
                case InfoType.CategoryArea:
                    n = InfoType.CategoryCity;
                    infoLevel = InfoLevel.City;
                    refID = FindRevertedRefID(t, refID);
                    field = "CategoryCityID";
                    break;

                case InfoType.ContractorCategory:
                    n = InfoType.None;
                    break;
                case InfoType.ContractorCategoryCity:
                    n = InfoType.ContractorCategory;
                    infoLevel = InfoLevel.Global;
                    refID = FindRevertedRefID(t, refID);
                    field = "ContractorCategoryID";
                    break;
                case InfoType.ContractorCategoryArea:
                    n = InfoType.ContractorCategoryCity;
                    infoLevel = InfoLevel.City;
                    refID = FindRevertedRefID(t, refID);
                    field = "ContractorCategoryCityID";
                    break;
            }

            return n;
        }

        /// <summary>
        /// Finds the reference id of the object one level up from the specified InfoType
        /// </summary>
        private int FindRevertedRefID(InfoType t, int currentRefID)
        {
            string sql = "";
            int refid = 0;

            switch (t)
            {
                case InfoType.CategoryCity:
                    sql = "SELECT Category.CategoryID FROM CategoryCity, Category WHERE (CategoryCity.CategoryCityID = @ID) AND " +
                        "(Category.CategoryID = CategoryCity.CategoryID)";
                    break;
                case InfoType.CategoryArea:
                    sql = "SELECT CategoryCity.CategoryCityID FROM CategoryArea, CategoryCity, Area WHERE (CategoryArea.CategoryAreaID = @ID) AND " +
                        "(Area.AreaID = CategoryArea.AreaID) AND (CategoryCity.CityID = Area.CityID) AND " +
                        "(CategoryCity.CategoryID = CategoryArea.CategoryID)";
                    break;
                case InfoType.ContractorCategoryCity:
                    sql = "SELECT ContractorCategory.ContractorCategoryID FROM ContractorCategoryCity, ContractorCategory WHERE (ContractorCategoryCity.ContractorCategoryCityID = @ID) AND " +
                        "(ContractorCategory.ContractorCategoryID = ContractorCategoryCity.ContractorCategoryID)";
                    break;
                case InfoType.ContractorCategoryArea:
                    sql = "SELECT ContractorCategoryCity.* FROM ContractorCategoryArea, ContractorCategoryCity, Area WHERE (ContractorCategoryArea.ContractorCategoryAreaID = @ID) AND " +
                        "(Area.AreaID = ContractorCategoryArea.AreaID) AND (ContractorCategoryCity.CityID = Area.CityID) AND " +
                        "(ContractorCategoryCity.ContractorCategoryID = ContractorCategoryArea.ContractorCategoryID)";
                    break;
            }

            object o = Data.ExecuteScalar(sql, new SqlParameter("@ID", currentRefID));
            if (o != null)
                refid = int.Parse(o.ToString());

            return refid;
        }

        public string GetCityUrlFromID(string cityid)
        {
            string cityurl = "";

            string sql = "SELECT UrlName FROM CityInfo WHERE CityID = @CITYID";
            object o = Data.ExecuteScalar(sql, new SqlParameter("@CITYID", cityid));
            if (o != null)
                cityurl = o.ToString();

            return cityurl;
        }

        public string GetAreaUrlFromID(string areaid)
        {
            string areaurl = "";

            string sql = "SELECT UrlName FROM AreaInfo WHERE AreaID = @AREAID";
            object o = Data.ExecuteScalar(sql, new SqlParameter("@AREAID", areaid));
            if (o != null)
                areaurl = o.ToString();

            return areaurl;
        }

        public string GetCityNameFromID(string cityid)
        {
            string cityname = "";

            string sql = "SELECT DisplayName FROM CityInfo WHERE CityID = @CITYID";
            object o = Data.ExecuteScalar(sql, new SqlParameter("@CITYID", cityid));
            if (o != null)
                cityname = o.ToString();

            return cityname;
        }

        public string GetAreaNameFromID(string areaid, string cityid)
        {
            string areaname = "";

            string sql = "SELECT DisplayName FROM AreaInfo WHERE AreaID = @AREAID AND CityID = @CITYID";
            object o = Data.ExecuteScalar(sql,
                new SqlParameter("@CITYID", cityid),
                new SqlParameter("@AREAID", areaid));
            if (o != null)
                areaname = o.ToString();

            return areaname;
        }

        public string GetCategoryUrlFromID(string catid)
        {
            string caturl = "";

            string sql = "SELECT UrlName FROM CategoryInfo WHERE CategoryID = @CATID";
            object o = Data.ExecuteScalar(sql,
                new SqlParameter("@CATID", catid));
            if (o != null)
                caturl = o.ToString();

            return caturl;
        }

        public string GetCategoryNameFromID(string catid)
        {
            string catname = "";

            string sql = "SELECT BlogCatName FROM BlogCategories WHERE BlogCatID = @CATID";
            object o = Data.ExecuteScalar(sql, new SqlParameter("@CATID", catid));
            if (o != null)
            {
                catname = o.ToString();
            }

            return catname;
        }

        /// <summary>
        /// Get top categories by Area ID 
        /// </summary>
        /// <param name="areaId">Area ID</param>
        /// <param name="limitNumber">Limit number of returned categories</param>
        /// <returns></returns>
        public DataTable GetTopCategories(string areaId, int limitNumber)
        {
            string sql = String.Format(@"SELECT TOP {0} CI.CategoryID, CI.DisplayName AS CategoryName, CI.UrlName
							FROM CategoryArea CA
								JOIN Area A ON (CA.AreaID = A.AreaID)
								JOIN CategoryInfo CI ON (CA.CategoryID = CI.CategoryID)
							WHERE A.AreaID = @areaId AND isTopCategory = 1
							ORDER BY CI.DisplayName", limitNumber);
            DataSet topCategoriesSet = Data.ExecuteDataset(sql, new SqlParameter("@AREAID", areaId));

            return topCategoriesSet.Tables[0];
        }
    }
}