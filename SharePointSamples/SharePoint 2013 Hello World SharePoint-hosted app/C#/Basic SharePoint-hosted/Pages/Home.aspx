<%@ Page language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:ScriptLink name="sp.js" runat="server" OnDemand="true" LoadAfterUI="true" Localizable="false" />
    <link rel="Stylesheet" type="text/css" href="../Styles/App.css" />
    <script type="text/javascript" src="../Scripts/App.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.6.2.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderMain" runat="server">
    
    <script type="text/javascript">

        SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function() {sharePointReady();});
              </script>
              <div>
                 <button id="getListCount">Get count of lists in web</button>
              </div>

<div id="starter">
</div>


    <SharePoint:SPAppIFrame   ID="SPAppIFrame1" runat="server" Src="http://www.bing.com" Width="100%" Height="100%"></SharePoint:SPAppIFrame>

        <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="full" Title="loc:full" >
<WebPartPages:XsltListViewWebPart ID="XsltListViewWebPart2" runat="server" ListUrl="Lists/TestCustomList" IsIncluded="True" NoDefaultStyle="TRUE" Title="TestCustomList" PageType="PAGE_NORMALVIEW" Default="False" ViewContentTypeId="0x">
</WebPartPages:XsltListViewWebPart>

        </WebPartPages:WebPartZone>
    </asp:Content>
