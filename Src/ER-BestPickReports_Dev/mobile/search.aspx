<%@ Page Title="" Language="C#" MasterPageFile="~/mobile/SiteMobile.Master" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="ER_BestPickReports_Dev.mobile.search" %>

<asp:Content ID="HeadSection" ContentPlaceHolderID="head" runat="server">
    <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/search")%>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">

<div class="ribbon-banner">
	<span class="ribbon-banner-title">Search Results</span>
</div>

<asp:Label runat="server" ID="KeywordTitle" CssClass="search-term" />


<div class="search-filter">
	<label class="search-filter-label">Filter By: </label>
    <asp:DropDownList CssClass="search-filter-dropdown" runat="server" ID="FilterBy" AutoPostBack="true">
        <asp:ListItem Value="0">All Results</asp:ListItem>
        <asp:ListItem Value="1">Category</asp:ListItem>
        <asp:ListItem Value="2">Company</asp:ListItem>
        <asp:ListItem Value="3">Blog</asp:ListItem>
        <asp:ListItem Value="4">Article</asp:ListItem>
        <asp:ListItem Value="5">Other</asp:ListItem>
    </asp:DropDownList>
	<label class="search-filter-label">Showing Results For: </label>
    <asp:DropDownList CssClass="search-filter-dropdown" runat="server" ID="ShowResultsFor" AutoPostBack="true"></asp:DropDownList>
