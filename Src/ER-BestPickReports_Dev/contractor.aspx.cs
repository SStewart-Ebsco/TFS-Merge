using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ER_BestPickReports_Dev.App_Code;
using System.Linq;
using System.Collections.Generic;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev
{
    public partial class contractor : BasePage
    {
        public string concatid = "";
        int cityID = 0;
        public int areaID = 0;
        public int categoryID = 0;
        public int contractorID = 0;
        public string coninfoid = "";
        string caturl = "";
        string cityurl = "";
        string areaurl = "";
        string conurl = "";
        string citydisplayname = "";
        string areadisplayname = "";
        public string strcity = "";
        public string strarea = "";
        public string basedomain = "";
        public string rsummary = "";
        public string isppc = "";
        string dataYear = "";

        private AppCookies bprPreferences = AppCookies.CreateInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            int contractorCategoryAreaID = 0;
            //Get value from route handler
            if (HttpContext.Current.Items != null)
            {
                Int32.TryParse(HttpContext.Current.Items["contractorcategoryareaid"].ToString(), out contractorCategoryAreaID);
                concatid = BWSession.tempContractorCategoryID;
                cityID = BWSession.tempCityID;
                areaID = BWSession.tempAreaID;
                categoryID = BWSession.tempCategoryID;
                contractorID = BWSession.tempContractorID;
                strcity = BWSession.tempCityID_String;
                strarea = BWSession.tempAreaID_String;
                isppc = BWSession.tempIsPPC_String;
            }

            if (cityID <= 0 || areaID <= 0 || categoryID <= 0 || contractorID <= 0)
                Page.Response.Redirect("Default.aspx");

            //Set cityid and areaid on master page
            ((ER_BestPickReports_Dev.SiteMaster)Master).cityID = cityID;
            ((ER_BestPickReports_Dev.SiteMaster)Master).areaID = areaID;
            ((ER_BestPickReports_Dev.SiteMaster)Master).catid = categoryID.ToString();
            ((ER_BestPickReports_Dev.SiteMaster)Master).contractorid = contractorID.ToString();

            //Set refertype to ppc=2 if this is a ppc page
            if (bool.Parse(isppc))
                refertype = "2";
            else
            {
                if (refertype != "1" && refertype != "3")
                    refertype = "0";
            }

            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

			SetRightRailAttributes();

            string sql = "";

            //Get data year
            dataYear = App_Code.CityDL.GetMobileYear(cityID);

            if (!IsPostBack)
            {
                //Hide email form modal popup panel initially
                FormContainer.Style.Value = "display:none";
            }

            bool isPrimary = AreaDL.IsPrimary(InfoType.ContractorCategoryArea, contractorCategoryAreaID);
            ContractorDL currentContractor = ContractorDL.Get(InfoType.ContractorCategoryArea, contractorCategoryAreaID);
            if (currentContractor != null)
            {
                if (!isPrimary)
                {
                    coninfoid = currentContractor.infoID.ToString();
                    rsummary = currentContractor.reviewSummary;
                    ContractorContent_Frame.Visible = true;
                    ContractorContent.Visible = false;
                    TestimonialContent_Frame.Visible = true;
                    TestimonialContent.Visible = false;
                }
                else
                {
                    conurl = currentContractor.urlName;
                    ContractorName.Text = currentContractor.name;

                    ContractorRibbon.ImageUrl = "/images/" + dataYear + "_pic_company.png";
                    ContractorRibbon.AlternateText = dataYear + " EBSCO Research Best Pick";
                    ContractorRibbon.ToolTip = dataYear + " EBSCO Research Best Pick";

                    PhoneDL phoneNumber = null;
                    if (refertype == "0")
                        phoneNumber = currentContractor.phoneNumbers.Where(p => p.type == PhoneType.regular).FirstOrDefault();
                    else if (refertype == "1")
                        phoneNumber = currentContractor.phoneNumbers.Where(p => p.type == PhoneType.organic).FirstOrDefault();
                    else if (refertype == "2")
                        phoneNumber = currentContractor.phoneNumbers.Where(p => p.type == PhoneType.ppc).FirstOrDefault();
                    else if (refertype == "3")
                        phoneNumber = currentContractor.phoneNumbers.Where(p => p.type == PhoneType.ppc).FirstOrDefault();

                    if (phoneNumber == null)
                        phoneNumber = currentContractor.phoneNumbers.Where(p => p.type == PhoneType.regular).FirstOrDefault();

                    if (phoneNumber != null)
                        ContractorPhone.Text = phoneNumber.number;

                    BestPickText.Text = currentContractor.bestPickText;

                    if (!String.IsNullOrEmpty(currentContractor.servicesOffered))
                    {
                        ServicesOffered.Text = currentContractor.servicesOffered;
                        servicesofferedrow.Visible = true;
                    }

                    if (!String.IsNullOrEmpty(currentContractor.servicesNotOffered))
                    {
                        ServicesNotOffered.Text = currentContractor.servicesNotOffered;
                        servicesnotofferedrow.Visible = true;
                    }

                    if (!String.IsNullOrEmpty(currentContractor.specializations))
                    {
                        Specializations.Text = currentContractor.specializations;
                        specializationsrow.Visible = true;
                    }

                    if (!String.IsNullOrEmpty(currentContractor.minimumJob))
                    {
                        MinimumJob.Text = currentContractor.minimumJob;
                        minimumjobamountrow.Visible = true;
                    }

                    if (!String.IsNullOrEmpty(currentContractor.warranty))
                    {
                        Warranty.Text = currentContractor.warranty;
                        warrantyinformationrow.Visible = true;
                    }

                    if (!String.IsNullOrEmpty(currentContractor.licenseNumber))
                    {
                        litLicenses.Text = currentContractor.licenseNumber;
                        liLicenses.Visible = true;
                    }

                    if (!String.IsNullOrEmpty(currentContractor.awardsAndCertifications))
                    {
                        AwardsCertifications.Text = currentContractor.awardsAndCertifications;
                        awardsandcertificationsrow.Visible = true;
                    }

                    if (!String.IsNullOrEmpty(currentContractor.organizations))
                    {
                        Organizations.Text = currentContractor.organizations;
                        organizationsrow.Visible = true;
                    }

                    if (!String.IsNullOrEmpty(currentContractor.companyHistory))
                    {
                        CompanyHistory.Text = currentContractor.companyHistory;
                        companyhistoryrow.Visible = true;
                    }

                    if (!String.IsNullOrEmpty(currentContractor.employeeInformation))
                    {
                        EmployeeInformation.Text = currentContractor.employeeInformation;
                        employeeinformationrow.Visible = true;
                    }

                    if (!String.IsNullOrEmpty(currentContractor.productInformation))
                    {
                        ProductInformation.Text = currentContractor.productInformation;
                        productinformationrow.Visible = true;
                    }

                    if (!String.IsNullOrEmpty(currentContractor.hrStatus))
                    {
                        HRStatus.Text = currentContractor.hrStatus;
                        hrstatusrow.Visible = true;
                    }

                    if (!String.IsNullOrEmpty(currentContractor.additionalInformation))
                    {
                        AdditionalInformation.Text = currentContractor.additionalInformation;
                        additionalinformationrow.Visible = true;
                    }

                    if (!String.IsNullOrEmpty(currentContractor.reviewSummary))
                    {
                        ReviewSummary.Text = currentContractor.reviewSummary;
                        ReviewSummary.Visible = true;
                    }

                    if (!String.IsNullOrEmpty(currentContractor.email))
                    {
                        EmailPanel.Visible = true;
                        SendEmailButton.CommandArgument = contractorID + "|" + currentContractor.name + "|" + currentContractor.email;
                    }

                    // See if we have quote info
                    Quote.Text = currentContractor.quote.text;
                    if (!String.IsNullOrEmpty(currentContractor.quote.name))
                    {
                        QuoteName.Text = currentContractor.quote.name;
                        if (!String.IsNullOrEmpty(currentContractor.quote.title))
                            QuoteTitle.Text = " | " + currentContractor.quote.title;
                        quoteinfo.Visible = true;
                    }

                    using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
                    {
                        conn.Open();

                        //Get areas served
                        string areas = "";
                        sql = "SELECT ContractorCategoryAreaRel.AreaName FROM ContractorCategoryAreaRel INNER JOIN Area ON Area.AreaID = ContractorCategoryAreaRel.AreaID WHERE (Area.CityID = @CITYID) AND (ContractorCategoryAreaRel.ContractorCategoryID = @CONCATID)";
                        SqlDataReader rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                            new SqlParameter("@CITYID", cityID),
                            new SqlParameter("@CONCATID", concatid));

                        while (rdr.Read())
                        {
                            areas += rdr["AreaName"].ToString().Trim() + ", ";
                        }
                        char[] trimChars = new char[] { ' ', ',' };
                        areas = areas.TrimEnd(trimChars);
                        AreasServed.Text = areas;
                        rdr.Close();

                        conn.Close();
                    }

                    ContractorContent_Frame.Visible = false;
                    ContractorContent.Visible = true;
                    TestimonialContent_Frame.Visible = false;
                    TestimonialContent.Visible = true;
                }

                //Add meta info
                HtmlMeta meta1;
                HtmlMeta meta2;

                string mkey = currentContractor.metaData.key;
                string mdesc = currentContractor.metaData.description;
                string mtitle = currentContractor.metaData.title;

                //Check the contractorcategoryarea table for expicit content and use that if it is not null
                MetaDataDL areaData = ContractorDL.GetMetadataByCategoryArea(contractorCategoryAreaID);
                if (areaData != null)
                {
                    if (!String.IsNullOrEmpty(areaData.key))
                        mkey = areaData.key;
                    if (!String.IsNullOrEmpty(areaData.description))
                        mdesc = areaData.description;
                    if (!String.IsNullOrEmpty(areaData.title))
                        mtitle = areaData.title;
                }

                meta2 = new HtmlMeta();
                meta2.Name = "keywords";
                meta2.Content = mkey;
                Page.Header.Controls.Add(meta2);
                Page.Header.Controls.Add(new LiteralControl("\n"));

                meta1 = new HtmlMeta();
                meta1.Name = "description";
                meta1.Content = mdesc;
                Page.Header.Controls.Add(meta1);
                Page.Header.Controls.Add(new LiteralControl("\n"));

                Page.Title = mtitle;
            }

            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                conn.Open();

                //Get breadcrumb info
                sql = "SELECT CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName, AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaDisplayName, CityInfo.UrlName AS CityUrlName, CityInfo.DisplayName AS CityDisplayName " +
                    "FROM CategoryArea INNER JOIN CategoryInfo ON CategoryArea.CategoryID = CategoryInfo.CategoryID INNER JOIN " +
                    "AreaInfo ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CityInfo ON AreaInfo.CityID = CityInfo.CityID WHERE (AreaInfo.AreaID = @AREAID) AND " +
                    "(CityInfo.CityID = @CITYID) AND (CategoryInfo.CategoryID = @CATID)";

                SqlDataReader rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@AREAID", areaID),
                        new SqlParameter("@CITYID", cityID),
                        new SqlParameter("@CATID", categoryID));

                if (rdr.Read())
                {
                    CategoryBreadLink.Text = rdr["CatName"].ToString();

                    if (bool.Parse(isppc))
                        CategoryBreadLink.NavigateUrl = "/ppc/" + rdr["CatUrlName"].ToString() + "/" + rdr["CityURLName"].ToString() + "/" + rdr["AreaURLName"].ToString();
                    else
                        CategoryBreadLink.NavigateUrl = "/" + rdr["CatUrlName"].ToString() + "/" + rdr["CityURLName"].ToString() + "/" + rdr["AreaURLName"].ToString();

                    caturl = rdr["CatUrlName"].ToString();
                    cityurl = rdr["CityURLName"].ToString();
                    areaurl = rdr["AreaURLName"].ToString();
                    citydisplayname = rdr["CityDisplayName"].ToString();
                    areadisplayname = rdr["AreaDisplayName"].ToString();
                }
                rdr.Close();

                sql = "SELECT TipArticle.Title, TipArticle.UrlTitle, CategoryInfo.UrlName AS CatName FROM TipArticle INNER JOIN TipArticleArea ON " +
                        "TipArticle.ArticleID = TipArticleArea.ArticleID INNER JOIN CategoryInfo ON TipArticle.CategoryID = CategoryInfo.CategoryID " +
                        "WHERE (TipArticle.CategoryID = @CATID) AND (TipArticleArea.AreaID = @AREAID) ORDER BY TipArticle.PublishDate DESC";

                rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@CATID", categoryID),
                    new SqlParameter("@AREAID", areaID));

                ArticleList.DataSource = rdr;
                ArticleList.DataBind();

                if (ArticleList.Items.Count > 0)
                    TipPanel.Visible = true;

                rdr.Close();

                conn.Close();
            }

            //Set cookie for city/area preference based on url values
            bprPreferences.CityId = cityID;
            bprPreferences.CityName = citydisplayname;
            bprPreferences.CityUrlName = cityurl;
            bprPreferences.AreaId = areaID;
            bprPreferences.AreaName = areadisplayname;
            bprPreferences.AreaUrlName = areaurl;
            bprPreferences.SetExpiration(DateTime.Now.AddDays(365));
        }

        protected void EmailButton_Click(object sender, EventArgs e)
        {
            //set this to false so we know they started to send email
            BWSession.emailStartedAndSent = false;

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
            EmailComplete.Visible = false;
            ModalEmailForm.Visible = true;

            SendEmail.Show();
            Name.Focus();
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

                //Parse comma separated list of emails
                if (!BWSession.emailStartedAndSent)
                {
                if (ContractorEmail.Value.Trim() != "")
                {
                    string subjectMsg = "";
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
                }

                // commit contractorMsg to database
                string insertsql = "INSERT INTO EmailData (DateSent, AreaID, CityID, CategoryID, ContractorID, FirstName, LastName, Address, City, Zip, Email, Phone, Phone2, WorkType, Message, IsPPC) " +
                    "VALUES (@DATESENT, @AREAID, @CITYID, @CATEGORYID, @CONTRACTORID, @FIRSTNAME, @LASTNAME, @ADDRESS, @CITY, @ZIP, @EMAIL, @PHONE, @PHONE2, @WORKTYPE, @MESSAGE, @PPC)";
                DataAccessHelper.Data.ExecuteNonQuery(insertsql,
                    new SqlParameter("@DATESENT", DateTime.Now),
                    new SqlParameter("@AREAID", areaID),
                    new SqlParameter("@CITYID", cityID),
                    new SqlParameter("@CATEGORYID", categoryID),
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

        protected void dpgTestimonials_PreRender(object sender, EventArgs e)
        {
            List<Testimonial> testimonialList = Testimonial.GetByContractorIDCategoryIDAreaID(contractorID, categoryID, areaID, DateTime.Now);
            testimonialList.Sort((x, y) => y.publishedDate.CompareTo(x.publishedDate));

            TestimonialList.DataSource = testimonialList;
            TestimonialList.DataBind();
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
