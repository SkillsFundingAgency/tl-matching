"use strict";

var editQualifications = (function () {
    $('.tl-expandable--header').click(function (e) {
        e.preventDefault();
        $(this).parent().toggleClass('active');
    });

    $('.tl-editquals-item').submit(function (event) {
        event.preventDefault();

        $('#ShortTitle_' + this.elements.QualificationId.value).val($('#SelectShortTitle_' + this.elements.QualificationId.value).val());

        var formData = $(this).serialize();

        $.ajax({
            url: $(this).attr('action'),
            type: "POST",
            data: formData,
            success: function (result) {
                alert("Succes");
            },
            error: function (result) {
                alert("Succes");
            }

        });
    });

    var queryMinLength = 2;

    $("select").each(function () {
        var shortTitleDefaultValue = $("#" + this.name.replace("Select", "")).val();

        accessibleAutocomplete.enhanceSelectElement({
            defaultValue: shortTitleDefaultValue,
            autoselect: false,
            selectElement: document.querySelector("#" + this.id),
            minLength: queryMinLength,
            source: searchShortTitle,
            showNoOptionsFound: false,
            name: this.id
        });
    });

    function searchShortTitle(query, populateResults) {
        $.ajax({
            url: "/search-short-title",
            contentType: "application/json",
            data: { query: query },
            success: function (shortTitles) {
                var shortTitlesList = $.map(shortTitles, function (st) {
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
})();