function deleteSavedSearch(node) {
    var me = this;

    var target = node.parentNode;
    $(target).remove();
}
$(premptiveSearchCache);

const doneTypingDelay = 200;
var timesCached = 0;
var typingTimer;
var ajaxCache = "/search/cache";

//Search Caching
function premptiveSearchCache() {
    $('#search-text').on('input', function (event) {
        clearTimeout(typingTimer);
        typingTimer = window.setTimeout(premptiveSearchDoneTyping, doneTypingDelay);
    });
    $('#searchForm').on('submit', function () {
        clearTimeout(typingTimer);
    });
}

function premptiveSearchDoneTyping(event) {
    //Don't cache if we have already done it couple of times
    if (timesCached > 2) { return; }
    var form = $('#searchForm');
    var endpoint = ajaxCache + '?' + form.serialize();
    $.ajax(endpoint, {
        method: "get",
        success: function () {
            //Success response
        }
    });
}

//Advanced Tools Bar - Toggle
$(function () {
    $('#filter-bar-toggle').on('click', function () {
        var content = $("#filter-bar");
        content.slideToggle(300, function () {
            var caret = $('#filter-bar-toggle i');

            if ($(caret).hasClass('fa-chevron-up')) {
                $(caret).removeClass('fa-chevron-up');
                $(caret).addClass('fa-chevron-down');
            }
            else {
                $(caret).removeClass('fa-chevron-down');
                $(caret).addClass('fa-chevron-up');
            }
        });
    });
});

//Advanced Tools - Mobile Toggle
$(function () {
    $('#advanced-tools-mobile-toggle').on('click', function () {
        var content = $("#advanced-tools-mobile");
        content.slideToggle(300, function () {
            var caret = $('#advanced-tools-mobile-toggle i');

            if ($(caret).hasClass('fa-chevron-up')) {
                $(caret).removeClass('fa-chevron-up');
                $(caret).addClass('fa-chevron-down');
            }
            else {
                $(caret).removeClass('fa-chevron-down');
                $(caret).addClass('fa-chevron-up');
            }
        });
    });
});

//Prior to Form Submit
$(function() {
    $('[id="searchForm"]').on('submit', function (event) {
        var form = event.currentTarget;
        var selectedVals = $("#searchForm > [data-id='multiselect']").attr('title');
        var checkboxes = $("[type='checkbox']", form);
        for(var i = 0; i != checkboxes.length; i++)
        {
            var checkbox = checkboxes[i];
            if(!$(checkbox).is(':checked'))
            {
                $('[name="' + $(checkbox).attr('name') + '"]', form).prop('disabled', true);
            }
            else
            {
                $('[name="' + $(checkbox).attr('name') + '"]', form).prop('disabled', true);
                $(checkbox).prop('disabled', false);
            }
        }
    });
});

//Dropdown box
$(function () {
    $('.dropdown-container').on('click', function (event) {
        $('.dropdown-content', event.currentTarget).show();
        $(event.currentTarget).addClass('clicked');
    });

    $('.downdown-item').on('click', function (event) {
        $('.glyphicon-check', event.currentTarget).show();
    });

    $(window).on('click', function (event) {
        if($(event.target).parents('.dropdown-container').length === 0)
        {
            $('.dropdown-container').removeClass('clicked');
            $('.dropdown-content').hide();
        }
    });
});

//Init All Tooltips
$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

//Quickscroll
$(".quick-scroll").click(function (target) {
    if ($(target.currentTarget).css('opacity') == 0) { return; }
    $("html,body").animate({
        scrollTop: document.body.scrollHeight
    }, 1000);
});

$(".quick-scroll-mobile").click(function (target) {
    if ($(target.currentTarget).css('opacity') == 0) { return; }
    $("html,body").animate({
        scrollTop: document.body.scrollHeight
    }, 1000);
});

$(window).scroll(function () {

    if ($(this).scrollTop() >= 150) {
        var quickScroll = $('.quick-scroll');
        var quickScrollMobile = $('.quick-scroll-mobile');
        if ($(quickScroll).css('display') != 'none' && $(quickScroll).css('opacity') != '1')
        {
            $(quickScroll).css('opacity', '1');
            $(quickScroll).css('z-index', '9999');

        }
        if ($(quickScrollMobile).css('display') != 'none' && $(quickScrollMobile).css('opacity') != '1') {
            $(quickScrollMobile).css('z-index', '9999');
            $(quickScrollMobile).css('opacity', '1');
        }
    }
    else {
        var quickScroll = $('.quick-scroll');
        var quickScrollMobile = $('.quick-scroll-mobile');
        quickScroll.css('opacity', '0');
        quickScrollMobile.css('opacity', '0');
        quickScroll.css('z-index', '-1');
        quickScrollMobile.css('z-index', '-1');
    }
});
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