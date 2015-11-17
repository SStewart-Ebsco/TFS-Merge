using System;
using System.Data;
using System.Data.SqlClient;
using ER_BestPickReports_Dev.Helpers;

namespace PinnexLib.Data
{
    public class DataAccess
    {
        private string _ConnString = "";

        public DataAccess(string ConnString)
        {
            _ConnString = ConnString;
        }

        public SqlDataReader ExecuteDatareader(SqlConnection conn, string sql, params SqlParameter[] parameters)
        {
            SqlDataReader rdr = null;

            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (parameters != null)
                {
                    for (int idx = 0; idx < parameters.Length; idx++)
                        cmd.Parameters.Add(parameters[idx]);
                }
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return rdr;
            }
            catch (Exception exc)
            {
                throw new Exception("Error executing SQL: " + sql, exc);
            }
        }


        public void ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_ConnString))
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    if (parameters != null)
                    {
                        for (int idx = 0; idx < parameters.Length; idx++)
                            cmd.Parameters.Add(parameters[idx]);
                    }
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new Exception("Error executing SQL: " + sql, exc);
            }
        }


        public void ExecuteNonQuery(SqlConnection conn, string sql, params SqlParameter[] parameters)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (parameters != null)
                {
                    for (int idx = 0; idx < parameters.Length; idx++)
                        cmd.Parameters.Add(parameters[idx]);
                }

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                throw new Exception("Error executing SQL: " + sql, exc);
            }
        }


        public object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            object returnValue = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(_ConnString))
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    if (parameters != null)
                    {
                        for (int idx = 0; idx < parameters.Length; idx++)
                            cmd.Parameters.Add(parameters[idx]);
                    }
                    conn.Open();
                    returnValue = cmd.ExecuteScalar();
                }

                return returnValue;
            }
            catch (Exception exc)
            {
                throw new Exception("Error executing SQL: " + sql, exc);
            }
        }


        public object ExecuteScalarConnection(string sql, string connString, params SqlParameter[] parameters)
        {
            try
            {
                object returnValue = null;
                using (SqlConnection conn = new SqlConnection(_ConnString))
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    if (parameters != null)
                    {
                        for (int idx = 0; idx < parameters.Length; idx++)
                            cmd.Parameters.Add(parameters[idx]);
                    }
                    conn.Open();
                    returnValue = cmd.ExecuteScalar();
                }

                return returnValue;
            }
            catch (Exception exc)
            {
                throw new Exception("Error executing SQL: " + sql, exc);
            }
        }


		public DataSet ExecuteDataset(string sql, params SqlParameter[] parameters)
		{
			DataSet dataset = new DataSet();

			try
			{
				using (SqlConnection conn = new SqlConnection(_ConnString))
				{
					conn.Open();

					SqlCommand command = new SqlCommand(sql, conn);
					if (parameters != null)
					{
						foreach (SqlParameter parameter in parameters)
							command.Parameters.Add(parameter);
					}

					SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
					sqlDataAdapter.Fill(dataset);
				}

				return dataset;
			}
			catch (Exception exc)
			{
				throw new Exception("Error executing SQL: " + sql, exc);
			}
		}


		public DataTable ExecuteDatasetWithOneTable(string sqlQuery, params SqlParameter[] parameters)
		{
			DataSet dataSet = ExecuteDataset(sqlQuery, parameters);
			return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : null;
		}
	}
}
