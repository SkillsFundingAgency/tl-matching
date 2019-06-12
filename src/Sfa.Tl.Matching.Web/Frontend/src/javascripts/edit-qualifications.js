"use strict";

var editQualifications = (function () {
    $(".tl-expandable--header").click(function (e) {
        e.preventDefault();
        $(this).parent().toggleClass("active");
    });

    

    $(".tl-expandable").focusout(function (e) {
        if ($(this).find(e.relatedTarget).length === 0) {
            e.preventDefault();
            $(this).toggleClass("active");
        }
    });

    $(".tl-qual-row").on("change", "input", function () {
        console.log("test");
        $(this).closest(".tl-qual-row").addClass("test");
    });

    $(".tl-editquals-item").submit(function (event) {

        const qualEditform = $(this);
        const qualId = qualEditform[0].elements.QualificationId.value;
        event.preventDefault();

        const autoCompleteShortTitle = $(`#SelectShortTitle_${this.elements.QualificationId.value}`).val();

        $(`#ShortTitle_${this.elements.QualificationId.value}`).val(autoCompleteShortTitle);

        const formData = $(this).serialize();

        $.ajax({
            url: $(this).attr("action"),
            type: "POST",
            data: formData,
            success: function (result) {
                if (result.success === false) {
                    setValidationError(result.response, qualId);
                }
                if (result.success === true) {
                    clearValidationError(qualId);
                }
            },
            error: function () {
                console.log("An error occurred.");
            }
        });
    });

    function setValidationError(response, qualId) {
        const error = JSON.parse(response);

        clearValidationError(qualId);

        if (error.ShortTitle !== undefined && error.ShortTitle !== null) {
            // TODO Add Input Error 
            //$(`#tl-autocomplete_${qualId}`).addClass("govuk-input--error");
            $(`#tl-autocomplete_${qualId}`).addClass("tl-error");
        }

        if (error.Routes !== undefined && error.Routes !== null) {            
            $(`#tl-expandable_${qualId}`).addClass("tl-error");
        }

        //TODO Add Validation Summary Code
    }

    function clearValidationError(qualId) {
        //$(`#tl-autocomplete_${qualId}`).removeClass("govuk-input--error");

        $(`#tl-autocomplete_${qualId}`).removeClass("tl-error");
        $(`#tl-expandable_${qualId}`).removeClass("tl-error");

        //TODO Add Validation Summary Code
    }

    var queryMinLength = 2;

    $("select").each(function () {
        const shortTitleDefaultValue = $(`#${this.name.replace("Select", "")}`).val();

        accessibleAutocomplete.enhanceSelectElement({
            defaultValue: shortTitleDefaultValue,
            autoselect: false,
            selectElement: document.querySelector(`#${this.id}`),
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
                const shortTitlesList = $.map(shortTitles, function (st) {
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