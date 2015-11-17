<%@ Page Title="Share Your Feedback" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="feedback.aspx.cs" Inherits="ER_BestPickReports_Dev.feedback" MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="ER-BestPickReports_Dev" Namespace="ER_BestPickReports" TagPrefix="cc1" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="page">
    <div class="container">
		<h1>Feedback Form</h1>
		<asp:Panel runat="server" ID="ShowForm" CssClass="feedback">
			<div class="title">
				<h2>Company Information</h2>
			</div>
			<fieldset class="full error">
				<label>Company Name:* </label>
				<asp:TextBox runat="server" ID="CompanyName"></asp:TextBox>
                <cc1:MyCustomValidator ID="CompanyNameVal" runat="server" ControlToValidate="CompanyName" Text="Company Name is required!" SetFocusOnError="true" OnServerValidate="ServerValidation"></cc1:MyCustomValidator>
			</fieldset>
			<fieldset class="phone-b">
				<label>Company Phone: </label>
				<asp:TextBox runat="server" ID="CompanyPhone"></asp:TextBox>
			</fieldset>
			<fieldset class="full">
				<label>Company Location: </label>
				<asp:TextBox runat="server" ID="CompanyLocation"></asp:TextBox>
			</fieldset>
			<fieldset class="full error">
				<label>Company Type:* </label>
				<asp:TextBox runat="server" ID="CompanyType"></asp:TextBox>&nbsp;(ex. Electrician)&nbsp;
			    <cc1:MyCustomValidator ID="CompanyTypeVal" runat="server" ControlToValidate="CompanyType" Text="Company Type is required!" SetFocusOnError="true" OnServerValidate="ServerValidation"></cc1:MyCustomValidator>
			</fieldset>
			<fieldset class="full">
				<label>Description of Work/Job: </label>
				<asp:TextBox runat="server" ID="Description" Rows="5" TextMode="MultiLine" Width="300px"></asp:TextBox>
			</fieldset>
			<div class="title rate-ti">
				<h2>Company Ratings</h2>
			</div>
			<div class="ratings">
				<div class="head">
					<ul>
						<li>A</li>
						<li>B</li>
						<li>C</li>
						<li>D</li>
						<li>F</li>
						<li>NA</li>
					</ul>
				</div>
				<fieldset class="item">
					<ul>
						<li><asp:RadioButton runat="server" ID="OverallA" GroupName="Overall" /></li>
						<li><asp:RadioButton runat="server" ID="OverallB" GroupName="Overall" /></li>
						<li><asp:RadioButton runat="server" ID="OverallC" GroupName="Overall" /></li>
						<li><asp:RadioButton runat="server" ID="OverallD" GroupName="Overall" /></li>
						<li><asp:RadioButton runat="server" ID="OverallF" GroupName="Overall" /></li>
						<li><asp:RadioButton runat="server" ID="OverallNA" GroupName="Overall" /></li>
					</ul>
					<label><strong>Overall</strong> (An A means you would definitely recommend this company to a close friend.)</label>
				</fieldset>
				<fieldset class="item">
					<ul>
						<li><asp:RadioButton runat="server" ID="QualityA" GroupName="Quality" /></li>
						<li><asp:RadioButton runat="server" ID="QualityB" GroupName="Quality" /></li>
						<li><asp:RadioButton runat="server" ID="QualityC" GroupName="Quality" /></li>
						<li><asp:RadioButton runat="server" ID="QualityD" GroupName="Quality" /></li>
						<li><asp:RadioButton runat="server" ID="QualityF" GroupName="Quality" /></li>
						<li><asp:RadioButton runat="server" ID="QualityNA" GroupName="Quality" /></li>
					</ul>
					<label><strong>Work Quality</strong> (Measure of the actual quality of the job completed.)</label>
				</fieldset>
				<fieldset class="item">
					<ul>
						<li><asp:RadioButton runat="server" ID="TimelinessA" GroupName="Timeliness" /></li>
						<li><asp:RadioButton runat="server" ID="TimelinessB" GroupName="Timeliness" /></li>
						<li><asp:RadioButton runat="server" ID="TimelinessC" GroupName="Timeliness" /></li>
						<li><asp:RadioButton runat="server" ID="TimelinessD" GroupName="Timeliness" /></li>
						<li><asp:RadioButton runat="server" ID="TimelinessF" GroupName="Timeliness" /></li>
						<li><asp:RadioButton runat="server" ID="TimelinessNA" GroupName="Timeliness" /></li>
					</ul>
					<label><strong>Timeliness</strong> (Did the company show up when promised? Was the work finished in a timely manner?)</label>
				</fieldset>
                <fieldset class="item">
					<ul>
						<li><asp:RadioButton runat="server" ID="CourteousA" GroupName="Courteous" /></li>
						<li><asp:RadioButton runat="server" ID="CourteousB" GroupName="Courteous" /></li>
						<li><asp:RadioButton runat="server" ID="CourteousC" GroupName="Courteous" /></li>
						<li><asp:RadioButton runat="server" ID="CourteousD" GroupName="Courteous" /></li>
						<li><asp:RadioButton runat="server" ID="CourteousF" GroupName="Courteous" /></li>
						<li><asp:RadioButton runat="server" ID="CourteousNA" GroupName="Courteous" /></li>
					</ul>
					<label><strong>Courteous</strong> (Were office personnel, workers, and other representatives courteous? Were calls returned?)</label>
				</fieldset>
				<fieldset class="item">
					<ul>
						<li><asp:RadioButton runat="server" ID="NeatA" GroupName="Neat" /></li>
						<li><asp:RadioButton runat="server" ID="NeatB" GroupName="Neat" /></li>
						<li><asp:RadioButton runat="server" ID="NeatC" GroupName="Neat" /></li>
						<li><asp:RadioButton runat="server" ID="NeatD" GroupName="Neat" /></li>
						<li><asp:RadioButton runat="server" ID="NeatF" GroupName="Neat" /></li>
						<li><asp:RadioButton runat="server" ID="NeatNA" GroupName="Neat" /></li>
					</ul>
					<label><strong>Neat &amp; Tidy</strong> (Were employees and vehicles neat and clean? Was the job site left neat?)</label>
				</fieldset>
				<div class="head four">
					<ul>
						<li>High</li>
						<li>Avg.</li>
						<li>Low</li>
						<li>NA</li>
					</ul>
				</div>
				<fieldset class="item four">
					<ul>
						<li><asp:RadioButton runat="server" ID="PriceHigh" GroupName="Price" /></li>
						<li><asp:RadioButton runat="server" ID="PriceAvg" GroupName="Price" /></li>
						<li><asp:RadioButton runat="server" ID="PriceLow" GroupName="Price" /></li>
						<li><asp:RadioButton runat="server" ID="PriceNA" GroupName="Price" /></li>
					</ul>
					<label><strong>Price</strong> (How did this company's price compare to the competitor's prices?)</label>
				</fieldset>
			</div>
			<fieldset class="comments">
				<label>General Comments:</label>
				<asp:TextBox runat="server" ID="Comments" Rows="5" TextMode="MultiLine" Width="300px"></asp:TextBox> 
			</fieldset>
			<div class="title">
				<h2>Personal Information</h2>
			</div>
			<div class="personal">
				<fieldset class="full error">
					<label>Full Name:* </label>
					<asp:TextBox runat="server" ID="Name"></asp:TextBox>
					<cc1:MyCustomValidator ID="NameVal" runat="server" ControlToValidate="Name" Text="Name is required!" SetFocusOnError="true" OnServerValidate="ServerValidation"></cc1:MyCustomValidator>
				</fieldset>
				<fieldset class="phone-i error">
					<label>Phone:* </label>
					<asp:TextBox runat="server" ID="Phone"></asp:TextBox>
					<cc1:MyCustomValidator ID="PhoneVal" runat="server" ControlToValidate="Phone" Text="Phone is required!" SetFocusOnError="true" OnServerValidate="ServerValidation"></cc1:MyCustomValidator>
				</fieldset>
				<fieldset class="city error">
					<label>City:* </label>
					<asp:TextBox runat="server" ID="City"></asp:TextBox>
					<cc1:MyCustomValidator ID="CityVal" runat="server" ControlToValidate="City" Text="City is required!" SetFocusOnError="true" OnServerValidate="ServerValidation"></cc1:MyCustomValidator>
				</fieldset>
				<fieldset class="state error">
					<label>State:* </label>
					<asp:TextBox runat="server" ID="State"></asp:TextBox>
					<cc1:MyCustomValidator ID="StateVal" runat="server" ControlToValidate="State" Text="State is required!" SetFocusOnError="true" OnServerValidate="ServerValidation"></cc1:MyCustomValidator>
				</fieldset>
				<fieldset class="zip error">
					<label>Zip Code:* </label>
					<asp:TextBox runat="server" ID="Zip"></asp:TextBox>
					<cc1:MyCustomValidator ID="ZipVal" runat="server" ControlToValidate="Zip" Text="Zip Code is required!" SetFocusOnError="true" OnServerValidate="ServerValidation"></cc1:MyCustomValidator>
				</fieldset>
				<fieldset class="full error">
					<label>Email Address:* </label>
					<asp:TextBox runat="server" ID="Email"></asp:TextBox>
					<cc1:MyCustomValidator ID="EmailVal" runat="server" ControlToValidate="Email" Text="Email Address is required!" SetFocusOnError="true" OnServerValidate="ServerValidation"></cc1:MyCustomValidator>
				</fieldset>
			</div>
			<div class="cache">
				<p>Enter the text EXACTLY as you see below before submitting the form!</p>
				<div class="img">
                    <telerik:RadCaptcha runat="server" ID="Captcha" Skin="" CaptchaImage-RenderImageOnly="true" ValidatedTextBoxID="CaptchBox" Display="None"></telerik:RadCaptcha>
                </div>
				<asp:TextBox runat="server" ID="CaptchBox"></asp:TextBox>
                <asp:Label runat="server" ID="CaptchNote" Text="" Visible="false" CssClass="incorrectcode"></asp:Label>
				<asp:Button runat="server" ID="SubmitButton" Text="Submit Form" onclick="Feedback_Click" />
			</div>
		</asp:Panel>

        <asp:Panel runat="server" ID="FormSuccess" Visible="false">
            <p>Thank you for contacting EBSCO Research. We appreciate your feedback.</p>
        </asp:Panel>
	</div>
</div>
</asp:Content>
