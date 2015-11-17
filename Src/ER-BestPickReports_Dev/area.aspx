<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="area.aspx.cs" Inherits="ER_BestPickReports_Dev.area" MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="container">
    <section>
	    <article class="contact-con">
		    <div class="containerInner">
			    <div class="headerArrow">
				    <div class="arrowBorder"></div>
				    <div class="arrowDown">▼</div>
			    </div>
				
				<div class="contact-item">
					<h3>Sign up for Seasonal </h3>
					<h3>Home Care Tips and Articles</h3>
					<p>Receive our blog articles directly to your inbox once a month. Published weekly, the EBSCO Research blog provides homeowners with valuable insight and helpful tips on everything from maintaining your HVAC system to keeping your lawn green.</p>
					<fieldset>
						<asp:TextBox runat="server" ID="Newsletter_FirstName" CssClass="first-name"></asp:TextBox>
							<cc1:TextBoxWatermarkExtender runat="server" ID="Newsletter_FirstName_WM" TargetControlID="Newsletter_FirstName" WatermarkText="First Name"></cc1:TextBoxWatermarkExtender>
						<asp:TextBox runat="server" ID="Newsletter_MI" CssClass="mi"></asp:TextBox>
							<cc1:TextBoxWatermarkExtender runat="server" ID="Newsletter_MI_WM" TargetControlID="Newsletter_MI" WatermarkText="M.I."></cc1:TextBoxWatermarkExtender>
						<asp:TextBox runat="server" ID="Newsletter_LastName" CssClass="last-name"></asp:TextBox>
							<cc1:TextBoxWatermarkExtender runat="server" ID="Newsletter_LastName_WM" TargetControlID="Newsletter_LastName" WatermarkText="Last Name"></cc1:TextBoxWatermarkExtender>
						<asp:TextBox runat="server" ID="Newsletter_EMail" CssClass="email"></asp:TextBox>
							<cc1:TextBoxWatermarkExtender runat="server" ID="Newsletter_Email_WM" TargetControlID="Newsletter_Email" WatermarkText="Email Address"></cc1:TextBoxWatermarkExtender>
						<asp:Panel runat="server" ID="NewsletterErrorPanel" CssClass="formpanel"><asp:Literal runat="server" ID="NewsletterError"></asp:Literal></asp:Panel>
						<asp:Button runat="server" ID="Newsletter_Submit" Text="Submit" OnClick="Newsletter_Submit_Click" />
					</fieldset>
				</div>

                <asp:Panel runat="server" ID="RequestPanel" CssClass="contact-item">
			        <h3>Request a <%=DateTime.Now.Year%> Guide</h3>
			        <p>Have one of our publications delivered to your door—free of charge.</p>
			        <fieldset>
				        <asp:TextBox runat="server" ID="Guide_FirstName" CssClass="first-name"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender runat="server" ID="Guide_FirstName_WM" TargetControlID="Guide_FirstName" WatermarkText="First Name"></cc1:TextBoxWatermarkExtender>
				        <asp:TextBox runat="server" ID="Guide_MI" CssClass="mi"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender runat="server" ID="Guide_MI_WM" TargetControlID="Guide_MI" WatermarkText="M.I."></cc1:TextBoxWatermarkExtender>
                        <asp:TextBox runat="server" ID="Guide_LastName" CssClass="last-name"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender runat="server" ID="Guide_LastName_WM" TargetControlID="Guide_LastName" WatermarkText="Last Name"></cc1:TextBoxWatermarkExtender>
                        <asp:TextBox runat="server" ID="Guide_Address" CssClass="email"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender runat="server" ID="Guide_Address_WM" TargetControlID="Guide_Address" WatermarkText="Address"></cc1:TextBoxWatermarkExtender>
                        <asp:TextBox runat="server" ID="Guide_City" CssClass="city"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender runat="server" ID="Guide_City_WM" TargetControlID="Guide_City" WatermarkText="City"></cc1:TextBoxWatermarkExtender>
                        <asp:TextBox runat="server" ID="Guide_State" CssClass="state"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender runat="server" ID="Guide_State_WM" TargetControlID="Guide_State" WatermarkText="State"></cc1:TextBoxWatermarkExtender>
                        <asp:TextBox runat="server" ID="Guide_Zip" CssClass="zipcode"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender runat="server" ID="Guide_Zip_WM" TargetControlID="Guide_Zip" WatermarkText="Zip"></cc1:TextBoxWatermarkExtender>
                        <asp:TextBox runat="server" ID="Guide_Email" CssClass="email"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender runat="server" ID="Guide_Email_WM" TargetControlID="Guide_Email" WatermarkText="Email Address"></cc1:TextBoxWatermarkExtender>
				        <div class="checkboxrow2"><asp:CheckBox runat="server" ID="Guide_Updates" Text="Receive Monthly Update Emails" /></div>
                        <%--<div class="checkboxrow2"><asp:CheckBox runat="server" ID="Guide_FutureEditions" Text="Receive Future Editions" /></div>--%>
                        <asp:Panel runat="server" ID="GuideErrorPanel" CssClass="formpanel"><asp:Literal runat="server" ID="GuideError"></asp:Literal></asp:Panel>
				        <asp:Button runat="server" ID="Guide_Submit" Text="Submit" OnClick="Guide_Submit_Click" />
			        </fieldset>
                </asp:Panel>
				
				<div class="footerArrow">
				    <div class="arrowBorder"></div>
				    <div class="arrowDown">▼</div>
					<div class="arrowText">Read More Below</div>
			    </div>
		    </div>
	    </article>    
	    <article class="best-diff">
		    <div class="title">
			    <h2>The Best Pick Difference</h2>
		    </div>
		    <div class="left-side">
			    <h3>Best Pick&trade; Companies found in our report:</h3>
			    <ul>
				    <li>Are required to maintain an A grade annually</li>
				    <li>Average over 100 homeowner reviews each</li>
				    <li>Hold proper licenses and insurance</li>
				    <li>Must requalify each year</li>
			    </ul>
		    </div>
		    <div class="right-side">
			    <asp:Image runat="server" ID="RibbonImage" CssClass="pic-left" />
                <div class="text-right">
                    <asp:Literal runat="server" ID="RibbonText"></asp:Literal>
                </div>
		    </div>
	    </article>
    </section>
