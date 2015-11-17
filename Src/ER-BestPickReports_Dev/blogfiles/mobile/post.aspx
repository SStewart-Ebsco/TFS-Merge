<%@ Page Title="" Language="C#" MasterPageFile="~/blogfiles/mobile/BlogMobile.Master" AutoEventWireup="true" CodeBehind="post.aspx.cs" Inherits="ER_BestPickReports_Dev.blogfiles.mobile.post" %>
<%@ MasterType VirtualPath="~/blogfiles/mobile/BlogMobile.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<input runat="server" ID="IsInMarketUser" class="hIsInMarketUser" type="hidden"/>
<div class="post" itemscope itemtype="http://schema.org/Article">
    <div class="post-datetitle container">
		<div class="date">
            <span class="date-month"><asp:Literal runat="server" ID="PostDateMonth"></asp:Literal></span>
            <span class="date-date"><asp:Literal runat="server" ID="PostDate"></asp:Literal></span>
        </div>
		<h2 class="post-title" itemprop="about"><asp:Literal runat="server" ID="PostTitle"></asp:Literal></h2>
	</div><!-- datetitle -->
    <div class="posttext container">
        <asp:Image runat="server" ID="PostImage" Visible="false" CssClass="post-image" />
        <div class="post-author" itemscope itemtype="http://schema.org/Person" itemprop="author">
            <span class="post-author-name" itemprop="name">
                <asp:Literal runat="server" ID="PostAuthorName"></asp:Literal>
            </span>
            <span class="post-author-title" itemprop="jobTitle">
                <asp:Literal runat="server" ID="PostAuthorTitle"></asp:Literal>
            </span>
        </div>
        <div class="post-content">
        	<asp:Literal runat="server" ID="PostBody"></asp:Literal>
        	<div class="post-local-best">
        		<span class="post-local-best-text"></span>
        	</div>
        </div>
    </div>
    <div class="post-footer container">
		<div class="share">
			<!-- AddThis Button BEGIN -->
            <div class="addthis_toolbox addthis_default_style ">
                <a class="addthis_button_facebook_like" <%="fb:like:layout"%>="button_count" <%="ffb:like:href"%>="http://www.facebook.com/BestPickReports"></a>
                <a class="addthis_button_tweet" <%="ftw:count"%>="none"></a>
                <a class="addthis_button_google_plusone" <%="fg:plusone:count"%>="false" <%="fg:plusone:size"%>="medium" <%="fg:plusone:href"%>="https://plus.google.com/115147162962806414209/"></a> 
                <a class="addthis_counter addthis_pill_style"></a>
            </div>
            <script type="text/javascript">                var addthis_config = { "data_track_addressbar": false };</script>
            <script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-5127eb9e4963bdca"></script>
			<!-- AddThis Button END -->
		</div><!-- share -->

        <asp:Panel runat="server" ID="CatPanel" CssClass="categories">
        	<ul class="categories-list">
				<asp:Literal runat="server" ID="Cats"></asp:Literal>
        	</ul>
		</asp:Panel>
		<ul class="tags categories-list">
			<asp:Literal runat="server" ID="Tags"></asp:Literal>
		</ul>
	</div><!-- top -->


	<asp:HyperLink ID="NextArticleLink" CssClass="post-pager newpost" runat="server">
		<div class="post-pager-header">
			<i class="fa fa-play-circle"></i>
			<span class="post-pager-header-content">Next Article</span>
			</div>
		<div class="post-pager-text"><asp:Literal ID="NextArticleLinkText" runat="server"></asp:Literal></div>
	</asp:HyperLink>
	<asp:HyperLink ID="PreviousArticleLink" CssClass="post-pager oldpost" runat="server">
		<div class="post-pager-header">
			<i class="fa fa-play-circle"></i>
			<span class="post-pager-header-content">Previous Article</span>
			</div>
		<div class="post-pager-text"><asp:Literal ID="PreviousArticleLinkText" runat="server"></asp:Literal></div>
	</asp:HyperLink>

	<asp:Panel ID="PostAuthorDescriptionContainer" CssClass="aboutauthor container" Visible="False" runat="server">
		<div class="title-header">
			<div class="title-header-text">About the Authors</div>
			<span class="title-subheader">EBSCO Research Editorial Staff</span>
		</div>
		<div class="title-content">
		    <asp:Literal runat="server" ID="PostAuthorDescription"></asp:Literal>
        </div>
	</asp:Panel>
</div>
</asp:Content>

<asp:Content ID="scripts" ContentPlaceHolderID="scripts" runat="server">
    <%= System.Web.Optimization.Scripts.Render("~/Scripts/blog-mobile/post")%>
</asp:Content>