<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="category.aspx.cs" Inherits="ER_BestPickReports_Dev.category" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="cat-page">
	<div class="container">
		<div id="main-content">
			<article class="featured-cat">
                <asp:Panel runat="server" ID="CategoryContent_Frame" Visible="false">
                    <iframe id="categoryiframe" name="categoryiframe" src="/inlinecontent.aspx?type=category&infoid=<%=catinfoid%>&catid=<%=catid%>&city=<%=strcity%>&cityname=<%=citydisplayname%>&isexpanded=<%=isexpanded %>" frameborder="0" width="100%" height="100%" scrolling="no" class="autoHeight"></iframe>
                </asp:Panel>
                <asp:Panel runat="server" ID="CategoryContent">
				    <div class="head">
                        <asp:Panel runat="server" ID="IconPanel" CssClass="ico"><asp:Image runat="server" ID="IconImage" /></asp:Panel>
					    <h1><asp:Literal runat="server" ID="CategoryName"></asp:Literal></h1>
					    <h2><asp:Literal runat="server" ID="CategoryAbout"></asp:Literal></h2>
				    </div>
				    <div class="entry">
					    <asp:Literal runat="server" ID="Desc"></asp:Literal>
                        <asp:Panel runat="server" ID="ExtDesc" CssClass="hidden" Visible="false">
		                    <asp:Literal runat="server" ID="LongDesc"></asp:Literal>
	                    </asp:Panel>
                        <asp:Panel runat="server" ID="ShowHide" Visible="false" CssClass="read-more">
	                        <a href="#">Read More Information <span></span></a>
                        </asp:Panel>
				    </div>
                </asp:Panel>
			</article>
			<section class="best-picks">

            <asp:LinkButton runat="server" ID="MultipleEMailButton" Text="Email Multiple Contractors" OnClick="MultipleEMailButton_Click" CssClass="emailmultiple"></asp:LinkButton>

				<div class="title">
					<h2><asp:Literal runat="server" ID="BestPickHeader"></asp:Literal></h2>
				</div>

                <asp:ListView runat="server" ID="ContractorList" OnItemDataBound="ContractorList_ItemDataBound">
	                <LayoutTemplate>
                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                    </LayoutTemplate>
                    <ItemTemplate>

                        <article>
                            <asp:Panel runat="server" ID="ContractorContent_Frame" Visible="false">
                                <iframe runat="server" id="conframe" src="" frameborder="0" width="100%" height="100%" scrolling="no" class="autoHeight"></iframe>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="ContractorContent">
                                <div class="head">
							        <div class="ico"><asp:Image runat="server" ID="RibbonImage" /></div>
							        <div class="phone-c"><asp:Literal runat="server" ID="ContractorPhone"></asp:Literal></div>
							        <h3><asp:Hyperlink runat="server" ID="ContractorName"></asp:Hyperlink></h3>
							        <asp:Panel runat="server" ID="EmailPanel" Visible="false" CssClass="request">
                                        <asp:LinkButton runat="server" ID="SendEmailButton" OnClick="EmailButton_Click" Text="Request Information"></asp:LinkButton>
                                    </asp:Panel>
                                    <h4><asp:Literal runat="server" ID="BestPickText"></asp:Literal></h4>
						        </div>
						        <div class="features">
							        <ul>
								        <li runat="server" id="licenseitem"><asp:Literal runat="server" ID="License"></asp:Literal></li>
								        <li runat="server" id="liabilityitem"><asp:Literal runat="server" ID="Liability"></asp:Literal></li>
								        <li runat="server" id="insurancestateitem"><asp:Literal runat="server" ID="Insurance"></asp:Literal></li>
							        </ul>
						        </div>
						        <div class="desc">
							        <asp:Literal runat="server" ID="ShortDesc"></asp:Literal>
						        </div>
						        <div class="full"><asp:HyperLink runat="server" ID="ReadMoreLink">View company name's full profile and reviews</asp:HyperLink></div>
                            </asp:Panel>
					    </article>
                               
                    </ItemTemplate>
	            </asp:ListView>
					    
			</section>
            <section class="bottom-articles">

                    <asp:ListView runat="server" ID="ArticleList" Visible="false">
	                    <LayoutTemplate>
                            <div class="title">
					            <h2>Helpful Articles</h2>
				            </div>
                            <div class="articles">
					            <ul>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                </ul>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li><a href="<%=basedomain %>/<%# Eval("CatName") %>/tips/<%# Eval("UrlTitle") %>" rel="nofollow"><%# Eval("Title") %></a></li>
                        </ItemTemplate>
	                </asp:ListView>

            </section>

            <asp:Panel runat="server" ID="AdditionalPanel" CssClass="bottom-articles" Visible="false">

                    <div class="title">
					    <h2>Local Service Areas</h2>
				    </div>
                    <asp:Literal runat="server" ID="AreaAdditionalInfo"></asp:Literal>

            </asp:Panel>

		</div>
		<aside id="sidebar">
			<div class="best-spicks">
				<h3><asp:Image runat="server" ID="RightRailRibbon" /><asp:Literal runat="server" ID="RightRailText" Text="About<br />Best Pick<br />Reports&reg;"></asp:Literal></h3>
			</div>
			<div class="videoBanner">
				<asp:HyperLink runat="server" ID="VideoBannerLink" ImageUrl="/images/banner_bpr_video.gif" EnableViewState="False" CssClass="videoButton fancybox.iframe">
				</asp:HyperLink>
			</div>
			<div class="download">
			    <h3>Download<br>The Best Pick&trade; App</h3>
			    <div class="ico"><img src="/images/pic_app.png" alt="Best Pick App" /></div>
			    <div class="links">
				    <a href="http://itunes.apple.com/us/app/best-pick-reports/id494057962?mt=8" target="_blank"><img src="/images/btn_app_store.png" alt="Best Pick App for iPhone/iPad" /></a>
				    <a href="https://play.google.com/store/apps/details?id=com.brotherfish" target="_blank"><img src="/images/btn_play.png" alt="Best Pick App for Android" /></a>
			    </div>
		    </div>
			<div class="proccess">
			    <h3>Our Company Screening Process</h3>
			    <div class="banner"><img src="/images/banner_research_process.png" alt="Our Company Screening Process" /></div>
		    </div>
		</aside>
	</div>
