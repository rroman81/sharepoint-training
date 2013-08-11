"use strict";

var hostweburl;
var appweburl;
var listid; 

// Load the required SharePoint libraries
$(document).ready(function () {
    //Get the URI decoded URLs.
    appweburl =
        decodeURIComponent(
            getQueryStringParameter("SPAppWebUrl")
    );
    hostweburl =
        decodeURIComponent(
            getQueryStringParameter("SPHostUrl")
    );
    listid =
        decodeURIComponent(
            getQueryStringParameter("list")
            );
    // Trim '{' and '}' from the guid
    listid = listid.substring(1, listid.length - 1);

    // resources are in URLs in the form:
    // web_url/_layouts/15/resource
    var scriptbase = hostweburl + "/_layouts/15/";

    // Load the js file and continue to the 
    //   success event handler
    $.getScript(scriptbase + "SP.RequestExecutor.js");
});

$("#lnkRequest").click(function () {
    var executor;
    var formDigestValue;
    var formDigestEndpoint;
    var fileContent;
    var fileEndpoint;
    var bookTitle;

    bookTitle = document.getElementById("txtTitle").value;

    // The contextinfo endpoint contains the formDigest value
    formDigestEndpoint = appweburl + "/_api/contextinfo";
    // The endpoint to the host web is constructed by using
    // the SP.AppContextSite operator
    fileEndpoint = appweburl + "/_api/SP.AppContextSite(@target)/web/lists('" + listid + "')/rootfolder/files/add(url='" + bookTitle + ".txt')?@target='" + hostweburl + "'";
    fileContent = document.getElementById("content").value;
    executor = new SP.RequestExecutor(appweburl);

    // Issue a request to the contextinfo object to get the FormDigestValue
    executor.executeAsync(
        {
            url: formDigestEndpoint,
            method: "POST",
            headers: { "Accept": "application/json; odata=verbose" },
            success: function (data) {
                var jsonObject = JSON.parse(data.body);
                formDigestValue = jsonObject.d.GetContextWebInformation.FormDigestValue;
                uploadFile(formDigestValue, fileEndpoint, fileContent);
            },
            error: function (data, errorCode, errorMessage) {
                var errMsg = "Error retrieving the form digest value: "
                    + errorMessage;
                document.getElementById("divMsg").innerText = errMsg;
            }
        }
    );
});

function uploadFile(formDigestValue, fileEndpoint, content) {
    var executor;

    executor = new SP.RequestExecutor(appweburl);
    // Issue a request to the contextinfo object to get the FormDigestValue
    executor.executeAsync(
        {
            url:
                fileEndpoint,
            method: "POST",
            headers: {
                "Accept": "application/json; odata=verbose",
                "Content-type": "text/html;charset=UTF-8",
                "X-RequestDigest": formDigestValue
            },
            body : content,
            success: function (data) {
                document.getElementById("divMsg").innerText = "Book requested, closing dialog...";
                //Close the window and refresh the page
                window.parent.postMessage("CloseCustomActionDialogRefresh", "*");
            },
            error: function (data, errorCode, errorMessage) {
                var errMsg = "Error uploading the file: "
                    + errorMessage;
                document.getElementById("divMsg").innerText = errMsg;
            }
        }
    );
}