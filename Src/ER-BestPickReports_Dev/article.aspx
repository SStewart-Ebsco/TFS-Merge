<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="article.aspx.cs" Inherits="ER_BestPickReports_Dev.article" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="page">
    <div class="container">
		<h1><asp:Literal runat="server" ID="PageHeading"></asp:Literal></h1>
		<asp:Literal runat="server" ID="Body"></asp:Literal>
	</div>
</div>
</asp:Content>
