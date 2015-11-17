<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BWCMonitoring.aspx.cs" Inherits="ER_BestPickReports_Dev.BWCMonitoring" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>BlueWave Computing Basic Web Monitoring</h1>

        <fieldset>
            <h3>Monitoring Details</h3>
            <asp:Literal ID="litDetails" runat="server"></asp:Literal>
        </fieldset>

        <fieldset>
            <h3>Testing Event Log/N-Able</h3>
            Click here to test <asp:Button ID="btnTestEventLog" runat="server" Text="Test Log" OnClick="TestEventLog" />
            <br />
            Total Tests: <asp:Label ID="testCount" runat="server" Text="0"></asp:Label>
        </fieldset>
    </div>
    </form>
</body>
</html>
