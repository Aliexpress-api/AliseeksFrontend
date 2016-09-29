$(function () {
    $('[data-ajax=true]').click(ajaxEvent);
});

function ajaxEvent(event)
{
    var target = event.currentTarget;
    var href = "";
    if($(target).is('a'))
    {
        href = $(target).attr('href');
    }
    else {
        href = $(target).attr('data-ajax-url');
    }

    var method = $(target).attr('data-ajax-method');
    var success = $(target).attr('data-ajax-success');

    $.ajax({
        url: href,
        method: method,
        success: window[success]
    });

    event.preventDefault();
}