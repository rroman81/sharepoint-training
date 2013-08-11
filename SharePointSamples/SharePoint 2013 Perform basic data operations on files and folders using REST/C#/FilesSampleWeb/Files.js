var hostweburl;
var appweburl;
var formdigest;

// Load the required SharePoint libraries.
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

    //Assign events to buttons
    $("#createFolderButton").click(function (event) {
        createFolder();
        event.preventDefault();
    });

    $("#deleteFolderButton").click(function (event) {
        deleteFolder();
        event.preventDefault();
    });

    $("#createFileButton").click(function (event) {
        createFile();
        event.preventDefault();
    });

    $("#deleteFileButton").click(function (event) {
        deleteFile();
        event.preventDefault();
    });

    $("#getFileButton").click(function (event) {
        getFile();
        event.preventDefault();
    });

    $("#updateFileButton").click(function (event) {
        updateFile();
        event.preventDefault();
    });

    // Resources are in URLs in the form:
    // web_url/_layouts/15/resource
    var scriptbase = hostweburl + "/_layouts/15/";

    // Load the js file and continue to load the page with information
    //   about the site's folders and files.
    $.getScript(scriptbase + "SP.RequestExecutor.js", loadPage);

});

function loadPage() {
    getFormDigest();
    getFolders();
}


//Folders

//Retrieve all of the site's folders.
function getFolders() {
    var executor;

    // Initialize the RequestExecutor with the app web URL.
    executor = new SP.RequestExecutor(appweburl);

    executor.executeAsync(
    {
        url:
            appweburl +
            "/_api/SP.AppContextSite(@target)/web/Folders?@target='" +
            hostweburl + "'",
        method: "GET",
        headers: { "Accept": "application/json; odata=verbose" },
        success: getFoldersSuccessHandler,
        error: getFoldersErrorHandler
    }
);

}

//Create a new folder.
function createFolder() {
    getFormDigest();
    var executor;

    // Initialize the RequestExecutor with the app web URL.
    executor = new SP.RequestExecutor(appweburl);
    var createFolderBox = document.getElementById("createFolderBox");
    var serverRelativeUrl = createFolderBox.value;

    var metadata = "{ '__metadata': { 'type': 'SP.Folder' }, 'ServerRelativeUrl': '/" + serverRelativeUrl + "'}"

    executor.executeAsync(
    {
        url:
            appweburl +
            "/_api/SP.AppContextSite(@target)/web/Folders?@target='" +
            hostweburl + "'",
        method: "POST",
        body: metadata,
        headers: { "Accept": "application/json; odata=verbose", "content-type": "application/json; odata=verbose", "X-RequestDigest": formdigest, "content-length": metadata.length },
        success: createFolderSuccessHandler,
        error: createFolderErrorHandler
    }
);

}

//Delete the selected folder.
function deleteFolder() {
    getFormDigest();
    var executor;

    // Initialize the RequestExecutor with the app web URL.
    executor = new SP.RequestExecutor(appweburl);

    var selectFolderBox = document.getElementById("selectFolderBox");
    var selectedFolder = selectFolderBox.value;

    executor.executeAsync(
    {
        url:
            appweburl +
            "/_api/SP.AppContextSite(@target)/web/GetFolderByServerRelativeUrl('/" + selectedFolder + "')?@target='" +
            hostweburl + "'",
        method: "POST",
        headers: { "Accept": "application/json; odata=verbose", "X-RequestDigest": formdigest, "X-HTTP-Method":"DELETE", "IF-MATCH":"*" },
        success: deleteFolderSuccessHandler,
        error: deleteFolderErrorHandler
    }
);
}

//Files

//Retrieve all of the files for the selected folder.
function getFiles() {
    var executor;

    // Initialize the RequestExecutor with the app web URL.
    executor = new SP.RequestExecutor(appweburl);

    var selectFolderBox = document.getElementById("selectFolderBox");
    var selectedFolder = selectFolderBox.value;

    executor.executeAsync(
    {
        url:
            appweburl +
            "/_api/SP.AppContextSite(@target)/web/GetFolderByServerRelativeUrl('/" + selectedFolder + "')/Files?@target='" +
            hostweburl + "'",
        method: "GET",
        headers: { "Accept": "application/json; odata=verbose" },
        success: getFilesSuccessHandler,
        error: getFilesErrorHandler
    }
);
}

