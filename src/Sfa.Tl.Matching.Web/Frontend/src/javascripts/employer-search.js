"use strict";

var employer = (function () {
    accessibleAutocomplete.enhanceSelectElement({
        defaultValue: "",
        selectElement: document.querySelector("#businessName"),
        minLength: 2,
        source: search
    });

    function search(query, populateResults) {
        $.ajax({
            url: "/employer-search",
            contentType: "application/json",
            data: { query: query },
            success: function (employers) {
                var filteredResults = employers.filter(e => e.indexOf(query) !== -1);
                populateResults(filteredResults);
            },
            timeout: 5000,
            error: function () {
                alert("An error occurred.");
            }
        });
    }
})();