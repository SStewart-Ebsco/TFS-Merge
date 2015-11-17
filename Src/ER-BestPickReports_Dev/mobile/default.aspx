<%@ Page Title="" Language="C#" MasterPageFile="~/mobile/SiteMobile.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ER_BestPickReports_Dev.mobile._default" %>
<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/default")%>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
    <span class="home-title">Best Pick Reports</span>
    <span class="home-subtitle">The clear choice for home service recommendations</span>
    <div class="home-video">
    	<iframe width="560" height="315" src="http://youtube.com/embed/RnkoYGxHhyg?autoplay=1&autoplay=1" frameborder="0" allowfullscreen></iframe>
    </div>
    <button id="home-category-link" class="button-big">Browse All Service Categories</button>

	<div class="tip-area">
		<hr class="home-line-triangle-top">
		<div class="container">
			<ui class="tip-area-list">
				<li id="seasonal-home-care-tips" class="tip-area-list-item tip-area-list-item-newsletter">
				    <div class="sprite home-newsletter tip-area-list-item-marker"></div>
                    <asp:HyperLink runat="server" NavigateUrl="~/newsletter-sign-up" CssClass="tip-area-list-item-title" ID="NewsletterSignUp" ClientIDMode="Static"> 
					Sign up for Seasonal Home Care Tips and Articles
                    </asp:HyperLink>
				</li>
				<li id="requestaguide" class="tip-area-list-item tip-area-list-item-requestaguide">
				    <div class="sprite home-request tip-area-list-item-marker"></div>
                    <asp:HyperLink runat="server" NavigateUrl="~/requestaguide" CssClass="tip-area-list-item-title" ID="RequestAGuide" ClientIDMode="Static">
                    Request a <%=DateTime.Now.Year%> Guide
                    </asp:HyperLink>
				</li>
			</ui>
		</div>
		<hr class="home-line-triangle-bottom">
	</div>


    <asp:Panel ID="TopCategoriesPanel" runat="server" Visible="false">
        <div class="ribbon-banner">
		    <span class="ribbon-banner-title">Top Categories</span>
	    </div>

	    <asp:Repeater runat="server" ID="TopCategories" OnItemDataBound="TopCategories_ItemDataBound" EnableViewState="False">
		    <HeaderTemplate>
			    <ul class="top-categories">
		    </HeaderTemplate>
		    <ItemTemplate>
			    <li class="top-category">
				    <a href="" ID="TopCategoryLink" runat="server" class="top-category-link">
                        <asp:Image ID="TopCategoryIcon" runat="server" CssClass="top-category-icon" />
                        <asp:Label ID="TopCategoryTitle" Text="#title#" runat="server" CssClass="top-category-title"/>
				    </a>
			    </li>
		    </ItemTemplate>
		    <FooterTemplate></ul></FooterTemplate>
	    </asp:Repeater>
    </asp:Panel>
	
    <div class="home-section">
	    <div class="ribbon-banner">
		    <span class="ribbon-banner-title">The Best Pick Difference</span>
	    </div>
        
		<asp:Image width="119" height="202" ID="RibbonImage" CssClass="reward-big" runat="server" />

	    <p class="home-text">For the past 18 years, the Best Pick ribbon has represented a commitment to quality and service. Companies cannot buy this award. A company earns our Best Pick certification when our research has determined it to be exceptional with regard to its quality of work and customer service.</p>
	    <p class="home-text">When you see the ribbon, you can feel confident about your home service provider selection, because each Best Pick has excelled in every step of our research process.</p>

	    <h3 class="home-company-requirements-title">Best Pick&trade; Companies found in our report:</h3>
	    <ul class="home-company-requirements-list">
		    <li class="home-company-requirements-item">Are required to maintain an A grade annually</li>
		    <li class="home-company-requirements-item">Average over 100 homeowner reviews each</li>
		    <li class="home-company-requirements-item">Hold proper licenses and insurance</li>
		    <li class="home-company-requirements-item">Must requalify each year</li>
	    </ul>
    </div>


	<div class="home-section">
	    <div class="ribbon-banner">
		    <span class="ribbon-banner-title">Check Out Our Blog</span>
	    </div>

        <div class="post-thumbtack sprite home-thumbtack"></div>
	    <div id="go-to-post-btn" class="post sprite home-example-post-background">
		    <div class="post-datetitle">
			    <div class="date sprite post-calendar">
			        <span class="date-month">May</span>
		            <span class="date-date">14</span>
	            </div>
			    <h2 class="post-title">
				    Learn About the Tile Installation<br>Process for Your Flooring Project
			    </h2>
		    </div>
		    <div class="posttext">
			    <div class="post-image sprite home-example-post-image">
				    <%--<img src="/mobile/images/home-example-post-image.jpg" class="post-image-preview" />--%>
			    </div>

			    <div class="post-author">
				    <span class="post-author-name">Laura Mangum</span>
				    <span class="post-author-title">Technical Writer</span>
			    </div>
			    <span class="post-summary">
                Tile is a lovely, long-lasting flooring choice—given that it is installed properly. If the process is not executed correctly, there are increased risks for cracking, mildew, and other complications...
			    </span>
			    <div class="button">Continue Reading</div>
		    </div>
	    </div>
	
	    <p class="home-text">The Best Pick blog features weekly, in-depth articles—vetted by our Best Pick experts—on the latest industry trends and maintenance advice to help you stay informed while caring for your home.</p>

        <a href="/blog?redirect=false" class="button-big">Visit Our Home Improvement Blog</a>
    </div>


    <div class="home-section">
	    <div class="ribbon-banner">
		    <span class="ribbon-banner-title">On The Go?</span>
	    </div>

	    <div class="sprite home-logo-shine home-logo-shine-wrapper">
		    <div id="HomeLogo" class="home-logo sprite home-logo"></div>
	    </div>

	    <p class="home-text">Take Best Pick Reports with you! Our easy-to-use app has everything you need to find the Best Pick company you are looking for—anytime and anywhere.</p>
	    <p class="home-text">Download it for your iPhone® or Android™ smartphone today!</p>
    
        <button class="button-big" id="DownloadButton">Download Mobile App</button>
    </div>

    
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="scripts" runat="server">
    <%= System.Web.Optimization.Scripts.Render("~/Scripts/site-mobile/home")%>
</asp:Content>
