<%@ Page Title="" Language="C#" MasterPageFile="~/blogfiles/Blog.Master" AutoEventWireup="true" CodeBehind="post.aspx.cs" Inherits="ER_BestPickReports_Dev.blogfiles.post" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<input runat="server" ID="IsInMarketUser" class="hIsInMarketUser" type="hidden"/>
<div class="post">
    <div class="datetitle">
		<div class="date">
		    <asp:Label CssClass="month" ID="PostMonth" runat="server" />
            <br/>
		    <asp:Label CssClass="day" runat="server" ID="PostDate" />
		</div>
        <asp:HiddenField runat="server" ID="PostYear"/>
		<h2><asp:Literal runat="server" ID="PostTitle"></asp:Literal></h2>
	</div><!-- datetitle -->
    <div class="posttext">
        <asp:Image runat="server" ID="PostImage" Visible="false" CssClass="post-view"/>
        <div class="social-top">
<%--            <!-- AddThis Button BEGIN -->
            <div class="addthis_toolbox addthis_default_style ">
                <a class="addthis_button_facebook_like" <%="fb:like:layout"%>="button_count" <%="ffb:like:href"%>="http://www.facebook.com/BestPickReports"></a>
                <a class="addthis_button_tweet" <%="ftw:count"%>="none"></a>
                <a class="addthis_button_google_plusone" <%="fg:plusone:count"%>="false" <%="fg:plusone:size"%>="medium" <%="fg:plusone:href"%>="https://plus.google.com/115147162962806414209/"></a> 
                <a class="addthis_counter addthis_pill_style"></a>
            </div>
            <script type="text/javascript">                    var addthis_config = { "data_track_addressbar": false };</script>
            <script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-5127eb9e4963bdca"></script>
            <!-- AddThis Button END -->--%>

            <!-- Go to www.addthis.com/dashboard to customize your tools -->
            <div class="addthis_sharing_toolbox"></div>

        </div><!--social-->
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
    <div class="post-footer">
		<div class="top">
			<div class="social">
                <!-- AddThis Button BEGIN -->
                <div class="addthis_toolbox addthis_default_style ">
                    <a class="addthis_button_facebook_like" <%="fb:like:layout"%>="button_count" <%="ffb:like:href"%>="http://www.facebook.com/BestPickReports"></a>
                    <a class="addthis_button_tweet" <%="ftw:count"%>="none"></a>
                    <a class="addthis_button_google_plusone" <%="fg:plusone:count"%>="false" <%="fg:plusone:size"%>="medium" <%="fg:plusone:href"%>="https://plus.google.com/115147162962806414209/"></a> 
                    <a class="addthis_counter addthis_pill_style"></a>
                </div>
                <script type="text/javascript">    var addthis_config = { "data_track_addressbar": false };</script>
                <script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-5127eb9e4963bdca"></script>
                <!-- AddThis Button END -->
            </div><!--social-->


<%--            <!-- Go to www.addthis.com/dashboard to customize your tools -->
            <div class="addthis_sharing_toolbox"></div>--%>

		</div><!-- top -->
        <asp:Panel runat="server" ID="CatPanel" CssClass="categories">
			<strong>Category:</strong> <asp:Literal runat="server" ID="Cats"></asp:Literal>
		</asp:Panel><!-- cat -->
		<div class="tags">
			<strong>Tags:</strong> <asp:Literal runat="server" ID="Tags"></asp:Literal>
		</div><!-- tags -->

        <!--Navigation-->
        <div class="postNavigation">
            <asp:HyperLink class="oldpost" runat="server" ID="NextNavigation">
                <div class="rotate">
                    <p>Next Article</p>
		            <asp:Label runat="server" ID="NextPostLink"></asp:Label>
                </div>
            </asp:HyperLink><br/>
            <asp:HyperLink class="newpost" runat="server" ID="PrevNavigation">
                <p>Previous Article</p>
		        <asp:Label runat="server" ID="PreviousPostLink"></asp:Label>
            </asp:HyperLink>
        </div><!--End Navigation-->

	<asp:Panel ID="AuthorsInfo" CssClass="about-authors" Visible="False" runat="server">
        <div class="header">
            <h1>About the Authors</h1>
            <p>EBSCO Research Editorial Staff</p>
        </div>
		<div class="content">
		    <asp:Literal runat="server" ID="PostAuthorDescription"></asp:Literal>
        </div>
	</asp:Panel>
	</div><!-- post-footer -->
</div>
</asp:Content>

<asp:Content ID="scripts" ContentPlaceHolderID="AdditionalScriptsHolder" runat="server">
    <script type="text/javascript">
        $(function () {
            if ($('.hIsInMarketUser').val() === 'false') {
                $('.post-local-best').hide();
            }
        });
    </script>
</asp:Content>
