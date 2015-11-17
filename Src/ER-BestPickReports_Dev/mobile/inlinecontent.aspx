<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="inlinecontent.aspx.cs" Inherits="ER_BestPickReports_Dev.mobile.inlinecontent" %>

<!DOCTYPE html>
<!--[if IE 7]> <html class="ie7 oldie" lang="en"> <![endif]-->
<!--[if IE 8]> <html class="ie8 oldie" lang="en"> <![endif]-->
<!--[if IE 9]> <html class="ie9" lang="en"> <![endif]-->
<!--[if gt IE 9]><!--> <html lang="en"> <!--<![endif]-->
<head>
    <meta charset="utf-8">
	<title></title>
    <script>
        (function (d) {
            var config = {
                kitId: 'fgn0ait',
                scriptTimeout: 3000
            },
        h = d.documentElement, t = setTimeout(function () { h.className = h.className.replace(/\bwf-loading\b/g, "") + " wf-inactive"; }, config.scriptTimeout), tk = d.createElement("script"), f = false, s = d.getElementsByTagName("script")[0], a; h.className += " wf-loading"; tk.src = '//use.typekit.net/' + config.kitId + '.js'; tk.async = true; tk.onload = tk.onreadystatechange = function () { a = this.readyState; if (f || a && a != "complete" && a != "loaded") return; f = true; clearTimeout(t); try { Typekit.load(config) } catch (e) { } }; s.parentNode.insertBefore(tk, s)
        })(document);
    </script> 
	<!--[if lt IE 9]>
		<script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
	<![endif]-->
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/sprite")%>
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/global")%>
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/master")%>
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/testimonials")%>
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/contractor")%>
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/category")%>
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/style_inline")%>
</head>
<body>
    <form id="form1" runat="server">
    
    <asp:Panel runat="server" ID="CategoryContent" class="featured-cat" Visible="false">
        <div class="category-header category-container">
            <asp:Image runat="server" ID="IconImage" class="category-categoryIcon" />
		    <h1 class="category-categoryName category-container-content"><asp:Literal runat="server" ID="CategoryName"></asp:Literal></h1>
        </div>
        <div style="clear:both;"></div>
    </asp:Panel>

    <asp:Panel runat="server" ID="ContractorListContent" Visible="false">
        <div class="category-contractor-item category-container contractor-item">
			<div class="category-ribbonIcon"><asp:Image runat="server" ID="RibbonImage" /></div>

            <div class="category-contractor-info category-container-content">
				<h3 class="category-contractor-name contractor-name"><asp:Hyperlink runat="server" ID="ContractorName"></asp:Hyperlink></h3>
                <h4><asp:Literal runat="server" ID="BestPickText"></asp:Literal></h4>

                <div class="category-contractor-contactList">
                    <div class="category-contractor-contact">
                        <a runat="server" ID="ReadMoreLink" class="" rel="nofollow">
                            <div class="sprite category-info category-contractor-contact-icon"></div>
		                    <span>Profile</span>
                        </a>
                    </div>
					<asp:Panel runat="server" ID="EmailPanel" Visible="false" CssClass="category-contractor-contact">
                        <a runat="server" ID="SendEmailButton" class="contractor-email-link">
                            <div class="sprite category-email category-contractor-contact-icon"></div>
                            <span>Email</span>
                        </a>
                    </asp:Panel>
					<div class="category-contractor-contact">
                        <a runat="server" ID="ContractorPhone" class="" rel="nofollow">
                            <div class="sprite category-call category-contractor-contact-icon"></div>
		                    <span>Call</span>
                        </a>
                    </div>
                </div>
            </div>
		</div>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="ContractorContent" Visible="false" CssClass="contractor-item">
        <div class="head">
		    <asp:Image runat="server" ID="ContractorRibbon" CssClass="ico" />
            <div class="contractor-title">
			    <div class="contractor-name-wrapper"><h1 class="contractor-name"><asp:Literal runat="server" ID="ContractorContactorName"></asp:Literal></h1></div>
			    <div class="best-pick-text"><asp:Label runat="server" ID="ContractorBestPickText"></asp:Label></div>
            </div>
	    </div>
        <div class="contact-buttons">
            <asp:HyperLink runat="server" ID="SendEmailButtonContractor" class="button-big contact-btn email-btn contractor-email-link"><div class="action-icon sprite contractor-email"></div>Email</asp:HyperLink>
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
    </asp:Panel>

    <asp:Panel runat="server" ID="TestimonialContent" Visible="false">
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
    </asp:Panel>
    </form>
</body>
</html>
