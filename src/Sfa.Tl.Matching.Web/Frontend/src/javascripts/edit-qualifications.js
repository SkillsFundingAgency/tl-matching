"use strict";

var editQualifications = (function () {
    $(".tl-expandable--header").click(function (e) {
        e.preventDefault();
        $(this).parent().toggleClass("active");
        e.stopPropagation();
    });

    $(".tl-expandable--content").click(function (e) {
        e.stopPropagation();
    });

    $(document).on('click', function (e) {
        $(".tl-expandable").removeClass("active");
    });



    $(".tl-qual-row").on("change", "input", function () {
        $(this).closest(".tl-qual-row").addClass("test");
    });


    $(".tl-qual-row").on("change", "input", function () {
        $(this).closest(".tl-qual-row").addClass("tl-qual-change");
        $(this).closest(".tl-qual-row").find(".tl-qual-button").removeClass("govuk-button--disabled").attr("disabled", false);
    });

    $(".tl-editquals-item").submit(function (event) {

        const qualEditform = $(this);
        const qualId = qualEditform[0].elements.QualificationId.value;
        event.preventDefault();

        const autoCompleteShortTitle = $(`#SelectShortTitle_${this.elements.QualificationId.value}`).val();

        $(this).closest(".tl-qual-row").find(".tl-qual-button").addClass("govuk-button--disabled").attr("disabled", true);

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
        $(`#tl-autocomplete_${qualId}`).closest(".tl-qual-row").find(".tl-qual-button").addClass("govuk-button--disabled").attr("disabled", true);

        clearValidationError(qualId);

        var errorsArray = [];
        var errors = new Array();

        if (error.ShortTitle !== undefined && error.ShortTitle !== null) {
            // TODO Add Input Error 
            //$(`#tl-autocomplete_${qualId}`).addClass("govuk-input--error");
            $(`#tl-autocomplete_${qualId}`).addClass("tl-error");

            var shortTitleError = {};
            shortTitleError.qualId = qualId;
            shortTitleError.fieldName = "SelectShortTitle";
            shortTitleError.error = error.ShortTitle[0];
            errorsArray.push(shortTitleError);

            errors.push(error.ShortTitle[0]);
        }

        if (error.Routes !== undefined && error.Routes !== null) {
            $(`#tl-expandable_${qualId}`).addClass("tl-error");

            var routeError = {};
            routeError.qualId = qualId;
            routeError.fieldName = "tl-expandable";
            routeError.error = error.Routes[0];
            errorsArray.push(routeError);

            errors.push(error.Routes[0]);
        }

        if (errorsArray.length > 0) {
            $(`#tl-error_${qualId}`).addClass("tl-visible");
            var errorsAsText = errors.join("<br/>");

            $(`#tl-error_${qualId}`).html(errorsAsText);

            populateErrorSummary(errorsArray);
        }
    }

    function populateErrorSummary(errorsArray) {
        $(`#tl-error-summary`).removeClass("tl-hidden");

        for (var i = 0; i < errorsArray.length; i++) {
            $(`#tl-error-summary ul`)
                .append(`<a href=#${errorsArray[i].fieldName}_${errorsArray[i].qualId}>${errorsArray[i].error}</a><br/>`);
        }
    }

    function clearValidationError(qualId) {
        //$(`#tl-autocomplete_${qualId}`).removeClass("govuk-input--error");

        $(`#tl-autocomplete_${qualId}`).removeClass("tl-error");
        $(`#tl-expandable_${qualId}`).removeClass("tl-error");
        $(`#tl-error_${qualId}`).removeClass("tl-visible");
        $(`#tl-error-summary`).addClass("tl-hidden");
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