getRateDifferance();
$("body").on("click", ".updateReferance", async function () {
    let data = new Object();
    data.id = parseInt($(this).attr("id"));
    data.name = $(this).closest('tr').find(".name").val();
    data.isView = $(this).closest('tr').find(".isView").is(':checked');
    var response = ajaxPost('Setting/updateReferance', JSON.stringify(data));
    console.log(response);
});
function getRateDifferance() {
    var data = ajaxGet('Setting/getRateDifferance');
    if (data.length > 0) {
        $('.gold').val(data[0].goldDifferance);
        $('.silver').val(data[0].silverDifferance);
    }
}
$("body").on("click", ".updateRateDifferance", async function () {
    let data = new Object();
    data.gold = $(".gold").val();
    data.silver = $(".silver").val();
    var response = ajaxPost('Setting/updateRateDifferance', JSON.stringify(data));
    console.log(response);
});
$("body").on("click", ".updatePassword", async function () {
    let data = new Object();
    data.oldPassword = $(".oldPassword").val();
    data.newPassword = $(".newPassword").val();
    data.confirmPassword = $(".confirmPassword").val();
    var response = ajaxPost('Setting/updatePassword', JSON.stringify(data));
    console.log(response);
});