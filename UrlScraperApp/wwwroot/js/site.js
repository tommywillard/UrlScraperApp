$(document).ready(function () {
    //Controller method to make api call
    const updateMethod = '/Home/UpdateContent'

    $('#urlForm').submit(function (e) {
        var targetUrl = $('#targetUrl').val();

        e.preventDefault();
        $.ajax({
            type: 'POST',
            url: `${updateMethod}?url=${targetUrl}`,
            dataType: 'json',
            success: function (data) {
                //Updating partial
                $('#scrapedContent').html(data.payload);
            },
            error: function (xhr, error, data) {
                console.log(`${xhr.statusText} - ${xhr.status}`);
            }
        });
    });
});