//Create a new file.
function createFile() {
    getFormDigest();
    var executor;

    // Initialize the RequestExecutor with the app web URL.
    executor = new SP.RequestExecutor(appweburl);
    var selectFolderBox = document.getElementById("selectFolderBox");
    var serverRelativeUrl = selectFolderBox.value;
    var fileUrl = createFileBox.value;
    var fileContent = submitTextFile.value;

    executor.executeAsync(
{
    url:
        appweburl +
        "/_api/SP.AppContextSite(@target)/web/GetFolderByServerRelativeUrl('/" + serverRelativeUrl + "')/Files/add(url='" + fileUrl + "',overwrite='true')?@target='" +
        hostweburl + "'",
    method: "POST",
    body: fileContent,
    headers: { "Accept": "application/json; odata=verbose", "X-RequestDigest": formdigest, "content-length": fileContent.length },
    success: createFileSuccessHandler,
    error: createFileErrorHandler
}
);
}

//Update a file.
function updateFile() {
    getFormDigest();
    var executor;

    // Initialize the RequestExecutor with the app web URL.
    executor = new SP.RequestExecutor(appweburl);
    var selectFolderBox = document.getElementById("selectFolderBox");
    var serverRelativeUrl = selectFolderBox.value;
    var fileUrl = selectFileBox.value;
    var fileContent = submitTextFile.value;

    executor.executeAsync(
{
    url:
        appweburl +
        "/_api/SP.AppContextSite(@target)/web/GetFileByServerRelativeUrl('/" + serverRelativeUrl + "/" + fileUrl + "')/$value?@target='" +
        hostweburl + "'",
    method: "POST",
    body: fileContent,
    headers: { "Accept": "application/json; odata=verbose", "X-RequestDigest": formdigest, "content-length": fileContent.length, "X-HTTP-Method":"PUT" },
    success: updateFileSuccessHandler,
    error: updateFileErrorHandler
}
);
}

//Retrieve the selected file and save it locally.
function getFile() {
    var executor;

    // Initialize the RequestExecutor with the app web URL.
    executor = new SP.RequestExecutor(appweburl);
    var selectFolderBox = document.getElementById("selectFolderBox");
    var selectFileBox = document.getElementById("selectFileBox");
    var serverRelativeUrl = selectFolderBox.value;
    var fileUrl = selectFileBox.value;

    executor.executeAsync(
{
    url:
        appweburl +
        "/_api/SP.AppContextSite(@target)/web/GetFileByServerRelativeUrl('/" + serverRelativeUrl + "/" + fileUrl + "')/$value?@target='" +
        hostweburl + "'",
    method: "GET",
    headers: { "Accept": "application/json; odata=verbose" },
    success: getFileSuccessHandler,
    error: getFileErrorHandler
}
);
}

//Delete a file.
function deleteFile() {
    getFormDigest();
    var executor;

    // Initialize the RequestExecutor with the app web URL.
    executor = new SP.RequestExecutor(appweburl);
    var selectFolderBox = document.getElementById("selectFolderBox");
    var selectFileBox = document.getElementById("selectFileBox");
    var serverRelativeUrl = selectFolderBox.value;
    var fileUrl = selectFileBox.value;

    executor.executeAsync(
{
    url:
        appweburl +
        "/_api/SP.AppContextSite(@target)/web/GetFileByServerRelativeUrl('/" + serverRelativeUrl + "/" + fileUrl + "')?@target='" +
        hostweburl + "'",
    method: "POST",
    headers: { "Accept": "application/json; odata=verbose", "X-RequestDigest": formdigest, "X-HTTP-Method":"DELETE", "IF-MATCH":"*" },
    success: deleteFileSuccessHandler,
    error: deleteFileErrorHandler
}
);

}

//Success Handlers


//Store the value of the form digest.
function contextSuccessHandler(data) {
    var jsonObject = JSON.parse(data.body);
    formdigest = jsonObject.d.GetContextWebInformation.FormDigestValue;
}

