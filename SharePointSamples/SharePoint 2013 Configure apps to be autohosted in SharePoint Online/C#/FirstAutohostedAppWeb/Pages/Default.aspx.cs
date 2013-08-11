using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.SharePoint.Client;
using Microsoft.IdentityModel.S2S.Tokens;
using System.Net;
using System.IO;
using System.Xml;
using System.Data;
using System.Xml.Linq;
using System.Xml.XPath;

using FirstAutohostedAppWeb;


namespace FirstAutohostedAppWeb.Pages
{
    public partial class Default : System.Web.UI.Page
    {
        SharePointContextToken contextToken;
        string accessToken;
        Uri sharepointUrl;


        protected void Page_Load(object sender, EventArgs e)
        {
            TokenHelper.TrustAllCertificates();
            string contextTokenString = TokenHelper.GetContextTokenFromRequest(Request);

            if (contextTokenString != null)
            {
                // Get context token
                contextToken = TokenHelper.ReadAndValidateContextToken(contextTokenString, Request.Url.Authority);

                // Get the host web's URL and the access token.
                sharepointUrl = new Uri(Request.QueryString["SPHostUrl"]);
                accessToken = TokenHelper.GetAccessToken(contextToken, sharepointUrl.Authority).AccessToken;

                // Pass the access token to the button event handler.
                Button1.CommandArgument = accessToken;
            }
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            string accessToken = ((Button)sender).CommandArgument;

            if (IsPostBack)
            {
                // Get the host web's URL.
                sharepointUrl = new Uri(Request.QueryString["SPHostUrl"]);
            }

            // REST/OData section

            // Use the $select query to bring only the fields that will actually be used over the network.
            string oDataUrl = "/_api/Web/lists/getbytitle('Composed Looks')/items?$select=Title,AuthorId,Name";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sharepointUrl.ToString() + oDataUrl);
            request.Method = "GET";
            request.Accept = "application/atom+xml";
            request.ContentType = "application/atom+xml;type=entry";
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Response markup parsing section
            XDocument oDataXML = XDocument.Load(response.GetResponseStream(), LoadOptions.None);
            XNamespace atom = "http://www.w3.org/2005/Atom";
            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

            // The ATOM markup for a SharePoint list nests field elements under <entry> <content> <properties>.
            List<XElement> entries = oDataXML.Descendants(atom + "entry")
                                     .Elements(atom + "content")
                                     .Elements(m + "properties")
                                     .ToList();

            var entryFieldValues = from entry in entries
                                   select new
                                   {
                                       Title = entry.Element(d + "Title").Value,
                                       AuthorId = entry.Element(d + "AuthorId").Value,
                                       Name = entry.Element(d + "Name").Value
                                   };

            // Bind data to the grid on the page.
            GridView1.DataSource = entryFieldValues;
            GridView1.DataBind();
        }

    }
}