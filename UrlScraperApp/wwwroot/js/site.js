$(document).ready(function () {
    const endpoint = 'https://localhost:7172/api/scraper'

    var form = $('#urlForm');
    $(form).submit(function (e) {
        e.preventDefault();
        $.ajax({
            type: 'GET',
            url: endpoint,
            data: `url=${$('#targetUrl').val()}`,
            dataType: 'json',
            success: function (data) {
                $('#scrapedContent').html(data);
            },
            error: function (xhr, status, error) {
                console.log(error);
            }
        });
    });
});