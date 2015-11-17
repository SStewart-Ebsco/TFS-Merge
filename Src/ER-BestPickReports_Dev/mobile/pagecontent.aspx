<%@ Page Title="" Language="C#" MasterPageFile="~/mobile/SiteMobile.Master" AutoEventWireup="true" CodeBehind="pagecontent.aspx.cs" Inherits="ER_BestPickReports_Dev.mobile.pagecontent" %>

<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/pagecontent")%>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<div class="page">
    <div class="container contentpage-container pagecontent">
		<h1><asp:Literal runat="server" ID="PageHeading"></asp:Literal></h1>
		<asp:Literal runat="server" ID="Body"></asp:Literal>
	</div>
</div>
</asp:Content>
