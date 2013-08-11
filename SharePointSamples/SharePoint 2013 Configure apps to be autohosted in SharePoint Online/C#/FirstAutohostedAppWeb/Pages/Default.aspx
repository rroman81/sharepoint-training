<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FirstAutohostedAppWeb.Pages.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body bgcolor="#000000">
    <form id="form1" runat="server">
    <div>
    <h2 style="font-family: Segoe UI; font-size: xx-large; font-weight: 100; font-style:normal; color: White">composed looks on the SharePoint host website</h2>
    </div>
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" 
        Text="GET THE COMPOSED LOOKS" BackColor="#00FFFF" ForeColor="Black" Font-Size="Large" 
        Style="font-family: 'Segoe UI'; border-style: none; text-wrap: normal; font-weight: normal" 
        Height="210px" Width="239px" />
    <asp:Literal ID="Literal1" runat="server"><br /><br /></asp:Literal>
    <asp:GridView ID="GridView1" runat="server" BackColor="#808080" ForeColor="White"
        BorderColor="#0033CC" BorderStyle="None" Caption="THE COMPOSED LOOKS" 
        CaptionAlign="Left" CellPadding="5" Style="font-family: 'Segoe UI'" GridLines="None" 
        HorizontalAlign="Left">
        <AlternatingRowStyle BackColor="White" ForeColor="Black" />
    </asp:GridView>
    </form>
</body>

</html>
