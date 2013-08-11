"use strict";

var hostweburl;
var appweburl;
var listid;
var itemid;

// Load the required SharePoint libraries
$(document).ready(function () {
    // Assign some of the localized strings in the page
    document.getElementById("purchasePageHeader").innerText = purchasePageHeader;
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
    listid = listid.substring(1, listid.length - 1)
    itemid = getQueryStringParameter("item");

    // resources are in URLs in the form:
    // web_url/_layouts/15/resource
    var scriptbase = hostweburl + "/_layouts/15/";

    // Load the js file and continue to the 
    //   success event handler
    // Load the js files and continue to the successHandler
    $.getScript(scriptbase + "SP.Runtime.js",
        function () {
            $.getScript(scriptbase + "SP.js",
                function () { $.getScript(scriptbase + "SP.RequestExecutor.js", getMetaData); }
                );
        }
    );
});

function getMetaData() {
    var executor;
    var endpoint;

    // Initialize the RequestExecutor with the app web URL.
    executor = new SP.RequestExecutor(appweburl);

    // Issue the call against the host web.
    // The endpoint to the host web is constructed by using
    // the SP.AppContextSite operator
    // The response formats the data in the JSON format.
    endpoint = "/_api/SP.AppContextSite(@target)/web/lists('" + listid + "')/items(" + itemid + ")/file?@target='" + hostweburl + "'";
    executor.executeAsync(
        {
            url:
                appweburl + endpoint,
            method: "GET",
            headers: { "Accept": "application/json; odata=verbose" },
            success: successGetBookMetadata,
            error: errorGetBookMetadata
        }
    );
}


// Function to handle the success event.
// Prints the book metadata to the page.
function successGetBookMetadata(data) {
    var jsonObject = JSON.parse(data.body);
    var bookTitle = jsonObject.d.Name.split(".")[0];
    var bookPrice = jsonObject.d.Length / 1024;

    //Render the title and price in the placeholders
    document.getElementById("lblBookTitle").innerText =
        bookTitle;
    document.getElementById("lblBookPrice").innerText =
        "$ " + bookPrice.toFixed(2);
    $("#lnkBuy").show();
}

// Function to handle the error event.
// Prints the error message to the page.
function errorGetBookMetadata(data, errorCode, errorMessage) {
    document.getElementById("lblBookTitle").innerText =
        "Could not complete cross-domain call: " + errorMessage;
}

function lnkBuy_Click() {
    // Issue a call to the app web to log the order
    // by creating a list item in the Orders list
    var context;
    var factory;
    var appContextSite;

    context = new SP.ClientContext(appweburl);
    factory = new SP.ProxyWebRequestExecutorFactory(appweburl);
    context.set_webRequestExecutorFactory(factory);

    // Get the list by using the localized title
    var oList = context.get_web().get_lists().getByTitle(ordersListName);

    var itemCreateInfo = new SP.ListItemCreationInformation();
    this.oListItem = oList.addItem(itemCreateInfo);
    oListItem.set_item("Bookname", document.getElementById("lblBookTitle").innerText);
    oListItem.set_item("Price", document.getElementById("lblBookPrice").innerText);
    oListItem.set_item("Orderstatus", 3);
    oListItem.update();

    context.load(this.oListItem);
    context.executeQueryAsync(successLogOrder, errorLogOrder);
}

// Function to handle the success event.
// Prints the book content to the page.
function successGetBookContents(data) {
    //Render the contents in the placeholder
    document.getElementById("bookContent").style.visibility = "visible";
    document.getElementById("txtBookContent").innerText =
        data.body;
}

// Function to handle the error event.
// Prints the error message to the page.
function errorGetBookContents(data, errorCode, errorMessage) {
    document.getElementById("lblBookTitle").innerText =
        "Could not complete cross-domain call: " + errorMessage;
}

// Function to issue the call against the file contents in 
//   the document library
function successLogOrder(data) {
    var executor;
    var endpoint;

    // Initialize the RequestExecutor with the app web URL.
    executor = new SP.RequestExecutor(appweburl);

    // Issue the call against the host web.
    // The endpoint to the host web is constructed by using
    // the SP.AppContextSite operator
    endpoint = "/_api/SP.AppContextSite(@target)/web/lists('" + listid + "')/items(" + itemid + ")/file/$value?@target='" + hostweburl + "'";
    executor.executeAsync(
        {
            url:
                appweburl + endpoint,
            method: "GET",
            headers: { "Accept": "application/json; odata=verbose" },
            success: successGetBookContents,
            error: errorGetBookContents
        }
    );

}

// Function to handle the error event.
// Prints the error message to the page.
function errorLogOrder() {
    document.getElementById("divMsg").innerText =
        "Error logging the order: " + arguments[1];
}