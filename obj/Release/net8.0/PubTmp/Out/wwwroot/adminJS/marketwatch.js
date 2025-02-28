﻿socket.on('connect', function () {
    socket.emit('client', $('.userName').text());
});
socket.on('mainProducts', function (data) {
    var html = '';
    var arrayProduct = JSON.parse(pako.inflate(data, { to: 'string' }));
    for (var i = 0; i < arrayProduct.length; i++) {
        html += '<tr>' +
            '    <td>' +
            '        <div class="content">' +
            '            <p>' + arrayProduct[i].name +'</p>' +
            '        </div>' +
            '    </td>' +
            '    <td>' +
            '        <div class="content">' +
            '            <p>' + arrayProduct[i].bid +'</p>' +
            '        </div>' +
            '    </td>' +
            '    <td>' +
            '        <div class="content">' +
            '            <p>' + arrayProduct[i].ask +'</p>' +
            '        </div>' +
            '    </td>' +
            '    <td>' +
            '        <div class="content">' +
            '            <p>' + arrayProduct[i].high +'</p>' +
            '        </div>' +
            '    </td>' +
            '    <td>' +
            '        <div class="content">' +
            '            <p>' + arrayProduct[i].low +'</p>' +
            '        </div>' +
            '    </td>' +
            '    <td class="wfive">' +
            '        <div class="content">' +
            '            <p>' + arrayProduct[i].time +'</p>' +
            '        </div>' +
            '    </td>' +
            '</tr>';
    }
    $('.mainProduct').html(html);
});