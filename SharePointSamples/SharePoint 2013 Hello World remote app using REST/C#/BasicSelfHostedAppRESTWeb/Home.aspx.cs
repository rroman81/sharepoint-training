using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Samples;
using Microsoft.IdentityModel.S2S.Tokens;
using System.Net;
using System.IO;
using System.Xml;

namespace BasicSelfHostedAppRESTWeb
{
    public partial class Home : System.Web.UI.Page
    {
        SharePointContextToken contextToken;
        string accessToken;
        Uri sharepointUrl;
        string siteNameREST;
        string currentUserREST;
        List<string> listOfUsersREST = new List<string>();
        List<string> listOfListsREST = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            TokenHelper.TrustAllCertificates();
            string contextTokenString = TokenHelper.GetContextTokenFromRequest(Request);

            if (contextTokenString != null)
            {
                contextToken =
                    TokenHelper.ReadAndValidateContextToken(contextTokenString, Request.Url.Authority);

                sharepointUrl = new Uri(Request.QueryString["SPHostUrl"]);
                accessToken =
                    TokenHelper.GetAccessToken(contextToken, sharepointUrl.Authority).AccessToken;
                REST.CommandArgument = accessToken;

            }
            else if (!IsPostBack)
            {
                Response.Write("Could not find a context token.");
                return;
            }
        }

        //This method retrieves some basic information about the host Web.
        private void RetrieveWithREST(string accessToken)
        {

            if (IsPostBack)
            {
                sharepointUrl = new Uri(Request.QueryString["SPHostUrl"]);
            }

            //Create a namespace manager for parsing the ATOM XML returned by the queries.
            XmlNamespaceManager xmlnspm = new XmlNamespaceManager(new NameTable());
            //Add pertinent namespaces to the namespace manager.
            xmlnspm.AddNamespace("atom", "http://www.w3.org/2005/Atom");
            xmlnspm.AddNamespace("d", "http://schemas.microsoft.com/ado/2007/08/dataservices");
            xmlnspm.AddNamespace("m", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");

            //Execute a REST request for the site name.

            HttpWebRequest request =
                (HttpWebRequest)HttpWebRequest.Create(sharepointUrl.ToString() + "/_api/Web/title");
            request.Method = "GET";
            request.Accept = "application/atom+xml";
            request.ContentType = "application/atom+xml;type=entry";
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            var titleXml = new XmlDocument();
            titleXml.LoadXml(reader.ReadToEnd());
            var webTitle = titleXml.SelectSingleNode("d:Title", xmlnspm);
            siteNameREST = webTitle.InnerXml;

            //Execute a REST request for the current user.

            HttpWebRequest currentUserRequest =
    (HttpWebRequest)HttpWebRequest.Create(sharepointUrl.ToString() + "/_api/Web/currentUser");
            currentUserRequest.Method = "GET";
            currentUserRequest.Accept = "application/atom+xml";
            currentUserRequest.ContentType = "application/atom+xml;type=entry";
            currentUserRequest.Headers.Add("Authorization", "Bearer " + accessToken);
            HttpWebResponse currentUserResponse = (HttpWebResponse)currentUserRequest.GetResponse();
            StreamReader currentUserReader = new StreamReader(currentUserResponse.GetResponseStream());
            var currentUserXml = new XmlDocument();
            currentUserXml.LoadXml(currentUserReader.ReadToEnd());
            var currentUserTitle = currentUserXml.SelectSingleNode("//atom:entry/atom:content/m:properties/d:LoginName", xmlnspm);
            currentUserREST = currentUserTitle.InnerXml;

            //Execute a REST request for all of the site's lists.

            HttpWebRequest listRequest =
                (HttpWebRequest)HttpWebRequest.Create(sharepointUrl.ToString() + "/_api/Web/lists");
            listRequest.Method = "GET";
            listRequest.Accept = "application/atom+xml";
            listRequest.ContentType = "application/atom+xml;type=entry";
            listRequest.Headers.Add("Authorization", "Bearer " + accessToken);
            HttpWebResponse listResponse = (HttpWebResponse)listRequest.GetResponse();
            StreamReader listReader = new StreamReader(listResponse.GetResponseStream());
            var listXml = new XmlDocument();
            listXml.LoadXml(listReader.ReadToEnd());

            var titleList = listXml.SelectNodes("//atom:entry/atom:content/m:properties/d:Title", xmlnspm);

            foreach (XmlNode title in titleList)
            {
                listOfListsREST.Add(title.InnerXml);
            }

            //Execute a REST request for all of the site's users.

            HttpWebRequest userRequest =
(HttpWebRequest)HttpWebRequest.Create(sharepointUrl.ToString() + "/_api/Web/siteusers");
            userRequest.Method = "GET";
            userRequest.Accept = "application/atom+xml";
            userRequest.ContentType = "application/atom+xml;type=entry";
            userRequest.Headers.Add("Authorization", "Bearer " + accessToken);
            HttpWebResponse userResponse = (HttpWebResponse)userRequest.GetResponse();
            StreamReader userReader = new StreamReader(userResponse.GetResponseStream());
            var userXml = new XmlDocument();
            userXml.LoadXml(userReader.ReadToEnd());

            var userList = userXml.SelectNodes("//atom:entry/atom:content/m:properties/d:LoginName", xmlnspm);

            foreach (XmlNode user in userList)
            {
                listOfUsersREST.Add(user.InnerXml);
            }


        }


        protected void REST_Click(object sender, EventArgs e)
        {
            string commandAccessToken = ((LinkButton)sender).CommandArgument;
            RetrieveWithREST(commandAccessToken);
            WebTitleLabelREST.Text = siteNameREST;
            CurrentUserLabelREST.Text = currentUserREST;
            UserListREST.DataSource = listOfUsersREST;
            UserListREST.DataBind();
            ListListREST.DataSource = listOfListsREST;
            ListListREST.DataBind();

        }

    }
}