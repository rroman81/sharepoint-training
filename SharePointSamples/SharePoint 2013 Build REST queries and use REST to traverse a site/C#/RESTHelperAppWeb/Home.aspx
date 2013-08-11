<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="RESTHelperAppWeb.Home" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

                    <h1>REST Helper</h1>
        
       <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
      <asp:UpdatePanel ID="RESTHelperPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>   
            <asp:Button runat="server" Text="Reset" ID="Reset" OnClick="Reset_Click" />
            <table><tr><td align="left"><b>What do you want to work with?</b>
                <asp:DropDownList ID="ResourceList" runat="server"><asp:ListItem>Lists</asp:ListItem><asp:ListItem>Folders</asp:ListItem><asp:ListItem>Files</asp:ListItem><asp:ListItem>Users</asp:ListItem></asp:DropDownList>
                <asp:Button ID="SelectResourceButton" runat="server" Text="Choose Resource" OnClick="SelectResourceButton_Click" />
                       </td>
            <td align="left"><b>What do you want to do?</b>
                <asp:DropDownList ID="ActionList" runat="server"><asp:ListItem>Create</asp:ListItem><asp:ListItem>Read</asp:ListItem><asp:ListItem>Update</asp:ListItem><asp:ListItem>Delete</asp:ListItem></asp:DropDownList>
                <asp:Button ID="SelectActionButton" runat="server" Text="Choose Action" OnClick="SelectActionButton_Click" />
            </td>   
            </tr>
            <tr>
            <td><h2>Current Endpoint</h2><asp:Label ID="CurrentEndpointLabel" runat="server" /></td>
            <td><h2>Entity Type:</h2><asp:Label ID="EntityTypeLabel" runat="server"/></td>
            </tr>
            </table>
            <h2>Available Endpoints</h2>
            <asp:ListBox ID="EndpointsListBox" runat="server"/><asp:Button runat="server" ID="SelectEndpoint" Text="Select Endpoint" OnClick="SelectEndpoint_Click" />

            <p><asp:Label ID="JSCodeBlock" runat="server" Visible="false"/></p>
            
            <asp:Label ID="EntityJSONLabel" runat="server" Visible="false"/>
            <h2>Select Entity Properties:</h2><asp:CheckBoxList ID="EntitySelectPropertiesList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="EntitySelectPropertiesList_SelectedIndexChanged"/>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
