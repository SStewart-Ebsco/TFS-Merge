using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ER_BestPickReports_Dev.App_Code;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using PinnexLib.Data;
using System.Configuration;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev.mobile
{
    public partial class Category : BasePage
    {
        int cityID = 0;
        public int areaID = 0;
        public string catid = "";
        string catareaid = "";
        public string citydisplayname = "";
        string areadisplayname = "";
        public string catinfoid = "";
        public string strcity = "";
        public string strcategory = "";
        public string strarea = "";
        public string isexpanded = "false";
        string isppc = "";
        string catname = "";
        public string basedomain = "";
        private int recindex = 0;
        private int reccount = 0;
        string dyear = "";
        DataTable clistTable = new DataTable();
        int hasemailcount = 0;

        private AppCookies bprPreferences = AppCookies.CreateInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get value from route handler
            cityID = BWSession.tempCityID;
            areaID = BWSession.tempAreaID;
            catareaid = (HttpContext.Current.Items["categoryareaid"].ToString() == "0") ? "" : HttpContext.Current.Items["categoryareaid"].ToString();
            catid = (HttpContext.Current.Items["categoryid"].ToString() == "0") ? "" : HttpContext.Current.Items["categoryid"].ToString();
            strcity = (HttpContext.Current.Items["city"].ToString() == "") ? "" : HttpContext.Current.Items["city"].ToString();
            strcategory = (HttpContext.Current.Items["category"].ToString() == "") ? "" : HttpContext.Current.Items["category"].ToString();
            strarea = (HttpContext.Current.Items["area"].ToString() == "") ? "" : HttpContext.Current.Items["area"].ToString();
            isppc = (HttpContext.Current.Items["isppc"].ToString() == "") ? "false" : HttpContext.Current.Items["isppc"].ToString();

            //Set cityid and areaid on master page
            ((ER_BestPickReports_Dev.mobile.SiteMobileMaster)Master).cityID = cityID;
            ((ER_BestPickReports_Dev.mobile.SiteMobileMaster)Master).areaID = areaID;
            ((ER_BestPickReports_Dev.mobile.SiteMobileMaster)Master).catid = catid;

            //Set refertype to ppc=2 if this is a ppc page
            if (bool.Parse(isppc))
                refertype = "2";

            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            //Fill fields of email form
            HiddenListCityId.Value = cityID.ToString();
            HiddenListAreaId.Value = areaID.ToString();
            HiddenListCategoryId.Value = catid.ToString();
            HiddenListIsPpc.Value = isppc.ToString();

            string sql = "";

            MobileCategoriesDL category = MobileCategoriesDL.GetByCategoryIdCityIdAreaId(int.Parse(catid), cityID, areaID);
            dyear = AreaDL.GetMobileYear(areaID);
            areadisplayname = category.AreaDisplayName;
            citydisplayname = category.CityDisplayName;

            BestPickHeader.Text = dyear + " Best Picks";

            //Check for icon
            if (String.IsNullOrEmpty(category.CategoryIcon))
                IconImage.ImageUrl = "/assets/icons/temp.png";
            else
                IconImage.ImageUrl = "/assets/icons/" + category.CategoryIcon;


            // Fill Related Articles list
            ArticleList.DataSource = ArticleDL.GetByAreaIDCategoryID(areaID, int.Parse(catid));
            ArticleList.DataBind();

            if (ArticleList.Items.Count > 0)
                ArticleList.Visible = true;


            InfoLevel catInfoLevel = InfoLevel.Area;
            bool isPrimary = false;
            DataRow catRow = DataAccessHelper.FindInfoRecord(InfoType.CategoryArea, int.Parse(catareaid), ref catInfoLevel, ref isPrimary);
            if (catRow != null)
            {
                if (catInfoLevel != InfoLevel.Area && !isPrimary)
                {
                    catinfoid = catRow["infoid"].ToString();
                    CategoryContent_Frame.Visible = true;
                    CategoryContent.Visible = false;

                    //Get category name
                    sql = "SELECT DisplayName FROM Info WHERE (InfoID = @INFOID)";
                    object o = DataAccessHelper.Data.ExecuteScalar(sql,
                        new SqlParameter("@INFOID", catinfoid));
                    if (o != null)
                        catname = o.ToString();
                }
                else
                {
                    catname = catRow["displayname"].ToString();
                    CategoryName.Text = catname;

                    IconImage.AlternateText = citydisplayname + " " + catname;
                    IconImage.ToolTip = citydisplayname + " " + catname;

                    CategoryContent_Frame.Visible = false;
                    CategoryContent.Visible = true;
                }

                //Add meta info
                HtmlMeta meta1;
                HtmlMeta meta2;

                string mkey = catRow["metakey"].ToString().Trim();
                string mdesc = catRow["metadesc"].ToString().Trim();
                string mtitle = catRow["metatitle"].ToString().Trim();

                //Check the contractorcategoryarea table for expicit content and use that if it is not null
                using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
                {
                    conn.Open();

                    sql = "SELECT metakey, metadesc, metatitle FROM CategoryArea WHERE (CategoryAreaID = @CATAREAID)";
                    SqlDataReader rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CATAREAID", catareaid));

                    if (rdr.Read())
                    {
                        if (rdr["metakey"].ToString().Trim() != "")
                            mkey = rdr["metakey"].ToString().Trim();

                        if (rdr["metadesc"].ToString().Trim() != "")
                            mdesc = rdr["metadesc"].ToString().Trim();

                        if (rdr["metatitle"].ToString().Trim() != "")
                            mtitle = rdr["metatitle"].ToString().Trim();
                    }

                    conn.Close();
                }

                meta2 = new HtmlMeta();
                meta2.Name = "keywords";
                meta2.Content = mkey;
                Page.Header.Controls.Add(meta2);
                Page.Header.Controls.Add(new LiteralControl("\n"));

                //meta3 = new HtmlMeta();
                //meta3.Name = "title";
                //meta3.Content = "Bestpickreports.com - Find top " + citydisplayname + " " + catname;
                //Page.Header.Controls.Add(meta3);
                //Page.Header.Controls.Add(new LiteralControl("\n"));

                meta1 = new HtmlMeta();
                meta1.Name = "description";
                meta1.Content = mdesc;
                Page.Header.Controls.Add(meta1);
                Page.Header.Controls.Add(new LiteralControl("\n"));

                Page.Title = mtitle;
            }


            //Show contractors
            sql = "SELECT * FROM ContractorAreaCategoryRel WHERE (CategoryID = @CATID) AND (AreaID = @AREAID) ORDER BY OrderIndex";
            DataTable dt = DataAccessHelper.Data.ExecuteDataset(sql,
                new SqlParameter("@CATID", catid),
                new SqlParameter("@AREAID", areaID)).Tables[0];

            //If this is a ppc page, remove all records that do not have a ppc phone
            if (bool.Parse(isppc))
            {
                foreach (DataRow row in dt.Rows)
                {
                    InfoLevel level = InfoLevel.Area;
                    DataRow conRow = DataAccessHelper.FindInfoRecord(InfoType.ContractorCategoryArea, int.Parse(row["contractorcategoryareaid"].ToString()), ref level, ref isPrimary);
                    if (conRow["ppcphone"].ToString() == "")
                        row.Delete();
                }
                dt.AcceptChanges();
            }

            //Create data table of contractors for choose email checkbox list
            clistTable.Clear();
            clistTable.Columns.Add("name");
            clistTable.Columns.Add("cid");
            clistTable.Columns.Add("email");
            bool showmultiple = false;
            hasemailcount = 0;

            foreach (DataRow row in dt.Rows)
            {
                InfoLevel level = InfoLevel.Area;
                DataRow conRow = DataAccessHelper.FindInfoRecord(InfoType.ContractorCategoryArea, int.Parse(row["contractorcategoryareaid"].ToString()), ref level, ref isPrimary);

                //Add to new data table
                clistTable.Rows.Add(new object[] { conRow["displayname"].ToString(), row["contractorcategoryareaid"].ToString(), conRow["email"].ToString() });

                if (conRow["email"].ToString().Trim() != "")
                {
                    showmultiple = true;
                    hasemailcount++;
                }
            }
            clistTable.AcceptChanges();

            if (hasemailcount < 2)
                showmultiple = false;

            MultipleEmailButton.Visible = showmultiple;

            ContractorList.DataSource = dt;
            ContractorList.DataBind();

            //Bind to checkbox list all contractors on the page
            EmailContractorList.DataSource = clistTable;
            EmailContractorList.DataBind();
            alertnote.Visible = clistTable.Rows.Count > hasemailcount;

            bprPreferences.CityId = cityID;
            bprPreferences.CityName = citydisplayname;
            bprPreferences.CityUrlName = strcity;
            bprPreferences.AreaId = areaID;
            bprPreferences.AreaName = areadisplayname;
            bprPreferences.AreaUrlName = strarea;
            bprPreferences.SetExpiration(DateTime.Now.AddDays(365));
        }

        protected void ContractorList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HyperLink name = (HyperLink)e.Item.FindControl("ContractorName");
                HtmlAnchor more = (HtmlAnchor)e.Item.FindControl("ReadMoreLink");
                HtmlAnchor phone = (HtmlAnchor)e.Item.FindControl("ContractorPhone");
                Literal bptext = (Literal)e.Item.FindControl("BestPickText");
                Panel emailpanel = (Panel)e.Item.FindControl("EmailPanel");
                Panel content = (Panel)e.Item.FindControl("ContractorContent");
                Panel frame = (Panel)e.Item.FindControl("ContractorContent_Frame");
                HtmlControl conframe = (HtmlControl)e.Item.FindControl("conframe");
                Image ribbon = (Image)e.Item.FindControl("RibbonImage");

                HtmlInputHidden cid = (HtmlInputHidden)e.Item.FindControl("HiddenListID");
                HtmlInputHidden cemail = (HtmlInputHidden)e.Item.FindControl("HiddenListEmail");

                DataTable dt = (DataTable)ContractorList.DataSource;
                DataRow row = dt.Rows[recindex];
                InfoLevel conInfoLevel = InfoLevel.Area;
                bool isPrimary = false;
                DataRow conRow = DataAccessHelper.FindInfoRecord(InfoType.ContractorCategoryArea, int.Parse(row["contractorcategoryareaid"].ToString()), ref conInfoLevel, ref isPrimary);
                if (conRow != null)
                {
                    if (!isPrimary)
                    {
                        conframe.Attributes["src"] = "/mobile/inlinecontent.aspx?type=contractorlist&infoid=" +
                                                     conRow["infoid"].ToString() + "&catid=" + catid + "&cid=" +
                                                     row["contractorid"].ToString() + "&cat=" + strcategory + "&areaid=" +
                                                     areaID + "&city=" + strcity + "&area=" + strarea;
                        frame.Visible = true;
                        content.Visible = false;
                    }
                    else
                    {
                        name.Text = conRow["displayname"].ToString();

                        ribbon.ImageUrl = "/images/ribbon_cat_" + dyear + ".png";
                        ribbon.AlternateText = dyear + " EBSCO Research Best Pick";
                        ribbon.ToolTip = dyear + " EBSCO Research Best Pick";
                        ribbon.Width = 32;

                        string link;
                        if (bool.Parse(isppc))
                        {
                            link = basedomain + "/ppc/" + strcategory + "/" + strcity + "/" + strarea + "/" + conRow["urlname"].ToString();
                        }
                        else
                        {
                            link = basedomain + "/" + strcategory + "/" + strcity + "/" + strarea + "/" + conRow["urlname"].ToString();

                        }
                        name.NavigateUrl = link;
                        more.HRef = link;

                        if (refertype == "0")
                            phone.HRef = "tel:" + conRow["phone"].ToString();
                        else if (refertype == "1" && conRow["organicphone"].ToString() != "")
                            phone.HRef = "tel:" + conRow["organicphone"].ToString();
                        else if (refertype == "2" && conRow["ppcphone"].ToString() != "")
                            phone.HRef = "tel:" + conRow["ppcphone"].ToString();
                        else if (refertype == "3" && conRow["facebookphone"].ToString() != "")
                            phone.HRef = "tel:" + conRow["facebookphone"].ToString();
                        else
                            phone.HRef = "tel:" + conRow["phone"].ToString();

                        bptext.Text = conRow["bestpicktext"].ToString();

                        // See if we have an email address
                        if (conRow["email"].ToString() != "")
                        {
                            emailpanel.Visible = true;
                        }

                        // Set hidden fields to work with multiple email form
                        cid.Value = row["contractorcategoryareaid"].ToString().Trim();
                        cemail.Value = conRow["email"].ToString().Trim();

                        frame.Visible = false;
                        content.Visible = true; 
                    }
                }

                recindex++;
            }
        }

        protected void ArticleList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HtmlAnchor articleLink = (HtmlAnchor)e.Item.FindControl("articleLink");
                Label articleName = (Label)e.Item.FindControl("articleName");

                ArticleDL dataItem = e.Item.DataItem as ArticleDL;

                articleLink.HRef = String.Concat(basedomain, "/", dataItem.UrlName, "/tips/", dataItem.UrlTitle);
                articleName.Text = dataItem.Title;
            }

        }

        protected void EmailContractorList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataTable dt = (DataTable)EmailContractorList.DataSource;
                DataRow row = dt.Rows[reccount];

                HiddenField cid = (HiddenField)e.Item.FindControl("HiddenListID");
                cid.Value = row["cid"].ToString().Trim();

                HiddenField cemail = (HiddenField)e.Item.FindControl("HiddenListEmail");
                cemail.Value = row["email"].ToString().Trim();

                Label name = (Label)e.Item.FindControl("ContractorLabel");
                name.Text = row["name"].ToString();

                HtmlInputCheckBox chk = (HtmlInputCheckBox)e.Item.FindControl("ContractorEmailID");

                if (row["email"].ToString().Trim() != "")
                {
                    chk.Visible = true;
                    chk.Checked = true;
                }
                else
                {
                    chk.Attributes["disabled"] = "disabled";
                }

                reccount++;
            }
        }


    }
}