</div>


    <asp:Panel runat="server" ID="DidYouMeanPanel" Visible="false" CssClass="didyoumean">
        Did You Mean: <asp:Literal ID="Suggestions" runat="server"></asp:Literal><br />
    </asp:Panel>
    
    <asp:Panel runat="server" ID="ErrorPanel" CssClass="errorpanel" Visible="false">
	    <asp:Label runat="server" ID="ErrorText" CssClass="emptylist"></asp:Label>
	</asp:Panel>

    <asp:Panel runat="server" ID="CategoryPanel">
        <div class="search-section-title">Category Results</div>
                    
        <asp:Panel runat="server" ID="CategorySimple">
            <asp:ListView runat="server" ID="CategorySimpleList" OnItemDataBound="CategorySimpleList_ItemDataBound">
                <LayoutTemplate>
					<ul class="search-result-list">
						<asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
					</ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li class="search-result-list-item"><asp:HyperLink runat="server" ID="CategoryLink"></asp:HyperLink></li>
                </ItemTemplate>
                <EmptyDataTemplate>
		            <div class="search-result-no">Your search returned no results.</div>
		        </EmptyDataTemplate>
            </asp:ListView>
        </asp:Panel>

        <asp:Panel runat="server" ID="CategoryAll">
            <asp:ListView runat="server" ID="CategoryAllList" OnItemDataBound="CategoryAllList_ItemDataBound">
                <LayoutTemplate>
                    <section class="category-results">          
						<asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
				    </section>
                </LayoutTemplate>
                <ItemTemplate>
                    <div>
                        <!-- Category Name -->
                        <asp:Label runat="server" ID="CategoryName" class="search-category-name"/>
                        <!-- Category state list -->
                        <asp:ListView runat="server" ID="CategoryStateList" OnItemDataBound="CategoryStateList_ItemDataBound">
                            <LayoutTemplate>
                                <ul class="search-category-list">
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li class="search-category-list-item">
                                    <asp:Label runat="server" ID="StateName"></asp:Label>
                                    <!-- Area list for each state -->
                                    <div class="search-category-list-item-links">
                                        <asp:Literal runat="server" ID="AreaLinks"></asp:Literal>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </asp:Panel>

    </asp:Panel>


    <asp:Panel runat="server" ID="CompanyPanel">
        <div class="search-section-title">Company Results</div>

        <asp:Panel runat="server" ID="CompanySimple">
            <asp:ListView runat="server" ID="CompanySimpleList" OnItemDataBound="CompanySimpleList_ItemDataBound">
                <LayoutTemplate>
				    <ul class="search-result-list">
					    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
				    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li class="search-result-list-item">
                        <asp:HyperLink runat="server" ID="CompanyLink" class="search-result-list-item-category"></asp:HyperLink> in <asp:Literal runat="server" ID="CategoryName"></asp:Literal>
                    </li>
                </ItemTemplate>
                <EmptyDataTemplate>
		            <div class="search-result-no">Your search returned no results.</div>
		        </EmptyDataTemplate>
            </asp:ListView>
        </asp:Panel>

        <asp:Panel runat="server" ID="CompanyAll">
                <asp:ListView runat="server" ID="CompanyAllList" OnItemDataBound="CompanyAllList_ItemDataBound">
                    <LayoutTemplate>
                        <section class="company-results">
						        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
				        </section>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <article>
                            <!-- Company Name -->
                            <h3><asp:Label runat="server" ID="CompanyName"/></h3>
                            <!-- Company category list -->
                            <asp:ListView runat="server" ID="CompanyCategoryList" OnItemDataBound="CompanyCategoryList_ItemDataBound">
                                <LayoutTemplate>
                                    <ul>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li>
                                        <div class="label"><asp:Label runat="server" ID="CategoryName"></asp:Label></div>
                                        <!-- Area list for each category -->
                                        <div class="list">
                                            <asp:Literal runat="server" ID="AreaLinks"></asp:Literal>
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:ListView>
                        </article>
                    </ItemTemplate>
                </asp:ListView>
            </asp:Panel>
        
    </asp:Panel>


    <!-- Format of this section stays the same but it
            goes off of the cookie or area selected -->
	<asp:Panel runat="server" ID="ArticlePanel">
		<div class="search-section-title">Article Results</div>
        <asp:ListView runat="server" ID="ArticleList" OnItemDataBound="ArticleList_ItemDataBound">
            <LayoutTemplate>
                <ul class="search-result-list">
			        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li class="search-result-list-item"><asp:HyperLink runat="server" ID="ArticleLink"></asp:HyperLink></li>
            </ItemTemplate>
            <EmptyDataTemplate>
		        <div class="search-result-no">Your search returned no results.</div>
		    </EmptyDataTemplate>
        </asp:ListView>
    </asp:Panel>

    <!-- Format of this section stays the same but it
            goes off of the cookie or area selected -->
    <asp:Panel runat="server" ID="BlogPanel">
        <div class="search-section-title">Blog Post Results</div>
        <asp:ListView runat="server" ID="BlogList" OnItemDataBound="BlogList_ItemDataBound">
		    <LayoutTemplate>
		        <ul class="search-result-list search-result-list-cursive">
		            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
		        </ul>
		    </LayoutTemplate>
		    <ItemTemplate>
				<li runat="server" id="ListItem" class="search-result-list-item"><asp:Hyperlink runat="server" ID="PostLink"><%# Eval("Title") %></asp:Hyperlink></li>
		    </ItemTemplate>
            <EmptyDataTemplate>
		        <div class="search-result-no">Your search returned no results.</div>
		    </EmptyDataTemplate>
		</asp:ListView>
    </asp:Panel>

    <!-- This data stays the same regardless of area or all -->
    <asp:Panel runat="server" ID="OtherPanel">
        <div class="search-section-title">Other Results</div>
        <asp:ListView runat="server" ID="ContentList" OnItemDataBound="ContentList_ItemDataBound">
		    <LayoutTemplate>
		        <ul class="search-result-list">
		            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
		        </ul>
		    </LayoutTemplate>
		    <ItemTemplate>
				<li runat="server" id="ListItem" class="search-result-list-item"><asp:Hyperlink runat="server" ID="ContentLink"><%# Eval("PageName") %></asp:Hyperlink></li>
		    </ItemTemplate>
            <EmptyDataTemplate>
		        <div class="search-result-no">Your search returned no results.</div>
		    </EmptyDataTemplate>
		</asp:ListView>
    </asp:Panel>

	
</asp:Content>
