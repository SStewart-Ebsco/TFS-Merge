<%@ Page Title="" Language="C#" MasterPageFile="~/mobile/SiteMobile.Master" AutoEventWireup="true" CodeBehind="article.aspx.cs" Inherits="ER_BestPickReports_Dev.mobile.article" %>
<asp:Content ID="ContentPlaceholder" ContentPlaceHolderID="Content" runat="server">
<div class="page">
    <div class="container">
		<h1><asp:Literal runat="server" ID="PageHeading"></asp:Literal></h1>
		<asp:Literal runat="server" ID="Body"></asp:Literal>
	</div>
</div>
</asp:Content>
