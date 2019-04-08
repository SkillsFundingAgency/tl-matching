"use strict";
var Searchresult = null;
var employer = (function () {
    var queryMinLength = 2;

    accessibleAutocomplete.enhanceSelectElement({
        defaultValue: "",
        autoSelect: true,
        selectElement: document.querySelector("#CompanyName"),
        minLength: queryMinLength,
        source: search,
        name: "CompanyName",
        onConfirm: setSelectedEmployerId
    });

    function search(query, populateResults) {
        var delayInMs = 100;

        setTimeout(function () {
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
                            var firstresult = Searchresult[0];
                            $("#SelectedEmployerId").val(firstresult.id);
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