<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dataforms.aspx.cs" Inherits="ER_BestPickReports_Dev.dataforms" %>
<%@ Register Assembly="ER-BestPickReports_Dev" Namespace="ER_BestPickReports" TagPrefix="cc1" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="page">
    <div class="container">
		<h1><asp:Literal runat="server" ID="PageHeading"></asp:Literal></h1>
		<asp:Panel runat="server" ID="NewsletterForm" CssClass="feedback" Visible="false">
			<fieldset class="full error">
				<label>First Name:* </label>
				<asp:TextBox runat="server" ID="Newsletter_FirstName"></asp:TextBox>      
			</fieldset>
			<fieldset class="zip">
				<label>Middle Initial: </label>
				<asp:TextBox runat="server" ID="Newsletter_MI"></asp:TextBox>
			</fieldset>
			<fieldset class="full error">
				<label>Last Name:* </label>
				<asp:TextBox runat="server" ID="Newsletter_LastName"></asp:TextBox>
			</fieldset>
			<fieldset class="full error">
				<label>Email Address:* </label>
                <asp:TextBox runat="server" ID="Newsletter_EMail"></asp:TextBox>
			</fieldset>

            <asp:Panel runat="server" ID="NewsletterErrorPanel" CssClass="formpanel"><asp:Literal runat="server" ID="NewsletterError"></asp:Literal></asp:Panel>

			<div>
				<asp:Button runat="server" ID="Newsletter_Submit" Text="Submit" OnClick="Newsletter_Submit_Click" />
			</div>
		</asp:Panel>

        <asp:Panel runat="server" ID="GuideForm" CssClass="feedback" Visible="false">
            <fieldset class="full error">
				<label>First Name:* </label>
				<asp:TextBox runat="server" ID="Guide_FirstName"></asp:TextBox>     
			</fieldset>
			<fieldset class="zip">
				<label>Middle Initial: </label>
				<asp:TextBox runat="server" ID="Guide_MI"></asp:TextBox>
			</fieldset>
			<fieldset class="full error">
				<label>Last Name:* </label>
				<asp:TextBox runat="server" ID="Guide_LastName"></asp:TextBox>
			</fieldset>
            <fieldset class="full error">
				<label>Address:* </label>
                <asp:TextBox runat="server" ID="Guide_Address"></asp:TextBox>
			</fieldset>
            <fieldset class="full error">
				<label>City:* </label>
                <asp:TextBox runat="server" ID="Guide_City"></asp:TextBox>
			</fieldset>
            <fieldset class="state error">
				<label>State:* </label>
                <asp:TextBox runat="server" ID="Guide_State"></asp:TextBox>
			</fieldset>
            <fieldset class="zip error">
				<label>Zip Code:* </label>
                <asp:TextBox runat="server" ID="Guide_Zip"></asp:TextBox>
			</fieldset>
            <fieldset class="full error">
				<label>Email Address:* </label>
                <asp:TextBox runat="server" ID="Guide_Email"></asp:TextBox>
			</fieldset>
            <fieldset class="full">
                <label>&nbsp;</label>
                <asp:CheckBox runat="server" ID="Guide_Updates" /> <span>Receive Monthly Update Emails</span>
            </fieldset>
            <%--<fieldset class="full">
                <label>&nbsp;</label>
                <asp:CheckBox runat="server" ID="Guide_FutureEditions" /> <span>Receive Future Editions</span>
            </fieldset>--%>

            <asp:Panel runat="server" ID="GuideErrorPanel" CssClass="formpanel"><asp:Literal runat="server" ID="GuideError"></asp:Literal></asp:Panel>

            <div>
				<asp:Button runat="server" ID="Guide_Submit" Text="Submit" OnClick="Guide_Submit_Click" />
			</div>
        </asp:Panel>

        <asp:Panel runat="server" ID="FormSuccess" Visible="false">
            <p><asp:Literal runat="server" ID="SuccessText"></asp:Literal></p>
        </asp:Panel>
	</div>
</div>
</asp:Content>
