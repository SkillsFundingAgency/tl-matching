$('.tl-pane-expandable').click(function () {
    $(this).toggleClass('active');
});

$('.tl-pane-expandable__content').click(function () {
    event.stopPropagation();
});