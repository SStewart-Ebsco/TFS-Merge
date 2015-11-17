using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using ER_BestPickReports_Dev.App_Code;

namespace ER_BestPickReports_Dev
{
    public partial class BWCMonitoring : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                litDetails.Text += "<ol><li>Checking for Monitoring Setup.....</li>";
                //Check if table exists
                if (DoesMonitoringTableExist())
                {
                    litDetails.Text += "<ul><li>Table Exists --- <span style='color:green'>SUCCESS</span></li></ul>";

                    //select row count from log
                    litDetails.Text += "<li>Checking log......</li>";
                    int rowCount = GetLogRowCount();
                    string successMessage = "<span style='color:green'>SUCCESS</span>";
                    if (rowCount == 0)
                        successMessage = "<span style='color:red'>FAILURE</span>";
                    litDetails.Text += "<ul><li>Row Count: " + rowCount.ToString() + " --- " + successMessage + "</li></ul>";

                    //insert new log entry
                    litDetails.Text += "<li>Inserting New Entry......</li>";
                    string updateErrors = "";
                    if (InsertUpdateEntry(out updateErrors))
                        litDetails.Text += "<ul><li>Entry Complete --- <span style='color:green'>SUCCESS</span></li></ul>";
                    else
                        litDetails.Text += "<ul><li>Entry Not Completed --- <span style='color:red'>ERROR: </span>" + updateErrors + "</li></ul>";
                }
                else    //If tables does NOT exist
                {
                    litDetails.Text += "<ul><li>Monitoring NOT setup</li></ul>";
                    string creationErrors = "";
                    litDetails.Text += "<li>Creating Monitoring Setup.....</li>";
                    //Create Table
                    if (CreateMonitoringTable(out creationErrors))
                    {
                        litDetails.Text += "<ul><li>Table Created --- <span style='color:green'>SUCCESS</span></li></ul>";

                        //write first entry for creation
                        litDetails.Text += "<li>Inserting initial entry.....</li>";
                        string insertionErrors = "";
                        if (InsertCreationEntry(out insertionErrors))
                            litDetails.Text += "<ul><li>Entry Created --- <span style='color:green'>SUCCESS</span></li></ul>";
                        else
                            litDetails.Text += "<ul><li>Entry Created --- <span style='color:red'>ERROR: </span>" + insertionErrors + "</li></ul>";
                    }
                    else
                        litDetails.Text += "<ul><li>Table Not Created --- <span style='color:red'>ERROR: </span>" + creationErrors + "</li></ul>";
                }

