$(function () {


    $("#SelectCategory").on("change", function () {
        var url = $(this).val();

        if (url) {
            window.location = "/shop/Products?catId=" + url;
        }
        return false;
    });


    $("a.delete").click(function () {
        if (!confirm("Confirm page deletion")) return false;
    });

});