/// <reference path="type/jquery.d.ts"/>
/// <reference path="type/jqueryui.d.ts"/>

module WidgetsContainer {
    $(function () {

        // Handle Add Widget button.
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

        var updateWidgetPlacementField = function () {
            var widgetPlacementField: JQuery = $("input[name='widgetPlacement']");
            var data = {
                zones: {}
            };
            $("div.widgets ul.widgets").each(function(){
                var zone: string = $(this).data("zone");
                
                data.zones[zone] = {
                    widgets: []
                };

                $(this).find("li").each(function(){
                    var widgetId: number = $(this).data("widget-id");
                    data.zones[zone].widgets.push(widgetId);
                });
            });

            var text: string = JSON.stringify(data);
            widgetPlacementField.val(text);
        };

        // Initialize sortable widgets.
        $("div.widgets ul.widgets").sortable({
            connectWith: "div.widgets ul.widgets",
            dropOnEmpty: true,
            placeholder: "sortable-placeholder",
            receive: function(e, ui){
                updateWidgetPlacementField();
            }
        });
    });
}