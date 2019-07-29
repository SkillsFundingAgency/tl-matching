//Select all checkboxes

$('.tl-selectall').click(function () {
    $('.tl-checkbox').prop('checked', this.checked);
});


$(".tl-checkbox").change(function () {
    if ($('.tl-checkbox:checked').length === $('.tl-checkbox').length) {
        $('.tl-selectall').prop('checked', true);
    }

    else {
        $('.tl-selectall').prop('checked', false);
    }
});

//Select entire table row
$(".tl-table-clickable tbody tr").click(function (e) {
    if (e.target.type == "checkbox" || $(e.target).is('a, a *')) {
        e.stopPropagation();
    }
    else {
        if ($(this).hasClass("checked")) {
            $(this).find("input.tl-checkbox").click();
            $(this).removeClass("checked");
        } else {
            $(this).find("input.tl-checkbox").click();
            $(this).addClass("checked");
        }
    }
})