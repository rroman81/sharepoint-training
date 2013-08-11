<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="O365_SharePointAutoHostedWeb.Pages.Default" %>

<!DOCTYPE>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1
        {
            width: 25%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        
        <section>
            <header>
                <h2>Office 365 Sample Code : Sharepoint Auto-Hosted App</h2>
            </header>
            <div role="main">
               
                <section>
                    <asp:Label ID="lblError" Text="" ForeColor="Red" runat="server"></asp:Label>
                    <table style="width:100%;">
                        <tr>
                            <td style="width:24%;">
                                Enter Employee First Name: 
                            </td>
                            <td class="auto-style1">
                                <asp:TextBox runat="server" id="txtEmployeeName" /> 
                            </td>
                            <td style="width:50%;">
                                <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:GridView runat="server" ID="gridViewEmployeeDetail" ForeColor="#333333" Caption="" 
                                    CaptionAlign="Left" CellPadding="4" Style="font-family: 'Segoe UI'" GridLines="None" 
                                    HorizontalAlign="Left" HeaderStyle-BackColor="Blue" Width="1000px">
                                    <AlternatingRowStyle BackColor="White" />
                                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle HorizontalAlign="Left" BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                    <SortedDescendingHeaderStyle BackColor="#820000" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </section>
            </div>
            
            <footer></footer>
        </section>
    </form>
</body>
</html>
