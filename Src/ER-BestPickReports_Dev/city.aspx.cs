using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace ER_BestPickReports_Dev
{
    public partial class city : BasePage
    {
        string cityid = "";
        string strcity = "";
        string basedomain = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get value from route handler
            cityid = (HttpContext.Current.Items["cityid"].ToString() == "0") ? "" : HttpContext.Current.Items["cityid"].ToString();
            strcity = (HttpContext.Current.Items["city"].ToString() == "") ? "" : HttpContext.Current.Items["city"].ToString();

            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            if (cityid != "")
            {
                string sql = "SELECT AreaInfo.UrlName AS AreaUrlName FROM CityInfo, AreaInfo WHERE (CityInfo.CityID = AreaInfo.CityID) AND (AreaInfo.CityID = @CITYID) AND (AreaInfo.IsPrimary = 1)";
                object o = DataAccessHelper.Data.ExecuteScalar(sql,
                    new SqlParameter("@CITYID", cityid),
                    new SqlParameter("@DATE", DateTime.Today));

                if (o != null)
                    Response.Redirect(basedomain + "/" + strcity + "/" + o.ToString());
                else
                    Response.Redirect(basedomain + "/error.aspx");
            }
            else
                Response.Redirect(basedomain + "/error.aspx");
        }
    }
}