<%@ Page Title="Best Pick Reports Blog | Quality Home Care Articles and Maintenance Advice" Language="C#" MasterPageFile="~/blogfiles/mobile/BlogMobile.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ER_BestPickReports_Dev.blogfiles.mobile._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:HiddenField runat="server" ID="PageSaved" />
<asp:HiddenField runat="server" ID="TotalPagesSaved" />
<asp:HiddenField runat="server" ID="TotalPostsSaved" />
<asp:HiddenField runat="server" ID="DeleteObjectID" />
    <asp:LinkButton runat="server" ID="AddPost" OnClick="AddPost_Click"></asp:LinkButton>
    

    <asp:Panel ID="CategoryFilterPanel" CssClass="category-filter" runat="server" Visible="false">
        <asp:Panel ID="CategoryFilterImage" runat="server" CssClass="title-header">
            <div class="title-header-text"><asp:Literal ID="CategoryFilterTitle" runat="server"></asp:Literal></div>
            <span class="title-subheader"><asp:Literal ID="CategoryFilterTotal" runat="server"></asp:Literal></span>
        </asp:Panel>
    </asp:Panel>

    <asp:ListView runat="server" ID="PostList" OnItemDataBound="PostList_ItemDataBound" OnDataBound="PostList_DataBound">
        <LayoutTemplate>
               <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
        </LayoutTemplate>

        <ItemTemplate>
            <div class="post container" itemscope itemtype="http://schema.org/Article">
                <%--<asp:Panel runat="server" ID="ListControls" CssClass="post-edit-controls" Visible="false">
			        <ul>
                        <li><asp:LinkButton runat="server" ID="EditPost" title="Edit Post" CommandArgument='<%# Eval("PostID") %>' OnCommand="EditPost_Click" CssClass="post-edit-controls-edit"></asp:LinkButton></li>
                        <li><asp:LinkButton runat="server" ID="DeletePost" title="Delete Post" CommandArgument='<%# Eval("PostID") %>' OnCommand="DeletePost_Click" CssClass="post-edit-controls-remove"></asp:LinkButton></li>
                        <li><asp:LinkButton runat="server" ID="AddPost" title="Add New Post" OnClick="AddPost_Click" CssClass="post-edit-controls-add"></asp:LinkButton></li>
                    </ul>
                </asp:Panel>--%>
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
                    <asp:Panel ID="PostAuthor" CssClass="post-author" itemscope itemtype="http://schema.org/Person" itemprop="author" runat="server">
                        <span class="post-author-name-preview" itemprop="name">
                            <asp:Literal runat="server" ID="PostAuthorName"></asp:Literal>
                        </span>
                        <span class="post-author-title" itemprop="jobTitle">
                            <asp:Literal runat="server" ID="PostAuthorTitle"></asp:Literal>
                        </span>
                    </asp:Panel>
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
                    <p runat="server" id="addlink" visible="false">
                        <asp:LinkButton runat="server" ID="AddPost" Text="Add New Post" OnClick="AddPost_Click" CssClass="addlink"></asp:LinkButton>
                    </p>
                </div>
            </div>
        
        </EmptyDataTemplate>
    </asp:ListView>
    <div class="pager">
        <asp:LinkButton runat="server" ID="Previous" Text="Previous Articles" OnClick="More_Click" CssClass="pager-button oldpost">
            <div class="button-content">
                <i class="fa fa-play-circle"></i>
                <span> Previous Articles</span>
            </div>
        </asp:LinkButton>
		<asp:LinkButton runat="server" ID="Newer" Text="Newer Articles" OnClick="Previous_Click" CssClass="pager-button newpost">
            <div class="button-content">
                <span> Newer Articles</span>
                <i class="fa fa-play-circle"></i>
            </div>
        </asp:LinkButton>
	</div><!-- pager -->
</asp:Content>
