using System;
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

namespace ER_BestPickReports_Dev.mobile
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

                //Fill fields of email form
                HiddenListCityId.Value = cityID.ToString();
                HiddenListAreaId.Value = areaID.ToString();
                HiddenListCategoryId.Value = categoryID.ToString();
                HiddenListIsPpc.Value = isppc.ToString();
            }

            if (cityID <= 0 || areaID <= 0 || categoryID <= 0 || contractorID <= 0)
                Page.Response.Redirect("Default.aspx");

            //Set cityid and areaid on master page
            ((ER_BestPickReports_Dev.mobile.SiteMobileMaster)Master).cityID = cityID;
            ((ER_BestPickReports_Dev.mobile.SiteMobileMaster)Master).areaID = areaID;
            ((ER_BestPickReports_Dev.mobile.SiteMobileMaster)Master).catid = categoryID.ToString();
            ((ER_BestPickReports_Dev.mobile.SiteMobileMaster)Master).contractorid = contractorID.ToString();

            //Set refertype to ppc=2 if this is a ppc page
            if (bool.Parse(isppc))
                refertype = "2";
            else
            {
                if (refertype != "1" && refertype != "3")
                    refertype = "0";
            }

            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"] != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            //Get data year
            dataYear = CityDL.GetMobileYear(cityID);

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
                    {
                        CallPhone.NavigateUrl = String.Concat("tel:", phoneNumber.number);
                    }
                    else
                    {
                        CallPhone.CssClass = "button-big contact-btn call-btn disabled";
                    }

                    BestPickText.Text = currentContractor.bestPickText;

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
                        SendEmailButton.CssClass = "button-big contact-btn email-btn disabled";
                    }
                    else
                    {
                        HiddenListID.Value = contractorCategoryAreaID.ToString();
                        HiddenListEmail.Value = currentContractor.email;
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

            var categoryInfo = CategoriesDL.GetCategoryInfo(areaID, cityID, categoryID);

            CategoryBreadLink.Text = categoryInfo.Name;

            if (bool.Parse(isppc))
                CategoryBreadLink.NavigateUrl = String.Concat("/ppc", categoryInfo.CatUrlName);
            else
                CategoryBreadLink.NavigateUrl = categoryInfo.CatUrlName;

            int nextId = categoryInfo.CatUrlName.IndexOf('/', 1);
            int lastId = categoryInfo.CatUrlName.LastIndexOf('/');

            caturl = categoryInfo.CatUrlName.Substring(1, nextId - 1);
            cityurl = categoryInfo.CatUrlName.Substring(nextId + 1, lastId - nextId - 1);
            areaurl = categoryInfo.CatUrlName.Substring(lastId + 1);
            //citydisplayname = rdr["CityDisplayName"].ToString();
            //areadisplayname = rdr["AreaDisplayName"].ToString();



            //Set cookie for city/area preference based on url values
            bprPreferences.CityId = cityID;
            bprPreferences.CityName = citydisplayname;
            bprPreferences.CityUrlName = cityurl;
            bprPreferences.AreaId = areaID;
            bprPreferences.AreaName = areadisplayname;
            bprPreferences.AreaUrlName = areaurl;
            bprPreferences.SetExpiration(DateTime.Now.AddDays(365));
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
