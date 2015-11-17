<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="contractor.aspx.cs" Inherits="ER_BestPickReports_Dev.contractor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<link rel="stylesheet" type="text/css" href="/css/TestimonialStyle.css"/>
<div class="cat-page">
    <div class="container">
	    <div id="main-content">
		    <div class="back-link"><asp:HyperLink runat="server" ID="CategoryBreadLink"></asp:HyperLink></div>
		    <article class="detail-item">

                <asp:Panel runat="server" ID="ContractorContent_Frame" Visible="false">
                    <iframe src="/inlinecontent.aspx?type=contractor&infoid=<%=coninfoid%>&concatid=<%=concatid%>&cid=<%=contractorID%>&catid=<%=categoryID%>&areaid=<%=areaID%>&city=<%=strcity%>" frameborder="0" width="100%" height="100%" scrolling="no" class="autoHeight"></iframe>
                </asp:Panel>

                <asp:Panel runat="server" ID="ContractorContent">
                    <div class="head">
				        <div class="ico"><asp:Image runat="server" ID="ContractorRibbon" /></div>
				        <div class="phone-c"><asp:Literal runat="server" ID="ContractorPhone"></asp:Literal></div>
				        <div class="h-h"><h1><asp:Literal runat="server" ID="ContractorName"></asp:Literal></h1></div>
				        <asp:Panel runat="server" ID="EmailPanel" Visible="false" CssClass="request">
                            <asp:LinkButton runat="server" ID="SendEmailButton" OnClick="EmailButton_Click" Text="Request Information"></asp:LinkButton>
                        </asp:Panel>
				        <h2><asp:Literal runat="server" ID="BestPickText"></asp:Literal></h2>
			        </div>
			        <blockquote>
				        <p><asp:Literal runat="server" ID="Quote"></asp:Literal></p>
				        <p runat="server" id="quoteinfo" visible="false" class="author"><strong><asp:Literal runat="server" ID="QuoteName"></asp:Literal></strong><asp:Literal runat="server" ID="QuoteTitle"></asp:Literal></p>
			        </blockquote>
			        <div class="served-items">
				        <div class="header">
					        <h3>Areas Served:</h3> 
					        <p><asp:Literal runat="server" ID="AreasServed"></asp:Literal></p>
				        </div>
				        <ul>
					        <li runat="server" id="servicesofferedrow" visible="false">
						        <div class="left">Services Offered</div>
						        <div class="right"><asp:Literal runat="server" ID="ServicesOffered"></asp:Literal></div>
					        </li>
                            <li runat="server" id="servicesnotofferedrow" visible="false">
						        <div class="left">Services Not Offered</div>
						        <div class="right"><asp:Literal runat="server" ID="ServicesNotOffered"></asp:Literal></div>
					        </li>
                            <li runat="server" id="specializationsrow" visible="false">
						        <div class="left">Specializations</div>
						        <div class="right"><asp:Literal runat="server" ID="Specializations"></asp:Literal></div>
					        </li>					        
                            <li runat="server" id="minimumjobamountrow" visible="false">
						        <div class="left">Minimum Job</div>
						        <div class="right"><asp:Literal runat="server" ID="MinimumJob"></asp:Literal></div>
					        </li>
					        <li runat="server" id="warrantyinformationrow" visible="false">
						        <div class="left">Warranty</div>
						        <div class="right"><asp:Literal runat="server" ID="Warranty"></asp:Literal></div>
					        </li>
                            <li runat="server" id="awardsandcertificationsrow" visible="false">
						        <div class="left">Awards & Certifications</div>
						        <div class="right"><asp:Literal runat="server" ID="AwardsCertifications"></asp:Literal></div>
					        </li>
                            <li runat="server" id="organizationsrow" visible="false">
						        <div class="left">Organizations</div>
						        <div class="right"><asp:Literal runat="server" ID="Organizations"></asp:Literal></div>
					        </li>
					        <li runat="server" id="companyhistoryrow" visible="false">
						        <div class="left">Company History</div>
						        <div class="right"><asp:Literal runat="server" ID="CompanyHistory"></asp:Literal></div>
					        </li>
					        <li runat="server" id="employeeinformationrow" visible="false">
						        <div class="left">Employee Information</div>
						        <div class="right"><asp:Literal runat="server" ID="EmployeeInformation"></asp:Literal></div>
					        </li>
					        <li runat="server" id="productinformationrow" visible="false">
						        <div class="left">Product Information</div>
						        <div class="right"><asp:Literal runat="server" ID="ProductInformation"></asp:Literal></div>
					        </li>
                            <li runat="server" id="additionalinformationrow" visible="false">
						        <div class="left">Additional Information</div>
						        <div class="right"><asp:Literal runat="server" ID="AdditionalInformation"></asp:Literal></div>
					        </li>
                            <li runat="server" id="liLicenses" visible="false">
						        <div class="left">Licenses</div>
						        <div class="right"><asp:Literal runat="server" ID="litLicenses"></asp:Literal></div>
					        </li>
                            <li runat="server" id="hrstatusrow" visible="false">
						        <div class="left">Honorable Mention Status</div>
						        <div class="right"><asp:Literal runat="server" ID="HRStatus"></asp:Literal></div>
					        </li>
				        </ul>
			        </div>
                </asp:Panel>
		    </article>
		    
            <div class="title">
				<h2>Select Quotes from Customer Interviews</h2>
			</div>

            <asp:Panel runat="server" ID="TestimonialContent_Frame" Visible="false">
                <iframe src="/inlinecontent.aspx?type=testimonials&cid=<%=contractorID%>&catid=<%=categoryID%>&areaid=<%=areaID%>&rsummary=<%=Server.UrlEncode(rsummary) %>" frameborder="0" width="100%" height="100%" scrolling="no" class="autoHeight"></iframe>
            </asp:Panel>

            <asp:Panel runat="server" ID="TestimonialContent">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <section class="interviews">
                        <h3><asp:Label runat="server" ID="ReviewSummary" Visible="false"></asp:Label></h3>
                        <br />
                        <div class="divCenter">
                            <asp:DataPager ID="dpgTestimonials" runat="server" PagedControlID="TestimonialList" PageSize="10" OnPreRender="dpgTestimonials_PreRender">
                                <Fields>
                                   <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowNextPageButton="False" 
                                        ButtonType="Button" ButtonCssClass="quoteButton" ShowPreviousPageButton="false"/>

                                    <asp:NextPreviousPagerField ShowNextPageButton="false" ButtonCssClass="quoteButtonPrevious" 
                                        ButtonType="Button" PreviousPageText=" " />
                                    
                                    <asp:NumericPagerField ButtonType="Button" CurrentPageLabelCssClass="quoteButtonSelected" 
                                         NumericButtonCssClass="quoteButton" NextPreviousButtonCssClass="quoteButtonElipse" NextPageText=" " PreviousPageText=" "
                                        ButtonCount="5"  />

                                        <asp:NextPreviousPagerField ShowPreviousPageButton="false" ButtonCssClass="quoteButtonNext" 
                                            ButtonType="Button" NextPageText=" "  />

                                    <asp:NextPreviousPagerField ShowLastPageButton="True" ShowPreviousPageButton="False" 
                                        ButtonType="Button" ButtonCssClass="quoteButton" ShowNextPageButton="false" />

                                    <asp:TemplatePagerField>
                                        <PagerTemplate>
                                            <br /><br />
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
                        <asp:ListView runat="server" ID="TestimonialList">
	                        <LayoutTemplate>
                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <article>
				                    <blockquote>
                                        <%# Eval("text") %>
                                    </blockquote>
                                </article>
                            </ItemTemplate>
	                    </asp:ListView>
                        <div class="divCenter">
                            <asp:DataPager ID="dpgTestimonials2" runat="server" PagedControlID="TestimonialList" PageSize="10" OnPreRender="dpgTestimonials_PreRender">
                                <Fields>
                                   <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowNextPageButton="False" 
                                        ButtonType="Button" ButtonCssClass="quoteButton" ShowPreviousPageButton="false"/>

                                    <asp:NextPreviousPagerField ShowNextPageButton="false" ButtonCssClass="quoteButtonPrevious" 
                                        ButtonType="Button" PreviousPageText=" " />
                                    
                                    <asp:NumericPagerField ButtonType="Button" CurrentPageLabelCssClass="quoteButtonSelected" 
                                         NumericButtonCssClass="quoteButton" NextPreviousButtonCssClass="quoteButtonElipse" NextPageText=" " PreviousPageText=" "
                                        ButtonCount="5"  />

                                        <asp:NextPreviousPagerField ShowPreviousPageButton="false" ButtonCssClass="quoteButtonNext" 
                                            ButtonType="Button" NextPageText=" "  />

                                    <asp:NextPreviousPagerField ShowLastPageButton="True" ShowPreviousPageButton="False" 
                                        ButtonType="Button" ButtonCssClass="quoteButton" ShowNextPageButton="false" />

                                    <asp:TemplatePagerField>
                                        <PagerTemplate>
                                            <br /><br />
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

	    </div>
	    <aside id="sidebar">
		    <div class="best-spicks">
			    <h3 class="no-img"><asp:Literal runat="server" ID="RightRailText" Text="About<br />Best Pick Reports&reg;"></asp:Literal></h3>
		    </div>
			<div class="videoBanner">
				<asp:HyperLink runat="server" ID="VideoBannerLink" ImageUrl="/images/banner_bpr_video.gif" EnableViewState="False" CssClass="videoButton fancybox.iframe">
				</asp:HyperLink>
			</div>
			<div class="download">
			    <h3>Download<br/>The Best Pick&trade; App</h3>
			    <div class="ico"><img src="/images/pic_app.png" alt="Best Pick App" /></div>
			    <div class="links">
				    <a href="http://itunes.apple.com/us/app/best-pick-reports/id494057962?mt=8" target="_blank"><img src="/images/btn_app_store.png" alt="Best Pick App for iPhone/iPad" /></a>
				    <a href="https://play.google.com/store/apps/details?id=com.brotherfish" target="_blank"><img src="/images/btn_play.png" alt="Best Pick App for Android" /></a>
			    </div>
		    </div>
		    <div class="proccess">
			    <h3>Our Company Screening Process</h3>
			    <div class="banner"><img src="/images/banner_research_process.png" alt="Our Company Screening Process" /></div>
		    </div>
		    
            <asp:Panel runat="server" ID="TipPanel" Visible="false" CssClass="articles">
                <h3>Helpful<br />Articles</h3>
                <asp:ListView runat="server" ID="ArticleList">
	                <LayoutTemplate>
                        <ul>
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li><a href="<%=basedomain %>/<%# Eval("CatName") %>/tips/<%# Eval("UrlTitle") %>" rel="nofollow"><%# Eval("Title") %></a></li>
                    </ItemTemplate>
	            </asp:ListView>
            </asp:Panel>
	    </aside>
    </div>
