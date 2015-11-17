<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="ER_BestPickReports_Dev.search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="cat-page">
			<div class="container">
				<article class="search-results">
					<div class="head">
						<h2>Search Results</h2>
						<h3>Search Term: <strong><asp:Label runat="server" ID="KeywordTitle" /></strong></h3>
					</div>
					<div class="filter-tools">
						<div class="filter">
							<label>Filter By: </label>
                            <asp:DropDownList runat="server" ID="FilterBy" AutoPostBack="true">
                                <asp:ListItem Value="0">All Results</asp:ListItem>
                                <asp:ListItem Value="1">Category</asp:ListItem>
                                <asp:ListItem Value="2">Company</asp:ListItem>
                                <asp:ListItem Value="3">Blog</asp:ListItem>
                                <asp:ListItem Value="4">Article</asp:ListItem>
                                <asp:ListItem Value="5">Other</asp:ListItem>
                            </asp:DropDownList>
						</div>
						<div class="results">
							<label>Showing Results For: </label>
                            <asp:DropDownList runat="server" ID="ShowResultsFor" AutoPostBack="true"></asp:DropDownList>
						</div>
					</div>
				</article>

                <asp:Panel runat="server" ID="DidYouMeanPanel" Visible="false" CssClass="didyoumean">
                    Did You Mean: <asp:Literal ID="Suggestions" runat="server"></asp:Literal><br />
                </asp:Panel>
    
                <asp:Panel runat="server" ID="ErrorPanel" CssClass="errorpanel" Visible="false">
	                <asp:Label runat="server" ID="ErrorText" CssClass="emptylist"></asp:Label>
	            </asp:Panel>

                <asp:Panel runat="server" ID="CategoryPanel">
                    <div class="title">
						<h2>Category Results</h2>
					</div>
                    
                <asp:Panel runat="server" ID="CategorySimple">
                    <asp:ListView runat="server" ID="CategorySimpleList" OnItemDataBound="CategorySimpleList_ItemDataBound">
                        <LayoutTemplate>
                            <div class="list-results">
					            <ul>
						            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
					            </ul>
				            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li><asp:HyperLink runat="server" ID="CategoryLink"></asp:HyperLink></li>
                        </ItemTemplate>
                        <EmptyDataTemplate>
		                    <ul>
		                        <li class="">Your search returned no results.</li>
		                    </ul>
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
                                <article>
                                    <!-- Category Name -->
                                    <h3><asp:Label runat="server" ID="CategoryName"/></h3>
                                    <!-- Category state list -->
                                    <asp:ListView runat="server" ID="CategoryStateList" OnItemDataBound="CategoryStateList_ItemDataBound">
                                        <LayoutTemplate>
                                                <ul>
                                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                                </ul>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <li>
                                                <div class="label"><asp:Label runat="server" ID="StateName"></asp:Label></div>
                                                <!-- Area list for each state -->
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


        <asp:Panel runat="server" ID="CompanyPanel">
        
            <div class="title">
				<h2>Company Results</h2>
			</div>

            <asp:Panel runat="server" ID="CompanySimple">
                <asp:ListView runat="server" ID="CompanySimpleList" OnItemDataBound="CompanySimpleList_ItemDataBound">
                    <LayoutTemplate>
                        <div class="list-results">
					                <ul>
						                <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
					                </ul>
				                </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li><asp:HyperLink runat="server" ID="CompanyLink"></asp:HyperLink> in <asp:Literal runat="server" ID="CategoryName"></asp:Literal></li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
		                <ul>
		                    <li class="">Your search returned no results.</li>
		                </ul>
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
				    <div class="list-results">
					    <div class="title">
						    <h2>Article Results</h2>
					    </div>
                        <asp:ListView runat="server" ID="ArticleList" OnItemDataBound="ArticleList_ItemDataBound">
                            <LayoutTemplate>
                                <ul>
			                        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li><asp:HyperLink runat="server" ID="ArticleLink"></asp:HyperLink></li>
                            </ItemTemplate>
                            <EmptyDataTemplate>
		                        <ul>
		                            <li class="">Your search returned no results.</li>
		                        </ul>
		                    </EmptyDataTemplate>
                        </asp:ListView>
				    </div>
                </asp:Panel>

                <!-- Format of this section stays the same but it
                     goes off of the cookie or area selected -->
                <asp:Panel runat="server" ID="BlogPanel">
                    <div class="list-results">
                        <div class="title">
						    <h2>Blog Post Results</h2>
					    </div>
                        <asp:ListView runat="server" ID="BlogList" OnItemDataBound="BlogList_ItemDataBound">
		                    <LayoutTemplate>
		                            <ul>
		                                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
		                            </ul>
		                    </LayoutTemplate>
		                    <ItemTemplate>
				                <li runat="server" id="ListItem"><asp:Hyperlink runat="server" ID="PostLink"><%# Eval("Title") %></asp:Hyperlink></li>
		                    </ItemTemplate>
                            <EmptyDataTemplate>
		                        <ul>
		                            <li class="">Your search returned no results.</li>
		                        </ul>
		                    </EmptyDataTemplate>
		                </asp:ListView>
                    </div>
                </asp:Panel>

                <!-- This data stays the same regardless of area or all -->
                <asp:Panel runat="server" ID="OtherPanel">
				    <div class="list-results">
                        <div class="title">
						    <h2>Other Results</h2>
					    </div>
                        <asp:ListView runat="server" ID="ContentList" OnItemDataBound="ContentList_ItemDataBound">
		                    <LayoutTemplate>
		                            <ul>
		                                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
		                            </ul>
		                    </LayoutTemplate>
		                    <ItemTemplate>
				                <li runat="server" id="ListItem"><asp:Hyperlink runat="server" ID="ContentLink"><%# Eval("PageName") %></asp:Hyperlink></li>
		                    </ItemTemplate>
                            <EmptyDataTemplate>
		                        <ul>
		                            <li class="">Your search returned no results.</li>
		                        </ul>
		                    </EmptyDataTemplate>
		                </asp:ListView>
                    </div>
                </asp:Panel>

			</div>
		</div>
</asp:Content>
