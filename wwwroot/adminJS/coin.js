getBankCalculation();
function getBankCalculation() {
    var data = ajaxGet('Coin/getBankCalculation');
    if (data.bank.length > 0) {
        var response = data.bank;
        $('#bankId').val(response[0].id);
        $('#premiumGold').val(response[0].premiumGold);
        $('#premiumSilver').val(response[0].premiumSilver);
        $('#spotTypeGold').val(response[0].spotTypeGold);
        $('#spotTypeSilver').val(response[0].spotTypeSilver);
        $('#interBankGold').val(response[0].interBankGold);
        $('#interBankSilver').val(response[0].interBankSilver);
        $('#conversionGold').val(response[0].conversionGold);
        $('#conversionSilver').val(response[0].conversionSilver);
        $('#customDutyGold').val(response[0].customDutyGold);
        $('#customDutySilver').val(response[0].customDutySilver);
        $('#marginGold').val(response[0].marginGold);
        $('#marginSilver').val(response[0].marginSilver);
        $('#gstGold').val(response[0].gstGold);
        $('#gstSilver').val(response[0].gstSilver);
        $('#divisionGold').val(response[0].divisionGold);
        $('#divisionSilver').val(response[0].divisionSilver);
        $('#multiplyGold').val(response[0].multiplyGold);
        $('#multiplySilver').val(response[0].multiplySilver);
    }
    if (data.contact.length > 0) {
        $('input[value="' + data.contact[0].isRate + '"]').prop("checked", true);
        $('input[value="' + data.contact[0].isRateType + '"]').prop("checked", true);
    }
}
$("body").on("click", ".updatePremium", async function () {
    let data = new Object();
    data.id = $(this)[0].id;
    data.isView = $(this).closest('tr').find(".isView").prop("checked");
    data.isStock = $(this).closest('tr').find(".isStock").prop("checked");
    data.name = $(this).closest('tr').find(".name").val();
    data.buyPremium = $(this).closest('tr').find(".buyPremium").val();
    data.sellPremium = $(this).closest('tr').find(".sellPremium").val();
    data.division = $(this).closest('tr').find(".division").val();
    data.multiply = $(this).closest('tr').find(".multiply").val();
    var response = ajaxPost('Coin/updatePremium', JSON.stringify(data)); 
    console.log(response);
});

$("body").on("click", ".saveAll", function () {
    var array = [];
    $(".symbolList tr").each(async function () {
        var data = new Object();
        data.id = $(this).closest('tr').find(".updatePremium").attr('id');
        data.isView = $(this).closest('tr').find(".isView").prop("checked");
        data.isStock = $(this).closest('tr').find(".isStock").prop("checked");
        data.name = $(this).closest('tr').find(".name").val();
        data.buyPremium = $(this).closest('tr').find(".buyPremium").val();
        data.sellPremium = $(this).closest('tr').find(".sellPremium").val();
        data.division = $(this).closest('tr').find(".division").val();
        data.multiply = $(this).closest('tr').find(".multiply").val();
        array.push(data);
    })
    var response = ajaxPost('Coin/saveAll', JSON.stringify(array)); 
});
function changeRateType(type) {
    let data = new Object();
    data.rateType = type;
    var response = ajaxPost('Coin/changeRateType', JSON.stringify(data));
    if (response==200) {
    }
}
$(".isRate").on("change", function () {
    let data = new Object();
    data.isRate = $(this).val();
    var response = ajaxPost('Coin/isRateUpdate', JSON.stringify(data));
    if (response == 200) {
    }
});
function setCommonPremium() {
    let data = new Object();
    data.goldBuyCommonPremium = $('.goldBuyCommonPremium').val();
    data.goldSellCommonPremium = $('.goldSellCommonPremium').val();
    data.silverBuyCommonPremium = $('.silverBuyCommonPremium').val();
    data.silverSellCommonPremium = $('.silverSellCommonPremium').val();
    var response = ajaxPost('Coin/setCommonPremium', JSON.stringify(data));
    if (response == 200) {
    }
}
$(function () {
    $("#sortable-table tbody").sortable({
        update: function (event, ui) {
            var sortedIds = $("#sortable-table tbody .updatePremium").map(function () {
                return this.id;
            }).get();
            var response = ajaxPost('Coin/updateSequance', JSON.stringify(sortedIds));
            if (response == 200) {
            }

        }
    });
    $("#sortable-table tbody").disableSelection();
});