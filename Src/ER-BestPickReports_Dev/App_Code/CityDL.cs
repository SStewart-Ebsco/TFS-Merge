using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace ER_BestPickReports_Dev.App_Code
{
    public class CityDL
    {
        public static int GetIDByName(string cityName)
        {
            int cID = 0;
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr = null;

            string queryString = @"SELECT cityID FROM CityInfo WHERE displayName = @cityName";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("cityName", SqlDbType.VarChar).Value = cityName;

            try
            {
                conn.Open();
                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        cID = Convert.ToInt32(rdr["cityID"]);
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
            return cID;
        }

        public static int GetIDByUrl(string cityURL)
        {
            int cID = 0;
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr = null;

            string queryString = @"SELECT cityID FROM CityInfo WHERE urlName = @urlName";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("urlName", SqlDbType.VarChar).Value = cityURL;

            try
            {
                conn.Open();
                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        cID = Convert.ToInt32(rdr["cityID"]);
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
            return cID;
        }

        public static string GetCityUrlByID(int cityID)
        {
            string cityUrl = "";

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);

            string queryString = @"SELECT UrlName FROM CityInfo WHERE CityID = @cityID";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("cityID", SqlDbType.Int).Value = cityID;

            try
            {
                conn.Open();
                cityUrl = command.ExecuteScalar().ToString();
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
            return cityUrl;
        }

        public static string GetMobileYear(int cID)
        {
            string mobileYear = "";
            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);

            string queryString = @"SELECT MobileYear FROM CityInfo WHERE CityID = @CityID";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("CityID", SqlDbType.Int).Value = cID;

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
    }
}