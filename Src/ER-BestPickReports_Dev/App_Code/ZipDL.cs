using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ER_BestPickReports_Dev.App_Code
{
    public class ZipDL
    {
        public static string GetZipByAreaId(string areaId)
        {
            string zipCode;

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);

            string queryString = @"SELECT ZipCode FROM Zip WHERE AreaID = @areaID";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("areaID", SqlDbType.Int).Value = areaId;

            try
            {
                conn.Open();
                zipCode = command.ExecuteScalar().ToString();
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
            return zipCode;
        }
    }
}