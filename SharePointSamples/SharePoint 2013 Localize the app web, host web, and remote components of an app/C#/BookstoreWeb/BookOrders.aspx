<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookOrders.aspx.cs" Inherits="BookstoreWeb.BookOrders" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="msg"></div>
    <table>
        <thead></thead>
        <tbody></tbody>
    </table>
    <asp:ScriptManager 
        ID="ScriptManager1" 
        EnableScriptLocalization="true" 
        runat="server">
        <Scripts>
            <asp:ScriptReference Path="https://ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js" />
            <asp:ScriptReference Path="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js" />
            <asp:ScriptReference Path="Scripts/Common.js" />
            <asp:ScriptReference Path="Scripts/Strings.js" ResourceUICultures="es-ES" />
            <asp:ScriptReference Path="Scripts/BookOrders.js" />
            <asp:ScriptReference Path="Scripts/StyleLoader.js"/>
        </Scripts>
    </asp:ScriptManager>
    </form>
</body>
</html>