</div>

<asp:Panel runat="server" ID="FormContainer" DefaultButton="EmailOK" CssClass="registration-form">
    <asp:HiddenField runat="server" ID="EmailContractorID" Value="defaultvalue" />
    <asp:HiddenField runat="server" ID="ContractorEmail" />
    <asp:HiddenField runat="server" ID="ContractorName" />
    <asp:HiddenField runat="server" ID="MultipleEmailIDs" />

    <asp:LinkButton runat="server" ID="TopCancel" OnClick="Cancel_EmailClick" CssClass="btn-close2"></asp:LinkButton>
	<%--<a href="#" onclick="hidemodalemail()" class="btn-close"></a>--%>

    <asp:Panel runat="server" ID="ChooseEmailPanel" CssClass="modalPopup modalPopupEmail" Visible="false">
        <h3>Please select which Best Pick companies you<br />would like to contact about your project:</h3>
        <p class="emptynote"></p>
        <div class="fields">

            <asp:ListView runat="server" ID="EmailContractorList" OnItemDataBound="EmailContractorList_ItemDataBound">
	            <LayoutTemplate>
                    <table>
                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:CheckBox runat="server" ID="ContractorEmailID" ValidationGroup="GroupEmail" /><asp:HiddenField runat="server" ID="HiddenListID" /><asp:Image runat="server" ID="EmailAlert" ImageUrl="/images/alert.png" CssClass="alert" /><label><asp:Label runat="server" ID="ContractorLabel"></asp:Label><asp:Label runat="server" ID="Asterisk">*</asp:Label></label>
                        </td>
			        </tr>
                </ItemTemplate>
	        </asp:ListView>
            <p runat="server" id="alertnote" visible="false" class="bnote">*Note: Not all companies accept email information requests;<br />these companies prefer to be contacted by telephone.</p>
            <p runat="server" id="errornote" visible="false" class="bnote rednote">You must select at least one company in order to continue.</p>
            <fieldset class="submit">
                <asp:Button runat="server" ID="ContinueButton" Text="Continue" OnClick="ContinueButton_Click" />
            </fieldset>
        </div>
    </asp:Panel>

    <div runat="server" id="ModalEmailForm" class="modalPopup modalPopupEmail">
    <h3>You are requesting more information from:<strong><asp:Label runat="server" ID="EmailContractorName"></asp:Label></strong></h3>
		<p class="note">Please fill out the following form completely:</p>
        <div class="fields">
		    <fieldset>
			    <label class="req">First Name:*</label>
			    <asp:TextBox runat="server" ID="Name"></asp:TextBox>
                <asp:Label runat="server" ID="ReqName" Text="First Name is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
		    <fieldset>
			    <label class="req">Last Name:*</label>
			    <asp:TextBox runat="server" ID="LastName"></asp:TextBox>
                <asp:Label runat="server" ID="ReqLastName" Text="Last Name is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
		    <fieldset class="full">
			    <label class="req">Street Address:*</label>
			    <asp:TextBox runat="server" ID="Address" Columns="38"></asp:TextBox>
                <asp:Label runat="server" ID="ReqAddress" Text="Address is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
		    <fieldset>
			    <label>City:</label>
			    <asp:TextBox runat="server" ID="City"></asp:TextBox>
		    </fieldset>
		    <fieldset class="req short">
			    <label class="req">Zip Code:*</label>
			    <asp:TextBox runat="server" ID="Zip" Columns="10"></asp:TextBox>
                <asp:Label runat="server" ID="ReqZip" Text="Zip Code is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
		    <fieldset class="medium">
			    <label class="req">Email:*</label>
			    <asp:TextBox runat="server" ID="Email" Columns="30"></asp:TextBox>
                <asp:Label runat="server" ID="ReqEmail" Text="A valid Email is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
		    <fieldset class="half">
			    <label class="req">Primary Phone:*</label>
			    <asp:TextBox runat="server" ID="FormPhone"></asp:TextBox>
                <asp:Label runat="server" ID="ReqPhone" Text="A valid Phone is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
		    <fieldset class="half">
			    <label>Alternate Phone:</label>
			    <asp:TextBox runat="server" ID="FormPhone2"></asp:TextBox>
		    </fieldset>
		    <fieldset class="medium">
			    <label class="req">Type of Work Needed:*</label>
			    <asp:TextBox runat="server" ID="WorkType" Columns="38"></asp:TextBox>
                <asp:Label runat="server" ID="ReqWorkType" Text="Type of Work is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
		    <fieldset class="full">
			    <label class="req">Message:*</label>
			    <asp:TextBox runat="server" ID="Message" TextMode="MultiLine" Rows="3" Columns="41"></asp:TextBox>
                <asp:Label runat="server" ID="ReqMessage" Text="Message is required!" ForeColor="Red" Visible="false"></asp:Label>
		    </fieldset>
            <asp:Panel runat="server" ID="ReqPanel" Visible="false" CssClass="reqpanel">
                <p style="text-align:center; color:#ff0000;">*Please fill out all required fields.</p>
            </asp:Panel>
		    <fieldset class="submit">
			    <div class="cancel"><asp:LinkButton runat="server" ID="EmailCancel" Text="Cancel" OnClick="Cancel_EmailClick"></asp:LinkButton></div>
			    <asp:Button runat="server" ID="EmailOK" Text="Submit" OnClick="OK_EmailClick" />
		    </fieldset>
        </div>
    </div>
      
    <asp:Panel runat="server" ID="EmailComplete" CssClass="modalPopup modalPopupEmail" visible="false">
        <h3>Success!</h3>
        <p class="note">You have successfully completed the request for information form.  Your information will be forwarded on to the company or companies you selected, as well as to our help desk.</p>
        <fieldset class="submit">
            <asp:Button runat="server" ID="CloseEmail" Text="Close" OnClick="CloseEmail_Click" />
        </fieldset>
        <img src="http://aae.tradatracker.net/track/conv.gif?advertiser=5dec707028b05bcbd3a1db5640f842c5&action=BestPick_Lead" />
    </asp:Panel>

