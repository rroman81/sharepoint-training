﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="BasicSelfHostedAppRESTWeb.Home" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

      <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
      <asp:UpdatePanel ID="PopulateData" runat="server" UpdateMode="Conditional">
        <ContentTemplate>   
   
        <table border="1" cellpadding="10">
         <tr>
             <th><asp:LinkButton ID="REST" runat="server" Text="Populate Data" OnClick="REST_Click" /></th></tr>

         <tr>
              
        <td>
        <h2>SharePoint Site</h2>
        <asp:Label runat="server" ID="WebTitleLabelREST"/>
        <h2>Current User:</h2>
        <asp:Label runat="server" ID="CurrentUserLabelREST" />
        <h2>Site Users</h2>
        <asp:ListView ID="UserListREST" runat="server">     
            <ItemTemplate ><asp:Label ID="UserItemREST" runat="server" Text="<%# Container.DataItem.ToString()  %>"></asp:Label><br /></ItemTemplate>
        </asp:ListView>
        <h2>Site Lists</h2>
        <asp:ListView ID="ListListREST" runat="server">
            <ItemTemplate ><asp:Label ID="ListItemREST" runat="server" Text="<%# Container.DataItem.ToString()  %>"></asp:Label><br /></ItemTemplate>
        </asp:ListView>
        </td>
        </tr>
        </table>
        </ContentTemplate>
      </asp:UpdatePanel>
    
    </div>
    </form>
</body>
</html>
