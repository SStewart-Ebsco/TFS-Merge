<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ad.aspx.cs" Inherits="ER_BestPickReports_Dev.mobile.ad" %>
<!DOCTYPE html>
<html lang="en">
	<head>
		<meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1">
		<title>Download The Best Pick App</title>
        
        <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/global")%>
        <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/ad")%>
        <%= System.Web.Optimization.Styles.Render("~/bundles/site-mobile/sprite")%>

	</head>
	<body class="ad">
		<form id="form" runat="server">
				<div class="container">
					<span class="ad-text">For an enhanced mobile experience on the go, use our</span>
	                <span class="ad-title">Best Pick™ App</span>
                    <div class="ad-image sprite"></div>
				</div>
                <div class="ad-footer">
                	<asp:Button CssClass="button-big" id="DownloadButton" runat="server" OnClick="DownloadButton_Click" Text="Download The Best Pick App" />
                	<asp:Button CssClass="button-big" id="ContinueButton" runat="server" OnClick="ContinueButton_Click" Text="Continue to Mobile Site" />
                </div>
		</form>
        
        <script>
            (function (d) {
                var config = {
                    kitId: 'fgn0ait',
                    scriptTimeout: 3000
                },
            h = d.documentElement, t = setTimeout(function () { h.className = h.className.replace(/\bwf-loading\b/g, "") + " wf-inactive"; }, config.scriptTimeout), tk = d.createElement("script"), f = false, s = d.getElementsByTagName("script")[0], a; h.className += " wf-loading"; tk.src = '//use.typekit.net/' + config.kitId + '.js'; tk.async = true; tk.onload = tk.onreadystatechange = function () { a = this.readyState; if (f || a && a != "complete" && a != "loaded") return; f = true; clearTimeout(t); try { Typekit.load(config) } catch (e) { } }; s.parentNode.insertBefore(tk, s)
            })(document);
        </script> 
	</body>
</html>