<%@ Page Title="" Language="C#" MasterPageFile="~/blogfiles/Blog.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ER_BestPickReports_Dev.blogfiles.login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pageheading">
Login
</div>
<table>
        <tr>
            <td align="right">
                <asp:Label ID="lblUserName" runat="server" Text="Username: "></asp:Label>&nbsp;
            </td>
            <td>
                <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblPassword" runat="server" Text="Password: "></asp:Label>&nbsp; 
            </td>
            <td>
               <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2"><asp:Button ID="bnLogin" runat="server" Text="Log In" onclick="bnLogin_Click" /></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2"> <asp:Label ID="lblLoginError" runat="server" Text="Error" Visible="False" 
                    Font-Bold="True" ForeColor="Red" ></asp:Label></td>
        </tr>
    </table>
</asp:Content>
