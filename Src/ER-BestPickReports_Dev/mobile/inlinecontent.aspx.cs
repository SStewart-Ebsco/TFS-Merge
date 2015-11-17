using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ER_BestPickReports_Dev.App_Code;

namespace ER_BestPickReports_Dev.mobile
{
    public partial class inlinecontent : BasePage
    {
        string type = "";
        string strcat = "";
        string strCityUrl = "";
        string strarea = "";
        public int infoID = 0;
        int areaID = 0;
        int categoryID = 0;
        int contractorID = 0;
        public string isexpanded = "false";
        string basedomain = "";
        string rsummary = "";
        string dataYear = "";
        string cityname = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            type = (Request["type"] == "" || Request["type"] == null) ? "" : Request["type"];
            strcat = (Request["cat"] == "" || Request["cat"] == null) ? "" : Request["cat"];
            strCityUrl = (Request["city"] == "" || Request["city"] == null) ? "" : Request["city"];
            strarea = (Request["area"] == "" || Request["area"] == null) ? "" : Request["area"];
            infoID = BWCommon.GetRequestInteger(0, Request, "infoid");
            areaID = BWCommon.GetRequestInteger(0, Request, "areaid");
            categoryID = BWCommon.GetRequestInteger(0, Request, "catid");
            contractorID = BWCommon.GetRequestInteger(0, Request, "cid");
            rsummary = (Request["rsummary"] == "" || Request["rsummary"] == null) ? "" : Request["rsummary"];
            cityname = (Request["cityname"] == "" || Request["cityname"] == null) ? "" : Request["cityname"];

            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            if (areaID > 0)
                dataYear = AreaDL.GetMobileYear(areaID);

            
            if (!IsPostBack)
            {
                string sql = "";
                using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
                {
                    conn.Open();

                    #region Category
                    if (type == "category")
                    {
                        sql = "SELECT * FROM Info WHERE (InfoID = @ID)";
                        SqlDataReader rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql, new SqlParameter("@ID", infoID));

                        if (rdr.Read())
                        {
                            string catname = rdr["displayname"].ToString();
                            CategoryName.Text = catname;

                            IconImage.AlternateText = cityname + " " + catname;
                            IconImage.ToolTip = cityname + " " + catname;
                        }
                        rdr.Close();

                        //Get category icon from global category level
                        sql = "SELECT Website FROM Info WHERE (InfoType = 1) AND (ReferenceID = @CATID)";
                        object o = DataAccessHelper.Data.ExecuteScalar(sql,
                            new SqlParameter("@CATID", categoryID));
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

                        CategoryContent.Visible = true;
                    }
                    #endregion
                    #region Contractor List
                    else if (type == "contractorlist")
                    {
                        ContractorDL currentContractor = ContractorDL.GetByInfoID(infoID);

                        if (currentContractor != null)
                        {
                            ContractorName.Text = currentContractor.name;

                            RibbonImage.ImageUrl = "/images/ribbon_cat_" + dataYear + ".png";
                            RibbonImage.AlternateText = dataYear + " EBSCO Research Best Pick";
                            RibbonImage.ToolTip = dataYear + " EBSCO Research Best Pick";
                            RibbonImage.Width = 32;

                            string link = basedomain + "/" + strcat + "/" + strCityUrl + "/" + strarea + "/" + currentContractor.urlName;

                            ContractorName.NavigateUrl = link;
                            ContractorName.Target = "_parent";
                            ReadMoreLink.HRef = link;
                            ReadMoreLink.Target = "_parent";

                            PhoneDL phoneNumber = null;
                            if (refertype == "0")
                                phoneNumber = currentContractor.phoneNumbers.FirstOrDefault(p => p.type == PhoneType.regular);
                            else if (refertype == "1")
                                phoneNumber = currentContractor.phoneNumbers.FirstOrDefault(p => p.type == PhoneType.organic);
                            else if (refertype == "2")
                                phoneNumber = currentContractor.phoneNumbers.FirstOrDefault(p => p.type == PhoneType.ppc);
                            else if (refertype == "3")
                                phoneNumber = currentContractor.phoneNumbers.FirstOrDefault(p => p.type == PhoneType.ppc);

                            if (phoneNumber == null)
                                phoneNumber = currentContractor.phoneNumbers.FirstOrDefault(p => p.type == PhoneType.regular);

                            if (phoneNumber != null)
                                ContractorPhone.HRef = "tel:" + phoneNumber.number;

                            BestPickText.Text = currentContractor.bestPickText;

                            // See if we have an email address
                            if (!string.IsNullOrEmpty(currentContractor.email))
                            {
                                int contractorCategoryAreaID = GetContractorCategoryAreaID(conn);

                                EmailPanel.Visible = true;
                                SendEmailButton.Attributes.Add("onclick", "parent.showEmailform(" +
                                    "{cid: '" + contractorCategoryAreaID + "', " +
                                    "email: '" + currentContractor.email + "', " +
                                    "name: '" + currentContractor.name.Replace("'", "\\'") + "'});");
                            }
                        }

                        ContractorListContent.Visible = true;
                    }
                    #endregion
                    #region Contractor
                    else if (type == "contractor")
                    {
                        ContractorDL currentContractor = ContractorDL.GetByInfoID(infoID);

                        if (currentContractor != null)
                        {
                            // string conurl = currentContractor.urlName;
                            ContractorContactorName.Text = currentContractor.name;

                            ContractorRibbon.ImageUrl = "/images/" + dataYear + "_pic_company.png";
                            ContractorRibbon.AlternateText = dataYear + " EBSCO Research Best Pick";
                            ContractorRibbon.ToolTip = dataYear + " EBSCO Research Best Pick";

                            PhoneDL phoneNumber = null;
                            if (refertype == "0")
                                phoneNumber =
                                    currentContractor.phoneNumbers.Where(p => p.type == PhoneType.regular)
                                        .FirstOrDefault();
                            else if (refertype == "1")
                                phoneNumber =
                                    currentContractor.phoneNumbers.Where(p => p.type == PhoneType.organic)
                                        .FirstOrDefault();
                            else if (refertype == "2")
                                phoneNumber =
                                    currentContractor.phoneNumbers.Where(p => p.type == PhoneType.ppc).FirstOrDefault();
                            else if (refertype == "3")
                                phoneNumber =
                                    currentContractor.phoneNumbers.Where(p => p.type == PhoneType.ppc).FirstOrDefault();

                            if (phoneNumber == null)
                                phoneNumber =
                                    currentContractor.phoneNumbers.Where(p => p.type == PhoneType.regular)
                                        .FirstOrDefault();

                            if (phoneNumber != null)
                            {
                                CallPhone.NavigateUrl = String.Concat("tel:", phoneNumber.number);
                            }
                            else
                            {
                                CallPhone.CssClass = "button-big contact-btn call-btn disabled";
                            }

                            ContractorBestPickText.Text = currentContractor.bestPickText;

                            if (!String.IsNullOrEmpty(currentContractor.servicesOffered))
                            {
                                ServiceOfferedText.Text = currentContractor.servicesOffered;
                                ServiceOffered.Visible = true;
                            }

                            if (!String.IsNullOrEmpty(currentContractor.servicesNotOffered))
                            {
                                ServiceNotOfferedText.Text = currentContractor.servicesNotOffered;
                                ServiceNotOffered.Visible = true;
                            }

                            if (!String.IsNullOrEmpty(currentContractor.specializations))
                            {
                                SpecializationsText.Text = currentContractor.specializations;
                                Specializations.Visible = true;
                            }

                            if (!String.IsNullOrEmpty(currentContractor.minimumJob))
                            {
                                MinimumJobText.Text = currentContractor.minimumJob;
                                MinimumJob.Visible = true;
                            }

                            if (!String.IsNullOrEmpty(currentContractor.warranty))
                            {
                                WarrantyText.Text = currentContractor.warranty;
                                Warranty.Visible = true;
                            }

                            if (!String.IsNullOrEmpty(currentContractor.licenseNumber))
                            {
                                LicensesText.Text = currentContractor.licenseNumber;
                                License.Visible = true;
                            }

                            if (currentContractor.workersCompensation != null)
                            {
                                switch (currentContractor.workersCompensation)
                                {
                                    case 'V':
                                        Insurance.Text = "Verified Workers'<br/>Comp";
                                        Insurance.Visible = true;
                                        break;
                                    case 'E':
                                        Insurance.Text = "Workers' Comp<br/>Exempt";
                                        Insurance.Visible = true;
                                        break;
                                }
                            }

                            if (currentContractor.hasLiability)
                            {
                                Liability.Visible = true;
                            }

                            if (!String.IsNullOrEmpty(currentContractor.awardsAndCertifications))
                            {
                                AwardsCertificationsText.Text = currentContractor.awardsAndCertifications;
                                AwardsCertifications.Visible = true;
                            }

                            if (!String.IsNullOrEmpty(currentContractor.organizations))
                            {
                                OrganizationsText.Text = currentContractor.organizations;
                                Organizations.Visible = true;
                            }

                            if (!String.IsNullOrEmpty(currentContractor.companyHistory))
                            {
                                CompanyHistoryText.Text = currentContractor.companyHistory;
                                CompanyHistory.Visible = true;
                            }

                            if (!String.IsNullOrEmpty(currentContractor.employeeInformation))
                            {
                                EmployeeInformationText.Text = currentContractor.employeeInformation;
                                EmployeeInformation.Visible = true;
                            }

                            if (!String.IsNullOrEmpty(currentContractor.productInformation))
                            {
                                ProductInformationText.Text = currentContractor.productInformation;
                                ProductInformation.Visible = true;
                            }

                            if (!String.IsNullOrEmpty(currentContractor.hrStatus))
                            {
                                HonorableMentionStatusText.Text = currentContractor.hrStatus;
                                HonorableMentionStatus.Visible = true;
                            }

                            if (!String.IsNullOrEmpty(currentContractor.additionalInformation))
                            {
                                AdditionalInformationText.Text = currentContractor.additionalInformation;
                                AdditionalInformation.Visible = true;
                            }

                            if (!String.IsNullOrEmpty(currentContractor.reviewSummary))
                            {
                                ReviewSummary.Text = currentContractor.reviewSummary.Replace(":", String.Empty);
                                ReviewSummary.Visible = true;
                            }

                            if (String.IsNullOrEmpty(currentContractor.email))
                            {
                                SendEmailButtonContractor.CssClass = "button-big contact-btn email-btn disabled";
                            }
                            else
                            {
                                int contractorCategoryAreaID = GetContractorCategoryAreaID(conn);

                                SendEmailButtonContractor.Attributes.Add("onclick", "parent.showEmailform(" +
                                    "{cid: '" + contractorCategoryAreaID + "', " +
                                    "email: '" + currentContractor.email + "', " +
                                    "name: '" + currentContractor.name.Replace("'", "\\'") + "'});");
                            }
                        }

                        ContractorContent.Visible = true;
                    }
                    #endregion
                    #region Testimonials
                    else if (type == "testimonials")
                    {
                        ReviewSummary.Text = rsummary;
                        if (rsummary.Trim() != "")
                            ReviewSummary.Visible = true;

                        TestimonialContent.Visible = true;
                    }
                    #endregion

                    conn.Close();
                }
            }
        }

        protected void dpgTestimonials_PreRender(object sender, EventArgs e)
        {
            List<Testimonial> testimonialList = Testimonial.GetByContractorIDCategoryIDAreaID(contractorID, categoryID, areaID, DateTime.Now);
            testimonialList.Sort((x, y) => y.publishedDate.CompareTo(x.publishedDate));

            TestimonialList.DataSource = testimonialList;
            TestimonialList.DataBind();
        }

        private int GetContractorCategoryAreaID(SqlConnection connection)
        {
            int contractorCategoryAreaID = 0;

            string sqlCommand =
                @"SELECT ContractorCategoryAreaID FROM ContractorCategoryArea CCA JOIN ContractorCategory CC ON (CCA.ContractorCategoryID = CC.ContractorCategoryID) WHERE CC.CategoryID = @categoryID AND CC.ContractorID = @contractorID AND CCA.AreaID = @areaID";

            SqlDataReader rdr = DataAccessHelper.Data.ExecuteDatareader(connection, sqlCommand,
                new[]
                {
                    new SqlParameter("@categoryID", categoryID),
                    new SqlParameter("@contractorID", contractorID),
                    new SqlParameter("@areaID", areaID)
                });

            if (rdr.Read())
            {
                int.TryParse(rdr["ContractorCategoryAreaID"].ToString(), out contractorCategoryAreaID);
            }
            rdr.Close();
        
            return contractorCategoryAreaID;
        }
    }
}
