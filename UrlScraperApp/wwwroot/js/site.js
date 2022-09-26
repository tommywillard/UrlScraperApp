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

    var form = $('#urlForm');
    $(form).submit(function () {
        post(form.attr('action'), form.serialize());
    });
});