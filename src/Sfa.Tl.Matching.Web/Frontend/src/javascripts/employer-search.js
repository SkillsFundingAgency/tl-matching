"use strict";
var Searchresult = null;
var employer = (function () {
    var queryMinLength = 2;
    var timeoutId;

    accessibleAutocomplete.enhanceSelectElement({
        defaultValue: $("#companyNameHidden").val(),
        autoSelect: true,
        selectElement: document.querySelector("#CompanyName"),
        minLength: queryMinLength,
        source: search,
        name: "CompanyName",
        onConfirm: setSelectedEmployerId
    });

    $("#SelectedEmployerId").val("");

    function search(query, populateResults) {
        if (query.trim().length < queryMinLength) return;

        const delayInMs = 400;

        clearTimeout(timeoutId);

        timeoutId = setTimeout(function () {
            $.ajax({
                url: "/employer-search",
                contentType: "application/json",
                data: { query: query },
                success: function (employers) {
                    var employerNames = $.map(employers, function (e) {
                        return getEmployerNameWithAka(e);
                    });

                    Searchresult = employers;

                    if (Searchresult !== undefined && Searchresult !== null) {
                        if (Searchresult[0] !== undefined && Searchresult[0] !== null) {
                            $("#SelectedEmployerId").val(Searchresult[0].id);
                        }
                    }

                    populateResults(employerNames);
                },
                timeout: 5000,
                error: function () {
                    console.log("An error occurred.");
                }
            });
        }, delayInMs);
    }

    function getEmployerNameWithAka(e) {
        if (!e.alsoKnownAs) {
            return e.companyName;
        }
        return e.companyName + " (" + e.alsoKnownAs + ")";
    }

    function setSelectedEmployerId(confirmed) {
        $.each(Searchresult, function () {
            if (getEmployerNameWithAka(this) === confirmed) {
                $("#SelectedEmployerId").val(this.id);
            }
        });
    }
})();