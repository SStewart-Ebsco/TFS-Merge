<%@ Page Title="" Language="C#" MasterPageFile="~/mobile/SiteMobile.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="ER_BestPickReports_Dev.mobile.Category" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/category")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

    <article class="featured-cat">
        <asp:Panel runat="server" ID="CategoryContent_Frame" Visible="false">
            <iframe id="categoryiframe" name="categoryiframe" src="/mobile/inlinecontent.aspx?type=category&infoid=<%=catinfoid%>&catid=<%=catid%>&city=<%=strcity%>&cityname=<%=citydisplayname%>&isexpanded=<%=isexpanded %>" frameborder="0" width="100%" height="100%" scrolling="no" class="autoHeight"></iframe>
        </asp:Panel>
        <asp:Panel runat="server" ID="CategoryContent">
            <div class="category-header category-container">
                <asp:Image runat="server" ID="IconImage" class="category-categoryIcon" />
		        <h1 class="category-categoryName category-container-content"><asp:Literal runat="server" ID="CategoryName"></asp:Literal></h1>
            </div>
            <div style="clear:both;"></div>
        </asp:Panel>
    </article>

    <asp:Panel runat="server" CssClass="savetime-background sprite category-btn_emailmultiple" ID="MultipleEmailButton"></asp:Panel>

    <div class="ribbon-banner">
		<span class="ribbon-banner-title"><asp:Literal runat="server" ID="BestPickHeader"></asp:Literal></span>
	</div>
    <div class="category-banner-alert">
        <span>*Select a company to view their detailed profile</span>
    </div>


    <asp:ListView runat="server" ID="ContractorList" OnItemDataBound="ContractorList_ItemDataBound">
	    <LayoutTemplate>
            <div class="category-contractor-list">
                <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
            </div>
        </LayoutTemplate>
        <ItemTemplate>

            <article>
                <asp:Panel runat="server" ID="ContractorContent_Frame" Visible="false">
                    <iframe runat="server" id="conframe" src="" frameborder="0" width="100%" height="100%" scrolling="no" class="autoHeight"></iframe>
                </asp:Panel>
                <asp:Panel runat="server" ID="ContractorContent">
                    <div class="category-contractor-item category-container contractor-item">
						<div class="category-ribbonIcon"><asp:Image runat="server" ID="RibbonImage" /></div>

                        <div class="category-contractor-info category-container-content">
						    <h3 class="category-contractor-name contractor-name"><asp:Hyperlink runat="server" ID="ContractorName"></asp:Hyperlink></h3>
                            <h4><asp:Literal runat="server" ID="BestPickText"></asp:Literal></h4>

                            <div class="category-contractor-contactList">
                                <div class="category-contractor-contact">
                                    <a runat="server" ID="ReadMoreLink" class="" rel="nofollow">
                                        <div class="sprite category-info category-contractor-contact-icon"></div>
		                                <span>Profile</span>
                                    </a>
                                </div>
						        <asp:Panel runat="server" ID="EmailPanel" Visible="false" CssClass="category-contractor-contact">
                                    <a runat="server" ID="SendEmailButton" class="contractor-email-link">
                                        <div class="sprite category-email category-contractor-contact-icon"></div>
                                        <span>Email</span>
                                    </a>
                                </asp:Panel>
						        <div class="category-contractor-contact">
                                    <a runat="server" ID="ContractorPhone" class="" rel="nofollow">
                                        <div class="sprite category-call category-contractor-contact-icon"></div>
		                                <span>Call</span>
                                    </a>
                                </div>
                            </div>
                            <input type="hidden" class="contractor-cid" runat="server" ID="HiddenListID" />
                            <input type="hidden" class="contractor-email" runat="server" ID="HiddenListEmail" />
                        </div>
					</div>
                </asp:Panel>
			</article>
                               
        </ItemTemplate>
        <ItemSeparatorTemplate>
            <div class="category-separator" ></div>
        </ItemSeparatorTemplate>
	</asp:ListView>


    <asp:ListView runat="server" ID="ArticleList" Visible="false" OnItemDataBound="ArticleList_ItemDataBound">
	    <LayoutTemplate>
            <div class="ribbon-banner">
				<span class="ribbon-banner-title">Related Articles</span>
			</div>
            <div class="category-articles">
                <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
            </div>
            <div style="clear:both;"></div>
        </LayoutTemplate>
        <ItemTemplate>
            <a runat="server" ID="articleLink" class="category-container category-article-item" rel="nofollow">
                <div class="sprite category-article_item category-articleIcon"></div>
		        <asp:Label runat="server" ID="articleName" CssClass="category-container-content"></asp:Label>
            </a>
        </ItemTemplate>
	</asp:ListView>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
	
    <!-- Modal windows -->
    <asp:Panel runat="server" ID="ModalWindows" ClientIDMode="Static">

        <asp:HiddenField runat="server" ID="EmailContractorID" Value="defaultvalue" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="ContractorEmail" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="ContractorName" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="MultipleEmailIDs" ClientIDMode="Static" />

        <asp:HiddenField runat="server" ID="HiddenListCityId" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="HiddenListAreaId" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="HiddenListCategoryId" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="HiddenListIsPpc" ClientIDMode="Static" />
    
        <asp:Panel runat="server" ID="ChooseEmailPanel" CssClass="modal-win-center" ClientIDMode="Static">
            <div class="modal-win-bg"></div>
            <div class="modal-win-close"></div>
            <div class="modal-win">
                <div class="modal-win-body">
                    <span class="modal-win-header">Please select which Best Pick companies you would like to contact about your project:</span>
                    <hr class="modal-win-line" />
                    <asp:ListView runat="server" ID="EmailContractorList" OnItemDataBound="EmailContractorList_ItemDataBound">
                        <LayoutTemplate>
                            <ul class="modal-win-list">
                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                            </ul>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li>
                                <input type="checkbox" runat="server" ID="ContractorEmailID" class="modal-win-checkbox" />
                                <asp:Label runat="server" ID="ContractorLabel" CssClass="modal-win-list-text js-contractor-label" AssociatedControlID="ContractorEmailID"></asp:Label>
                                <div class="clear"></div>
                                <asp:HiddenField runat="server" ID="HiddenListID" />
                                <asp:HiddenField runat="server" ID="HiddenListEmail" />
                            </li>
                        </ItemTemplate>
                    </asp:ListView>
                    <span runat="server" id="alertnote" visible="false" class="modal-win-note">*Note: Not all companies accept email information requests; these companies prefer to be contacted by telephone.</span>
                    <p runat="server" id="errornote" visible="true" style="display:none;" class="modal-win-note modal-win-rednote" clientidmode="Static">You must select at least one company in order to continue.</p>
                    <button id="ContractorsModalContinue" class="modal-button">Continue</button>
                </div>
            </div>
        </asp:Panel>

        <div runat="server" id="ModalEmailForm" class="emailform" ClientIDMode="Static">
            <div class="emailform-content container">
                <span class="emailform-header">You are requesting more information from:</span>
                <asp:Label runat="server" ID="EmailContractorName" CssClass="emailform-header-company"></asp:Label>
                <asp:HiddenField runat="server" ID="EmailFormContractorName"/>
                <hr class="emailform-line" />
                <span class="emailform-intro">Please fill out the following form completely:</span>
                <div class="emailform-field-container fifty">
                    <div class="emailform-field-label required">
                        First Name:</div>
                    <asp:TextBox runat="server" ID="Name" fieldName="firstName" type="text" CssClass="emailform-field"></asp:TextBox>
                    <asp:requiredfieldvalidator id="NameFieldValidator"
                        controltovalidate="Name"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your First Name"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                </div>
                <div class="emailform-field-container fifty">
                    <div class="emailform-field-label required">
                        Last Name:</div>
                    <asp:TextBox runat="server" ID="LastName" fieldName="lastName" type="text" CssClass="emailform-field"></asp:TextBox>
                    <asp:requiredfieldvalidator id="LastNameFieldValidator"
                        controltovalidate="LastName"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Last Name"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                </div>
                <div class="emailform-field-container">
                    <div class="emailform-field-label required">
                        Street Address:</div>
                    <asp:TextBox runat="server" ID="Address" fieldName="streetAddress" Columns="38" type="text"
                        CssClass="emailform-field"></asp:TextBox>
                    <asp:requiredfieldvalidator id="AddressFieldValidator"
                        controltovalidate="Address"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Address"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                </div>
                <div class="emailform-field-container sixty">
                    <div class="emailform-field-label ">
                        City:</div>
                    <asp:TextBox runat="server" ID="City" fieldName="city" type="text" CssClass="emailform-field"></asp:TextBox>
                </div>
                <div class="emailform-field-container forty">
                    <div class="emailform-field-label required">
                        Zip Code:</div>
                    <asp:TextBox runat="server" ID="ZipForMail" fieldName="zipCode" Columns="10" type="text"
                        CssClass="emailform-field"></asp:TextBox>
                    <asp:requiredfieldvalidator id="ZipFieldValidator"
                        controltovalidate="ZipForMail"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Zip Code"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                    <asp:RegularExpressionValidator id="ZipFieldValidator2"
                        ValidationExpression="^[0-9]{5}$"
                        controltovalidate="ZipForMail"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Zip Code correctly"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:RegularExpressionValidator>
                </div>
                <div class="emailform-field-container">
                    <div class="emailform-field-label required">
                        Email:</div>
                    <asp:TextBox runat="server" ID="Email" fieldName="email" Columns="30" type="text" CssClass="emailform-field"></asp:TextBox>
                    <asp:requiredfieldvalidator id="EmailFieldValidator"
                        controltovalidate="Email"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Email"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                    <asp:RegularExpressionValidator id="EmailFieldValidator2"
                        controltovalidate="Email"
                        ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Email correctly"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:RegularExpressionValidator>
                </div>
                <div class="emailform-field-container fifty ">
                    <div class="emailform-field-label required">
                        Primary Phone:</div>
                    <asp:TextBox runat="server" ID="FormPhone" fieldName="primaryPhone" type="phone" CssClass="emailform-field"></asp:TextBox>
                    <asp:requiredfieldvalidator id="FormPhoneFieldValidator"
                        controltovalidate="FormPhone"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Primary Phone"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                    <asp:RegularExpressionValidator id="FormPhoneFieldValidator2"
                        ValidationExpression="^\D*(\d{1}\D*){10}$"
                        controltovalidate="FormPhone"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Phone correctly."
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:RegularExpressionValidator>
                </div>
                <div class="emailform-field-container fifty">
                    <div class="emailform-field-label">
                        Alternate Phone:</div>
                    <asp:TextBox runat="server" ID="FormPhone2" fieldName="alternatePhone" type="text" CssClass="emailform-field"></asp:TextBox>
                    <asp:RegularExpressionValidator id="FormPhone2FieldValidator2"
                        ValidationExpression="^\D*(\d{1}\D*){10}$"
                        controltovalidate="FormPhone2"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter your Phone correctly."
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:RegularExpressionValidator>
                </div>
                <div class="emailform-field-container">
                    <div class="emailform-field-label required">
                        Type of Work:</div>
                    <asp:TextBox runat="server" ID="WorkType" fieldName="typeOfWork" Columns="38" type="text"
                        CssClass="emailform-field"></asp:TextBox>
                    <asp:requiredfieldvalidator id="WorkTypeFieldValidator"
                        controltovalidate="WorkType"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter Type of Work"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                </div>
                <div class="emailform-field-container">
                    <div class="emailform-field-label required">
                        Message:</div>
                    <asp:TextBox runat="server" ID="Message" fieldName="message" TextMode="MultiLine" Rows="3" Columns="41"
                        name="message" type="text" CssClass="emailform-field multiline"></asp:TextBox>
                    <asp:requiredfieldvalidator id="MessageFieldValidator"
                        controltovalidate="Message"
                        validationgroup="EmailFormGroup"
                        errormessage="Enter Message"
                        runat="Server"
                        CssClass="emailform-validator">
                    </asp:requiredfieldvalidator>
                </div>
            </div>
                <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="True" DisplayMode="BulletList" ValidationGroup="EmailFormGroup"/>--%>
                <%--<p style="text-align: center; color: #ff0000;">
                    *Please fill out all required fields correctly.</p>--%>
            <div class="emailform-footer container">
                <div class="emailform-buttons">
                    <button id="cancelButton" class="emailform-button cancel-button">
                        Cancel</button>
                    <asp:Button runat="server" ID="SubmitEmailformButton" Text="Submit" CssClass="emailform-button submit-button"
                        CausesValidation="True" ValidationGroup="EmailFormGroup" ClientIDMode="Static"/>
                </div>
            </div>
        </div>

        <asp:Panel runat="server" ID="EmailComplete" CssClass="modal-win-center" ClientIDMode="Static">
            <div class="modal-win-bg">
            </div>
            <div class="modal-win-close">
            </div>
            <div class="modal-win">
                <div class="modal-win-body">
                    <span class="modal-win-header">Success!</span>
                    <hr class="modal-win-line" />
                    <span class="modal-win-text">Your email request has been succesfully delivered.</span>
                    <button class="modal-button">
                        Continue</button>
                </div>
            </div>
        </asp:Panel>

    </asp:Panel>
    
    <%= System.Web.Optimization.Scripts.Render("~/Scripts/autoHeight")%>
</asp:Content>