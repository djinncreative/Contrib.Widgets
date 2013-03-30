var WidgetsContainer;
(function (WidgetsContainer) {
    $(function () {
        $(".add-widget").on("click", function (e) {
            e.preventDefault();
            var hostId = $(this).data("host-id");
            var form = $(this).parents("form:first");
            var fieldset = $(this).parents("fieldset:first");
            var formActionValue = fieldset.find("input[name='submit.Save']");
            var url = $(this).attr("href");
            if(hostId === 0) {
                form.attr("action", url);
            } else {
                formActionValue.val("submit.Save");
                $("input[type='hidden'][name='returnUrl']").val(url);
            }
            form.submit();
        });
    });
})(WidgetsContainer || (WidgetsContainer = {}));

