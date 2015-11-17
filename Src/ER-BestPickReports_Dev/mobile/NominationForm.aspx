<%@ Page Title="" Language="C#" MasterPageFile="~/mobile/SiteMobile.Master" AutoEventWireup="true" CodeBehind="NominationForm.aspx.cs" Inherits="ER_BestPickReports_Dev.mobile.NominationForm" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
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
                    <asp:requiredfieldvalidator id="BYourNameFieldValidator"
                        controltovalidate="BYourName"
                        validationgroup="BusinessFormGroup"
                        errormessage="Your name is required"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
			    </fieldset>
                <fieldset class="full error">
				    <label>Company Name:* </label>
                    <asp:TextBox runat="server" ID="BName" Columns="50"></asp:TextBox>
                    <asp:requiredfieldvalidator id="BNameFieldvalidator"
                        controltovalidate="BName"
                        validationgroup="BusinessFormGroup"
                        errormessage="Business name is required"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
			    </fieldset>
                <fieldset class="full error">
				    <label>Company Address:* </label>
                    <asp:TextBox runat="server" ID="BAddress" Columns="50"></asp:TextBox>
                    <asp:requiredfieldvalidator id="BAddressFieldvalidator"
                        controltovalidate="BAddress"
                        validationgroup="BusinessFormGroup"
                        errormessage="Business address is required"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
			    </fieldset>
                <fieldset class="full error">
				    <label>City, State and Zipcode:* </label>
                    <asp:TextBox runat="server" ID="BCityStateZip" Columns="50"></asp:TextBox>
                    <asp:requiredfieldvalidator id="BCityStateZipFieldvalidator"
                        controltovalidate="BCityStateZip"
                        validationgroup="BusinessFormGroup"
                        errormessage="Business city, state and zip code is required"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
			    </fieldset>
                <fieldset class="full error">
				    <label>Company Phone:* </label>
                    <asp:TextBox runat="server" ID="BPhone" Columns="30"></asp:TextBox>
                    <asp:requiredfieldvalidator id="BPhoneFieldvalidator"
                        controltovalidate="BPhone"
                        validationgroup="BusinessFormGroup"
                        errormessage="Business phone is required"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
			    </fieldset>
                <fieldset class="full">
				    <label>Company Website:</label>
                    <asp:TextBox runat="server" ID="BWebsite" Columns="40"></asp:TextBox>
			    </fieldset>
                <fieldset class="full error">
				    <label>Email Address:* </label>
                    <asp:TextBox runat="server" ID="BEmail" Columns="40"></asp:TextBox>
                    <asp:requiredfieldvalidator id="BEmailFieldvalidator"
                        controltovalidate="BEmail"
                        validationgroup="BusinessFormGroup"
                        errormessage="Email address is required"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                    <asp:RegularExpressionValidator id="BEmailFieldvalidator2"
                        controltovalidate="BEmail"
                        ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                        validationgroup="BusinessFormGroup"
                        errormessage="Correct Email address is required"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:RegularExpressionValidator>
			    </fieldset>
                <fieldset class="full error">
				    <label>Services Provided:* </label>
                    <asp:TextBox runat="server" ID="BServices" Columns="55" TextMode="MultiLine" Rows="4"></asp:TextBox>
                    <asp:requiredfieldvalidator id="BServicesFieldvalidator"
                        controltovalidate="BServices"
                        validationgroup="BusinessFormGroup"
                        errormessage="Services provided is required"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
			    </fieldset>      
			</asp:Panel>

            <asp:Panel runat="server" ID="HomePanel" Visible="false">
			    <fieldset class="full error">
			        <label>Your Full Name:* </label>
                    <asp:TextBox runat="server" ID="HYourName" Columns="30"></asp:TextBox>
                    <asp:requiredfieldvalidator id="HYourNameFieldvalidator"
                        controltovalidate="HYourName"
                        validationgroup="HomeFormGroup"
                        errormessage="Your name is required"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
		        </fieldset>
                <fieldset class="full error">
			        <label>Company Name:* </label>
                    <asp:TextBox runat="server" ID="HName" Columns="50"></asp:TextBox>
                    <asp:requiredfieldvalidator id="HNameFieldvalidator"
                        controltovalidate="HName"
                        validationgroup="HomeFormGroup"
                        errormessage="Business name is required"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