</div>

<asp:Panel runat="server" ID="FormContainer" DefaultButton="EmailOK" CssClass="registration-form">
    <asp:HiddenField runat="server" ID="EmailContractorID" Value="defaultvalue" />
    <asp:HiddenField runat="server" ID="ContractorEmail" />
    <asp:HiddenField runat="server" ID="HiddenContractorName" />

    
	<a href="#" onclick="hidemodalemail()" class="btn-close"></a>

    <div runat="server" id="ModalEmailForm">
    <h3>You are requesting more information from:<strong><asp:Label runat="server" ID="EmailContractorName"></asp:Label></strong></h3>
		<p class="note">Please fill out the following form completely:</p>
        <div class="fields">
		    <fieldset>
			    <label class="req">First Name:*</label>
			    <asp:TextBox runat="server" ID="Name"></asp:TextBox>
                <asp:Label runat="server" ID="ReqName" Text="First Name is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
		    <fieldset>
			    <label class="req">Last Name:*</label>
			    <asp:TextBox runat="server" ID="LastName"></asp:TextBox>
                <asp:Label runat="server" ID="ReqLastName" Text="Last Name is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
		    <fieldset class="full">
			    <label class="req">Street Address:*</label>
			    <asp:TextBox runat="server" ID="Address" Columns="38"></asp:TextBox>
                <asp:Label runat="server" ID="ReqAddress" Text="Address is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
		    <fieldset>
			    <label>City:</label>
			    <asp:TextBox runat="server" ID="City"></asp:TextBox>
		    </fieldset>
		    <fieldset class="req short">
			    <label class="req">Zip Code:*</label>
			    <asp:TextBox runat="server" ID="Zip" Columns="10"></asp:TextBox>
                <asp:Label runat="server" ID="ReqZip" Text="Zip Code is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
		    <fieldset class="medium">
			    <label class="req">Email:*</label>
			    <asp:TextBox runat="server" ID="Email" Columns="30"></asp:TextBox>
                <asp:Label runat="server" ID="ReqEmail" Text="A valid Email is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
		    <fieldset class="half">
			    <label class="req">Primary Phone:*</label>
			    <asp:TextBox runat="server" ID="FormPhone"></asp:TextBox>
                <asp:Label runat="server" ID="ReqPhone" Text="A valid Phone is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
		    <fieldset class="half">
			    <label>Alternate Phone:</label>
			    <asp:TextBox runat="server" ID="FormPhone2"></asp:TextBox>
		    </fieldset>
		    <fieldset class="medium">
			    <label class="req">Type of Work Needed:*</label>
			    <asp:TextBox runat="server" ID="WorkType" Columns="38"></asp:TextBox>
                <asp:Label runat="server" ID="ReqWorkType" Text="Type of Work is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
		    <fieldset class="full">
			    <label class="req">Message:*</label>
			    <asp:TextBox runat="server" ID="Message" TextMode="MultiLine" Rows="3" Columns="41"></asp:TextBox>
                <asp:Label runat="server" ID="ReqMessage" Text="Message is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
            <asp:Panel runat="server" ID="ReqPanel" Visible="false" CssClass="reqpanel">
                <p style="text-align:center; color:#ff0000;">*Please fill out all required fields.</p>
            </asp:Panel>
		    <fieldset class="submit">
			    <div class="cancel"><asp:LinkButton runat="server" ID="EmailCancel" Text="Cancel" OnClick="Cancel_EmailClick"></asp:LinkButton></div>
			    <asp:Button runat="server" ID="EmailOK" Text="Submit" OnClick="OK_EmailClick" />
		    </fieldset>
        </div>
    </div>
      
    <asp:Panel runat="server" ID="EmailComplete" CssClass="modalPopup modalPopupEmail" visible="false">
        <h3>Success!</h3>
        <p class="note">You have successfully completed the request for information form.  Your information will be forwarded on to the contractor, as well as to our help desk.</p>
        <fieldset class="submit">
            <asp:Button runat="server" ID="CloseEmail" Text="Close" OnClick="CloseEmail_Click" />
        </fieldset>
        <img src="http://aae.tradatracker.net/track/conv.gif?advertiser=5dec707028b05bcbd3a1db5640f842c5&action=BestPick_Lead" />
    </asp:Panel>

