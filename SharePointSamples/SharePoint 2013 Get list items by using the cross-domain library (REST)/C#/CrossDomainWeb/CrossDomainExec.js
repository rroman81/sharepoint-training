﻿var hostweburl;
var appweburl;

// Load the required SharePoint libraries
$(document).ready(function () {
    //Get the URI decoded URLs.
    hostweburl =
        decodeURIComponent(
            getQueryStringParameter("SPHostUrl")
    );
    appweburl =
        decodeURIComponent(
            getQueryStringParameter("SPAppWebUrl")
    );

    // resources are in URLs in the form:
    // web_url/_layouts/15/resource
    var scriptbase = hostweburl + "/_layouts/15/";

    // Load the js files and continue to the successHandler
    $.getScript(scriptbase + "SP.RequestExecutor.js", execCrossDomainRequest);
});

// Function to prepare and issue the request to get
//  SharePoint data
function execCrossDomainRequest() {
    // executor: The RequestExecutor object
    // Initialize the RequestExecutor with the app web URL.
    var executor = new SP.RequestExecutor(appweburl);

    // Issue the call against the app web.
    // To get the title using REST we can hit the endpoint:
    //      appweburl/_api/web/lists/getbytitle('listname')/items
    // The response formats the data in the JSON format.
    // The functions successHandler and errorHandler attend the
    //      sucess and error events respectively.
    executor.executeAsync(
        {
            url:
                appweburl +
                "/_api/web/lists/getbytitle('Announcements')/items",
            method: "GET",
            headers: { "Accept": "application/json; odata=verbose" },
            success: successHandler,
            error: errorHandler
        }
    );
}

// Function to handle the success event.
// Prints the data to the page.
function successHandler(data) {
    var jsonObject = JSON.parse(data.body);
    var announcementsHTML = "";

    var results = jsonObject.d.results;
    for (var i = 0; i < results.length; i++) {
        announcementsHTML = announcementsHTML +
            "<p><h1>" + results[i].Title +
            "</h1>" + results[i].Body +
            "</p><hr>";
    }

    document.getElementById("renderAnnouncements").innerHTML =
        announcementsHTML;
}

// Function to handle the error event.
// Prints the error message to the page.
function errorHandler(data, errorCode, errorMessage) {
    document.getElementById("renderAnnouncements").innerText =
        "Could not complete cross-domain call: " + errorMessage;
}

// Function to retrieve a query string value.
// For production purposes you may want to use
//  a library to handle the query string.
function getQueryStringParameter(paramToRetrieve) {
    var params =
        document.URL.split("?")[1].split("&");
    var strParams = "";
    for (var i = 0; i < params.length; i = i + 1) {
        var singleParam = params[i].split("=");
        if (singleParam[0] == paramToRetrieve)
            return singleParam[1];
    }
}