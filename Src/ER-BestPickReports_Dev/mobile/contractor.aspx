<%@ Page Title="" Language="C#" MasterPageFile="~/mobile/SiteMobile.Master" AutoEventWireup="true" CodeBehind="contractor.aspx.cs" Inherits="ER_BestPickReports_Dev.mobile.contractor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="HeadSection" ContentPlaceHolderID="head" runat="server">
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/testimonials")%>
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/contractor")%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
<div class="cat-page" id="#Top">
	    <div id="main-content">
		    <div class="back-to-label"><div class="back-arrow-background back-arrow"></div> Back to <asp:HyperLink runat="server" ID="CategoryBreadLink" CssClass="back-to-category-label"></asp:HyperLink></div>
		        <div class="detail-item">
		        
                    <asp:Panel runat="server" ID="ContractorContent_Frame" Visible="false">
                        <iframe src="/mobile/inlinecontent.aspx?type=contractor&infoid=<%=coninfoid%>&concatid=<%=concatid%>&cid=<%=contractorID%>&catid=<%=categoryID%>&areaid=<%=areaID%>&city=<%=strcity%>" frameborder="0" width="100%" height="100%" scrolling="no" class="autoHeight"></iframe>
                    </asp:Panel>

                    <asp:Panel runat="server" ID="ContractorContent" CssClass="contractor-item">
                        <div class="head">
				            <asp:Image runat="server" ID="ContractorRibbon" CssClass="ico" />
                            <div class="contractor-title">
				                <div class="contractor-name-wrapper"><h1 class="contractor-name"><asp:Literal runat="server" ID="ContractorName"></asp:Literal></h1></div>
				                <div class="best-pick-text"><asp:Label runat="server" ID="BestPickText"></asp:Label></div>
                            </div>
			            </div>
                        <div class="contact-buttons">
                            <asp:HyperLink runat="server" ID="SendEmailButton" class="button-big contact-btn email-btn contractor-email-link"><div class="action-icon sprite contractor-email"></div>Email</asp:HyperLink>
                            <asp:HyperLink runat="server" ID="CallPhone" class="button-big contact-btn call-btn"><div class="action-icon sprite contractor-call"></div>Call</asp:HyperLink>
                        </div>
                        <div class="license-insurance">
                            <asp:Label runat="server" ID="License" Visible="False">Verified<br/>License(s)</asp:Label>
                            <asp:Label runat="server" ID="Liability" Visible="False">Verified General<br/>Liability Insurance</asp:Label>
			                <asp:Label runat="server" ID="Insurance" Visible="False"></asp:Label>
                        </div>
                        <div class="contractor-details">
                            <asp:Panel runat="server" ID="ServiceOffered" Visible="False">
                                <div class="details-item"><span class="bold-text">Services Offered:&nbsp;</span><asp:Literal runat="server" ID="ServiceOfferedText"></asp:Literal></div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="ServiceNotOffered" Visible="False">
						            <div class="details-item"><span class="bold-text">Services Not Offered:&nbsp;</span><asp:Literal runat="server" ID="ServiceNotOfferedText"></asp:Literal></div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="Specializations" Visible="False">
						            <div class="details-item"><span class="bold-text">Specializations:&nbsp;</span><asp:Literal runat="server" ID="SpecializationsText"></asp:Literal></div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="MinimumJob" Visible="False">
						            <div class="details-item"><span class="bold-text">Minimum Job:&nbsp;</span><asp:Literal runat="server" ID="MinimumJobText"></asp:Literal></div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="Warranty" Visible="False">
						            <div class="details-item"><span class="bold-text">Warranty:&nbsp;</span><asp:Literal runat="server" ID="WarrantyText"></asp:Literal></div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="AwardsCertifications" Visible="False">
						            <div class="details-item"><span class="bold-text">Awards &amp; Certifications:&nbsp;</span><asp:Literal runat="server" ID="AwardsCertificationsText"></asp:Literal></div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="Organizations" Visible="False">
						            <div class="details-item"><span class="bold-text">Organizations:&nbsp;</span><asp:Literal runat="server" ID="OrganizationsText"></asp:Literal></div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="CompanyHistory" Visible="False">
						            <div class="details-item"><span class="bold-text">Company History:&nbsp;</span><asp:Literal runat="server" ID="CompanyHistoryText"></asp:Literal></div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="EmployeeInformation" Visible="False">
						            <div class="details-item"><span class="bold-text">Employee Information:&nbsp;</span><asp:Literal runat="server" ID="EmployeeInformationText"></asp:Literal></div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="ProductInformation" Visible="False">
						            <div class="details-item"><span class="bold-text">Product Information:&nbsp;</span><asp:Literal runat="server" ID="ProductInformationText"></asp:Literal></div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="AdditionalInformation" Visible="False">
						            <div class="details-item"><span class="bold-text">Additional Information:&nbsp;</span><asp:Literal runat="server" ID="AdditionalInformationText"></asp:Literal></div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="Licenses" Visible="False">
						            <div class="details-item"><span class="bold-text">Licenses:&nbsp;</span><asp:Literal runat="server" ID="LicensesText"></asp:Literal></div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="HonorableMentionStatus" Visible="False">
						            <div class="details-item"><span class="bold-text">Honorable Mention Status:&nbsp;</span><asp:Literal runat="server" ID="HonorableMentionStatusText"></asp:Literal></div>
                            </asp:Panel>
                        </div>


                        <input type="hidden" class="contractor-cid" runat="server" ID="HiddenListID" />
                        <input type="hidden" class="contractor-email" runat="server" ID="HiddenListEmail" />
		            </asp:Panel>
                </div>

                <div class="quotes">
                <hr class="home-line-triangle-top">
                <div class="title">
				    <h2>Select Quotes from<br />Customer Interviews</h2>
			    </div>

                <asp:Panel runat="server" ID="TestimonialContent_Frame" Visible="false">
                    <iframe src="/mobile/inlinecontent.aspx?type=testimonials&cid=<%=contractorID%>&catid=<%=categoryID%>&areaid=<%=areaID%>&rsummary=<%=Server.UrlEncode(rsummary) %>" frameborder="0" width="100%" height="100%" scrolling="no" class="autoHeight"></iframe>
                </asp:Panel>

                <asp:Panel runat="server" ID="TestimonialContent">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                        <ContentTemplate>
                            <section class="interviews">
                                <h3 class="testimonials-review"><asp:Label runat="server" ID="ReviewSummary" Visible="false"></asp:Label></h3>
                                <asp:ListView runat="server" ID="TestimonialList">
	                                <LayoutTemplate>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <article class="testimonial">
				                            <blockquote class="testimonial-quote">
                                                <%# Eval("text") %>
                                            </blockquote>
                                        </article>
                                    </ItemTemplate>
	                            </asp:ListView>
                                <div class="divCenter">
                                    <asp:DataPager ID="dpgTestimonials2" runat="server" PagedControlID="TestimonialList" PageSize="10" OnPreRender="dpgTestimonials_PreRender">
                                        <Fields>
                                           <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowNextPageButton="False"  RenderNonBreakingSpacesBetweenControls="False"
                                                ButtonType="Button" ButtonCssClass="quoteButton" ShowPreviousPageButton="false"/>

                                            <asp:NextPreviousPagerField ShowNextPageButton="false" ButtonCssClass="quoteButtonPrevious"  RenderNonBreakingSpacesBetweenControls="False"
                                                ButtonType="Button" PreviousPageText="" />
                                    
                                            <asp:NumericPagerField ButtonType="Button" CurrentPageLabelCssClass="quoteButtonSelected"
                                                 NumericButtonCssClass="quoteButton" NextPreviousButtonCssClass="quoteButtonElipse" RenderNonBreakingSpacesBetweenControls="False" ButtonCount="5" />

                                            <asp:NextPreviousPagerField ShowPreviousPageButton="false" RenderNonBreakingSpacesBetweenControls="False" ButtonCssClass="quoteButtonNext" 
                                                 ButtonType="Button" NextPageText="" />

                                            <asp:NextPreviousPagerField ShowLastPageButton="True" ShowPreviousPageButton="False" RenderNonBreakingSpacesBetweenControls="False"
                                                ButtonType="Button" ButtonCssClass="quoteButton" ShowNextPageButton="false" />

                                            <asp:TemplatePagerField>
                                                <PagerTemplate>
                                                    <div style="text-align:center; font:inherit;">
                                                        <h3> Page 
                                                            <asp:Label runat="server" ID="CurrentPageLabel" 
                                                                Text="<%# Container.TotalRowCount>0 ? (Container.StartRowIndex / Container.PageSize) + 1 : 0 %>" />
                                                            of
                                                            <asp:Label runat="server" ID="TotalPagesLabel" 
                                                            Text="<%# Math.Ceiling ((double)Container.TotalRowCount / Container.PageSize) %>" />
                                                        <br /> </h3>
                                                    </div>
                                                </PagerTemplate>
                                            </asp:TemplatePagerField>
                                        </Fields>
                                    </asp:DataPager>
                                </div>
                            </section>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>

                <a href="#Top" class="back-to-top">
                    <div class="sprite arrow-up"></div>
                    Back to Top
                </a>
            </div>

	        </div>
        </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
	
    <!-- Modal windows -->
    <asp:Panel runat="server" ID="ModalWindows" ClientIDMode="Static">

    <asp:HiddenField runat="server" ID="EmailContractorID" Value="defaultvalue" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="ContractorEmail" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="HiddenContractorName" ClientIDMode="Static" />

    <asp:HiddenField runat="server" ID="HiddenListCityId" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="HiddenListAreaId" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="HiddenListCategoryId" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="HiddenListIsPpc" ClientIDMode="Static" />

        <div runat="server" id="ModalEmailForm" class="emailform" ClientIDMode="Static">
            <div class="emailform-content container">
                <span class="emailform-header">You are requesting more information from:</span>
                <asp:Label runat="server" ID="EmailContractorName" CssClass="emailform-header-company"></asp:Label>
                <asp:HiddenField runat="server" ID="EmailFormContractorName"/>
                <hr class="emailform-line" />
                <span class="emailform-intro">Please fill out the following form completely:</span>
                <div class="emailform-field-container fifty">
                    <div class="emailform-field-label required">
                        First Name:</div>
                    <asp:TextBox runat="server" ID="Name" fieldName="firstName" type="text" CssClass="emailform-field"></asp:TextBox>
                    <asp:requiredfieldvalidator id="NameFieldValidator"
                        controltovalidate="Name"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your First Name"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                </div>
                <div class="emailform-field-container fifty">
                    <div class="emailform-field-label required">
                        Last Name:</div>
                    <asp:TextBox runat="server" ID="LastName" fieldName="lastName" type="text" CssClass="emailform-field"></asp:TextBox>
                    <asp:requiredfieldvalidator id="LastNameFieldValidator"
                        controltovalidate="LastName"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Last Name"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                </div>
                <div class="emailform-field-container">
                    <div class="emailform-field-label required">
                        Street Address:</div>
                    <asp:TextBox runat="server" ID="Address" fieldName="streetAddress" Columns="38" type="text"
                        CssClass="emailform-field"></asp:TextBox>
                    <asp:requiredfieldvalidator id="AddressFieldValidator"
                        controltovalidate="Address"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Address"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                </div>
                <div class="emailform-field-container sixty">
                    <div class="emailform-field-label ">
                        City:</div>
                    <asp:TextBox runat="server" ID="City" fieldName="city" type="text" CssClass="emailform-field"></asp:TextBox>
                </div>
                <div class="emailform-field-container forty">
                    <div class="emailform-field-label required">
                        Zip Code:</div>
                    <asp:TextBox runat="server" ID="ZipForMail" fieldName="zipCode" Columns="10" type="text"
                        CssClass="emailform-field"></asp:TextBox>
                    <asp:requiredfieldvalidator id="ZipFieldValidator"
                        controltovalidate="ZipForMail"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Zip Code"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                    <asp:RegularExpressionValidator id="ZipFieldValidator2"
                        ValidationExpression="^[0-9]{5}$"
                        controltovalidate="ZipForMail"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Zip Code correctly"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:RegularExpressionValidator>
                </div>
                <div class="emailform-field-container">
                    <div class="emailform-field-label required">
                        Email:</div>
                    <asp:TextBox runat="server" ID="Email" fieldName="email" Columns="30" type="text" CssClass="emailform-field"></asp:TextBox>
                    <asp:requiredfieldvalidator id="EmailFieldValidator"
                        controltovalidate="Email"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Email"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                    <asp:RegularExpressionValidator id="EmailFieldValidator2"
                        controltovalidate="Email"
                        ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Email correctly"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:RegularExpressionValidator>
                </div>
                <div class="emailform-field-container fifty ">
                    <div class="emailform-field-label required">
                        Primary Phone:</div>
                    <asp:TextBox runat="server" ID="FormPhone" fieldName="primaryPhone" type="phone" CssClass="emailform-field"></asp:TextBox>
                    <asp:requiredfieldvalidator id="FormPhoneFieldValidator"
                        controltovalidate="FormPhone"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Primary Phone"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                    <asp:RegularExpressionValidator id="FormPhoneFieldValidator2"
                        ValidationExpression="^\D*(\d{1}\D*){10}$"
                        controltovalidate="FormPhone"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Phone correctly."
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:RegularExpressionValidator>
                </div>
                <div class="emailform-field-container fifty">
                    <div class="emailform-field-label">
                        Alternate Phone:</div>
                    <asp:TextBox runat="server" ID="FormPhone2" fieldName="alternatePhone" type="text" CssClass="emailform-field"></asp:TextBox>
                    <asp:RegularExpressionValidator id="FormPhone2FieldValidator2"
                        ValidationExpression="^\D*(\d{1}\D*){10}$"
                        controltovalidate="FormPhone2"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Phone correctly."
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:RegularExpressionValidator>
                </div>
                <div class="emailform-field-container">
                    <div class="emailform-field-label required">
                        Type of Work:</div>
                    <asp:TextBox runat="server" ID="WorkType" fieldName="typeOfWork" Columns="38" type="text"
                        CssClass="emailform-field"></asp:TextBox>
                    <asp:requiredfieldvalidator id="WorkTypeFieldValidator"
                        controltovalidate="WorkType"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter Type of Work"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                </div>
                <div class="emailform-field-container">
                    <div class="emailform-field-label required">
                        Message:</div>
                    <asp:TextBox runat="server" ID="Message" fieldName="message" TextMode="MultiLine" Rows="3" Columns="41"
                        name="message" type="text" CssClass="emailform-field multiline"></asp:TextBox>
                    <asp:requiredfieldvalidator id="MessageFieldValidator"
                        controltovalidate="Message"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter Message"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                </div>
            </div>
                <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="True" DisplayMode="BulletList" ValidationGroup="EmailFormGroup"/>--%>
                <%--<p style="text-align: center; color: #ff0000;">
                    *Please fill out all required fields correctly.</p>--%>
            <div class="emailform-footer container">
                <div class="emailform-buttons">
                    <button id="cancelButton" class="emailform-button cancel-button">
                        Cancel</button>
                    <asp:Button runat="server" ID="SubmitEmailformButton" Text="Submit" CssClass="emailform-button submit-button"
                        CausesValidation="True" ValidationGroup="EmailFormGroup" ClientIDMode="Static"/>
                </div>
            </div>
        </div>

        <asp:Panel runat="server" ID="EmailComplete" CssClass="modal-win-center" ClientIDMode="Static">
            <div class="modal-win-bg">
            </div>
            <div class="modal-win-close">
            </div>
            <div class="modal-win">
                <div class="modal-win-body">
                    <span class="modal-win-header">Success!</span>
                    <hr class="modal-win-line" />
                    <span class="modal-win-text">Your email request has been succesfully delivered.</span>
                    <button class="modal-button">
                        Continue</button>
                </div>
            </div>
        </asp:Panel>
 
    </asp:Panel>

<%= System.Web.Optimization.Scripts.Render("~/Scripts/autoHeight")%>

</asp:Content>