<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SP_AutohostedVideoPlayer_csWeb.Pages.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">

        // This function handles the click event of the tiles that are added dynamically
        // to the VideoList panel by code in default.aspx.cs
        function playVid(videoPath) {

            // We effectively build an HTML5 <video> tag and set the src attribute of the <source>
            // element to the URL that is passed into this handler. See default.aspx.cs for how the 
            // videoPath parameter is retireved and passed into this handler.
            var player = document.getElementById("VideoPlayer");
            var html5PlayerStart = "<video width='684' height='384' controls='controls' autoplay='autoplay'>";
            var html5PlayerSource = "<source src='" + videoPath + "' type='video/MP4' />";
            var html5PlayerEnd = "</video>";
            var playerHtml = html5PlayerStart + html5PlayerSource + html5PlayerEnd;
            player.innerHTML = playerHtml;
        }
    </script>
    <link type="text/css" href="../CSS/point8020metro.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:Label ID="SiteTitle" CssClass="heading" runat="server" Text="Label"></asp:Label>
        <br />
        <asp:Panel ID="VideoList" CssClass="scroller fl" runat="server"></asp:Panel>
        <asp:Panel ID="VideoPlayer" CssClass="fl" runat="server" style="margin-top:10px;background-color:#AFAFAF" Width="684" Height="384"></asp:Panel>
    </form>
</body>
</html>
