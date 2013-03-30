var WidgetsContainer;
(function (WidgetsContainer) {
    $(function () {
        $(".add-widget").on("click", function (e) {
            e.preventDefault();
            var form = $(this).parents("form:first");
            $("input[type='hidden'][name='returnUrl']").val($(this).attr("href"));
            form.submit();
        });
    });
})(WidgetsContainer || (WidgetsContainer = {}));

