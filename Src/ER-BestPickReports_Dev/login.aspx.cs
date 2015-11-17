using System;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using ER_BestPickReports_Dev.Helpers;

namespace ER_BestPickReports_Dev
{
    public partial class login : System.Web.UI.Page
    {
        private readonly DataAccessHelper _dataAccessHelper = new DataAccessHelper();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if we are on the live site
            if(!BWConfig.IsPreStaging())
                Response.Redirect("http://www.bestpickreports.com");

            txtUser.Focus();
        }

        protected void bnLogin_Click(object sender, EventArgs e)
        {
            //check and see if the login exists
            BasePage bp = new BasePage();

            //Clear any previous login Sessions
            HttpContext.Current.Session["BPRDevLogin"] = null;

            SqlParameter SqlUSERID = new SqlParameter("@USERID", SqlDbType.VarChar, 50, ParameterDirection.Input,
                false, 0, 0, "USERID", DataRowVersion.Default, txtUser.Text);
            SqlParameter SqlPASSWORD = new SqlParameter("@PASSWORD", SqlDbType.VarChar, 50, ParameterDirection.Input,
                false, 0, 0, "PASSWORD", DataRowVersion.Default, txtPassword.Text);

            DataSet dsTemp = _dataAccessHelper.Data.ExecuteDataset("SELECT * FROM LOGIN WHERE Username=@USERID AND Password=@PASSWORD",
                SqlUSERID, SqlPASSWORD);

            if (dsTemp != null)
            {
                if (dsTemp.Tables[0].Rows.Count >= 1)
                {
                    //SET Session Variable
                    HttpContext.Current.Session["BPRDevLogin"] = true;
                    HttpContext.Current.Session["BPRDevReferer"] = txtRefer.Text.ToString().Trim();
                    lblLoginError.Visible = false;
                    Response.Redirect("/default.aspx");

                }
                else
                {
                    HttpContext.Current.Session["BPRDevLogin"] = false;
                    HttpContext.Current.Session["BPRDevReferer"] = "";
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
