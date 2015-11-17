<%@ Page Title="Best Pick Reports Blog | Quality Home Care Articles and Maintenance Advice" Language="C#" MasterPageFile="~/blogfiles/Blog.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ER_BestPickReports_Dev.blogfiles._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:HiddenField runat="server" ID="PageSaved" />
<asp:HiddenField runat="server" ID="TotalPagesSaved" />
<asp:HiddenField runat="server" ID="DeleteObjectID" />
    

    <asp:Panel class="category-posts-header" runat="server" ID="CategorySelected">
        <asp:Image runat="server" ID="CategoryIcon"/>
<div class="title">
        <asp:Label runat="server" ID="CategoryTitle"></asp:Label>
</div>
<div class="counter">
        <asp:Label runat="server" ID="CategoriesCounter"></asp:Label>
</div>
    </asp:Panel>
    

    <asp:ListView runat="server" ID="PostList" OnItemDataBound="PostList_ItemDataBound">
        <LayoutTemplate>
               <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
        </LayoutTemplate>

        <ItemTemplate>
            <div class="post">
                <div class="datetitle">
				    <div class="date">
		                <asp:Label CssClass="month" ID="PostMonth" runat="server" />
                        <br/>
		                <asp:Label CssClass="day" runat="server" ID="PostDate" />
		            </div>
				    <h2><asp:HyperLink runat="server" ID="PostTitle"></asp:HyperLink></h2>
			    </div><!-- datetitle -->
			    <div class="posttext">
                    <asp:HyperLink ID="PostImageLink" runat="server">
                        <asp:Image runat="server" ID="PostImage" Visible="false" />
                    </asp:HyperLink>
                    <div class="post-summary">
                        <asp:Panel class="authors-info" runat="server" id="AuthorsInfo">
                            <asp:Label runat="server" ID="Names" CssClass="names"></asp:Label>
                           <%-- <span>|</span>--%>
                            <asp:Label runat="server" ID="Position"></asp:Label>
                        </asp:Panel>
                        <div class="summary">
				            <asp:Label runat="server" ID="Summary" ></asp:Label>
                        </div>
                        <asp:HyperLink runat="server" ID="ReadMoreLink" Text="Continue Reading" CssClass="btn-contread" ></asp:HyperLink>
                        <asp:Panel runat="server" id="AddThis" CssClass="addthis_toolbox addthis_default_style" >
                            <%--<a class="addthis_counter addthis_pill_style btn-share"></a>--%>
                        </asp:Panel>
                        <br class="clear"/>
                    </div>
                    <br class="clear"/>
			    </div><!-- posttext -->
                <br class="clear"/>
            </div><!-- post -->
        </ItemTemplate>

        <EmptyDataTemplate>
            <div class="post">
                <div class="posttext">
                    <p class="alert">There are currently no blog posts.</p>
                </div>
            </div>
        
        </EmptyDataTemplate>
    </asp:ListView>

    <div class="pager">
        <asp:Panel class="newpost" runat="server" ID="PrevNavigation">
		    <asp:LinkButton runat="server" ID="Previous" Text="Previous Articles" OnClick="Previous_Click"></asp:LinkButton>
        </asp:Panel>
        <asp:Panel class="oldpost" runat="server" ID="NextNavigation">
		    <asp:LinkButton runat="server" ID="More" Text="Newer Articles" OnClick="More_Click"></asp:LinkButton>
        </asp:Panel>
	</div><!-- pager -->

    
    <script type="text/javascript">
        var addthis_config = { "data_track_addressbar": false };</script>
    <script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-5127eb9e4963bdca"></script>
</asp:Content>
