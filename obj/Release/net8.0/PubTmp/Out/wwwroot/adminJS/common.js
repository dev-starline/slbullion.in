const socketUrl = window.location.hostname+':10001';
const socket = io(socketUrl, {});
socket.on('connect', function () {
    socket.emit('admin', $('.userName').text());
});
socket.on('activeUsers', function (data) {
    $('.activeUsers').text(data);
});
function ajaxPost(url, data) {
    var Response
    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        url: url,
        data: data,
        async: false,
        success: function (response) {
            Response = response;
        },
        error: function (xhr, textStatus, error) {
            console.log("Error: " + error);
            Response = error;
        }
    });
    return Response;
}
function ajaxGet(url) {
    var Response
    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        url: url,
        async: false,
        success: function (response) {
            Response = response;
        },
        error: function (xhr, textStatus, error) {
            console.log("Error: " + error);
            Response = error;
        }
    });
    return Response;
}

$("body").on("click", ".ajax-model", async function () {
    var divElement = $("#modelPopup");
    var url = $(this).data('url');
    var decodeUrl = decodeURIComponent(url);

    $.get(decodeUrl)
        .done(function (data) {
            divElement.html(data);
            var editModal = divElement.find('#editModal');
            if (editModal.length > 0) {
                editModal.modal('show');
            } else {
                console.log('Modal element not found');
            }
        })
        .fail(function (xhr, status, error) {
            console.error(status + " : " + error);
        });
})
$("body").on("click", ".ajax-model-master", async function () {
    var divElement = $("#modelPopup");
    var url = $(this).data('url');
    var decodeUrl = decodeURIComponent(url);
    var id = parseInt($(this).attr("id"));
    $.get(decodeUrl)
        .done(function (data) {
            divElement.html(data);
            var editModal = divElement.find('#editModal');
            if (editModal.length > 0) {
                editModal.modal('show');
                getReferance(id);
            } else {
                console.log('Modal element not found');
            }
        })
        .fail(function (xhr, status, error) {
            console.error(status + " : " + error);
        });
})
function confirmDelete() {
    return confirm('Are you sure you want to delete this item?');
}