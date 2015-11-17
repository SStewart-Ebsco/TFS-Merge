<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="homeownertestimonials.aspx.cs" Inherits="ER_BestPickReports_Dev.homeownertestimonials" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<asp:HiddenField runat="server" ID="HiddenTestimonialCount" />
<asp:HiddenField runat="server" ID="HiddenTotal" />
<div class="cat-page">
		<div class="container">
			<section class="testimonials-full">
				<h1>Homeowner Testimonials</h1>
				<%--<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse vitae risus sed metus ultricies gravida vel id eros. Vivamus non mi porttitor enim viverra adipiscing eu in mauris. Fusce ac nisi ligula. Vestibulum eu orci in dolor congue fringilla. Integer fringilla ipsum et odio porttitor.</p>--%>
				
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>

                            <asp:ListView runat="server" ID="TestimonialList">
	                            <LayoutTemplate>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <article>
				                        <blockquote>
                                            <p><%# Eval("Body") %></p>
                                        </blockquote>
                                    </article>
                                </ItemTemplate>
	                        </asp:ListView>

		                
                    </ContentTemplate>
                </asp:UpdatePanel>
                
			</section>
            <asp:Panel runat="server" ID="MoreContainer" CssClass="bot-more"><asp:LinkButton runat="server" ID="LoadReviews" Text="Read More Reviews" OnClick="LoadReviews_Click"></asp:LinkButton></asp:Panel>
		</div>
	</div>
</asp:Content>
