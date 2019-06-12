"use strict";
var missingQualShortTitle = (function () {
    var queryMinLength = 2;

    accessibleAutocomplete.enhanceSelectElement({
        defaultValue: "",
        autoselect: false,
        selectElement: document.querySelector("#ShortTitle"),
        minLength: queryMinLength,
        source: searchShortTitle,
        showNoOptionsFound: false,
        name: "ShortTitle",
        confirm: setSelectedShortTitle
    });

    function searchShortTitle(query, populateResults) {
        if (query.trim().length < queryMinLength) return;

        $.ajax({
            url: "/search-short-title",
            contentType: "application/json",
            data: { query: query },
            success: function (shortTitles) {
                const shortTitlesList = $.map(shortTitles,
                    function (st) {
                        return st.shortTitle;
                    });

                populateResults(shortTitlesList);
            },
            timeout: 5000,
            error: function () {
                console.log("An error occurred.");
            }
        });
    }

    function setSelectedShortTitle() {
        $.each(Searchresult,
            function () {
                $("#shortTitleHidden").val($("#ShortTitle").val());
            });
    }
})();