</asp:Panel>

<!-- Fake button to appease the MPE -->
<asp:Button runat="server" ID="EmailFakeButton" style="display:none" />
<cc1:ModalPopupExtender runat="server" ID="SendEmail" PopupControlID="FormContainer" TargetControlID="EmailFakeButton" BackgroundCssClass="overlay" PopupDragHandleControlID="EmptyPanel" RepositionMode="None"></cc1:ModalPopupExtender>
<asp:Panel runat="server" ID="EmptyPanel"></asp:Panel>
<%= System.Web.Optimization.Scripts.Render("~/Scripts/autoHeight")%>
<script type="text/javascript">

    function showmodalemail(id, email, name) {
        // set hidden form values
        document.getElementById('<%= EmailContractorID.ClientID %>').value = id;
        document.getElementById('<%= ContractorEmail.ClientID %>').value = email;
        document.getElementById('<%= EmailContractorName.ClientID %>').innerHTML = name;
        document.getElementById('<%= ContractorName.ClientID %>').value = name;
        document.getElementById('<%= ModalEmailForm.ClientID %>').style.display = "block";

        var element = document.getElementById('<%= ReqPanel.ClientID %>');
        if (element != null && element.value != '') {
            element.style.display = "none";
        }

        document.getElementById('<%= Name.ClientID %>').value = "";
        document.getElementById('<%= LastName.ClientID %>').value = "";
        document.getElementById('<%= City.ClientID %>').value = "";
        document.getElementById('<%= Address.ClientID %>').value = "";
        document.getElementById('<%= Zip.ClientID %>').value = "";
        document.getElementById('<%= Email.ClientID %>').value = "";
        document.getElementById('<%= WorkType.ClientID %>').value = "";
        document.getElementById('<%= FormPhone.ClientID %>').value = "";
        document.getElementById('<%= FormPhone2.ClientID %>').value = "";
        document.getElementById('<%= Message.ClientID %>').value = "";

        $find('<%= SendEmail.ClientID %>').show();
    }

    function showExt(catinfoid, catid, citydisplayname, isexpanded, strcity) {
        // toggle is expanded
        if (isexpanded == "true")
            isexpanded = "false";
        else
            isexpanded = "true";

        var url = "/inlinecontent.aspx?type=category&infoid=" + catinfoid + "&catid=" + catid + "&city=" + strcity + "&cityname=" + citydisplayname + "&isexpanded=" + isexpanded;
        //alert(url);

        document.getElementById('categoryiframe').src = url;
    }

    function hidemodalemail() {
        $.find('<%= SendEmail.ClientID %>').hide();
    }
</script>

</asp:Content>
