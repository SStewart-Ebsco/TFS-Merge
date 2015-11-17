<%@ Page Title="" Language="C#" MasterPageFile="~/blogfiles/Blog.Master" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="ER_BestPickReports_Dev.blogfiles.search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:HiddenField runat="server" ID="PageSaved" />
<asp:HiddenField runat="server" ID="TotalPagesSaved" />
<div class="pageheading">
You searched for: <strong><asp:Literal runat="server" ID="Keyword"></asp:Literal></strong>
</div>
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
                          <%--  <span>|</span>--%>
                            <asp:Label runat="server" ID="Position"></asp:Label>
                        </asp:Panel>
                        <div class="summary">
				            <asp:Label runat="server" ID="Summary" ></asp:Label>
                        </div>
                        <asp:HyperLink runat="server" ID="ReadMoreLink" Text="Continue Reading" CssClass="btn-contread" ></asp:HyperLink>
                        <a class="addthis_counter addthis_pill_style btn-share"></a>
                        <script type="text/javascript">                            var addthis_config = { "data_track_addressbar": false };</script>
                        <script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-5127eb9e4963bdca"></script>
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
                    <p class="post-empty">There are currently no blog posts.</p>
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
</asp:Content>
