$(function () {
    $('[data-ajax=true]').click(ajaxEvent);
    $('[data-ajax-form=true]').submit(ajaxEvent);
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

    var data = [];

    if ($(target).is('form'))
    {
        data = $(target).serialize();
    }

    $.ajax({
        url: href,
        method: method,
        success: window[success],
        data: data
    });

    event.preventDefault();
}