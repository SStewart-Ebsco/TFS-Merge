<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="blogredirect.aspx.cs" Inherits="ER_BestPickReports_Dev.blogredirect" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="cat-page">
	<div class="container">
		<article class="search-results">
			<div class="head">
				<h2>Blog Redirect</h2>                    
				<h3>Thank you for choosing Best Pick Reports.<br /><br />To get you to the appropriate <strong><asp:Literal runat="server" ID="CatName"></asp:Literal></strong> page, please click on your area below:</h3>
			</div>
		</article>

        <asp:Panel runat="server" ID="AtlantaList" Visible="false">
            <div class="title">
				<h2>Atlanta</h2>
			</div>
            <asp:ListView runat="server" ID="AtlantaAreaList">
            	<LayoutTemplate>
                    <div class="list-results">
                        <ul>
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                        </ul>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <li><a href='<%=basedomain %>/<%=atlcat%>/<%# Eval("CityUrlName") %>/<%# Eval("AreaUrlName") %>'><%# Eval("AreaName") %></a></li>
                </ItemTemplate>
            </asp:ListView>
        </asp:Panel>

        <asp:Panel runat="server" ID="ChicagoList" Visible="false">
            <div class="title">
                <h2>Chicago</h2>
            </div>
            <asp:ListView runat="server" ID="ChicagoAreaList">
            	<LayoutTemplate>
                    <div class="list-results">
                        <ul>
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                        </ul>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <li><a href='<%=basedomain %>/<%=chicat%>/<%# Eval("CityUrlName") %>/<%# Eval("AreaUrlName") %>'><%# Eval("AreaName") %></a></li>
                </ItemTemplate>
            </asp:ListView>
        </asp:Panel>

        <asp:Panel runat="server" ID="DallasList" Visible="false">
            <div class="title">
                <h2>Dallas</h2>
            </div>
            <asp:ListView runat="server" ID="DallasAreaList" OnItemDataBound="DallasAreaList_ItemDataBound">
            	<LayoutTemplate>
                    <div class="list-results">
                        <ul>
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                        </ul>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <li><asp:HyperLink runat="server" ID="DallasAreaLink"></asp:HyperLink></li>
                </ItemTemplate>
            </asp:ListView>
        </asp:Panel>

        <asp:Panel runat="server" ID="NovaList" Visible="false">
            <div class="title">
                <h2>Northern Virginia</h2>
            </div>
            <asp:ListView runat="server" ID="NoVaAreaList">
            	<LayoutTemplate>
                    <div class="list-results">
                        <ul>
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                        </ul>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <li><a href='<%=basedomain %>/<%=novacat%>/<%# Eval("CityUrlName") %>/<%# Eval("AreaUrlName") %>'><%# Eval("AreaName") %></a></li>
                </ItemTemplate>
            </asp:ListView>
        </asp:Panel>

        <asp:Panel runat="server" ID="HoustonList" Visible="false">
            <div class="title">
                <h2>Houston</h2>
            </div>
            <asp:ListView runat="server" ID="HoustonAreaList">
            	<LayoutTemplate>
                    <div class="list-results">
                        <ul>
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                        </ul>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <li><a href='<%=basedomain %>/<%=houcat%>/<%# Eval("CityUrlName") %>/<%# Eval("AreaUrlName") %>'><%# Eval("AreaName") %></a></li>
                </ItemTemplate>
            </asp:ListView>
        </asp:Panel>

        <asp:Panel runat="server" ID="MarylandList" Visible="false">
            <div class="title">
                <h2>Maryland</h2>
            </div>
            <asp:ListView runat="server" ID="MarylandAreaList">
            	<LayoutTemplate>
                    <div class="list-results">
                        <ul>
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                        </ul>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <li><a href='<%=basedomain %>/<%=marycat%>/<%# Eval("CityUrlName") %>/<%# Eval("AreaUrlName") %>'><%# Eval("AreaName") %></a></li>
                </ItemTemplate>
            </asp:ListView>
        </asp:Panel>

        <asp:Panel runat="server" ID="DCList" Visible="false">
            <div class="title">
                <h2>Washington, D.C.</h2>
            </div>
            <asp:ListView runat="server" ID="DCAreaList">
            	<LayoutTemplate>
                    <div class="list-results">
                        <ul>
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                        </ul>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <li><a href='<%=basedomain %>/<%=dccat%>/<%# Eval("CityUrlName") %>/<%# Eval("AreaUrlName") %>'><%# Eval("AreaName") %></a></li>
                </ItemTemplate>
            </asp:ListView>
        </asp:Panel>

        <asp:Panel runat="server" ID="BhamList" Visible="false">
            <div class="title">
                <h2>Birmingham</h2>
            </div>
            <asp:ListView runat="server" ID="BhamAreaList">
            	<LayoutTemplate>
                    <div class="list-results">
                        <ul>
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                        </ul>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <li><a href='<%=basedomain %>/<%=bhamcat%>/<%# Eval("CityUrlName") %>/<%# Eval("AreaUrlName") %>'><%# Eval("AreaName") %></a></li>
                </ItemTemplate>
            </asp:ListView>
        </asp:Panel>

    </div>
</div>
</asp:Content>