</div>
<div class="testimonials">
    <div class="divider"></div>
	<h3 class="LightBlue">What People Are Saying...</h3>
	<a id="start" style="display: none;" href="#"></a>
	<div class="items">
		<asp:ListView runat="server" ID="TList">
	        <LayoutTemplate>
                <ul class="slides">
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li>
				    <blockquote><p><a href="<%=basedomain%>/testimonials"><%# Eval("Body") %></a></p></blockquote>
			    </li>
            </ItemTemplate>
	    </asp:ListView>
	</div>
    <div class="divider"></div>
</div>
<div class="container">
    <section>  
		<div class="title">
			<h2>How We Are Different</h2>
        </div>
        <div class="bpr_Difference">
            <br />
            <asp:Image runat="server" ID="Image3" CssClass="bpr_Difference_image" ImageUrl="/images/Best_pick_difference.png" />
            <br />
            <p>Since our founding in 1997, our principles have been based on accurate and consistent information, and we uphold, share, 
            and publish our standards for your benefit. 
            <a href="http://www.bestpickreports.com/content/methodology">Click Here</a>
            to learn more about our Research Methodology.</p>
		</div>
    </section>
</div>
<div class="container">	    
    <section>
		<article class="blog-con">
		    <div class="title">
			    <h2>Check Out Our Blog</h2>
		    </div>
		    <div class="left-blog"><asp:HyperLink runat="server" ID="BlogCityLink"><asp:Image runat="server" ID="BlogImage" ImageUrl="/images/blogscreen.png" AlternateText="EBSCO Research Blog" ToolTip="EBSCO Research Blog" /></asp:HyperLink></div>
		    <div class="right-side">
			    <asp:Literal runat="server" ID="BlogText"></asp:Literal>
			    <ul class="blog-feeds">
				    <li><asp:HyperLink runat="server" ID="FeedCityLink"><asp:Literal runat="server" ID="FeedCityName"></asp:Literal></asp:HyperLink></li>
			    </ul>
		    </div>
	    </article>
	    <article class="app-con">
		    <div class="title">
			    <h2>On The Go?</h2>
		    </div>
		    <div class="left-side">	
			    <div class="phone"><img src="/images/pic_reports.png" alt="Take Best Pick Reports With You" /></div>
			    <p>Take Best Pick Reports with you! Our easy-to-use app has everything you need to find the Best Pick company you are looking for—anytime and anywhere.</p>
                <p>Download it for your iPhone&reg; or Android&trade; smartphone today!</p>
		    </div>
		    <div class="download-app">
			    <h3>Download the <strong>Best Pick&trade;</strong> App</h3>
			    <div class="ico"><img src="/images/pic_app.png" alt="Best Pick App" /></div>
			    <div class="links">
				    <a href="http://itunes.apple.com/us/app/best-pick-reports/id494057962?mt=8" target="_blank"><img src="/images/btn_app_store.png" alt="Best Pick App for iPhone/iPad" /></a>
				    <a href="https://play.google.com/store/apps/details?id=com.brotherfish" target="_blank"><img src="/images/btn_play.png" alt="Best Pick App for Android" /></a>
			    </div>
		    </div>
	    </article>
    </section>
</div>
</asp:Content>
