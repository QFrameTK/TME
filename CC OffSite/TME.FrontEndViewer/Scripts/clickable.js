﻿//$(function () {
//
//    $(".clickable").children("table").click(function () {
//        $(this).parent().children(".clickable").slideToggle();
//    });
//});

$(function () {
    function isAnyVisible() {
        return !!$('.object .content:visible').length
    }

    function updateToggleAll() {
        var symbol = $('.toggle-all .symbol')
        var action = $('.toggle-all .action')

        if (isAnyVisible()) {
            symbol.text('-')
            action.text('Hide')
        } else {
            symbol.text('+')
            action.text('Expand')
        }
    }

    $(document).on('click', '.object .title', function () {
        var object = $(this).parents('.object').eq(0)
        var linkedObject = object.data('linked-item')

        object.children('.content').slideToggle(updateToggleAll)
        if (linkedObject)
            linkedObject.children('.content').slideToggle()
    })

    $(document).on('click', '.toggle-all', function () {
        if (isAnyVisible())
            $('.object .content:visible').hide()
        else
            $('.object .content:hidden').show()

        updateToggleAll()
    })


    updateToggleAll()
})