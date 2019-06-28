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