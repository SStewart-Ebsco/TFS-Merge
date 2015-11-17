using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using ER_BestPickReports_Dev;

namespace EBSCOResearch
{
    public partial class searchredirect : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string catid = (Request["CategoryID"] == "" || Request["CategoryID"] == null) ? "" : Request["CategoryID"].ToString();
            string contractorid = (Request["ContractorID"] == "" || Request["ContractorID"] == null) ? "" : Request["ContractorID"].ToString();
            string areaid = (Request["AreaID"] == "" || Request["AreaID"] == null) ? "" : Request["AreaID"].ToString();
            string cityid = (Request["CityID"] == "" || Request["CityID"] == null) ? "" : Request["CityID"].ToString();

            if (catid != "" && contractorid != "" && areaid != "" && cityid != "")
            {
                //Get combination areaid/cityid/categoryid/contractorid
                string sql = "SELECT CityInfo.CityID, AreaInfo.AreaID, ContractorInfo.UrlName AS ContractorURLName, CategoryInfo.UrlName AS CategoryURLName, " +
                    "CityInfo.DisplayName AS CityDisplayName, CityInfo.UrlName AS CityURLName, AreaInfo.DisplayName AS AreaDisplayName, AreaInfo.UrlName AS AreaURLName FROM ContractorCategory INNER JOIN " +
                    "CategoryInfo ON ContractorCategory.CategoryID = CategoryInfo.CategoryID INNER JOIN ContractorInfo ON ContractorCategory.ContractorID = ContractorInfo.ContractorID " +
                    "INNER JOIN CityInfo INNER JOIN ContractorCategoryAreaRel INNER JOIN AreaInfo ON ContractorCategoryAreaRel.AreaID = AreaInfo.AreaID ON CityInfo.CityID = AreaInfo.CityID ON " +
                    "ContractorCategory.ContractorCategoryID = ContractorCategoryAreaRel.ContractorCategoryID WHERE (CityInfo.CityID = @CITYID) AND (AreaInfo.AreaID = @AREAID) AND " +
                    "(CategoryInfo.CategoryID = @CATID) AND (ContractorInfo.ContractorID = @CID)";

                using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
                {
                    conn.Open();

                    SqlDataReader rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@AREAID", areaid),
                        new SqlParameter("@CITYID", cityid),
                        new SqlParameter("@CATID", catid),
                        new SqlParameter("@CID", contractorid));

                    if (rdr.Read())
                    {
                        Response.Cookies["devbprpreferences"]["cityid"] = rdr["CityID"].ToString();
                        Response.Cookies["devbprpreferences"]["cityname"] = rdr["CityDisplayName"].ToString();
                        Response.Cookies["devbprpreferences"]["cityurlname"] = rdr["CityURLName"].ToString();
                        Response.Cookies["devbprpreferences"]["areaid"] = rdr["AreaID"].ToString();
                        Response.Cookies["devbprpreferences"]["areaname"] = rdr["AreaDisplayName"].ToString();
                        Response.Cookies["devbprpreferences"]["areaurlname"] = rdr["AreaURLName"].ToString();
                        Response.Cookies["devbprpreferences"].Expires = DateTime.Now.AddDays(365);

                        Response.Redirect("/" + rdr["CategoryURLName"].ToString() + "/" + rdr["CityURLName"].ToString() + "/" + rdr["AreaURLName"].ToString() + "/" + rdr["ContractorURLName"].ToString());
                    }
                    else
                    {
                        Response.Redirect("/");
                    }
                    rdr.Close();

                    conn.Close();
                }
            }
            else
            {
                Response.Redirect("/default.aspx");
            }
        }
    }
}
