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

    $(document).on('click', function () {
        $(".tl-expandable").removeClass("active");
    });

    $(".tl-qual-row").on("change", "input", function () {
        $(this).closest(".tl-qual-row").addClass("tl-qual-change");
        $(this).closest(".tl-qual-row").find(".tl-qual-button").removeClass("govuk-button--disabled").attr("disabled", false);
    });

    $(".tl-editquals-item").submit(function (event) {

        const qualEditform = $(this);
        const qualId = qualEditform[0].elements.QualificationId.value;
        event.preventDefault();
        
        const qualTitle = qualEditform[0].elements.Title.value;
        const autoCompleteShortTitle = $(`#SelectShortTitle_${this.elements.QualificationId.value}`).val();

        $(this).nextUntil(".tl-qual-row").next().find(".tl-qual-button").addClass("govuk-button--disabled").attr("disabled", true);

        $(`#ShortTitle_${this.elements.QualificationId.value}`).val(autoCompleteShortTitle);

        const formData = $(this).serialize();

        $.ajax({
            url: $(this).attr("action"),
            type: "POST",
            data: formData,
            success: function (result) {
                if (result.success === false) {
                    setValidationError(result.response, qualId);
                    hideSuccessMessage(qualTitle);
                }
                if (result.success === true) {
                    clearValidationError(qualId);
                    showSuccessMessage(qualTitle);
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

        const errorsArray = [];

        if (error.ShortTitle !== undefined && error.ShortTitle !== null) {
            $(`#tl-autocomplete_${qualId}`).addClass("tl-error");

            const shortTitleError = {};
            shortTitleError.qualId = qualId;
            shortTitleError.fieldName = "SelectShortTitle";
            shortTitleError.error = error.ShortTitle[0];
            errorsArray.push(shortTitleError);
        }

        if (error.Routes !== undefined && error.Routes !== null) {
            $(`#tl-expandable_${qualId}`).addClass("tl-error");

            const routeError = {};
            routeError.qualId = qualId;
            routeError.fieldName = "tl-expandable";
            routeError.error = error.Routes[0];
            errorsArray.push(routeError);
        }

        if (errorsArray.length > 0) {
            $(`#tl-error_${qualId}`).addClass("tl-visible");

            const errors = $.map(errorsArray,
                function (e) {
                    return e.error;
                });

            $(`#tl-error_${qualId}`).html(errors.join("<br/>"));

            populateErrorSummary(errorsArray, qualId);
        }
    }

    function populateErrorSummary(errorsArray, qualId) {
        $(`#tl-error-summary`).removeClass("tl-hidden");

        $(`#tl-error-summary`).append(`
            <div class="govuk-error-summary__body" id="errorSummary_${qualId}">
                <div>
                    <ul class="govuk-list govuk-error-summary__list">
                    </ul>
                <div>
                <br/>
            </div>
        `);

        for (let i = 0; i < errorsArray.length; i++) {
            $(`#errorSummary_${qualId} ul`)
                .append(`<a href=#${errorsArray[i].fieldName}_${errorsArray[i].qualId}>${errorsArray[i].error}</a><br/>`);
        }

    }

    function clearValidationError(qualId) {
        $(`#tl-autocomplete_${qualId}`).removeClass("tl-error");
        $(`#tl-expandable_${qualId}`).removeClass("tl-error");
        $(`#tl-error_${qualId}`).removeClass("tl-visible");
        $(`#errorSummary_${qualId}`).remove();

        if ($('.govuk-error-summary__body').length === 0) {
            $(`#tl-error-summary`).addClass("tl-hidden");
        }
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

    var timeoutId;

    function searchShortTitle(query, populateResults) {
        if (query.trim().length < queryMinLength) return;
        const delayInMs = 400;

        clearTimeout(timeoutId);

        timeoutId = setTimeout(function () {
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
        }, delayInMs);
    }

    function showSuccessMessage(qualTitle) {
        $(".tl-sticky").addClass("tl-sticky--success");
        setTimeout(function () {
            hideSuccessMessage();
        }, 3000);
        $(".tl-sticky--success span").text(`Your changes to ${qualTitle} have been saved`);
    }

    function hideSuccessMessage(qualTitle) {
        $(".tl-sticky").removeClass("tl-sticky--success");
        $(".tl-sticky--success span").text("");
    }
})();