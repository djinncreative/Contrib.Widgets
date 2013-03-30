/// <reference path="type/jquery.d.ts"/>

module WidgetsContainer {
    $(function () {
        var widgetPartLayerId = $("#WidgetPart_LayerId");
        var fieldset = widgetPartLayerId.parents("fieldset:first");
        fieldset.hide();
    });
}