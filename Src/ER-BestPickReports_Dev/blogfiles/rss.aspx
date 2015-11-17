<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rss.aspx.cs" Inherits="ER_BestPickReports_Dev.blogfiles.rss" %>
<rss version="2.0" xmlns:atom="http://www.w3.org/2005/Atom">
                <channel>
                <atom:link xmlns:atom="http://www.w3.org/2005/Atom" rel="hub" href="<%=basedomain %>/blog/rss/<%=strcity %>/"/>
                <atom:link href="<%=basedomain %>/blog/rss/<%=strcity %>" rel="self" type="application/rss+xml" />
                    <title><asp:Literal runat="server" ID="RSSTitle"></asp:Literal></title>
                    <link><asp:Literal runat="server" ID="RSSLink"></asp:Literal></link>
                    <description><asp:Literal runat="server" ID="RSSDesc"></asp:Literal></description>
    
    <asp:Repeater runat="server" ID="rptRSS" OnItemDataBound="rptRSS_ItemDataBound">
        
        <ItemTemplate>
            <item>
                <title><%# FormatForXML(Eval("Title")) %></title>
                <description><asp:Literal runat="server" ID="ItemTeaser"></asp:Literal></description>
                <link><asp:Literal runat="server" ID="ItemLink"></asp:Literal></link>
                <guid><asp:Literal runat="server" ID="GLink"></asp:Literal></guid>
                <pubDate><%# Convert.ToDateTime(Eval("PublishDate")).ToString("ddd, dd MMM yyyy hh:mm:ss")%> EST</pubDate>
            </item>
        </ItemTemplate>
        
    </asp:Repeater>
    
    </channel>
</rss>
