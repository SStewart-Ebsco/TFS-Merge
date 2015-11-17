<%@ Page Title="" Language="C#" MasterPageFile="~/blogfiles/Blog.Master" AutoEventWireup="true" CodeBehind="postform.aspx.cs" Inherits="ER_BestPickReports_Dev.blogfiles.postform" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<telerik:RadScriptManager runat="server" ID="RSM1"></telerik:RadScriptManager>
<script type="text/javascript">

    $(document).ready(function () {

        $(".hiddenDiv_general").hide();
        $(".hiddenDiv_links").hide();
        $(".hiddenDiv_content").hide();
        $(".hiddenDiv_image").hide();
        $(".show_hide_general").show();
        $(".show_hide_links").show();
        $(".show_hide_content").show();
        $(".show_hide_image").show();

        $('.show_hide_general').click(function () {
            $(".hiddenDiv_general").toggle();
        });

        $('.show_hide_links').click(function () {
            $(".hiddenDiv_links").toggle();
        });

        $('.show_hide_content').click(function () {
            $(".hiddenDiv_content").toggle();
        });

        $('.show_hide_image').click(function () {
            $(".hiddenDiv_image").toggle();
        });

    });

</script>
<asp:HiddenField runat="server" ID="HiddenImagePath" />
<div class="edit-page">
    <div class="edit-widget">
        <div class="datetitle">
			<h2><asp:Label runat="server" ID="ContentAction"></asp:Label></h2>
		</div><!-- datetitle -->
        <a href="#" class="show_hide_general">Post Information</a>
        <div class="hiddenDiv_general">
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
				<label>Metro:</label>
				<asp:CheckBoxList runat="server" ID="CityID" RepeatColumns="3" RepeatDirection="Vertical" RepeatLayout="Table"></asp:CheckBoxList>
			</li>
            <li>
				<label>Category:</label>
				<asp:CheckBoxList runat="server" ID="BlogCatID" RepeatColumns="2" RepeatDirection="Vertical" RepeatLayout="Table"></asp:CheckBoxList>
			</li>
            <li>
				<label>Tags:</label>
				<asp:TextBox runat="server" ID="Tags" CssClass="txt1"></asp:TextBox>
			</li>
            <li>
				<label>Meta Description:</label>
				<asp:TextBox runat="server" ID="MetaDesc" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
			</li>
		</ul>
        </div>
		<a href="#" class="show_hide_content">Post Content</a>
        <div class="hiddenDiv_content">
			<label>Summary:</label>
			<div class="wysiwyg">
                <telerik:RadEditor runat="server" ID="SummaryEditor" NewLineBr="False" Width="99%" Height="200px"></telerik:RadEditor>
            </div>

			<label>Body:</label>
			<div class="wysiwyg">
                <telerik:RadEditor runat="server" ID="BodyEditor" NewLineBr="False" Width="99%"></telerik:RadEditor>
            </div>
		</div>
        <a href="#" class="show_hide_image">Post Image</a>
        <div class="hiddenDiv_image">
            <ul>
                <li>
					<label>Image Path:</label>
                    <asp:FileUpload runat="server" ID="ImagePath" CssClass="uploadfield" /><br />
					<asp:Button runat="server" ID="UploadImage" OnClick="UploadImage_Click" Text="Upload Image" CssClass="uploadbutton" /> 
			    </li>
                <li>
                    <label>Image Position:</label>
                    <table>
                        <tr>
                            <td><asp:CheckBox runat="server" ID="ShowInSummary" Text="Show with Summary" /></td>
                            <td><asp:CheckBox runat="server" ID="ShowInBody" Text="Show with Body" /></td>
                        </tr>
                    </table>
                </li>
                <li runat="server" id="imagerow" visible="false">
                    <asp:Image runat="server" ID="ImageFile" />
                </li>
            </ul>
        </div>
        <a href="#" class="show_hide_links">Related Links</a>
        <div class="hiddenDiv_links">
            <table width="600">
                <tr>
                    <td width="50%"><label class="resetlabel">Atlanta Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="AtlantaLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></td>
                    <td width="50%"><label class="resetlabel">Chicago Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="ChicagoLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></td>
                </tr>
                <tr>
                    <td width="50%"><label class="resetlabel">Dallas Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="DallasLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></td>
                    <td width="50%"><label class="resetlabel">Houston Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="HoustonLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></td>
                </tr>
                <tr>
                    <td width="50%"><label class="resetlabel">Northern Virginaia Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="NovaLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></td>
                    <td width="50%"><label class="resetlabel">Maryland Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="MarylandLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></td>
                </tr>
                <tr>
                    <td width="50%"><label class="resetlabel">Washington, D.C. Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="DCLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></td>
                    <td width="50%"><label class="resetlabel">Birmingham Links:</label><div class="wysiwyg"><telerik:RadEditor runat="server" ID="BhamLinks" NewLineBr="False" Width="270px"></telerik:RadEditor></div><div style="clear:both;"></div><br clear="all" /></td>
                </tr>
            </table>
        </div>
	</div><!-- edit-widget -->
</div>

    <asp:Button runat="server" ID="Submit" OnClick="Submit_Click" Text="Submit"  CssClass="uploadbutton" />
    <asp:Button runat="server" ID="Cancel" OnClick="Cancel_Click" Text="Cancel" CssClass="uploadbutton" />

</asp:Content>
