$(document).ready(function () {
    const updateMethod = '/Home/UpdateContent'

    $('#urlForm').submit(function (e) {
        var targetUrl = $('#targetUrl').val();

        e.preventDefault();
        $.ajax({
            type: 'POST',
            url: `${updateMethod}?url=${targetUrl}`,
            dataType: 'json',
            success: function (data) {
                $('#scrapedContent').html(data.payload);
            },
            error: function (xhr, error, data) {
                console.log(`${xhr.statusText} - ${xhr.status}`);
            }
        });
    });
});