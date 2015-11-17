<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ER_BestPickReports_Dev.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form runat="server" id="form1">

<h2>Login</h2>

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
            <td align="right">
                <asp:Label ID="lblReferer" runat="server" Text="Refer Test: "></asp:Label>&nbsp; 
            </td>
            <td>
               <asp:TextBox ID="txtRefer" runat="server" Columns="75"></asp:TextBox>
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

</form>
</body>
</html>
