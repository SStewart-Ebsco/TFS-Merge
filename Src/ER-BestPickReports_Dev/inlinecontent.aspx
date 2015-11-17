<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="inlinecontent.aspx.cs" Inherits="ER_BestPickReports_Dev.inlinecontent" %>

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
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-desktop/style_inline")%>
	<!--[if lt IE 9]>
		<script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
	<![endif]-->
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-desktop/TestimonialStyle")%>
</head>
<body>
    <form id="form1" runat="server">
    
    <asp:Panel runat="server" ID="CategoryContent" Visible="false" CssClass="featured-cat">
        <div class="head">
			<asp:Panel runat="server" ID="IconPanel" CssClass="ico"><asp:Image runat="server" ID="IconImage" /></asp:Panel>
			<h1><asp:Literal runat="server" ID="CategoryName"></asp:Literal></h1>
			<h2><asp:Literal runat="server" ID="CategoryAbout"></asp:Literal></h2>
		</div>
		<div class="entry">
			<asp:Literal runat="server" ID="Desc"></asp:Literal>
            <asp:Panel runat="server" ID="ExtDesc" Visible="false">
		        <asp:Literal runat="server" ID="LongDesc"></asp:Literal>
	        </asp:Panel>
            <asp:Panel runat="server" ID="ShowHide" Visible="false" CssClass="read-more">
	            <a href="#" runat="server" id="ShowHideLink">Read More Information <span></span></a>
            </asp:Panel>
		</div>
    </asp:Panel>

    <asp:Panel runat="server" ID="ContractorListContent" Visible="false" CssClass="best-picks">
        <div class="head">
		    <div class="ico"><asp:Image runat="server" ID="RibbonImage" /></div>
		    <div class="phone-c"><asp:Literal runat="server" ID="ContractorPhone"></asp:Literal></div>
		    <h3><asp:Hyperlink runat="server" ID="ContractorName"></asp:Hyperlink></h3>
		    <asp:Panel runat="server" ID="EmailPanel" Visible="false" CssClass="request">
                <asp:LinkButton runat="server" ID="SendEmailButton" Text="Request Information"></asp:LinkButton>
            </asp:Panel>
            <h4><asp:Literal runat="server" ID="BestPickText"></asp:Literal></h4>
	    </div>
	    <div class="features">
		    <ul>
			    <li runat="server" id="licenseitem"><asp:Literal runat="server" ID="License"></asp:Literal></li>
			    <li runat="server" id="liabilityitem"><asp:Literal runat="server" ID="Liability"></asp:Literal></li>
			    <li runat="server" id="insurancestateitem"><asp:Literal runat="server" ID="Insurance"></asp:Literal></li>
		    </ul>
	    </div>
	    <div class="desc">
		    <asp:Literal runat="server" ID="ShortDesc"></asp:Literal>
	    </div>
	    <div class="full"><asp:HyperLink runat="server" ID="ReadMoreLink">View company name's full profile and reviews</asp:HyperLink></div>
    </asp:Panel>

    <asp:Panel runat="server" ID="ContractorContent" Visible="false" CssClass="detail-item">
        <div class="head">
			<div class="ico"><asp:Image runat="server" ID="ContractorRibbon" /></div>
			<div class="phone-c"><asp:Literal runat="server" ID="ContractorPhone_Detail"></asp:Literal></div>
			<div class="h-h"><h1><asp:Literal runat="server" ID="ContractorName_Detail"></asp:Literal></h1></div>
			<asp:Panel runat="server" ID="EmailPanel_Detail" Visible="false" CssClass="request">
                <asp:LinkButton runat="server" ID="SendEmailButton_Detail" Text="Request Information"></asp:LinkButton>
            </asp:Panel>
			<h2><asp:Literal runat="server" ID="BestPickText_Detail"></asp:Literal></h2>
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

    <asp:Panel runat="server" ID="TestimonialContent" Visible="false">   
        <section class="interviews">
            <h3><asp:Label runat="server" ID="ReviewSummary" Visible="false"></asp:Label></h3>
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
                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="TestimonialList" PageSize="10" OnPreRender="dpgTestimonials_PreRender">
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
    </asp:Panel>
    </form>
</body>
</html>
