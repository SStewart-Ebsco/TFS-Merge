<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ER_BestPickReports_Dev.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
    <script type="text/javascript">
        function initialize() {
			
            var latitude = <asp:literal runat="server" id="myLat" />;
            var longitude = <asp:literal runat="server" id="myLong" />;
        	var latlng = new google.maps.LatLng(latitude,longitude);			

            var myOptions = {
                zoom: 10,
                center: latlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
			
			var marker = new google.maps.Marker({
      		position: latlng, 
      		map: map, 
      		title:"Your Current Location!"
  			}); 
        }

    </script>
</head>
<body onload="initialize()">
    <h2>Your IP Address Information</h2>		
    <form id="form1" runat="server">
	<div>		
        <table>
            <tr>					
                <td style="width: 100px"><asp:Label ID="Label8" runat="server" Text="IP Address: " CssClass = "lbl" Width="120px" /></td>
                <td style="width: 188px"><asp:Label ID="lblIP" runat="server" CssClass = "text" Width="186px" /></td>
            </tr>				
            <tr>					
                <td><asp:Label ID="Label7" runat="server" Text="Latitude: " CssClass = "lbl" /></td>
                <td><asp:Label ID="lblLat" runat="server" CssClass = "text" /></td>
            </tr>				
            <tr>					
                <td><asp:Label ID="Label6" runat="server" Text="Longitude: " CssClass = "lbl" /></td>
                <td><asp:Label ID="lblLong" runat="server" CssClass = "text" /></td>
            </tr>				
            <tr>					
                <td><asp:Label ID="Label5" runat="server" Text="Zip/Postal Code: " CssClass = "lbl" /></td>
                <td><asp:Label ID="lblZip" runat="server" CssClass = "text" /></td>
            </tr>				
            <tr>					
                <td><asp:Label ID="Label1" runat="server" Text="City: " CssClass = "lbl" /></td>
                <td><asp:Label ID="lblCity" runat="server" CssClass = "text" /></td>
            </tr>
             <tr>
                <td><asp:Label ID="Label2" runat="server" Text="State/Province: " CssClass = "lbl" /></td>
                <td><asp:Label ID="lblRegion" runat="server" CssClass = "text" /></td>
            </tr>
             <tr>
                <td><asp:Label ID="Label3" runat="server" Text="Country: " CssClass = "lbl" /></td>
                <td><asp:Label ID="lblCountry" runat="server" CssClass = "text" /></td>
            </tr>
             <tr>
                <td><asp:Label ID="Label4" runat="server" Text="Country Code: " CssClass = "lbl" /></td>
                <td><asp:Label ID="lblCountryCode" runat="server" CssClass = "text" /></td>
            </tr>
        </table>
    </div>
    <div>
        <div id="map_canvas" style="width: 50%; height: 50%"></div>
    </div>			
    </form>
<br/>		
</body>
</html>
