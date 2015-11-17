using System;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev.blogfiles
{
    public partial class login : BasePage
    {
        private AppCookies bprPreferences = AppCookies.CreateInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            txtUser.Focus();
        }

        protected void bnLogin_Click(object sender, EventArgs e)
        {
            //check and see if the login exists

            //Clear any previous login Sessions
            HttpContext.Current.Session["BPRLogin"] = null;

            SqlParameter SqlUSERID = new SqlParameter("@USERID", SqlDbType.VarChar, 50, ParameterDirection.Input,
                false, 0, 0, "USERID", DataRowVersion.Default, txtUser.Text);
            SqlParameter SqlPASSWORD = new SqlParameter("@PASSWORD", SqlDbType.VarChar, 50, ParameterDirection.Input,
                false, 0, 0, "PASSWORD", DataRowVersion.Default, txtPassword.Text);

            DataSet dsTemp = DataAccessHelper.Data.ExecuteDataset("SELECT * FROM LOGIN WHERE Username=@USERID AND Password=@PASSWORD",
                SqlUSERID, SqlPASSWORD);

            if (dsTemp != null)
            {
                if (dsTemp.Tables[0].Rows.Count >= 1)
                {
                    //SET Session Variable
                    LoggedIn = true;
                    lblLoginError.Text = "";
                    lblLoginError.Visible = false;
                    Response.Redirect("/articleListing");

                }
                else
                {
                    LoggedIn = false;
                    //Then return back to the login page with error
                    txtPassword.Text = "";
                    lblLoginError.Text = "Username or Password is incorrect.";
                    lblLoginError.Visible = true;
                    //log incorrect login attempt
                    //after 3 lock the account
                }
            }
        }
    }
}