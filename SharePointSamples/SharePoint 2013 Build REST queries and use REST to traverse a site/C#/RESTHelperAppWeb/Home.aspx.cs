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

namespace RESTHelperAppWeb
{
    public partial class Home : System.Web.UI.Page
    {
        SharePointContextToken contextToken;
        string accessToken;
        Uri sharepointUrl;
        //Create a namespace manager for parsing the ATOM XML returned by the queries.
        XmlNamespaceManager xmlnspm = new XmlNamespaceManager(new NameTable());

        // The Page_load method fetches the context token and the access token. The access token is used by all of the data retrieval methods.
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
                SelectEndpoint.CommandArgument = accessToken;
                SelectResourceButton.CommandArgument = accessToken;
                Reset.CommandArgument = accessToken;
                SelectActionButton.CommandArgument = accessToken;

                RetrieveEndpoints(accessToken, "Web");
            }
            else if (!IsPostBack)
            {
                Response.Write("Could not find a context token.");
            }
        }

        private void RetrieveEndpoints(string accessToken, string selectedEndpoint)
        {
            if (IsPostBack)
            {
                sharepointUrl = new Uri(Request.QueryString["SPHostUrl"]);
            }

            EntitySelectPropertiesList.Items.Clear();
            CurrentEndpointLabel.Text = selectedEndpoint;

            //Add pertinent namespaces to the namespace manager.
            xmlnspm.AddNamespace("atom", "http://www.w3.org/2005/Atom");
            xmlnspm.AddNamespace("d", "http://schemas.microsoft.com/ado/2007/08/dataservices");
            xmlnspm.AddNamespace("m", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
            xmlnspm.AddNamespace("edmx", "http://schemas.microsoft.com/ado/2007/06/edmx");
            xmlnspm.AddNamespace("SP", "http://schemas.microsoft.com/ado/2009/11/edm");

            //Execute a REST request for all of the available endpoints, entities, and entity properties.

            HttpWebRequest endpointsRequest =
                (HttpWebRequest)HttpWebRequest.Create(sharepointUrl.ToString() + "/_api/" + selectedEndpoint);
            endpointsRequest.Method = "GET";
            endpointsRequest.Accept = "application/atom+xml";
            endpointsRequest.ContentType = "application/atom+xml;type=entry";
            endpointsRequest.Headers.Add("Authorization", "Bearer " + accessToken);
            HttpWebResponse endpointsResponse = (HttpWebResponse)endpointsRequest.GetResponse();
            StreamReader endpointsReader = new StreamReader(endpointsResponse.GetResponseStream());
            var endpointsXml = new XmlDocument();
            endpointsXml.LoadXml(endpointsReader.ReadToEnd());

            //Retrieve all of the endpoints.
            var endpointsList = endpointsXml.SelectNodes("//atom:entry/atom:link", xmlnspm);
            //Retrieve the first entity so that we can get the entity type for this endpoint.
            var entityType = endpointsXml.SelectSingleNode("//atom:entry/atom:category", xmlnspm);

            //Retrieve the properties node of the first entity so that we can construct a sample JSON representation.
            var propertiesNode = endpointsXml.SelectSingleNode("//atom:entry/atom:content/m:properties", xmlnspm);


            if (endpointsList.Count > 0)
            {
                string properties = "";
                int nodeCounter = 1;
                foreach (XmlNode node in propertiesNode.ChildNodes)
                {

                    var selectPropertiesListItem = new System.Web.UI.WebControls.ListItem(node.Name.Replace("d:", ""));
                    selectPropertiesListItem.Selected = true;
                    EntitySelectPropertiesList.Items.Add(selectPropertiesListItem);
                    properties += "'" + node.Name.Replace("d:", "") + "': '[value]'";
                    if (nodeCounter != propertiesNode.ChildNodes.Count)
                    {
                        properties += ", ";
                    }
                    nodeCounter++;
                }
                EntityTypeLabel.Text = entityType.Attributes.GetNamedItem("term").InnerXml;
                EntityJSONLabel.Text = "{'__metadata':{'type':'" + entityType.Attributes.GetNamedItem("term").InnerXml + "'}," + properties + "}";
            }
            else
            {
                EntityTypeLabel.Text = "No entities at this endpoint.";
                EntityJSONLabel.Text = "";
            }

            foreach (XmlNode node in endpointsList)
            {
                System.Web.UI.WebControls.ListItem listItem = new System.Web.UI.WebControls.ListItem();
                listItem.Text = node.Attributes.GetNamedItem("href").InnerXml;

                EndpointsListBox.Items.Add(listItem);

            }


        }


        protected void SelectEndpoint_Click(object sender, EventArgs e)
        {
            string commandAccessToken = ((Button)sender).CommandArgument;
            string selectedEndpoint = EndpointsListBox.SelectedItem.Text;
            EndpointsListBox.Items.Clear();
            JSCodeBlock.Visible = false;
            RetrieveEndpoints(commandAccessToken, selectedEndpoint);
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            string commandAccessToken = ((Button)sender).CommandArgument;
            EndpointsListBox.Items.Clear();
            JSCodeBlock.Visible = false;
            RetrieveEndpoints(commandAccessToken, "Web");
        }

        protected void SelectResourceButton_Click(object sender, EventArgs e)
        {
            string commandAccessToken = ((Button)sender).CommandArgument;
            EndpointsListBox.Items.Clear();
            JSCodeBlock.Visible = false;
            if (ResourceList.SelectedItem.Text == "Lists")
            {
                RetrieveEndpoints(commandAccessToken, "Web/Lists");
            }
            else if (ResourceList.SelectedItem.Text == "Users")
            {
                RetrieveEndpoints(commandAccessToken, "Web/SiteUsers");
            }
            else if (ResourceList.SelectedItem.Text == "Folders")
            {
                RetrieveEndpoints(commandAccessToken, "Web/Folders");
            }
            else if (ResourceList.SelectedItem.Text == "Files")
            {
                RetrieveEndpoints(commandAccessToken, "web/GetFolderByServerRelativeUrl('/Shared Documents')/Files");
            }
        }

        protected void SelectActionButton_Click(object sender, EventArgs e)
        {
            string commandAccessToken = ((Button)sender).CommandArgument;
            JSCodeBlock.Visible = true;
            JSCodeBlock.Text = "<h2>Javascript Sample Code</h2><textarea cols='80' rows='10'>";

            if (ActionList.SelectedItem.Text == "Create" && ResourceList.SelectedItem.Text == "Folders")
            {
                JSCodeBlock.Text += "var executor = new SP.RequestExecutor(appweburl);\n";
                JSCodeBlock.Text += "executor.executeAsync(\n";
                JSCodeBlock.Text += "{\n";
                JSCodeBlock.Text += "url: appweburl + \"/_api/" + CurrentEndpointLabel.Text + "\",\n";
                JSCodeBlock.Text += "method: \"POST\",\n";
                JSCodeBlock.Text += "headers: { \"accept\": \"application/json;odata=verbose\", \"content-type\": \"application/json;odata=verbose\", \"X-RequestDigest\": formDigest},\n";
                JSCodeBlock.Text += "data: \" { \'__metadata\': { \'type\': \'SP.Folder\' }, \'ServerRelativeUrl\': \'/[document library relative url]/[folder name]\'} \",\n";
                JSCodeBlock.Text += "success: successHandler,\n";
                JSCodeBlock.Text += "error: errorHandler\n";
                JSCodeBlock.Text += "}\n";
                JSCodeBlock.Text += ");\n</textarea>";
            }

            else if (ActionList.SelectedItem.Text == "Create" && ResourceList.SelectedItem.Text == "Files")
            {
                JSCodeBlock.Text += "var executor = new SP.RequestExecutor(appweburl);\n";
                JSCodeBlock.Text += "executor.executeAsync(\n";
                JSCodeBlock.Text += "{\n";
                JSCodeBlock.Text += "url: appweburl + \"/_api/web/GetFolderByServerRelativeUrl(\'/[document library relative url]\')/Files/add(url=\'a.txt\',overwrite=true) \",\n";
                JSCodeBlock.Text += "method: \"POST\",\n";
                JSCodeBlock.Text += "headers: { \"accept\": \"application/json;odata=verbose\", \"content-type\": \"application/json;odata=verbose\", \"X-RequestDigest\": formDigest},\n";
                JSCodeBlock.Text += "data: \"[Comment]\",\n";
                JSCodeBlock.Text += "success: successHandler,\n";
                JSCodeBlock.Text += "error: errorHandler\n";
                JSCodeBlock.Text += "}\n";
                JSCodeBlock.Text += ");\n</textarea>";
            }

            else if (ActionList.SelectedItem.Text == "Create")
            {

                JSCodeBlock.Text += "var executor = new SP.RequestExecutor(appweburl);\n";
                JSCodeBlock.Text += "executor.executeAsync(\n";
                JSCodeBlock.Text += "{\n";
                JSCodeBlock.Text += "url: appweburl + \"/_api/" + CurrentEndpointLabel.Text + "\",\n";
                JSCodeBlock.Text += "method: \"POST\",\n";
                JSCodeBlock.Text += "headers: { \"accept\": \"application/json;odata=verbose\", \"content-type\": \"application/json;odata=verbose\", \"X-RequestDigest\": formDigest},\n";
                JSCodeBlock.Text += "data: \"" + EntityJSONLabel.Text + "\",\n";
                JSCodeBlock.Text += "success: successHandler,\n";
                JSCodeBlock.Text += "error: errorHandler\n";
                JSCodeBlock.Text += "}\n";
                JSCodeBlock.Text += ");\n</textarea>";

            }

            else if (ActionList.SelectedItem.Text == "Read" && ResourceList.SelectedItem.Text == "Folders")
            {
                JSCodeBlock.Text += "var executor = new SP.RequestExecutor(appweburl);\n";
                JSCodeBlock.Text += "executor.executeAsync(\n";
                JSCodeBlock.Text += "{\n";
                JSCodeBlock.Text += "url: appweburl + \"/_api/web/GetFolderByServerRelativeUrl('/[document library relative url]')\",\n";
                JSCodeBlock.Text += "method: \"GET\",\n";
                JSCodeBlock.Text += "headers: { \"accept\": \"application/json;odata=verbose\"},\n";
                JSCodeBlock.Text += "success: successHandler,\n";
                JSCodeBlock.Text += "error: errorHandler\n";
                JSCodeBlock.Text += "}\n";
                JSCodeBlock.Text += ");\n</textarea>";
            }
            else if (ActionList.SelectedItem.Text == "Read" && ResourceList.SelectedItem.Text == "Files")
            {
                JSCodeBlock.Text += "var executor = new SP.RequestExecutor(appweburl);\n";
                JSCodeBlock.Text += "executor.executeAsync(\n";
                JSCodeBlock.Text += "{\n";
                JSCodeBlock.Text += "url: appweburl + \"/_api/web/GetFolderByServerRelativeUrl('/[document library relative url]')/Files(\'[file name]\')\",\n";
                JSCodeBlock.Text += "method: \"GET\",\n";
                JSCodeBlock.Text += "headers: { \"accept\": \"application/json;odata=verbose\"},\n";
                JSCodeBlock.Text += "success: successHandler,\n";
                JSCodeBlock.Text += "error: errorHandler\n";
                JSCodeBlock.Text += "}\n";
                JSCodeBlock.Text += ");\n</textarea>";
            }

            else if (ActionList.SelectedItem.Text == "Read")
            {

                JSCodeBlock.Text += "var executor = new SP.RequestExecutor(appweburl);\n";
                JSCodeBlock.Text += "executor.executeAsync(\n";
                JSCodeBlock.Text += "{\n";
                JSCodeBlock.Text += "url: appweburl + \"/_api/" + CurrentEndpointLabel.Text + "\",\n";
                JSCodeBlock.Text += "method: \"GET\",\n";
                JSCodeBlock.Text += "headers: { \"accept\": \"application/json;odata=verbose\"},\n";
                JSCodeBlock.Text += "success: successHandler,\n";
                JSCodeBlock.Text += "error: errorHandler\n";
                JSCodeBlock.Text += "}\n";
                JSCodeBlock.Text += ");\n</textarea>";
            }

            else if (ActionList.SelectedItem.Text == "Update" && ResourceList.SelectedItem.Text == "Folders")
            {
                JSCodeBlock.Text += "var executor = new SP.RequestExecutor(appweburl);\n";
                JSCodeBlock.Text += "executor.executeAsync(\n";
                JSCodeBlock.Text += "{\n";
                JSCodeBlock.Text += "url: appweburl + \"/_api/web/GetFolderByServerRelativeUrl(\'/[document library relative url]\')\",\n";
                JSCodeBlock.Text += "method: \"POST\",\n";
                JSCodeBlock.Text += "headers: { \"accept\": \"application/json;odata=verbose\", \"content-type\": \"application/json;odata=verbose\", \"X-RequestDigest\": formDigest, \"If-Match\": eTag, \"X-Http-Method\": \"MERGE\"},\n";
                JSCodeBlock.Text += "data: \"" + EntityJSONLabel.Text + "\",\n";
                JSCodeBlock.Text += "success: successHandler,\n";
                JSCodeBlock.Text += "error: errorHandler\n";
                JSCodeBlock.Text += "}\n";
                JSCodeBlock.Text += ");\n</textarea>";
            }
            else if (ActionList.SelectedItem.Text == "Update" && ResourceList.SelectedItem.Text == "Files")
            {
                JSCodeBlock.Text += "var executor = new SP.RequestExecutor(appweburl);\n";
                JSCodeBlock.Text += "executor.executeAsync(\n";
                JSCodeBlock.Text += "{\n";
                JSCodeBlock.Text += "url: appweburl + \"/_api/web/GetFolderByServerRelativeUrl(\'/[document library relative url]\')/Files/add(url=\'a.txt\',overwrite=true)\",\n";
                JSCodeBlock.Text += "method: \"PUT\",\n";
                JSCodeBlock.Text += "headers: { \"accept\": \"application/json;odata=verbose\", \"content-type\": \"application/json;odata=verbose\", \"X-RequestDigest\": formDigest, \"If-Match\": eTag, \"X-Http-Method\": \"MERGE\"},\n";
                JSCodeBlock.Text += "data: \"[Comment]\",\n";
                JSCodeBlock.Text += "success: successHandler,\n";
                JSCodeBlock.Text += "error: errorHandler\n";
                JSCodeBlock.Text += "}\n";
                JSCodeBlock.Text += ");\n</textarea>";
            }

            else if (ActionList.SelectedItem.Text == "Update")
            {

                JSCodeBlock.Text += "var executor = new SP.RequestExecutor(appweburl);\n";
                JSCodeBlock.Text += "executor.executeAsync(\n";
                JSCodeBlock.Text += "{\n";
                JSCodeBlock.Text += "url: appweburl + \"/_api/" + CurrentEndpointLabel.Text + "\",\n";
                JSCodeBlock.Text += "method: \"POST\",\n";
                JSCodeBlock.Text += "headers: { \"accept\": \"application/json;odata=verbose\", \"content-type\": \"application/json;odata=verbose\", \"X-RequestDigest\": formDigest, \"If-Match\": eTag, \"X-Http-Method\": \"MERGE\"},\n";
                JSCodeBlock.Text += "data: \"" + EntityJSONLabel.Text + "\",\n";
                JSCodeBlock.Text += "success: successHandler,\n";
                JSCodeBlock.Text += "error: errorHandler\n";
                JSCodeBlock.Text += "}\n";
                JSCodeBlock.Text += ");\n</textarea>";
            }

            else if (ActionList.SelectedItem.Text == "Delete" && ResourceList.SelectedItem.Text == "Folders")
            {
                JSCodeBlock.Text += "var executor = new SP.RequestExecutor(appweburl);\n";
                JSCodeBlock.Text += "executor.executeAsync(\n";
                JSCodeBlock.Text += "{\n";
                JSCodeBlock.Text += "url: appweburl + \"/_api/web/GetFolderByServerRelativeUrl(\'/[document library relative url]\')\",\n";
                JSCodeBlock.Text += "method: \"POST\",\n";
                JSCodeBlock.Text += "headers: {\"X-RequestDigest\": formDigest, \"If-Match\": eTag, \"X-Http-Method\": \"DELETE\"},\n";
                JSCodeBlock.Text += "success: successHandler,\n";
                JSCodeBlock.Text += "error: errorHandler\n";
                JSCodeBlock.Text += "}\n";
                JSCodeBlock.Text += ");\n</textarea>";
            }

            else if (ActionList.SelectedItem.Text == "Delete" && ResourceList.SelectedItem.Text == "Files")
            {
                JSCodeBlock.Text += "var executor = new SP.RequestExecutor(appweburl);\n";
                JSCodeBlock.Text += "executor.executeAsync(\n";
                JSCodeBlock.Text += "{\n";
                JSCodeBlock.Text += "url: appweburl + \"/_api/web/GetFolderByServerRelativeUrl(\'/[document library relative url]\')/Files/[file name]\",\n";
                JSCodeBlock.Text += "method: \"POST\",\n";
                JSCodeBlock.Text += "headers: {\"X-RequestDigest\": formDigest, \"If-Match\": eTag, \"X-Http-Method\": \"DELETE\"},\n";
                JSCodeBlock.Text += "success: successHandler,\n";
                JSCodeBlock.Text += "error: errorHandler\n";
                JSCodeBlock.Text += "}\n";
                JSCodeBlock.Text += ");\n</textarea>";
            }

            else if (ActionList.SelectedItem.Text == "Delete")
            {


                JSCodeBlock.Text += "var executor = new SP.RequestExecutor(appweburl);\n";
                JSCodeBlock.Text += "executor.executeAsync(\n";
                JSCodeBlock.Text += "{\n";
                JSCodeBlock.Text += "url: appweburl + \"/_api/" + CurrentEndpointLabel.Text + "\",\n";
                JSCodeBlock.Text += "method: \"POST\",\n";
                JSCodeBlock.Text += "headers: {\"X-RequestDigest\": formDigest, \"If-Match\": eTag, \"X-Http-Method\": \"DELETE\"},\n";
                JSCodeBlock.Text += "success: successHandler,\n";
                JSCodeBlock.Text += "error: errorHandler\n";
                JSCodeBlock.Text += "}\n";
                JSCodeBlock.Text += ");\n</textarea>";
            }
        }

        protected void EntitySelectPropertiesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string properties = "";
            List<string> selectedProperties = new List<string>();
            for (int i = 0; i < EntitySelectPropertiesList.Items.Count; i++)
            {
                if (EntitySelectPropertiesList.Items[i].Selected)
                {
                    selectedProperties.Add(EntitySelectPropertiesList.Items[i].Value);
                }
            }

            int propertyCounter = 1;
            foreach (string property in selectedProperties)
            {
                properties += "'" + property + "': '[value]'";
                if (propertyCounter != selectedProperties.Count)
                {
                    properties += ", ";
                }
                propertyCounter++;
            }

            EntityJSONLabel.Text = "{'__metadata':{'type':'" + EntityTypeLabel.Text + "'}," + properties + "}";
        }



    }
}