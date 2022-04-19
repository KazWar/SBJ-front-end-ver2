$(function () {
    var _tenantDashboardService = abp.services.app.tenantDashboard;
    var _widgetBase = app.widgetBase.create();
    var _$Container = $('.HelloWorldContainer');

    var getHelloWorld = function (name) {
        abp.ui.setBusy(_$Container);

        _tenantDashboardService
            .getHelloWorldData({ name: name })
            .done(function (result) {
                _$Container.find(".hello-response").text(result.outPutName);
            }).always(function () {
                abp.ui.clearBusy(_$Container);
            });
    };

    _widgetBase.runDelayed(function () {
        getHelloWorld("First Attempt");
    });

    //event which your filter send
    //abp.event.on('app.dashboardFilters.helloFilter.onNameChange', function (name) {
    //    _widgetBase.runDelayed(function () {
    //        getHelloWorld(name);
    //    });
    //});
});
