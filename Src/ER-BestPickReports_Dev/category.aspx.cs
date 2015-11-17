using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ER_BestPickReports_Dev.App_Code;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev
{
    public partial class category : BasePage
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
        int ecnt = 0;
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
            ((ER_BestPickReports_Dev.SiteMaster)Master).cityID = cityID;
            ((ER_BestPickReports_Dev.SiteMaster)Master).areaID = areaID;
            ((ER_BestPickReports_Dev.SiteMaster)Master).catid = catid;

            //Set refertype to ppc=2 if this is a ppc page
            if (bool.Parse(isppc))
                refertype = "2";

            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            string sql = "";

            if (!IsPostBack)
            {
                //Hide email form modal popup panel initially
                FormContainer.Style.Value = "display:none";
            }

	        SetRightRailAttributes();

            //Get additional category field from categoryarea table
            sql = "SELECT ExtDesc FROM AreaInfo WHERE (AreaID = @AREAID)";
            object addinfo = DataAccessHelper.Data.ExecuteScalar(sql,
                new SqlParameter("@AREAID", areaID));
            if (addinfo != null)
            {
                if (addinfo.ToString() != "")
                {
                    AreaAdditionalInfo.Text = addinfo.ToString();
                    AdditionalPanel.Visible = true;
                }
            }

            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                conn.Open();

                //Get city name and year
                sql = "SELECT Cityinfo.DisplayName AS CityDisplayName, Cityinfo.MobileYear AS DataYear, AreaInfo.DisplayName AS AreaDisplayName FROM CityInfo INNER JOIN AreaInfo ON CityInfo.CityID = AreaInfo.CityID WHERE (CityInfo.CityID = @CITYID) AND (AreaInfo.CityID = @CITYID) AND (AreaInfo.AreaID = @AREAID)";

                SqlDataReader rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@AREAID", areaID),
                    new SqlParameter("@CITYID", cityID));

                if (rdr.Read())
                {
                    citydisplayname = rdr["CityDisplayName"].ToString();
                    areadisplayname = rdr["AreaDisplayName"].ToString();

                    BestPickHeader.Text = rdr["DataYear"].ToString() + " Best Picks";

                    RightRailRibbon.ImageUrl = "/images/ribbon_rail_" + rdr["DataYear"].ToString() + ".png";
                    RightRailRibbon.AlternateText = rdr["DataYear"].ToString() + " EBSCO Research Best Pick";
                    RightRailRibbon.ToolTip = rdr["DataYear"].ToString() + " EBSCO Research Best Pick";
                    dyear = rdr["DataYear"].ToString();
                }
                rdr.Close();

                sql = "SELECT TipArticle.Title, TipArticle.UrlTitle, CategoryInfo.UrlName AS CatName FROM TipArticle INNER JOIN TipArticleArea ON " +
                                "TipArticle.ArticleID = TipArticleArea.ArticleID INNER JOIN CategoryInfo ON TipArticle.CategoryID = CategoryInfo.CategoryID " +
                                "WHERE (TipArticle.CategoryID = @CATID) AND (TipArticleArea.AreaID = @AREAID) ORDER BY TipArticle.PublishDate DESC";

                rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@CATID", catid),
                    new SqlParameter("@AREAID", areaID));

                ArticleList.DataSource = rdr;
                ArticleList.DataBind();

                if (ArticleList.Items.Count > 0)
                    ArticleList.Visible = true;

                rdr.Close();  

                //Get category icon from global category level
                sql = "SELECT Website FROM Info WHERE (InfoType = 1) AND (ReferenceID = @CATID)";
                object o = DataAccessHelper.Data.ExecuteScalar(sql,
                    new SqlParameter("@CATID", catid));
                if (o != null)
                {
                    //Check for icon
                    if (o.ToString() != "")
                        IconImage.ImageUrl = "/assets/icons/" + o.ToString();
                    else
                        IconImage.ImageUrl = "/assets/icons/temp.png";
                }
                else
                {
                    IconImage.ImageUrl = "/assets/icons/temp.png";
                }

                conn.Close();
            }

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
                    CategoryName.Text = catRow["displayname"].ToString();
                    catname = catRow["displayname"].ToString();

                    if (catRow["subtitle"].ToString() != "")
                        CategoryAbout.Text = catRow["subtitle"].ToString();
                    else
                    {
                        if (citydisplayname == "Northern Virginia")
                            CategoryAbout.Text = "About " + citydisplayname + " " + catRow["displayname"].ToString();
                        else
                            CategoryAbout.Text = "About Metro-" + citydisplayname + " " + catRow["displayname"].ToString();
                    }

                    Desc.Text = catRow["longdesc"].ToString();

                    IconImage.AlternateText = citydisplayname + " " + catRow["displayname"].ToString();
                    IconImage.ToolTip = citydisplayname + " " + catRow["displayname"].ToString();

                    // See if we have an extended desc
                    if (catRow["extdesc"].ToString() != "")
                    {
                        LongDesc.Text = catRow["extdesc"].ToString();
                        ExtDesc.Visible = true;
                        ShowHide.Visible = true;
                    }

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

            MultipleEMailButton.Visible = showmultiple;

            ContractorList.DataSource = dt;
            ContractorList.DataBind();

            bprPreferences.CityId = cityID;
            bprPreferences.CityName = citydisplayname;
            bprPreferences.CityUrlName = strcity;
            bprPreferences.AreaId = areaID;
            bprPreferences.AreaName = areadisplayname;
            bprPreferences.AreaUrlName = strarea;
            bprPreferences.SetExpiration(DateTime.Now.AddDays(365));
        }

        protected void EmailContractorList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int alertcnt = 0;

                DataTable dt = (DataTable)EmailContractorList.DataSource;
                DataRow row = dt.Rows[reccount];

                HiddenField cid = (HiddenField)e.Item.FindControl("HiddenListID");
                cid.Value = row["cid"].ToString().Trim();

                Label name = (Label)e.Item.FindControl("ContractorLabel");
                Label ast = (Label)e.Item.FindControl("Asterisk");
                name.Text = row["name"].ToString();

                Image img = (Image)e.Item.FindControl("EmailAlert");
                CheckBox chk = (CheckBox)e.Item.FindControl("ContractorEmailID");

                if (row["email"].ToString().Trim() != "")
                {
                    img.Visible = false;
                    chk.Visible = true;
                    chk.Checked = true;
                    ast.Visible = false;
                }
                else
                {
                    name.Attributes.Add("style", "color:#909090;");
                    img.Visible = true;
                    chk.Visible = false;
                    ast.Visible = true;
                    alertcnt++;
                }

                if (alertcnt > 0)
                    alertnote.Visible = true;

                reccount++;
            }
        }

        protected void ContractorList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HyperLink name = (HyperLink)e.Item.FindControl("ContractorName");
                HyperLink more = (HyperLink)e.Item.FindControl("ReadMoreLink");
                Literal phone = (Literal)e.Item.FindControl("ContractorPhone");
                Literal bptext = (Literal)e.Item.FindControl("BestPickText");
                Literal desc = (Literal)e.Item.FindControl("ShortDesc");
                Panel emailpanel = (Panel)e.Item.FindControl("EmailPanel");
                LinkButton emailbtn = (LinkButton)e.Item.FindControl("SendEmailButton");
                Panel content = (Panel)e.Item.FindControl("ContractorContent");
                Panel frame = (Panel)e.Item.FindControl("ContractorContent_Frame");
                HtmlControl conframe = (HtmlControl)e.Item.FindControl("conframe");
                Literal lic = (Literal)e.Item.FindControl("License");
                Literal lia = (Literal)e.Item.FindControl("Liability");
                Literal ins = (Literal)e.Item.FindControl("Insurance");
                HtmlControl licrow = (HtmlControl)e.Item.FindControl("licenseitem");
                HtmlControl liarow = (HtmlControl)e.Item.FindControl("liabilityitem");
                HtmlControl insrow = (HtmlControl)e.Item.FindControl("insurancestateitem");
                Image ribbon = (Image)e.Item.FindControl("RibbonImage");

                //NoFollow for company links
                name.Attributes.Add("rel", "nofollow");
                more.Attributes.Add("rel", "nofollow");

                DataTable dt = (DataTable)ContractorList.DataSource;
                DataRow row = dt.Rows[recindex];
                InfoLevel conInfoLevel = InfoLevel.Area;
                bool isPrimary = false;
                DataRow conRow = DataAccessHelper.FindInfoRecord(InfoType.ContractorCategoryArea, int.Parse(row["contractorcategoryareaid"].ToString()), ref conInfoLevel, ref isPrimary);
                if (conRow != null)
                {
                    if (!isPrimary)
                    {
                        conframe.Attributes["src"] = "/inlinecontent.aspx?type=contractorlist&infoid=" + conRow["infoid"].ToString() + "&cid=" + row["contractorid"].ToString() + "&cat=" + strcategory + "&areaid=" + areaID + "&city=" + strcity + "&area=" + strarea;
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

                        if (bool.Parse(isppc))
                            name.NavigateUrl = basedomain + "/ppc/" + strcategory + "/" + strcity + "/" + strarea + "/" + conRow["urlname"].ToString();
                        else
                            name.NavigateUrl = basedomain + "/" + strcategory + "/" + strcity + "/" + strarea + "/" + conRow["urlname"].ToString();

                        more.Text = "Read more about " + conRow["displayname"].ToString();

                        if (bool.Parse(isppc))
                            more.NavigateUrl = basedomain + "/ppc/" + strcategory + "/" + strcity + "/" + strarea + "/" + conRow["urlname"].ToString();
                        else
                            more.NavigateUrl = basedomain + "/" + strcategory + "/" + strcity + "/" + strarea + "/" + conRow["urlname"].ToString();

                        if (refertype == "0")
                            phone.Text = conRow["phone"].ToString();
                        else if (refertype == "1" && conRow["organicphone"].ToString() != "")
                            phone.Text = conRow["organicphone"].ToString();
                        else if (refertype == "2" && conRow["ppcphone"].ToString() != "")
                            phone.Text = conRow["ppcphone"].ToString();
                        else if (refertype == "3" && conRow["facebookphone"].ToString() != "")
                            phone.Text = conRow["facebookphone"].ToString();
                        else
                            phone.Text = conRow["phone"].ToString();

                        bptext.Text = conRow["bestpicktext"].ToString();
                        desc.Text = conRow["shortdesc"].ToString();

                        if (!String.IsNullOrEmpty(conRow["licensenumber"].ToString()))
                            lic.Text = "Verified Licenses";
                        else
                            licrow.Visible = false;

                        string wcValue = conRow["workersCompensation"].ToString().ToUpper();
                        if (!String.IsNullOrEmpty(wcValue))
                        {
                            switch(wcValue)
                            {
                                case "V": ins.Text = "Verified Workers' Comp";
                                    break;
                                case "E": ins.Text = "Workers' Comp Exempt";
                                    break;
                            }
                        }
                        else
                            insrow.Visible = false;

                        if (bool.Parse(conRow["hasliability"].ToString()))
                            lia.Text = "Verified General Liability Insurance";
                        else
                            liarow.Visible = false;

                        // See if we have an email address
                        if (conRow["email"].ToString() != "")
                        {
                            emailpanel.Visible = true;
                            emailbtn.CommandArgument = row["contractorid"].ToString() + "|" + conRow["displayname"].ToString() + "|" + conRow["email"].ToString();
                            //bppanel.Attributes.Add("class", "fltlft companyRecord");
                        }
                        else
                        {
                            //bppanel.Attributes.Add("class", "fltlft companyRecord_long");
                        }

                        frame.Visible = false;
                        content.Visible = true;
                    }
                }

                recindex++;
            }

        }

        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in EmailContractorList.Items)        
            {
                CheckBox chkbox = item.FindControl("ContractorEmailID") as CheckBox;
                HiddenField emailid = item.FindControl("HiddenListID") as HiddenField;

                if (chkbox.Checked)            
                {               
                    MultipleEmailIDs.Value += emailid.Value + ",";
                    ecnt++;
                }
            }

            EmailContractorName.Text = "Multiple Companies";

            if (ecnt > 0)
            {
                if (MultipleEmailIDs.Value[MultipleEmailIDs.Value.Length - 1] == ',')
                    MultipleEmailIDs.Value = MultipleEmailIDs.Value.Substring(0, MultipleEmailIDs.Value.Length - 1);

                //Check and see if we have one or more companies
                if (ecnt == 1)
                {
                    //Lookup contractor id)
                    string sql = "SELECT DisplayName FROM ContractorAreaCategoryRel WHERE (ContractorCategoryAreaID = @CCAID)";
                    object o = DataAccessHelper.Data.ExecuteScalar(sql,
                        new SqlParameter("@CCAID", MultipleEmailIDs.Value.Trim()));
                    if (o != null)
                        EmailContractorName.Text = o.ToString();
                }

                //Clear all fields every time the form is called to show
                Name.Text = "";
                LastName.Text = "";
                Address.Text = "";
                City.Text = "";
                Zip.Text = "";
                Email.Text = "";
                FormPhone.Text = "";
                FormPhone2.Text = "";
                WorkType.Text = "";
                Message.Text = "";
                ReqPanel.Visible = false;
                EmailComplete.Visible = false;
                ChooseEmailPanel.Visible = false;
                ModalEmailForm.Visible = true;

                SendEmail.Show();
                Name.Focus();
            }
            else
            {
                errornote.Visible = true;
                SendEmail.Show();
            }
        }

        protected void EmailButton_Click(object sender, EventArgs e)
        {
            //set this to false so we know they started to send email
            BWSession.emailStartedAndSent = false;
            MultipleEmailIDs.Value = "";

            if (((LinkButton)sender).CommandArgument != null && ((LinkButton)sender).CommandArgument != "")
            {
                string[] comarg = ((LinkButton)sender).CommandArgument.Split('|');
                EmailContractorID.Value = comarg[0].Trim();
                EmailContractorName.Text = comarg[1].Trim();
                ContractorEmail.Value = comarg[2].Trim();
            }
            ModalEmailForm.Style.Value = "display:block";

            //TODO: Clear all fields every time the form is called to show
            Name.Text = "";
            LastName.Text = "";
            Address.Text = "";
            City.Text = "";
            Zip.Text = "";
            Email.Text = "";
            FormPhone.Text = "";
            FormPhone2.Text = "";
            WorkType.Text = "";
            Message.Text = "";
            ReqPanel.Visible = false;
            errornote.Visible = false;
            EmailComplete.Visible = false;
            ChooseEmailPanel.Visible = false;
            ModalEmailForm.Visible = true;

            SendEmail.Show();
            Name.Focus();
        }

        protected void MultipleEMailButton_Click(object sender, EventArgs e)
        {
            //set this to false so we know they started to send email
            BWSession.emailStartedAndSent = false;
            ModalEmailForm.Visible = false;
            EmailComplete.Visible = false;
            ChooseEmailPanel.Visible = true;
            errornote.Visible = false;
            MultipleEmailIDs.Value = "";

            //Bind to checkbox list all contractors on the page
            EmailContractorList.DataSource = clistTable;
            EmailContractorList.DataBind();

            SendEmail.Show();
        }

        protected void Cancel_EmailClick(object sender, EventArgs e)
        {
            // Hide the sent div and show the form
            ModalEmailForm.Visible = true;
            EmailComplete.Visible = false;
            ReqPanel.Visible = false;
        }

        protected void CloseEmail_Click(object sender, EventArgs e)
        {
            // Hide the sent div and show the form
            ModalEmailForm.Visible = true;
            EmailComplete.Visible = false;
            ReqPanel.Visible = false;
        }

        protected void OK_EmailClick(object sender, EventArgs e)
        {
            string sql = "";

            bool error = false;
            string name = Name.Text.Trim();
            string lastname = LastName.Text.Trim();
            string addr = Address.Text.Trim();
            string zip = Zip.Text.Trim();
            string email = Email.Text.Trim();
            string phone = FormPhone.Text.Trim();
            string phone2 = FormPhone2.Text.Trim();
            string worktype = WorkType.Text.Trim();

            #region Error Checking
            ReqName.Visible = false;
            ReqLastName.Visible = false;
            ReqAddress.Visible = false;
            ReqZip.Visible = false;
            ReqEmail.Visible = false;
            ReqPhone.Visible = false;
            ReqWorkType.Visible = false;
            ReqMessage.Visible = false;
            if (name == "")
                ReqName.Visible = true;
            if (lastname == "")
                ReqLastName.Visible = true;
            if (addr == "")
                ReqAddress.Visible = true;
            if (zip == "")
                ReqZip.Visible = true;
            if (zip == "" || !Global.IsZipCode(zip))
                ReqZip.Visible = true;
            if (email == "" || !Global.IsEmail(email))
                ReqEmail.Visible = true;
            if (phone == "" || !Global.IsPhoneNumber(phone))
                ReqPhone.Visible = true;
            if (worktype == "")
                ReqWorkType.Visible = true;

            error = ReqName.Visible || ReqLastName.Visible || ReqAddress.Visible || ReqZip.Visible || ReqEmail.Visible || ReqPhone.Visible || ReqWorkType.Visible;

            ReqName.Visible = false;
            ReqLastName.Visible = false;
            ReqAddress.Visible = false;
            ReqZip.Visible = false;
            ReqEmail.Visible = false;
            ReqPhone.Visible = false;
            ReqWorkType.Visible = false;
            ReqMessage.Visible = false;

            #endregion // Error Checking

            if (!error)
            {
                string contractorMsg = "";
                contractorMsg += "<span style=\"font-family:Arial; font-size:12px;\"><strong>Name: </strong>" + name + " " + lastname + "<br/><strong>Address: </strong>" + addr + "<br/><strong>City: </strong>" + City.Text.Trim() + "<br/><strong>Zip Code: </strong>" + zip + "<br/><strong>Email: </strong>" + email + "<br>";
                contractorMsg += "<strong>Phone: </strong>" + phone + "<br/><strong>Phone 2: </strong>" + phone2 + "<br/><strong>Work Type: </strong>" + worktype + "<br/><strong>Message: </strong>" + Message.Text.Trim();
                contractorMsg += "</span>";
                //string adminMsg = "<font face='Arial' size='2'>Someone showed interest in <strong>" + ContractorName.Value + "</strong><br/><br/>" + contractorMsg + "</font>";

                string subjectMsg = "";

                //Parse comma separated list of emails - check if we are sending to multiples first
                if (MultipleEmailIDs.Value != "" && !BWSession.emailStartedAndSent)
                {
                    string localconid = "";

                    string[] ids = MultipleEmailIDs.Value.Trim().Split(',');
                    foreach (string strid in ids)
                    {
                        subjectMsg = "Best Pick Reports Website Lead";

                        //Lookup emails from contractorcategoryareaid
                        InfoLevel conInfoLevel = InfoLevel.Area;
                        bool isPrimary = false;
                        DataRow conRow = DataAccessHelper.FindInfoRecord(InfoType.ContractorCategoryArea, int.Parse(strid), ref conInfoLevel, ref isPrimary);
                        if (conRow != null)
                        {
                            //Lookup contractor id
                            sql = "SELECT ContractorID FROM ContractorCategoryInfo WHERE ContractorCategoryID = @CCID";
                            object o = DataAccessHelper.Data.ExecuteScalar(sql,
                                new SqlParameter("@CCID", conRow["contractorcategoryid"].ToString()));
                            if (o != null)
                                localconid = o.ToString();

                            string[] emails = conRow["email"].ToString().Trim().Split(',');
                            foreach (string stremail in emails)
                            {
                                if (Global.IsEmail(stremail.Trim()))
                                {
                                    try
                                    {
                                        // Send email to contractor
                                        //!!!!HARD CODED FOR TLC DECKS
                                        if (localconid == "215")
                                            subjectMsg = "Home Reports – Dispatch 6106";

                                        Global.SendEmailNotification(stremail.Trim(), subjectMsg, ConfigurationManager.AppSettings["EmailNotification"], contractorMsg, true);
                                        BWSession.emailStartedAndSent = true;
                                    }
                                    catch (Exception)
                                    {
                                        // TODO: log error in database that email couldn't be sent???
                                    }
                                }
                            }

                            // commit contractorMsg to database
                            string insertsql = "INSERT INTO EmailData (DateSent, AreaID, CityID, CategoryID, ContractorID, FirstName, LastName, Address, City, Zip, Email, Phone, Phone2, WorkType, Message, IsPPC) " +
                                "VALUES (@DATESENT, @AREAID, @CITYID, @CATEGORYID, @CONTRACTORID, @FIRSTNAME, @LASTNAME, @ADDRESS, @CITY, @ZIP, @EMAIL, @PHONE, @PHONE2, @WORKTYPE, @MESSAGE, @PPC)";
                            DataAccessHelper.Data.ExecuteNonQuery(insertsql,
                                new SqlParameter("@DATESENT", DateTime.Now),
                                new SqlParameter("@AREAID", areaID),
                                new SqlParameter("@CITYID", cityID),
                                new SqlParameter("@CATEGORYID", catid),
                                new SqlParameter("@CONTRACTORID", int.Parse(localconid)),
                                new SqlParameter("@FIRSTNAME", name),
                                new SqlParameter("@LASTNAME", lastname),
                                new SqlParameter("@ADDRESS", addr),
                                new SqlParameter("@CITY", City.Text.Trim()),
                                new SqlParameter("@ZIP", Zip.Text.Trim()),
                                new SqlParameter("@EMAIL", email),
                                new SqlParameter("@PHONE", phone),
                                new SqlParameter("@PHONE2", phone2),
                                new SqlParameter("@WORKTYPE", worktype),
                                new SqlParameter("@PPC", isppc),
                                new SqlParameter("@MESSAGE", Message.Text.Trim()));
                        }
                    }
                }
                else if (ContractorEmail.Value.Trim() != "" && !BWSession.emailStartedAndSent)
                {
                    string[] emails = ContractorEmail.Value.Trim().Split(',');
                    foreach (string stremail in emails)
                    {
                        subjectMsg = "Best Pick Reports Website Lead";

                        if (Global.IsEmail(stremail.Trim()))
                        {
                            try
                            {
                                // Send email to contractor
                                //!!!!HARD CODED FOR TLC DECKS
                                if (EmailContractorID.Value == "215")
                                    subjectMsg = "Home Reports – Dispatch 6106";

                                Global.SendEmailNotification(stremail.Trim(), subjectMsg, ConfigurationManager.AppSettings["EmailNotification"], contractorMsg, true);
                                BWSession.emailStartedAndSent = true;
                            }
                            catch (Exception)
                            {
                                // TODO: log error in database that email couldn't be sent???
                            }
                        }
                    }

                    // commit contractorMsg to database
                    string insertsql = "INSERT INTO EmailData (DateSent, AreaID, CityID, CategoryID, ContractorID, FirstName, LastName, Address, City, Zip, Email, Phone, Phone2, WorkType, Message, IsPPC) " +
                        "VALUES (@DATESENT, @AREAID, @CITYID, @CATEGORYID, @CONTRACTORID, @FIRSTNAME, @LASTNAME, @ADDRESS, @CITY, @ZIP, @EMAIL, @PHONE, @PHONE2, @WORKTYPE, @MESSAGE, @PPC)";
                    DataAccessHelper.Data.ExecuteNonQuery(insertsql,
                        new SqlParameter("@DATESENT", DateTime.Now),
                        new SqlParameter("@AREAID", areaID),
                        new SqlParameter("@CITYID", cityID),
                        new SqlParameter("@CATEGORYID", catid),
                        new SqlParameter("@CONTRACTORID", int.Parse(EmailContractorID.Value)),
                        new SqlParameter("@FIRSTNAME", name),
                        new SqlParameter("@LASTNAME", lastname),
                        new SqlParameter("@ADDRESS", addr),
                        new SqlParameter("@CITY", City.Text.Trim()),
                        new SqlParameter("@ZIP", Zip.Text.Trim()),
                        new SqlParameter("@EMAIL", email),
                        new SqlParameter("@PHONE", phone),
                        new SqlParameter("@PHONE2", phone2),
                        new SqlParameter("@WORKTYPE", worktype),
                        new SqlParameter("@PPC", isppc),
                        new SqlParameter("@MESSAGE", Message.Text.Trim()));
                }

                // Hide the email form and show the "Email Sent" div
                ModalEmailForm.Visible = false;
                ReqPanel.Visible = false;
                EmailComplete.Visible = true;
            }

            // If this style isn't set, the modal popup won't display
            ModalEmailForm.Style.Value = "display:block";
            ReqPanel.Visible = true;
            SendEmail.Show();
        }

		private void SetRightRailAttributes()
		{
			AreaGroupDistributor areaGroupDistributor = new AreaGroupDistributor();
			AreaGroup areaGroup = areaGroupDistributor.GetAreaGroupByAreaId(areaID);
			if (areaGroup != null)
			{
				VideoBannerLink.NavigateUrl = areaGroup.PromoClipUri;
			}
		}
    }
}