"use strict";

var editQualifications = (function () {
    $('.tl-expandable--header').click(function (e) {
        e.preventDefault();
        $(this).parent().toggleClass('active');
    });

    $('.tl-editquals-item').submit(function (event) {
        event.preventDefault();

        var myForm = $(this);

        $.ajax({
            url: $(this).attr('action'),
            type: "POST",
            data: $(this).serialize(),
            success: function (result) {
                //alert(result);
                myForm.replaceWith(result);
                //TODO: Reattach this event
            }
        });
    });

    var queryMinLength = 2;

    $("select").each(function () {
        accessibleAutocomplete.enhanceSelectElement({
            defaultValue: "",
            autoselect: false,
            selectElement: document.querySelector("#" + this.id),
            minLength: queryMinLength,
            source: searchShortTitle,
            showNoOptionsFound: false,
            name: this.id
        });

        $("#" + this.name).val($("#Hidden" + this.name).val());
    });

    function searchShortTitle(query, populateResults) {
        var delayInMs = 100;

        setTimeout(function () {
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
        }, delayInMs);
    }
})();