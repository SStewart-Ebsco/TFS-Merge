using System;
using System.Data.SqlClient;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using ER_BestPickReports_Dev.App_Code;

namespace ER_BestPickReports_Dev
{
    public partial class inlinecontent : BasePage
    {
        string type = "";
        string strcat = "";
        string strCityUrl = "";
        public string strcityname = "";
        string strarea = "";
        public int infoID = 0;
        int areaID = 0;
        int categoryID = 0;
        string concatid = "";
        int contractorID = 0;
        public string isexpanded = "false";
        string basedomain = "";
        string rsummary = "";
        string dataYear = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            type = (Request["type"] == "" || Request["type"] == null) ? "" : Request["type"].ToString();
            strcat = (Request["cat"] == "" || Request["cat"] == null) ? "" : Request["cat"].ToString();
            strCityUrl = (Request["city"] == "" || Request["city"] == null) ? "" : Request["city"].ToString();
            strcityname = (Request["cityname"] == "" || Request["cityname"] == null) ? "" : Request["cityname"].ToString();
            strarea = (Request["area"] == "" || Request["area"] == null) ? "" : Request["area"].ToString();
            infoID = BWCommon.GetRequestInteger(0, Request, "infoid");
            areaID = BWCommon.GetRequestInteger(0, Request, "areaid");
            categoryID = BWCommon.GetRequestInteger(0, Request, "catid");
            concatid = (Request["concatid"] == "" || Request["concatid"] == null) ? "" : Request["concatid"].ToString();
            contractorID = BWCommon.GetRequestInteger(0, Request, "cid");
            isexpanded = (Request["isexpanded"] == "false" || Request["isexpanded"] == null || Request["isexpanded"] == "") ? "false" : "true";
            rsummary = (Request["rsummary"] == "" || Request["rsummary"] == null) ? "" : Request["rsummary"].ToString();

            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            if (areaID > 0)
                dataYear = App_Code.AreaDL.GetMobileYear(areaID);

            
            if (!IsPostBack)
            {
                string sql = "";
                using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
                {
                    conn.Open();

                    if (type == "category")
                    {
                        sql = "SELECT * FROM Info WHERE (InfoID = @ID)";
                        SqlDataReader rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                            new SqlParameter("@ID", infoID));

                        if (rdr.Read())
                        {
                            CategoryName.Text = rdr["displayname"].ToString();

                            if (rdr["subtitle"].ToString() != "")
                                CategoryAbout.Text = rdr["subtitle"].ToString();
                            else
                            {
                                if (strcityname == "Northern Virginia")
                                    CategoryAbout.Text = "About " + strcityname + " " + rdr["displayname"].ToString();
                                else
                                    CategoryAbout.Text = "About Metro-" + strcityname + " " + rdr["displayname"].ToString();
                            }

                            IconImage.AlternateText = strcityname + " " + rdr["displayname"].ToString();
                            IconImage.ToolTip = strcityname + " " + rdr["displayname"].ToString();

                            Desc.Text = rdr["longdesc"].ToString();

                            // See if we have an extended desc
                            if (rdr["extdesc"].ToString() != "")
                            {
                                LongDesc.Text = rdr["extdesc"].ToString();
                                ShowHide.Visible = true;
                                ShowHideLink.Attributes.Add("onclick", "parent.showExt('" + infoID + "','" + categoryID + "','" + strcityname + "','" + isexpanded + "', '" + strCityUrl + "');");

                                if (isexpanded == "true")
                                {
                                    ShowHideLink.Attributes.Add("class", "active");
                                    ExtDesc.Attributes.Add("class", "active");
                                    ExtDesc.Visible = true;
                                }
                                else
                                {
                                    ExtDesc.Attributes.Add("class", "hidden");
                                }
                            }
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
                    else if (type == "contractorlist")
                    {
                        ContractorDL currentContractor = ContractorDL.GetByInfoID(infoID);

                        if (currentContractor != null)
                        {
                            ContractorName.Text = currentContractor.name;
                            ContractorName.NavigateUrl = basedomain + "/" + strcat + "/" + strCityUrl + "/" + strarea + "/" + currentContractor.urlName;
                            ContractorName.Target = "_parent";
                            ReadMoreLink.Text = "Read more about " + currentContractor.name;
                            ReadMoreLink.NavigateUrl = basedomain + "/" + strcat + "/" + strCityUrl + "/" + strarea + "/" + currentContractor.urlName;
                            ReadMoreLink.Target = "_parent";

                            //NoFollow for company links
                            ContractorName.Attributes.Add("rel", "nofollow");
                            ReadMoreLink.Attributes.Add("rel", "nofollow");

                            RibbonImage.ImageUrl = "/images/ribbon_cat_" + dataYear + ".png";
                            RibbonImage.AlternateText = dataYear + " EBSCO Research Best Pick";
                            RibbonImage.ToolTip = dataYear + " EBSCO Research Best Pick";
                            RibbonImage.Width = 32;

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
                                ContractorPhone.Text = phoneNumber.number;

                            BestPickText.Text = currentContractor.bestPickText;
                            ShortDesc.Text = currentContractor.shortDescription;

                            if (!String.IsNullOrEmpty(currentContractor.licenseNumber))
                                License.Text = "Verified Licenses";
                            else
                                licenseitem.Visible = false;

                            if (currentContractor.workersCompensation != null)
                            {
                                switch (currentContractor.workersCompensation)
                                {
                                    case 'V': Insurance.Text = "Verified Workers' Comp";
                                        break;
                                    case 'E': Insurance.Text = "Workers' Comp Exempt";
                                        break;
                                }
                            }
                            else
                                insurancestateitem.Visible = false;

                            if (currentContractor.hasLiability)
                                Liability.Text = "Verified General Liability Insurance";
                            else
                                liabilityitem.Visible = false;

                            // See if we have an email address
                            if (!String.IsNullOrEmpty(currentContractor.email))
                            {
                                EmailPanel.Visible = true;
                                SendEmailButton.Attributes.Add("onclick", "parent.showmodalemail('"
                                    + contractorID + "', '"
                                    + currentContractor.email + "', '"
                                    + currentContractor.name.Replace("'", "\\'") + "');");
                            }
                        }
                        ContractorListContent.Visible = true;
                    }
                    else if (type == "contractor")
                    {
                        ContractorDL currentContractor = ContractorDL.GetByInfoID(infoID);

                        if (currentContractor != null)
                        {
                            ContractorName_Detail.Text = currentContractor.name;

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
                                ContractorPhone_Detail.Text = phoneNumber.number;

                            BestPickText_Detail.Text = currentContractor.bestPickText;

                            ContractorRibbon.ImageUrl = "/images/" + dataYear + "_pic_company.png";
                            ContractorRibbon.AlternateText = dataYear + " EBSCO Research Best Pick";
                            ContractorRibbon.ToolTip = dataYear + " EBSCO Research Best Pick";


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

                            // See if we have an email address
                            if (!String.IsNullOrEmpty(currentContractor.email))
                            {
                                EmailPanel_Detail.Visible = true;
                                SendEmailButton_Detail.Attributes.Add("onclick", "parent.showmodalemail('"
                                    + contractorID + "', '"
                                    + currentContractor.email + "', '"
                                    + currentContractor.name.Replace("'", "\\'") + "');");
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
                        }

                        //Get areas served
                        int cityID = CityDL.GetIDByUrl(strCityUrl);
                        List<AreaDL> contractorAreas = AreaDL.GetByContractorIDCityIDCategoryID(contractorID, cityID, categoryID);

                        string areas = "";
                        foreach (AreaDL a in contractorAreas)
                        {
                            areas += a.name.Trim() + ", ";
                        }
                        char[] trimChars = new char[] { ' ', ',' };
                        areas = areas.TrimEnd(trimChars);
                        AreasServed.Text = areas;

                        ContractorContent.Visible = true;
                    }
                    else if (type == "testimonials")
                    {
                        ReviewSummary.Text = rsummary;
                        if (rsummary.Trim() != "")
                            ReviewSummary.Visible = true;

                        TestimonialContent.Visible = true;
                    }

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
    }
}