//Populate the selectFolderBox control after retrieving all of the site's folders.
function getFoldersSuccessHandler(data) {
    var jsonObject = JSON.parse(data.body);
    var selectFolderBox = document.getElementById("selectFolderBox");

    if (selectFolderBox.hasChildNodes()) {
        while (selectFolderBox.childNodes.length >= 1) {
            selectFolderBox.removeChild(selectFolderBox.firstChild);
        }
    }

    var results = jsonObject.d.results;
    for (var i = 0; i < results.length; i++) {
        var selectOption = document.createElement("option");
        selectOption.value = results[i].Name;
        selectOption.innerText = results[i].Name;
        selectFolderBox.appendChild(selectOption);
    }
    getFiles();
}

//Save the file locally after it has been retrieved.
function getFileSuccessHandler(data) {
    var selectFileBox = document.getElementById("selectFileBox");
    var selectedFile = selectFileBox.value;
    save_content_to_file(data.body, selectedFile);

}

//Populate the selectFileBox control after retrieving all of the files in the selected folder.
function getFilesSuccessHandler(data) {
    var jsonObject = JSON.parse(data.body);
    var selectFileBox = document.getElementById("selectFileBox");

    if (selectFileBox.hasChildNodes()) {
        while (selectFileBox.childNodes.length >= 1) {
            selectFileBox.removeChild(selectFileBox.firstChild);
        }
    }

 
    var results = jsonObject.d.results; 
    for (var i = 0; i < results.length; i++) {
        var selectOption = document.createElement("option");
        selectOption.value = results[i].Name;
        selectOption.innerText = results[i].Name;
        selectFileBox.appendChild(selectOption);
    } 

}

//Reload the page information after creating a new folder.
function createFolderSuccessHandler(data) {
    getFolders();
    getFiles();
}

//Reload the page information after deleting a folder.
function deleteFolderSuccessHandler(data) {
    getFolders();
    getFiles();
}

//Reload the files after creating a new file.
function createFileSuccessHandler(data) {
    getFiles();
}

//Reload the files after deleting a file.
function deleteFileSuccessHandler(data) {
    getFiles();
}

//Reload the files after updating a file.
function updateFileSuccessHandler(data) {
    getFiles();
}


//Error handlers

function createFolderErrorHandler(data, errorCode, errorMessage) {
    alert("Could not create folder: " + errorMessage);
}

function deleteFolderErrorHandler(data, errorCode, errorMessage) {
    alert("Could not delete folder: " + errorMessage);
}

function getFoldersErrorHandler(data, errorCode, errorMessage) {
    alert("Could not get folders: " + errorMessage);
}

function getFilesErrorHandler(data, errorCode, errorMessage) {
    alert("Could not get files: " + errorMessage);
}

function contextErrorHandler(data, errorCode, errorMessage) {
    alert("Could not get context info: " + errorMessage);
}

function deleteFileErrorHandler(data, errorCode, errorMessage) {
    alert("Could not delete file: " + errorMessage);
}

function createFileErrorHandler(data, errorCode, errorMessage) {
    alert("Could not create file: " + errorMessage);
}

function updateFileErrorHandler(data, errorCode, errorMessage) {
    alert("Could not update file: " + errorMessage);
}

function getFileErrorHandler(data, errorCode, errorMessage) {
    alert("Could not get file: " + errorMessage);
}


//Utilities

// Retrieve a query string value.
// For production purposes you may want to use
// a library to handle the query string.
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

//Retrieve the form digest value.
function getFormDigest() {
    var executor;

    // Initialize the RequestExecutor with the app web URL.
    executor = new SP.RequestExecutor(appweburl);

    executor.executeAsync(
        {
            url:
                appweburl +
                "/_api/contextinfo",
            method: "POST",
            headers: { "Accept": "application/json; odata=verbose" },
            success: contextSuccessHandler,
            error: contextErrorHandler
        }
    );
 
}

//Save the contents of a file to a file on the local computer.

function save_content_to_file(content, filename) {
    var dlg = false;
    with (document) {
        ir = createElement('iframe');
        ir.id = 'ifr';
        ir.location = 'about.blank';
        ir.style.display = 'none';
        body.appendChild(ir);
        with (getElementById('ifr').contentWindow.document) {
            open("text/plain", "replace");
            charset = "utf-8";
            write(content);
            close();
            document.charset = "utf-8";
            dlg = execCommand('SaveAs', false, filename);
        }
        body.removeChild(ir);
    }
    return dlg;
}

