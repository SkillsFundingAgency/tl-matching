"use strict";

var employer = (function () {
    accessibleAutocomplete.enhanceSelectElement({
        defaultValue: "",
        autoSelect: true,
        selectElement: document.querySelector("#BusinessName"),
        minLength: 2,
        source: search,
        name: "BusinessName"
    });

    function search(query, populateResults) {
        $.ajax({
            url: "/employer-search",
            contentType: "application/json",
            data: { query: query },
            success: function (employers) {
                var filteredResults = employers.filter(e => e.indexOf(query) !== -1);
                //if (filteredResults.length > 0) {
                    populateResults(filteredResults);
                //} else {
                    $("#BusinessName").val("");
                //}
            },
            timeout: 5000,
            error: function () {
                alert("An error occurred.");
            }
        });
    }
})();