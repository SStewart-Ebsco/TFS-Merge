<%@ Page Title="" Language="C#" MasterPageFile="~/blogfiles/mobile/BlogMobile.Master" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="ER_BestPickReports_Dev.blogfiles.mobile.search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:HiddenField runat="server" ID="PageSaved" />
<asp:HiddenField runat="server" ID="TotalPagesSaved" />






    <div class="search-title">
    <span class="search-title-text">Search results for:</span>
        <asp:Label ID="Keyword" runat="server" CssClass="search-title-keyword"></asp:Label>
    </div>


    <asp:ListView runat="server" ID="PostList" OnItemDataBound="PostList_ItemDataBound">
        <LayoutTemplate>
               <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
        </LayoutTemplate>
        
        <ItemTemplate>
            <div class="post container" itemscope itemtype="http://schema.org/Article">
                <div class="post-datetitle">
                    <div class="date">
                        <asp:Label runat="server" ID="PostDateMonth" CssClass="date-month"></asp:Label>
                        <asp:Label runat="server" ID="PostDate" CssClass="date-date"></asp:Label>
                    </div>
                    <h2 class="post-title" itemprop="about"><asp:HyperLink runat="server" ID="PostTitle"></asp:HyperLink></h2>
                </div><!-- datetitle -->
                <div class="posttext">
                    <asp:HyperLink ID="PostImageLink" runat="server" CssClass="post-image-link">
                        <asp:Image runat="server" ID="PostImage" Visible="false" CssClass="post-image-preview" />
                    </asp:HyperLink>
                    <div class="post-author" itemscope itemtype="http://schema.org/Person" itemprop="author">
                        <span class="post-author-name-preview" itemprop="name">Madeline Hagaman</span>
                        <span class="post-author-title" itemprop="jobTitle">Technical Writer</span>
                    </div>
                    <span class="post-summary">
                        <asp:Literal runat="server" ID="Summary" Text='<%# Eval("Summary") %>'></asp:Literal>
                    </span>
                    <asp:HyperLink runat="server" ID="ReadMoreLink" Text="Continue Reading" CssClass="button"></asp:HyperLink>
                </div><!-- posttext -->
            </div><!-- post -->
        </ItemTemplate>

        <EmptyDataTemplate>
            <div class="post container">
                <div class="posttext">
                    <p class="post-empty">There are currently no blog posts.</p>
                </div>
            </div>
        
        </EmptyDataTemplate>
    </asp:ListView>
    <div class="pager">
        <asp:LinkButton runat="server" ID="More" Text="Previous Articles" OnClick="More_Click" CssClass="pager-button oldpost">
            <div class="button-content">
                <i class="fa fa-play-circle"></i>
                <span> Previous Articles</span>
            </div>
        </asp:LinkButton>
        <asp:LinkButton runat="server" ID="Previous" Text="Newer Articles" OnClick="Previous_Click" CssClass="pager-button newpost">
            <div class="button-content">
                <span> Newer Articles</span>
                <i class="fa fa-play-circle"></i>
            </div>
        </asp:LinkButton>
    </div><!-- pager -->
</asp:Content>
