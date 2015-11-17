<%@ Page Title="Nominate a Company" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="nominationform.aspx.cs" Inherits="ER_BestPickReports_Dev.nominationform" MaintainScrollPositionOnPostback="true" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="ER-BestPickReports_Dev" Namespace="ER_BestPickReports" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="page">
    <div class="container">
        <h1>Best Pick&trade; Nomination Form</h1>
		<asp:Panel runat="server" ID="ShowForm" CssClass="feedback">
			<p>Thank you for your interest in nominating a company to be considered for our Best Pick&trade; research process. Please note, nominations do not count as consumer reviews towards research, nor do they guarantee that the company will qualify or subsequently participate as a Best Pick. To qualify, companies must first and foremost excel in our research. In addition, they must also maintain proper state-required insurance and licensing.</p>
            <p>Please tell us if you are a business owner or homeowner, and fill out the following form. </p>

            <asp:Panel runat="server" ID="SelectPanel" CssClass="search-results">
			    <div class="filter-tools">
				    <div class="type">
					    <label>Tell us who you are: </label>
					    <asp:DropDownList runat="server" ID="Type" AutoPostBack="true" OnSelectedIndexChanged="GetForm" Width="210">
                            <asp:ListItem Value="" Text="Select an Option -->"></asp:ListItem>
                            <asp:ListItem Value="Business Owner" Text="Business Owner"></asp:ListItem>
                            <asp:ListItem Value="Homeowner" Text="Homeowner"></asp:ListItem>
                        </asp:DropDownList>
				    </div>
			    </div>
                <p>&nbsp;</p>
			</asp:Panel>

            <asp:Panel runat="server" ID="BusinessPanel" Visible="false">
                <fieldset class="full error">
				    <label>Your Full Name:* </label>
                    <asp:TextBox runat="server" ID="BYourName" Columns="30"></asp:TextBox>
				    <asp:Label runat="server" ID="BYourNameReq" Text="Your name is required." ForeColor="Red" Visible="false"></asp:Label>
			    </fieldset>
                <fieldset class="full error">
				    <label>Company Name:* </label>
                    <asp:TextBox runat="server" ID="BName" Columns="50"></asp:TextBox>
				    <asp:Label runat="server" ID="BNameReq" Text="Business name is required." ForeColor="Red" Visible="false"></asp:Label>
			    </fieldset>
                <fieldset class="full error">
				    <label>Company Address:* </label>
                    <asp:TextBox runat="server" ID="BAddress" Columns="50"></asp:TextBox>
				    <asp:Label runat="server" ID="BAddressReq" Text="Business address is required." ForeColor="Red" Visible="false"></asp:Label>
			    </fieldset>
                <fieldset class="full error">
				    <label>City, State and Zipcode:* </label>
                    <asp:TextBox runat="server" ID="BCityStateZip" Columns="50"></asp:TextBox>
				    <asp:Label runat="server" ID="BCityStateZipReq" Text="Business city, state and zip code is required." ForeColor="Red" Visible="false"></asp:Label>
			    </fieldset>
                <fieldset class="full error">
				    <label>Company Phone:* </label>
                    <asp:TextBox runat="server" ID="BPhone" Columns="30"></asp:TextBox>
				    <asp:Label runat="server" ID="BPhoneReq" Text="Business phone is required." ForeColor="Red" Visible="false"></asp:Label>
			    </fieldset>
                <fieldset class="full">
				    <label>Company Website:</label>
                    <asp:TextBox runat="server" ID="BWebsite" Columns="40"></asp:TextBox>
			    </fieldset>
                <fieldset class="full error">
				    <label>Email Address:* </label>
                    <asp:TextBox runat="server" ID="BEmail" Columns="40"></asp:TextBox>
				    <asp:Label runat="server" ID="BEmailReq" Text="Email address is required." ForeColor="Red" Visible="false"></asp:Label>
			    </fieldset>
                <fieldset class="full error">
				    <label>Services Provided:* </label>
                    <asp:TextBox runat="server" ID="BServices" Columns="55" TextMode="MultiLine" Rows="4"></asp:TextBox>
				    <asp:Label runat="server" ID="BServicesReq" Text="Services provided is required." ForeColor="Red" Visible="false"></asp:Label>
			    </fieldset>      
			</asp:Panel>

            <asp:Panel runat="server" ID="HomePanel" Visible="false">
			    <fieldset class="full error">
			        <label>Your Full Name:* </label>
                    <asp:TextBox runat="server" ID="HYourName" Columns="30"></asp:TextBox>
			        <asp:Label runat="server" ID="HYourNameReq" Text="Your name is required." ForeColor="Red" Visible="false"></asp:Label>
		        </fieldset>
                <fieldset class="full error">
			        <label>Company Name:* </label>
                    <asp:TextBox runat="server" ID="HName" Columns="50"></asp:TextBox>
			        <asp:Label runat="server" ID="HNameReq" Text="Business name is required." ForeColor="Red" Visible="false"></asp:Label>
		        </fieldset>
                <fieldset class="full">
			        <label>Company Contact Name:</label>
                    <asp:TextBox runat="server" ID="HContact" Columns="30"></asp:TextBox>
		        </fieldset>
                <fieldset class="full">
			        <label>Company Phone:</label>
                    <asp:TextBox runat="server" ID="HPhone" Columns="30"></asp:TextBox>
		        </fieldset>
                <fieldset class="full">
			        <label>Company Website:</label>
                    <asp:TextBox runat="server" ID="HWebsite" Columns="40"></asp:TextBox>
		        </fieldset>
                <fieldset class="full">
			        <label>Services Provided:</label>
                    <asp:TextBox runat="server" ID="HServices" Columns="55" TextMode="MultiLine" Rows="4"></asp:TextBox>
		        </fieldset>
		    </asp:Panel>

            <asp:Panel runat="server" ID="ButtonPanel" Visible="false">
                <telerik:RadCaptcha runat="server" ID="Captcha" ProtectionMode="InvisibleTextBox" ValidationGroup="Nomination"></telerik:RadCaptcha>
			    <asp:Button runat="server" ID="SubmitButton" Text="Submit" onclick="Form_Click" ValidationGroup="Nomination" />
                <p>&nbsp;</p>
            </asp:Panel>

        </asp:Panel>

        <asp:Panel runat="server" ID="FormSuccess" Visible="false" CssClass="feedbackform">
            <p>Thank you for your nomination.</p>
        </asp:Panel>

    </div>
</div>
</asp:Content>
