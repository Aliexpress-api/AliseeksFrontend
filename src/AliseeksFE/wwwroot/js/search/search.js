$(premptiveSearchCache);

const doneTypingDelay = 200;
var typingTimer;
var ajaxCache = "/search/cache";

function premptiveSearchCache() {
    $('#search-text').on('input', function () {
        clearTimeout(typingTimer);
        typingTimer = window.setTimeout(premptiveSearchDoneTyping, doneTypingDelay);
    });
    $('#searchForm').on('submit', function () {
        clearTimeout(typingTimer);
    });
}

function premptiveSearchDoneTyping() {
    console.log("Triggered done typing event");
    let form = $('#searchForm');
    let endpoint = ajaxCache + '?' + form.serialize();
    console.log("Sending ajax to: " + endpoint);
    $.ajax(endpoint, {
        method: "get",
        success: function () {
            console.log("Successful response on ajax");
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
(function() {
    $('#searchForm').on('submit', function () {
        var selectedVals = $("#searchForm > [data-id='multiselect']").attr('title');
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