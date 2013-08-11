﻿$(document).ready(function () {
    var e = ExecuteOrDelayUntilScriptLoaded(showToolbar, "sp.js");
});

function showToolbar() {
    $("#toolbarDiv").show();
}

function executeQuery(queryTerms) {

    Results = {
        element: '',
        url: '',

        init: function (element) {
            Results.element = element;
            Results.url = _spPageContextInfo.webAbsoluteUrl + "/_api/search/query?querytext='" + queryTerms + "'";
        },

        load: function () {
            $.ajax(
                    {
                        url: Results.url,
                        method: "GET",
                        headers: {
                            "accept": "application/json",
                        },
                        success: Results.onSuccess,
                        error: Results.onError
                    }
                );
        },

        onSuccess: function (data) {
            var results = data.d.query.PrimaryQueryResult.RelevantResults.Table.Rows.results;
            var html = "<table>";

            for (var i = 0; i < results.length; i++) {
                html += "<tr><td>";
                html += results[i].Cells.results[3].Value;
                html += "</td><td>"
                html += results[i].Cells.results[6].Value;
                html += "</td><tr>";
            }

            html += "</table>";
            Results.element.html(html);
        },

        onError: function (err) {
            alert(JSON.stringify(err));
        }
    }

    Results.init($('#resultsDiv'));
    Results.load();

}

