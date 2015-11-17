<%@ Page Title="" Language="C#" MasterPageFile="~/blogfiles/Admin.Master" AutoEventWireup="true" CodeBehind="articleDetails.aspx.cs" Inherits="ER_BestPickReports_Dev.blogfiles.ArticleDetails" %>
<%@ MasterType VirtualPath="~/blogfiles/Admin.Master" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
    <link rel="stylesheet" href="/blogfiles/assets/css/blog-details.css" type="text/css" media="screen, projection" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:HiddenField runat="server" ID="HiddenImagePath" />
<div class="edit-page">
    <ul class="details edit-widget">
        <li class="datetitle">
			<h2><asp:Label runat="server" ID="ContentAction"></asp:Label></h2>
		</li><!-- datetitle -->

        <li class="details-block details-primary">
            <h3 class="details-block-header">Post Information</h3>
		    <ul>
                <li runat="server" id="errorrow" visible="false">
                    <p><asp:Literal runat="server" ID="ErrorText"></asp:Literal></p>
                </li>
			    <li>
				    <label><span>Post Title:</span></label>
				    <asp:TextBox runat="server" ID="PostTitle" CssClass="txt1"></asp:TextBox>
			    </li>
                <li>
				    <label><span>Post Slug:</span></label>
				    <asp:TextBox runat="server" ID="PostUrlTitle" CssClass="txt1"></asp:TextBox>
			    </li>
			    <li>
				    <label>Publish Date:</label>
				    <telerik:RadDateTimePicker runat="server" ID="PublishDate" EnableTyping="false" DateInput-BackColor="#faf9c3" DateInput-BorderColor="#abadb3" DateInput-HoveredStyle-BorderColor="#abadb3"></telerik:RadDateTimePicker>
			    </li>
                <li>
				    <label>Tags:</label>
				    <asp:TextBox runat="server" ID="Tags" CssClass="txt1"></asp:TextBox>
			    </li>
                <li>
				    <label>Meta Description:</label>
				    <asp:TextBox runat="server" ID="MetaDesc" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
			    </li>
                <li>
			        <label>Summary:</label>
			        <div class="wysiwyg">
                        <asp:TextBox runat="server" ID="SummaryEditor" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
                    </div>
                </li>
		    </ul>
            <br style="clear:both"/>
        </li>

        <li class="details-block details-author">
            <fieldset class="details-fieldset">
                <legend class="details-fieldset-legend">Author</legend>
                
				<label>Author Name(s):</label>
				<asp:TextBox runat="server" ID="AuthorName" TextMode="MultiLine" Rows="5"></asp:TextBox>
				<label>Author Title(s):</label>
				<asp:TextBox runat="server" ID="AuthorTitle" TextMode="MultiLine" Rows="5"></asp:TextBox>
				<label>Author Description(s):</label>
				<asp:TextBox runat="server" ID="AuthorDescription" TextMode="MultiLine" Rows="5"></asp:TextBox>
            </fieldset>
        </li>


        <li class="details-block clearfix">
            <fieldset class="details-fieldset">
                <legend class="details-fieldset-legend">Metro:</legend>
                <asp:CheckBoxList runat="server" ID="CityID" RepeatColumns="3" RepeatDirection="Vertical" RepeatLayout="Table"></asp:CheckBoxList>
            </fieldset>

            <fieldset class="details-fieldset">
                <legend class="details-fieldset-legend">Category:</legend>
			    <asp:CheckBoxList runat="server" ID="BlogCatID" RepeatColumns="3" RepeatDirection="Vertical" RepeatLayout="Table"></asp:CheckBoxList>
            </fieldset>
        </li>



        <li class="details-block">
			<h3 class="details-block-header">Body:</h3>
			<div class="wysiwyg">
                <telerik:RadEditor runat="server" ID="BodyEditor" NewLineBr="False" Width="99%"></telerik:RadEditor>
            </div>
		</li>

        <li class="details-block">
            <h3 class="details-block-header">Image Path:</h3>
            <asp:FileUpload runat="server" ID="ImagePath" CssClass="uploadfield" /><br />
		    <asp:Button runat="server" ID="UploadImage" OnClick="UploadImage_Click" Text="Upload Image" CssClass="uploadbutton" /> 

            <h3 class="details-block-header">Image Position:</h3>
            <table>
                <tr>
                    <td><asp:CheckBox runat="server" ID="ShowInSummary" Text="Show with Summary" /></td>
                    <td><asp:CheckBox runat="server" ID="ShowInBody" Text="Show with Body" /></td>
                </tr>
            </table>

            <div runat="server" id="imagerow" visible="false">
                <asp:Image runat="server" ID="ImageFile" />
            </div>
        </li>
		

        <li class="details-block">
            <div class="details-links">
                    <div class="details-links-item"><label>Atlanta Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="AtlantaLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></div>
                    <div class="details-links-item"><label>Chicago Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="ChicagoLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></div>
                    <div class="details-links-item"><label>Dallas Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="DallasLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></div>

                    <div class="details-links-item"><label>Houston Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="HoustonLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></div>
                    <div class="details-links-item"><label>Northern Virginaia Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="NovaLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></div>
                    <div class="details-links-item"><label>Maryland Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="MarylandLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></div>

                    <div class="details-links-item"><label>Washington, D.C. Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="DCLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></div>
                    <div class="details-links-item"><label>Birmingham Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="BhamLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></div>
                    <div class="details-links-item"><label>Boston Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="BostonLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></div>
                    
                    <div class="details-links-item"><label>Philadelphia Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="PhilLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></div>
            </div>
        </li>
	</ul><!-- edit-widget -->
</div>

    <asp:Button runat="server" ID="Submit" OnClick="Submit_Click" Text="Save and Close"  CssClass="uploadbutton" />
    <asp:Button runat="server" ID="Cancel" OnClick="Cancel_Click" Text="Cancel" CssClass="uploadbutton" />
    <asp:Button runat="server" ID="Delete" OnClick="Delete_Click" Text="Delete" CssClass="uploadbutton delete-button"/>
</asp:Content>
