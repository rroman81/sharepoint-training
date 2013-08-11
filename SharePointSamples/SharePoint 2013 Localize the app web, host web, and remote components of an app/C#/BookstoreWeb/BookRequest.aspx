<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookRequest.aspx.cs" Inherits="BookstoreWeb.BookRequest" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<!-- the body is hidden until the stylesheet is loaded -->
<body style="display: none">
    <form id="form1" runat="server">
    <div>
        <asp:Literal 
            ID="litBookTitle" 
            Text="Book title lit" 
            runat="server" 
            meta:resourcekey="litBookTitle"></asp:Literal>&nbsp;
        <input
            id="txtTitle" 
            type="text"/>
        <br />
        <a
            id="lnkRequest" 
            href="#">
            <asp:Literal 
            ID="litRequestBook" 
            Text="Request book lit" 
            runat="server" 
            meta:resourcekey="litRequestBook"></asp:Literal>
        </a>
        <div id="divMsg"></div>
        <input 
            id="content" 
            type="hidden"
            value="Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."/>
    </div>
    <script 
        src="https://ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js" 
        type="text/javascript">
    </script>
    <script 
        type="text/javascript" 
        src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js">
    </script>

    <script 
        type="text/javascript"
        src="Scripts/Common.js">
    </script>
    <!-- Script to load SharePoint resources
        and load the blank.html page in
        the invisible iframe
        -->
    <script 
        type="text/javascript"
        src="Scripts/StyleLoader.js"
        >
    </script>
    <script 
        type="text/javascript"
        src="Scripts/BookRequest.js">
    </script>
    </form>
</body>
</html>
