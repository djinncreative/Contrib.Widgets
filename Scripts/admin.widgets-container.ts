/// <reference path="type/jquery.d.ts"/>

module WidgetsContainer {
    $(function () {
        $(".add-widget").on("click", function (e: JQueryEventObject) {
            e.preventDefault();
            var form: JQuery = $(this).parents("form:first");
            $("input[type='hidden'][name='returnUrl']").val($(this).attr("href"));
            form.submit();
        });
    });
}