                litDetails.Text += "<li>DONE</li></ol>";
            }
        }

        private bool DoesMonitoringTableExist()
        {
            bool tableExists = true;
            using (SqlConnection sqlConn = new SqlConnection(BWConfig.ConnectionString))
            {
                string strCommand = "SELECT COUNT(*) FROM BWCMonitoringLog";
                SqlCommand sqlCommand = new SqlCommand(strCommand, sqlConn);
                sqlCommand.CommandType = System.Data.CommandType.Text;

                try
                {
                    sqlConn.Open();

                    int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
                }
                catch (SqlException)
                {
                    tableExists = false;
                }
                catch (Exception ex)
                {
                    EventLogHelper.WriteError("Error when checking if BWC Monitoring table exists: " + ex.Message);
                }

            }
            return tableExists;
        }

        private bool CreateMonitoringTable(out string errorMessages)
        {
            errorMessages = "";
            bool creationSuccess = true;

            using (SqlConnection sqlConn = new SqlConnection(BWConfig.ConnectionString))
            {
                string strCommand = @"CREATE TABLE BWCMonitoringLog(
	                                rowID INT IDENTITY NOT NULL PRIMARY KEY,
	                                logDate DATETIME NOT NULL DEFAULT(GetDate()),
	                                userName NVARCHAR(100),
	                                logDescription NVARCHAR(500),
                                    applicationName NVARCHAR(100)
                                )";
                SqlCommand sqlCommand = new SqlCommand(strCommand, sqlConn);
                sqlCommand.CommandType = System.Data.CommandType.Text;

                try
                {
                    sqlConn.Open();

                    sqlCommand.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                    creationSuccess = false;
                    errorMessages += "SQL Problem creating: " + sqlEx.Message;
                    //write to log file
                    EventLogHelper.WriteError("SQL Problem creating BWC Monitoring Table: " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    creationSuccess = false;
                    errorMessages += "Problem creating: " + ex.Message;
                    //write to log file
                    EventLogHelper.WriteError("Problem creating BWC Monitoring Table: " + ex.Message);
                }

            }
            return creationSuccess;
        }

        private int GetLogRowCount()
        {
            int rowCount = 0;
            using (SqlConnection sqlConn = new SqlConnection(BWConfig.ConnectionString))
            {
                string strCommand = "SELECT COUNT(*) FROM BWCMonitoringLog";
                SqlCommand sqlCommand = new SqlCommand(strCommand, sqlConn);
                sqlCommand.CommandType = System.Data.CommandType.Text;

                try
                {
                    sqlConn.Open();

                    rowCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
                }
                catch (SqlException sqlEx)
                {
                    //write to event log
                    EventLogHelper.WriteError("Error fetching BWC Monitoring Row Count: " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    //write to event log
                    EventLogHelper.WriteError("Error fetching BWC Monitoring Row Count: " + ex.Message);

                }

            }
            return rowCount;
        }

        private bool InsertCreationEntry(out string errors)
        {
            errors = "";
            bool sucess = false;
            using (SqlConnection sqlConn = new SqlConnection(BWConfig.ConnectionString))
            {
                string strCommand = "INSERT INTO BWCMonitoringLog (userName, logDescription, applicationName) VALUES (@userName, @description, @applicationName)";

                SqlCommand sqlCommand = new SqlCommand(strCommand, sqlConn);
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.Parameters.Add("userName", SqlDbType.NVarChar).Value = "Page Call Back";
                sqlCommand.Parameters.Add("description", SqlDbType.NVarChar).Value = "Created Monitoring Log";
                sqlCommand.Parameters.Add("applicationName", SqlDbType.NVarChar).Value = EventLogHelper.applicationName;

                try
                {
                    sqlConn.Open();
                    int rowCount = Convert.ToInt32(sqlCommand.ExecuteNonQuery());
                    if (rowCount == 1)
                        sucess = true;
                }
                catch (SqlException sqlEx)
                {
                    sucess = false;
                    errors = "SQL Problem Inserting Creation Entry: " + sqlEx.Message;
                    //write to event log
                }
                catch (Exception ex)
                {
                    sucess = false;
                    errors = "Problem Inserting Creation Entry: " + ex.Message;
                    //write to event log
                }

            }
            return sucess;
        }

        private bool InsertUpdateEntry(out string errors)
        {
            errors = "";
            bool sucess = false;
            using (SqlConnection sqlConn = new SqlConnection(BWConfig.ConnectionString))
            {
                string strCommand = "INSERT INTO BWCMonitoringLog (userName, logDescription, applicationName) VALUES (@userName, @description, @applicationName)";

                SqlCommand sqlCommand = new SqlCommand(strCommand, sqlConn);
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.Parameters.Add("userName", SqlDbType.NVarChar).Value = "Page Call Back";
                sqlCommand.Parameters.Add("description", SqlDbType.NVarChar).Value = "Checking Monitoring";
                sqlCommand.Parameters.Add("applicationName", SqlDbType.NVarChar).Value = EventLogHelper.applicationName;

                try
                {
                    sqlConn.Open();
                    int rowCount = Convert.ToInt32(sqlCommand.ExecuteNonQuery());
                    if (rowCount == 1)
                        sucess = true;
                }
                catch (SqlException sqlEx)
                {
                    sucess = false;
                    errors = "SQL Problem Inserting Creation Entry: " + sqlEx.Message;
                    //write to event log
                }
                catch (Exception ex)
                {
                    sucess = false;
                    errors = "Problem Inserting Creation Entry: " + ex.Message;
                    //write to event log
                }

            }
            return sucess;
        }

        protected void TestEventLog(object sender, EventArgs e)
        {
            EventLogHelper.WriteError("Testing the event log. Is N-Able catching? This is a test, please disregard this error.");
            int totalCount = Convert.ToInt32(testCount.Text);
            testCount.Text = (totalCount + 1).ToString();
        }
    }
}