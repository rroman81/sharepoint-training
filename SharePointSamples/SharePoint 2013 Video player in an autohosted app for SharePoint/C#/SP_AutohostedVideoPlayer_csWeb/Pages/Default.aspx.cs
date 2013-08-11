using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SP_AutohostedVideoPlayer_csWeb.Pages
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // The following code gets the client context that represents the host web.
            var contextToken = TokenHelper.GetContextTokenFromRequest(Page.Request);

            // Because this is an Autohosted App, SharePoint will pass in the host Url in the querystring.
            // Therefore, we'll retrieve it so that we can use it in GetClientContextWithContextToken method call
            var hostWeb = Page.Request["SPHostUrl"];

            // Then we'll build our context, exactly as implemented in the Visual Studio template for Autohosted apps
            using (var clientContext = TokenHelper.GetClientContextWithContextToken(hostWeb, contextToken, Request.Url.Authority))
            {
                // Now we will use some pretty standard CSOM operations to enumerate the 
                // document libraries in the host web...
                var hostedWeb = clientContext.Web;
                var libs = hostedWeb.Lists;
                clientContext.Load(hostedWeb);
                clientContext.Load(libs);
                clientContext.ExecuteQuery();
                var foundVideos = false;
                foreach (var lib in libs)
                {
                    if (lib.BaseType == Microsoft.SharePoint.Client.BaseType.DocumentLibrary)
                    {
                        // ... and for each document library we'll enumerate all the MP4 video files that
                        // may exist in the root folder of each library.
                        var folder = lib.RootFolder;
                        var files = folder.Files;
                        clientContext.Load(folder);
                        clientContext.Load(files);
                        clientContext.ExecuteQuery();
                        foreach (var file in files)
                        {
                            if (file.ServerRelativeUrl.ToLower().EndsWith(".mp4"))
                            {
                                // We know that we have at least one file, so we'll set the foundVideos variable to true
                                foundVideos = true;
                                // Then, for each MP4 file, we'll build an orange tile in the UI and set text of that tile to
                                // name of the file, and the client-side onclick event to the playVid function that exists
                                // in default.aspx. 
                                // Note that we provide the full URL to the video file as a parameter
                                // to the function, so that the video can be played dynamically.
                                // Also note that the classes tile, tileOrange, and fl are defined in our point8020metro.css
                                // file in the CSS folder of this project, and that the styles use a small image from the Images folder.
                                var videoItem = new LiteralControl("<div id='" + Guid.NewGuid()
                                    + "' class='tile tileOrange fl' onclick='playVid(&apos;" + hostedWeb.Url + file.ServerRelativeUrl + "&apos;);'>" 
                                    + file.Name + "</div>");
                                VideoList.Controls.Add(videoItem);
                            }
                        }
                    }
                }
                SiteTitle.Text = "Video Player: " + hostedWeb.Title;
                // If no videos have been found, build a red tile to inform the user
                if (!foundVideos)
                {
                    var videoItem = new LiteralControl("<div id='" + Guid.NewGuid()
                                    + "' class='tile tileRed fl'>There are no videos in the parent Web</div>");
                    VideoList.Controls.Add(videoItem);
                }
            }
        }
    }
}