</asp:Panel>

<!-- Fake button to appease the MPE -->
<asp:Button runat="server" ID="EmailFakeButton" style="display:none" />
<cc1:ModalPopupExtender runat="server" ID="SendEmail" PopupControlID="FormContainer" TargetControlID="EmailFakeButton" BackgroundCssClass="overlay" PopupDragHandleControlID="EmptyPanel" RepositionMode="None"></cc1:ModalPopupExtender>
<asp:Panel runat="server" ID="EmptyPanel"></asp:Panel>

<%= System.Web.Optimization.Scripts.Render("~/Scripts/autoHeight")%>
<script type="text/javascript">
    function showmodalemail(id, email, name) {
        // set hidden form values
        document.getElementById('<%= EmailContractorID.ClientID %>').value = id;
        document.getElementById('<%= ContractorEmail.ClientID %>').value = email;
        document.getElementById('<%= EmailContractorName.ClientID %>').innerHTML = name;
        document.getElementById('<%= HiddenContractorName.ClientID %>').value = name;
        document.getElementById('<%= ModalEmailForm.ClientID %>').style.display = "block";

        var element = document.getElementById('<%= ReqPanel.ClientID %>');
        if (element != null && element.value != '') {
            element.style.display = "none";
        }

        document.getElementById('<%= Name.ClientID %>').value = "";
        document.getElementById('<%= LastName.ClientID %>').value = "";
        document.getElementById('<%= City.ClientID %>').value = "";
        document.getElementById('<%= Address.ClientID %>').value = "";
        document.getElementById('<%= Zip.ClientID %>').value = "";
        document.getElementById('<%= Email.ClientID %>').value = "";
        document.getElementById('<%= WorkType.ClientID %>').value = "";
        document.getElementById('<%= FormPhone.ClientID %>').value = "";
        document.getElementById('<%= FormPhone2.ClientID %>').value = "";
        document.getElementById('<%= Message.ClientID %>').value = "";

        $find('<%= SendEmail.ClientID %>').show();
    }

    function hidemodalemail() {
        $find('<%= SendEmail.ClientID %>').hide();
    }
</script>

</asp:Content>
