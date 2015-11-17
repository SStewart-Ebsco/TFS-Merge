using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev
{
    public partial class blogredirect : BasePage
    {
        string strcat = "";
        string alt1 = "";
        string alt2 = "";
        string alt3 = "";
        string alt4 = "";
        bool atlmatch = false;
        bool chimatch = false;
        bool dalmatch = false;
        bool novamatch = false;
        bool houmatch = false;
        bool marymatch = false;
        bool dcmatch = false;
        bool bhammatch = false;
        public string atlcat = "";
        public string chicat = "";
        public string dalcat = "";
        public string novacat = "";
        public string houcat = "";
        public string marycat = "";
        public string dccat = "";
        public string bhamcat = "";
        public string basedomain = "";

        private AppCookies bprPreferences = AppCookies.CreateInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            strcat = (Request["category"] == null || Request["category"] == "") ? "" : Request["category"].ToString();
            alt1 = (Request["alt1"] == null || Request["alt1"] == "") ? "" : Request["alt1"].ToString();
            alt2 = (Request["alt2"] == null || Request["alt2"] == "") ? "" : Request["alt2"].ToString();
            alt3 = (Request["alt3"] == null || Request["alt3"] == "") ? "" : Request["alt3"].ToString();
            alt4 = (Request["alt4"] == null || Request["alt4"] == "") ? "" : Request["alt4"].ToString();

            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            string sql = "";
            SqlDataReader rdr = null;

            if (!IsPostBack)
            {
                //Error if qs is not correct
                if (strcat == "")
                    Response.Redirect("/error.aspx");

                //Get cat name
                sql = "SELECT DisplayName FROM CategoryInfo WHERE (UrlName = @CAT)";
                object o = DataAccessHelper.Data.ExecuteScalar(sql,
                    new SqlParameter("@CAT", strcat));
                if (o != null)
                    CatName.Text = o.ToString();

                //Check for user cookie
                if (bprPreferences.Exists())
                {
                    //Lookup category in users cookie - if no match, go to alts
                    using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
                    {
                        conn.Open();

                        sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                        "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (AreaInfo.UrlName = @AREANAME) AND (CityInfo.UrlName = @CITYNAME) AND (CategoryInfo.UrlName = @CATNAME)";

                        rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                            new SqlParameter("@CITYNAME", bprPreferences.CityUrlName),
                            new SqlParameter("@AREANAME", bprPreferences.AreaUrlName),
                            new SqlParameter("@CATNAME", strcat));

                        if (rdr.Read())
                        {
                            Response.Redirect(basedomain + "/" + strcat + "/" + rdr["CityUrlName"].ToString() + "/" + rdr["AreaUrlName"].ToString());
                        }
                        rdr.Close();

                        if (alt1 != "")
                        {
                            sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                            "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (AreaInfo.UrlName = @AREANAME) AND (CityInfo.UrlName = @CITYNAME) AND (CategoryInfo.UrlName = @CATNAME)";

                            rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                                new SqlParameter("@CITYNAME", bprPreferences.CityUrlName),
                                new SqlParameter("@AREANAME", bprPreferences.AreaUrlName),
                                new SqlParameter("@CATNAME", alt1));

                            if (rdr.Read())
                            {
                                Response.Redirect(basedomain + "/" + alt1 + "/" + rdr["CityUrlName"].ToString() + "/" + rdr["AreaUrlName"].ToString());
                            }
                            rdr.Close();
                        }

                        if (alt2 != "")
                        {
                            sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                            "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (AreaInfo.UrlName = @AREANAME) AND (CityInfo.UrlName = @CITYNAME) AND (CategoryInfo.UrlName = @CATNAME)";

                            rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                                new SqlParameter("@CITYNAME", bprPreferences.CityUrlName),
                                new SqlParameter("@AREANAME", bprPreferences.AreaUrlName),
                                new SqlParameter("@CATNAME", alt2));

                            if (rdr.Read())
                            {
                                Response.Redirect(basedomain + "/" + alt2 + "/" + rdr["CityUrlName"].ToString() + "/" + rdr["AreaUrlName"].ToString());
                            }
                            rdr.Close();
                        }

                        if (alt3 != "")
                        {
                            sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                            "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (AreaInfo.UrlName = @AREANAME) AND (CityInfo.UrlName = @CITYNAME) AND (CategoryInfo.UrlName = @CATNAME)";

                            rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                                new SqlParameter("@CITYNAME", bprPreferences.CityUrlName),
                                new SqlParameter("@AREANAME", bprPreferences.AreaUrlName),
                                new SqlParameter("@CATNAME", alt3));

                            if (rdr.Read())
                            {
                                Response.Redirect(basedomain + "/" + alt3 + "/" + rdr["CityUrlName"].ToString() + "/" + rdr["AreaUrlName"].ToString());
                            }
                            rdr.Close();
                        }

                        if (alt4 != "")
                        {
                            sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                            "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (AreaInfo.UrlName = @AREANAME) AND (CityInfo.UrlName = @CITYNAME) AND (CategoryInfo.UrlName = @CATNAME)";

                            rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                                new SqlParameter("@CITYNAME", bprPreferences.CityUrlName),
                                new SqlParameter("@AREANAME", bprPreferences.AreaUrlName),
                                new SqlParameter("@CATNAME", alt4));

                            if (rdr.Read())
                            {
                                Response.Redirect(basedomain + "/" + alt4 + "/" + rdr["CityUrlName"].ToString() + "/" + rdr["AreaUrlName"].ToString());
                            }
                            rdr.Close();
                        }

                        conn.Close();
                    }

                    GetMetros();
                }
                else
                {
                    GetMetros();
                }
            }
        }

        private void GetMetros()
        {
            string sql = "";
            SqlDataReader rdr = null;

            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                conn.Open();

                //Get areas for atlanta
                sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'atlanta') AND (CategoryInfo.UrlName = @CATNAME)";

                rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@CATNAME", strcat));

                if (rdr.HasRows)
                {
                    AtlantaAreaList.DataSource = rdr;
                    AtlantaAreaList.DataBind();
                    AtlantaList.Visible = true;
                    atlmatch = true;
                    atlcat = strcat;
                }
                rdr.Close();

                if (!atlmatch && alt1 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'atlanta') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt1));

                    if (rdr.HasRows)
                    {
                        AtlantaAreaList.DataSource = rdr;
                        AtlantaAreaList.DataBind();
                        AtlantaList.Visible = true;
                        atlmatch = true;
                        atlcat = alt1;
                    }
                    rdr.Close();
                }

                if (!atlmatch && alt2 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'atlanta') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt2));

                    if (rdr.HasRows)
                    {
                        AtlantaAreaList.DataSource = rdr;
                        AtlantaAreaList.DataBind();
                        AtlantaList.Visible = true;
                        atlmatch = true;
                        atlcat = alt2;
                    }
                    rdr.Close();
                }

                if (!atlmatch && alt3 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'atlanta') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt3));

                    if (rdr.HasRows)
                    {
                        AtlantaAreaList.DataSource = rdr;
                        AtlantaAreaList.DataBind();
                        AtlantaList.Visible = true;
                        atlmatch = true;
                        atlcat = alt3;
                    }
                    rdr.Close();
                }

                if (!atlmatch && alt4 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'atlanta') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt4));

                    if (rdr.HasRows)
                    {
                        AtlantaAreaList.DataSource = rdr;
                        AtlantaAreaList.DataBind();
                        AtlantaList.Visible = true;
                        atlmatch = true;
                        atlcat = alt4;
                    }
                    rdr.Close();
                }

                //Get areas for chicago
                sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'chicago') AND (CategoryInfo.UrlName = @CATNAME)";

                rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@CATNAME", strcat));

                if (rdr.HasRows)
                {
                    ChicagoAreaList.DataSource = rdr;
                    ChicagoAreaList.DataBind();
                    ChicagoList.Visible = true;
                    chimatch = true;
                    chicat = strcat;
                }
                rdr.Close();

                if (!chimatch && alt1 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'chicago') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt1));

                    if (rdr.HasRows)
                    {
                        ChicagoAreaList.DataSource = rdr;
                        ChicagoAreaList.DataBind();
                        ChicagoList.Visible = true;
                        chimatch = true;
                        chicat = alt1;
                    }
                    rdr.Close();
                }

                if (!chimatch && alt2 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'chicago') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt2));

                    if (rdr.HasRows)
                    {
                        ChicagoAreaList.DataSource = rdr;
                        ChicagoAreaList.DataBind();
                        ChicagoList.Visible = true;
                        chimatch = true;
                        chicat = alt2;
                    }
                    rdr.Close();
                }

                if (!chimatch && alt3 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'chicago') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt3));

                    if (rdr.HasRows)
                    {
                        ChicagoAreaList.DataSource = rdr;
                        ChicagoAreaList.DataBind();
                        ChicagoList.Visible = true;
                        chimatch = true;
                        chicat = alt3;
                    }
                    rdr.Close();
                }

                if (!chimatch && alt4 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'chicago') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt4));

                    if (rdr.HasRows)
                    {
                        ChicagoAreaList.DataSource = rdr;
                        ChicagoAreaList.DataBind();
                        ChicagoList.Visible = true;
                        chimatch = true;
                        chicat = alt4;
                    }
                    rdr.Close();
                }

                //Get areas for dallas
                sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'dallas') AND (CategoryInfo.UrlName = @CATNAME)";

                rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@CATNAME", strcat));

                if (rdr.HasRows)
                {
                    dalcat = strcat;
                    DallasAreaList.DataSource = rdr;
                    DallasAreaList.DataBind();
                    DallasList.Visible = true;
                    dalmatch = true;
                }
                rdr.Close();

                if (!dalmatch && alt1 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'dallas') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt1));

                    if (rdr.HasRows)
                    {
                        dalcat = alt1;
                        DallasAreaList.DataSource = rdr;
                        DallasAreaList.DataBind();
                        DallasList.Visible = true;
                        dalmatch = true;
                    }
                    rdr.Close();
                }

                if (!dalmatch && alt2 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'dallas') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt2));

                    if (rdr.HasRows)
                    {
                        dalcat = alt2;
                        DallasAreaList.DataSource = rdr;
                        DallasAreaList.DataBind();
                        DallasList.Visible = true;
                        dalmatch = true;
                    }
                    rdr.Close();
                }

                if (!dalmatch && alt3 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'dallas') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt3));

                    if (rdr.HasRows)
                    {
                        dalcat = alt3;
                        DallasAreaList.DataSource = rdr;
                        DallasAreaList.DataBind();
                        DallasList.Visible = true;
                        dalmatch = true;
                    }
                    rdr.Close();
                }

                if (!dalmatch && alt4 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'dallas') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt4));

                    if (rdr.HasRows)
                    {
                        dalcat = alt4;
                        DallasAreaList.DataSource = rdr;
                        DallasAreaList.DataBind();
                        DallasList.Visible = true;
                        dalmatch = true;
                    }
                    rdr.Close();
                }

                //Get areas for nova
                sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'northern-virginia') AND (CategoryInfo.UrlName = @CATNAME)";

                rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@CATNAME", strcat));

                if (rdr.HasRows)
                {
                    NoVaAreaList.DataSource = rdr;
                    NoVaAreaList.DataBind();
                    NovaList.Visible = true;
                    novamatch = true;
                    novacat = strcat;
                }
                rdr.Close();

                if (!novamatch && alt1 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'northern-virginia') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt1));

                    if (rdr.HasRows)
                    {
                        NoVaAreaList.DataSource = rdr;
                        NoVaAreaList.DataBind();
                        NovaList.Visible = true;
                        novamatch = true;
                        novacat = alt1;
                    }
                    rdr.Close();
                }

                if (!novamatch && alt2 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'northern-virginia') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt2));

                    if (rdr.HasRows)
                    {
                        NoVaAreaList.DataSource = rdr;
                        NoVaAreaList.DataBind();
                        NovaList.Visible = true;
                        novamatch = true;
                        novacat = alt2;
                    }
                    rdr.Close();
                }

                if (!novamatch && alt3 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'northern-virginia') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt3));

                    if (rdr.HasRows)
                    {
                        NoVaAreaList.DataSource = rdr;
                        NoVaAreaList.DataBind();
                        NovaList.Visible = true;
                        novamatch = true;
                        novacat = alt3;
                    }
                    rdr.Close();
                }

                if (!novamatch && alt4 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'northern-virginia') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt4));

                    if (rdr.HasRows)
                    {
                        NoVaAreaList.DataSource = rdr;
                        NoVaAreaList.DataBind();
                        NovaList.Visible = true;
                        novamatch = true;
                        novacat = alt4;
                    }
                    rdr.Close();
                }

                //Get areas for houston
                sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'houston') AND (CategoryInfo.UrlName = @CATNAME)";

                rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@CATNAME", strcat));

                if (rdr.HasRows)
                {
                    HoustonAreaList.DataSource = rdr;
                    HoustonAreaList.DataBind();
                    HoustonList.Visible = true;
                    houmatch = true;
                    houcat = strcat;
                }
                rdr.Close();

                if (!houmatch && alt1 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'houston') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt1));

                    if (rdr.HasRows)
                    {
                        HoustonAreaList.DataSource = rdr;
                        HoustonAreaList.DataBind();
                        HoustonList.Visible = true;
                        houmatch = true;
                        houcat = alt1;
                    }
                    rdr.Close();
                }

                if (!houmatch && alt2 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'houston') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt2));

                    if (rdr.HasRows)
                    {
                        HoustonAreaList.DataSource = rdr;
                        HoustonAreaList.DataBind();
                        HoustonList.Visible = true;
                        houmatch = true;
                        houcat = alt2;
                    }
                    rdr.Close();
                }

                if (!houmatch && alt3 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'houston') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt3));

                    if (rdr.HasRows)
                    {
                        HoustonAreaList.DataSource = rdr;
                        HoustonAreaList.DataBind();
                        HoustonList.Visible = true;
                        houmatch = true;
                        houcat = alt3;
                    }
                    rdr.Close();
                }

                if (!houmatch && alt4 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'houston') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt4));

                    if (rdr.HasRows)
                    {
                        HoustonAreaList.DataSource = rdr;
                        HoustonAreaList.DataBind();
                        HoustonList.Visible = true;
                        houmatch = true;
                        houcat = alt4;
                    }
                    rdr.Close();
                }

                //Get areas for maryland
                sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'maryland') AND (CategoryInfo.UrlName = @CATNAME)";

                rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@CATNAME", strcat));

                if (rdr.HasRows)
                {
                    MarylandAreaList.DataSource = rdr;
                    MarylandAreaList.DataBind();
                    MarylandList.Visible = true;
                    marymatch = true;
                    marycat = strcat;
                }
                rdr.Close();

                if (!marymatch && alt1 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'maryland') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt1));

                    if (rdr.HasRows)
                    {
                        MarylandAreaList.DataSource = rdr;
                        MarylandAreaList.DataBind();
                        MarylandList.Visible = true;
                        marymatch = true;
                        marycat = alt1;
                    }
                    rdr.Close();
                }

                if (!marymatch && alt2 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'maryland') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt2));

                    if (rdr.HasRows)
                    {
                        MarylandAreaList.DataSource = rdr;
                        MarylandAreaList.DataBind();
                        MarylandList.Visible = true;
                        marymatch = true;
                        marycat = alt2;
                    }
                    rdr.Close();
                }

                if (!marymatch && alt3 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'maryland') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt3));

                    if (rdr.HasRows)
                    {
                        MarylandAreaList.DataSource = rdr;
                        MarylandAreaList.DataBind();
                        MarylandList.Visible = true;
                        marymatch = true;
                        marycat = alt3;
                    }
                    rdr.Close();
                }

                if (!marymatch && alt4 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'maryland') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt4));

                    if (rdr.HasRows)
                    {
                        MarylandAreaList.DataSource = rdr;
                        MarylandAreaList.DataBind();
                        MarylandList.Visible = true;
                        marymatch = true;
                        marycat = alt4;
                    }
                    rdr.Close();
                }

                //Get areas for dc
                sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'washington-dc') AND (CategoryInfo.UrlName = @CATNAME)";

                rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@CATNAME", strcat));

                if (rdr.HasRows)
                {
                    DCAreaList.DataSource = rdr;
                    DCAreaList.DataBind();
                    DCList.Visible = true;
                    dcmatch = true;
                    dccat = strcat;
                }
                rdr.Close();

                if (!dcmatch && alt1 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'washington-dc') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt1));

                    if (rdr.HasRows)
                    {
                        DCAreaList.DataSource = rdr;
                        DCAreaList.DataBind();
                        DCList.Visible = true;
                        dcmatch = true;
                        dccat = alt1;
                    }
                    rdr.Close();
                }

                if (!dcmatch && alt2 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'washington-dc') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt2));

                    if (rdr.HasRows)
                    {
                        DCAreaList.DataSource = rdr;
                        DCAreaList.DataBind();
                        DCList.Visible = true;
                        dcmatch = true;
                        dccat = alt2;
                    }
                    rdr.Close();
                }

                if (!dcmatch && alt3 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'washington-dc') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt3));

                    if (rdr.HasRows)
                    {
                        DCAreaList.DataSource = rdr;
                        DCAreaList.DataBind();
                        DCList.Visible = true;
                        dcmatch = true;
                        dccat = alt3;
                    }
                    rdr.Close();
                }

                if (!dcmatch && alt4 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'washington-dc') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt4));

                    if (rdr.HasRows)
                    {
                        DCAreaList.DataSource = rdr;
                        DCAreaList.DataBind();
                        DCList.Visible = true;
                        dcmatch = true;
                        dccat = alt4;
                    }
                    rdr.Close();
                }

                //Get areas for birmingham
                sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'birmingham') AND (CategoryInfo.UrlName = @CATNAME)";

                rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@CATNAME", strcat));

                if (rdr.HasRows)
                {
                    BhamAreaList.DataSource = rdr;
                    BhamAreaList.DataBind();
                    BhamList.Visible = true;
                    bhammatch = true;
                    bhamcat = strcat;
                }
                rdr.Close();

                if (!bhammatch && alt1 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'birmingham') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt1));

                    if (rdr.HasRows)
                    {
                        BhamAreaList.DataSource = rdr;
                        BhamAreaList.DataBind();
                        BhamList.Visible = true;
                        bhammatch = true;
                        bhamcat = alt1;
                    }
                    rdr.Close();
                }

                if (!bhammatch && alt2 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'birmingham') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt2));

                    if (rdr.HasRows)
                    {
                        BhamAreaList.DataSource = rdr;
                        BhamAreaList.DataBind();
                        BhamList.Visible = true;
                        bhammatch = true;
                        bhamcat = alt2;
                    }
                    rdr.Close();
                }

                if (!bhammatch && alt3 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'birmingham') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt3));

                    if (rdr.HasRows)
                    {
                        BhamAreaList.DataSource = rdr;
                        BhamAreaList.DataBind();
                        BhamList.Visible = true;
                        bhammatch = true;
                        bhamcat = alt3;
                    }
                    rdr.Close();
                }

                if (!bhammatch && alt4 != "")
                {
                    sql = "SELECT AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaName, CityInfo.UrlName AS CityUrlName, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON " +
                    "Area.AreaID = AreaInfo.AreaID INNER JOIN CategoryArea ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CategoryInfo ON CategoryInfo.CategoryID = CategoryArea.CategoryID WHERE (CityInfo.UrlName = 'birmingham') AND (CategoryInfo.UrlName = @CATNAME)";

                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATNAME", alt4));

                    if (rdr.HasRows)
                    {
                        BhamAreaList.DataSource = rdr;
                        BhamAreaList.DataBind();
                        BhamList.Visible = true;
                        bhammatch = true;
                        bhamcat = alt4;
                    }
                    rdr.Close();
                }

                conn.Close();
            }
        }

        protected void DallasAreaList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                SqlDataReader rdr = (SqlDataReader)DallasAreaList.DataSource;

                HyperLink area = (HyperLink)e.Item.FindControl("DallasAreaLink");
                string areaname = rdr["AreaUrlName"].ToString();
                string arealabel = rdr["AreaName"].ToString();

                switch (areaname)
                {
                    case "southlake-grapevine-colleyville":
                        arealabel = "Southlake/Grapevine/Colleyville";
                        break;
                    case "flower-mound-lewisville-highland-village":
                        arealabel = "Flower Mound/Lewisville/Highland Village";
                        break;
                    case "plano-frisco-allen-mckinney":
                        arealabel = "Plano/Frisco/Allen/McKinney";
                        break;
                }

                area.Text = arealabel;
                area.NavigateUrl = basedomain + "/" + dalcat + "/" + rdr["CityUrlName"].ToString() + "/" + rdr["AreaUrlName"].ToString();
            }
        }
    }
}