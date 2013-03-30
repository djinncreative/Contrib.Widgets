/// <reference path="type/jquery.d.ts"/>

module WidgetsContainer {
    $(function () {
        $(".add-widget").on("click", function (e: JQueryEventObject) {
            e.preventDefault();
            var hostId: number = $(this).data("host-id");
            var form: JQuery = $(this).parents("form:first");
            var fieldset: JQuery = $(this).parents("fieldset:first");
            var formActionValue: JQuery = fieldset.find("input[name='submit.Save']");
            var url: string = $(this).attr("href");

            if(hostId === 0){
                form.attr("action", url);
            }
            else{
                formActionValue.val("submit.Save");
                $("input[type='hidden'][name='returnUrl']").val(url);
            }
            
            form.submit();
        });
    });
}