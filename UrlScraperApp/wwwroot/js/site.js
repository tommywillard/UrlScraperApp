// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
$(document).ready(function () {
    const endpoint = 'https://localhost:7172/api/scraper'

    var post = function (submitData) {
        $.ajax({
            type: 'GET',
            url: endpoint,
            data: submitData,
            dataType: 'json',
            success: function (data) {
                $('#scrapedContent').html(data);
            },
            error: function (xhr, status, error) {
                console.log(error);
            }
        });
    }

    //function getUrlData() {
    //    $.ajax({
    //        type: 'POST',
    //        //url: endpoint + "?url=" + targetUrl,
    //        url: endpoint,
    //        dataType: 'json',
    //        contentType: dataType,
    //        data: data,
    //        success: function (result) {
    //            console.log('Data received: ');
    //            console.log(result);
    //        },
    //        error: function (xhr, textstatus, error) {
    //            if (textstatus === 'timeout') {
    //                console.log('Request timed out');
    //            }
    //            console.log(textstatus);
    //        }
    //    })
    //}

    //function formSubmit() {
    //    $("form").submit(function () {
    //        var targetUrl = $("#targetUrl").val();

    //        getUrlData();
    //    });
    //}

    var form = $('#urlForm');
    $(form).submit(function () {
        post(form.attr('action'), form.serialize());
    });

    //formSubmit();
});