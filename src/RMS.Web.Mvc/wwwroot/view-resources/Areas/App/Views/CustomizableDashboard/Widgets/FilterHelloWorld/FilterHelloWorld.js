$(function () {
    var _$container = $(".filter-hello-world-container");
    var _$btn = _$container.find("button[name='btn-filter-hello']");
    var _$input = _$container.find("input[name='input-filter-hello']");

    _$btn.click(function () {
        abp.event.trigger('app.dashboardFilters.helloFilter.onNameChange', _$input.val());
    });
});
