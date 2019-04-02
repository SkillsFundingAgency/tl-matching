"use strict";

var employer = (function () {
    var businessNameMinLength = 2;

    accessibleAutocomplete.enhanceSelectElement({
        defaultValue: "",
        autoSelect: true,
        selectElement: document.querySelector("#EmployerName"),
        minLength: businessNameMinLength,
        source: search,
        name: "EmployerName",
        onConfirm: setSelectedEmployerId
    });

    $("#tl-continue").click(function () {
        var currentBusinessName = document.querySelector("#EmployerName").value;
        if (currentBusinessName.length < businessNameMinLength) {
            $("#SelectedEmployerId").val("");
            $("#findEmployer").submit();
            return;
        }

        var selectedEmployerId = $("#SelectedEmployerId").val();
        if (selectedEmployerId > 0) {
            $("#findEmployer").submit();
            return;
        }

        var employerNamesWithIds = $("#employerNamesWithIds").val();
        if (employerNamesWithIds === "") {
            $("#findEmployer").submit();
            return;
        }

        var employerNamesWithIdsCommaSplit = employerNamesWithIds.split(",");
        $.each(employerNamesWithIdsCommaSplit, function () {
            var splitValues = this.split(":");
            $.each(splitValues,
                function () {
                    $("#SelectedEmployerId").val(splitValues[1]);
                    return false;
                });
            return false;
        });
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

                    var employerNamesWithIds = $.map(employers, function (e) {
                        return getEmployerNameWithAka(e) + ":" + e.id;
                    });

                    $("#employerNamesWithIds").val(employerNamesWithIds.join(","));
                    $("#SelectedEmployerId").val("");

                    var filteredResults = employerNames.filter(e => e.toLowerCase().indexOf(query.toLowerCase()) !== -1);
                    populateResults(filteredResults);
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
            return e.employerName;
        }
        return e.employerName + " (" + e.alsoKnownAs + ")";
    }

    function setSelectedEmployerId(confirmed) {
        var employerNamesWithIds = $("#employerNamesWithIds").val().split(",");
        $.each(employerNamesWithIds, function () {
            var splitValues = this.split(":");
            $.each(splitValues,
                function () {
                    if (splitValues[0] === confirmed) {
                        $("#SelectedEmployerId").val(splitValues[1]);
                    }
                });
        });
    }
})();