<%@ Page Title="" Language="C#" MasterPageFile="~/mobile/SiteMobile.Master" AutoEventWireup="true" CodeBehind="testimonials.aspx.cs" Inherits="ER_BestPickReports_Dev.mobile.testimonials" %>
<asp:Content ID="HeadSection" ContentPlaceHolderID="head" runat="server">
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/testimonials")%>
</asp:Content>
<asp:Content ID="ContentSection" ContentPlaceHolderID="Content" runat="server">

    <asp:HiddenField runat="server" ID="HiddenTestimonialCount" />
    <asp:HiddenField runat="server" ID="HiddenTotal" />
    
		<div class="ribbon-banner">
		    <span class="ribbon-banner-title">Homeowner Testimonials</span>
	    </div>
		<asp:UpdatePanel runat="server" ID="Testimonials" ClientIDMode="Static">
            <ContentTemplate>
                <asp:ListView runat="server" ID="TestimonialList">
	                <LayoutTemplate>
                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <article class="testimonial">
				            <blockquote class="testimonial-quote">
                                <%# Eval("Body") %>
                            </blockquote>
                        </article>
                    </ItemTemplate>
	            </asp:ListView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:LinkButton runat="server" ID="LoadReviews" CssClass="button-big" Text="Read More Reviews" OnClick="LoadReviews_Click"></asp:LinkButton>
		
</asp:Content>
<asp:Content ID="ScriptsSection" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
