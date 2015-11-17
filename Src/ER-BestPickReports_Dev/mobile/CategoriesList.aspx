<%@ Page Language="C#" MasterPageFile="~/mobile/SiteMobile.Master" AutoEventWireup="true" CodeBehind="CategoriesList.aspx.cs" Inherits="ER_BestPickReports_Dev.mobile.CategoriesList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/categoriesList")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div>
        <asp:ListView runat="server" ID="CategoriesList1" OnItemDataBound="CategoriesList1_ItemDataBound">
            <LayoutTemplate>
                <ul class="submenu-list categoriesList-list">
                    <li runat="server" id="itemPlaceholder" />
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li>
                    <div class="categoriesList-item">
                        <a runat="server" ID="CatLink">
                            <asp:Image runat="server" ID="CatIcon" CssClass="categoriesList-icon" />
                            <asp:Label runat="server" ID="CatText" CssClass="categoriesList-text">
                            </asp:Label>
                        </a>
                    </div>
                            
                </li>
            </ItemTemplate>
            <EmptyDataTemplate>
				<div class="submenu-list-empty">
					There are no matching results.
				</div>
			</EmptyDataTemplate>
            <ItemSeparatorTemplate>
                <div class="categoriesList-separator" ></div>
            </ItemSeparatorTemplate>
        </asp:ListView>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
