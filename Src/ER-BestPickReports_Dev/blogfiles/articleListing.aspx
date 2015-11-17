<%@ Page Title="" Language="C#" MasterPageFile="~/blogfiles/Admin.Master" AutoEventWireup="true" CodeBehind="articleListing.aspx.cs" Inherits="ER_BestPickReports_Dev.blogfiles.articleListing" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
    <link rel="stylesheet" href="/blogfiles/assets/css/blog-listing.css" type="text/css" media="screen, projection" />
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <span class="blog-article-listing-title">Blog Article Management</span>
        <asp:LinkButton runat="server" ID="AddPost" OnClick="AddPost_Click" CssClass="blog-article-listing-addpost">
            <i class="blog-article-listing-addpost-icon"></i>
            Add New
            </asp:LinkButton>
    </div>
    <br style="clear: both;" />

     <asp:ListView runat="server" ID="PostList" OnItemDataBound="PostList_ItemDataBound" OnDataBound="PostList_DataBound">
        <LayoutTemplate>
            <table class="blog-article-listing" cellspacing="0">
                <thead>
                    <tr class="blog-article-listing-header">
                        <td></td>
                        <td>Title</td>
                        <td>Categories</td>
                        <td>Top Blog</td>
                        <td>Publish Date</td>
                    </tr>
                </thead>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
            </table>
        </LayoutTemplate>

        <ItemTemplate>
            <tr class="blog-article-listing-row">
                <td>
                    <asp:LinkButton runat="server" ID="EditPost" title="Edit Post" CommandArgument='<%# Eval("PostID") %>' OnCommand="EditPost_Click" CssClass="post-edit-controls-edit"></asp:LinkButton>
                </td>
                <td>
				    <asp:Label runat="server" ID="PostTitle"></asp:Label>
			    </td>
                <td>
                    <asp:Label ID="Categories" runat="server"></asp:Label>
                </td>
                <td class="blog-article-listing-top">
                    <asp:CheckBox ID="IsTopBlog" runat="server" OnCheckedChanged="IsTopBlog_Checked" AutoPostBack="true" CommandArgument='<%# Eval("PostID") %>'/>
                </td>
                <td class="blog-article-listing-publish">
                    <asp:Label runat="server" ID="PostDate" CssClass="date-date"></asp:Label>
                </td>
            </tr>
        </ItemTemplate>

        <EmptyDataTemplate>
            <div class="post container">
                <div class="posttext">
                    <p class="post-empty">There are currently no blog posts.</p>
                </div>
            </div>
        
        </EmptyDataTemplate>
    </asp:ListView>

</asp:Content>
