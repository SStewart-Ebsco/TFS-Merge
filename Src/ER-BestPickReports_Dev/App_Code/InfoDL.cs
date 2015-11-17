using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev.App_Code
{
    public class InfoDL
    {
        public static int FindID(InfoType infoType, int objectID)
        {
            int newID = 0;

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);

            string strSQL = "";
            switch (infoType)
            {
                case InfoType.CategoryCity:
                    strSQL = @"SELECT C.CategoryID FROM CategoryCity CC JOIN Category C ON (C.CategoryID = CC.CategoryID) WHERE CC.CategoryCityID = @ID";
                    break;
                case InfoType.CategoryArea:
                    strSQL = @"SELECT CC.CategoryCityID 
                                FROM CategoryArea CA
                                    JOIN CategoryCity CC ON (CA.CategoryID = CC.CategoryID)
                                    JOIN Area A ON (CA.AreaID = A.AreaID AND CC.CityID = A.CityID)
                                WHERE CA.CategoryAreaID = @ID";
                    break;
                case InfoType.ContractorCategoryCity:
                    strSQL = @"SELECT CC.ContractorCategoryID 
                                FROM ContractorCategoryCity CCC
                                    JOIN ContractorCategory CC ON (CCC.ContractorCategoryID = CC.ContractorCategoryID)
                                WHERE CCC.ContractorCategoryCityID = @ID";
                    break;
                case InfoType.ContractorCategoryArea:
                    strSQL = @"SELECT CCC.contractorCategoryCityID 
                                FROM ContractorCategoryArea CCA
                                    JOIN ContractorCategoryCity CCC ON  (CCA.ContractorCategoryID = CCC.ContractorCategoryID)
                                    JOIN Area A ON (CCA.AreaID = A.AreaID AND (CCC.CityID = A.CityID) ) 
                                WHERE (CCA.ContractorCategoryAreaID = @ID)";
                    break;
            }

            try
            {
                conn.Open();

                SqlCommand sqlCommand = new SqlCommand(strSQL, conn);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.Add("ID", SqlDbType.Int).Value = objectID;
                newID = Convert.ToInt32(sqlCommand.ExecuteScalar());
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

            return newID;
        }

        public static CookiesInfo GetCookiesInfoByZipCode(string zipCode)
        {
            CookiesInfo cookiesInfo = new CookiesInfo();

            string queryString = "SELECT AreaInfo.AreaID AS AreaID, AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaDisplayName, CityInfo.CityID AS CityID, CityInfo.UrlName AS CityUrlName, CityInfo.DisplayName AS CityDisplayName FROM Zip, CityInfo, AreaInfo WHERE " +
                                "(AreaInfo.AreaID = Zip.AreaID) AND (CityInfo.CityID = AreaInfo.CityID) AND " +
                                "(Zip.ZipCode = @ZIP)";

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);

            SqlCommand sqlCommand = new SqlCommand(queryString, conn);
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Parameters.Add("ZIP", SqlDbType.Int).Value = zipCode;

            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                rdr = sqlCommand.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.Read();
                    cookiesInfo = new CookiesInfo(rdr["CityID"].ToString(),
                                                  rdr["CityUrlName"].ToString(),
                                                  rdr["CityDisplayName"].ToString(),
                                                  rdr["AreaID"].ToString(),
                                                  rdr["AreaUrlName"].ToString(),
                                                  rdr["AreaDisplayName"].ToString());
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

            return cookiesInfo;
        }
    }
}