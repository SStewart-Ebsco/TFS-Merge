using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace ER_BestPickReports_Dev.App_Code
{
    public class AreaDL
    {
        private int _ID;
        private string _name;

        public string name
        { get { return _name; } }

        public AreaDL(int aID, string aName)
        {
            _ID = aID;
            _name = aName;
        }

        public static List<AreaDL> GetByContractorIDCityIDCategoryID(int contractorID, int cityID, int categoryID)
        {
            List<AreaDL> areaList = new List<AreaDL>();

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr = null;

            string queryString = @"SELECT DISTINCT A.AreaID, A.DisplayName
                                    FROM  ContractorInfo CI 
	                                    JOIN ContractorCategory CC ON (CC.ContractorID = CI.ContractorID)
	                                    JOIN ContractorCategoryArea CCA ON (CCA.ContractorCategoryID = CC.ContractorCategoryID)
	                                    JOIN AreaInfo A ON (A.AreaID = CCA.AreaID)
	                                    JOIN CityInfo CIT ON (A.CityID = CIT.CityID)
                                    WHERE CIT.CityID = @cityID AND CI.ContractorID = @contractorID AND CC.CategoryID = @categoryID";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("cityID", SqlDbType.Int).Value = cityID;
            command.Parameters.Add("contractorID", SqlDbType.Int).Value = contractorID;
            command.Parameters.Add("categoryID", SqlDbType.Int).Value = categoryID;

            try
            {
                conn.Open();

                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        int aID = Convert.ToInt32(rdr["areaID"]);
                        string aName = rdr["DisplayName"].ToString();
                        AreaDL a = new AreaDL(aID, aName);
                        areaList.Add(a);
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

            return areaList;
        }

        public static string GetMobileYear(int aID)
        {
            string mobileYear = "";
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);

            string queryString = @"SELECT CI.MobileYear 
                                    FROM CityInfo CI  
                                        JOIN Area A ON (CI.CityID = A.CityID)
                                    WHERE A.AreaID = @areaID";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("areaID", SqlDbType.Int).Value = aID;

            try
            {
                conn.Open();
                mobileYear = command.ExecuteScalar() == null ? "" : command.ExecuteScalar().ToString();
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
            return mobileYear;
        }

        public static bool IsPrimary(InfoType infoType, int objectID)
        {
            bool isPrimary = false;

            if (infoType == InfoType.CategoryArea)
                isPrimary = IsPrimary_Category(objectID);
            else if (infoType == InfoType.ContractorCategoryArea)
                isPrimary = IsPrimary_CategoryContractor(objectID);

            return isPrimary;
        }

        public static string GetAreaUrlByID(int areaID)
        {
            string areaUrl = "";

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);

            string queryString = @"SELECT UrlName FROM AreaInfo WHERE AreaID = @areaID";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("areaID", SqlDbType.Int).Value = areaID;

            try
            {
                conn.Open();
                areaUrl = command.ExecuteScalar().ToString();
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
            return areaUrl;
        }

        private static bool IsPrimary_Category(int objectID)
        {
            bool isPrimary = false;

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);

            try
            {
                conn.Open();

                string strSQL = "SELECT isprimary FROM CategoryArea WHERE categoryareaid = @ID";

                SqlCommand sqlCommand = new SqlCommand(strSQL, conn);
                sqlCommand.Parameters.Add("ID", SqlDbType.Int).Value = objectID;

                isPrimary = Convert.ToBoolean(sqlCommand.ExecuteScalar());
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }

            return isPrimary;
        }

        private static bool IsPrimary_CategoryContractor(int objectID)
        {
            bool isPrimary = false;

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);

            try
            {
                conn.Open();

                string strSQL = "SELECT isprimary FROM ContractorCategoryArea WHERE contractorcategoryareaid = @ID";

                SqlCommand sqlCommand = new SqlCommand(strSQL, conn);
                sqlCommand.Parameters.Add("ID", SqlDbType.Int).Value = objectID;

                isPrimary = Convert.ToBoolean(sqlCommand.ExecuteScalar());
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }

            return isPrimary;
        }

        public static int GetAreaIdByUrl(string url)
        {
            int areaId = 0;
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr = null;

            string queryString = @"SELECT AreaID FROM AreaInfo WHERE UrlName = @url";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("url", SqlDbType.VarChar).Value = url;

            try
            {
                conn.Open();
                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        areaId = Convert.ToInt32(rdr["AreaID"]);
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
            return areaId;
        }

        public static int GetAreaIdByCityId(string cityId)
        {
            int areaId = 0;
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr;

            string queryString = @"SELECT AreaID FROM Area WHERE CityID = @cityId AND IsPrimary = 1";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("cityId", SqlDbType.Int).Value = cityId;

            try
            {
                conn.Open();
                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        areaId = Convert.ToInt32(rdr["AreaID"]);
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
            return areaId;
        }

        public static bool IsAreaFromCity(int areaId, int cityId)
        {
            bool isAreaFromCity = false;

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr;

            string queryString = @"SELECT COUNT(AreaID) FROM Area WHERE AreaID = @areaId AND CityID = @cityId";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("areaId", SqlDbType.Int).Value = areaId;
            command.Parameters.Add("cityId", SqlDbType.Int).Value = cityId;

            try
            {
                conn.Open();
                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        int result  = Convert.ToInt32(rdr[0]);
                        isAreaFromCity = result == 1;
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

            return isAreaFromCity;
        }
    }
}