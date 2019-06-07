$('.tl-expandable--header').click(function (e) {
    e.preventDefault();
    $(this).parent().toggleClass('active');
});

$('.tl-editquals-item').submit(function (event) {
    event.preventDefault();
    alert('form submitted');
});