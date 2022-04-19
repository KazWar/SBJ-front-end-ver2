$(function () {
    var _tenantDashboardService = abp.services.app.tenantDashboard;
    var _widgetBase = app.widgetBase.create();
    var _$Container = $('.to-send-by-campaign');

    var getRegistrationsByStatus = function (name) {
        abp.ui.setBusy(_$Container);

        $.ajax({
            type: "GET",
            url: "TenantDashboard/GetDashboardTiles",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var allTiles = JSON.parse(response);
                for (let i = 0; i < allTiles.length; i++) {
                    if (allTiles[i].Title == "TO SEND BY CAMPAIGN") {
                        var accessToken = allTiles[i].EmbedToken.token;
                        var embedUrl = allTiles[i].EmbedUrl;
                        var embedTileId = allTiles[i].Id
                        var models = window['powerbi-client'].models;

                        var config = {
                            type: 'tile',
                            tokenType: models.TokenType.Embed,
                            accessToken: accessToken,
                            embedUrl: embedUrl,
                            id: embedTileId,
                            dashboardId: allTiles[i].DashboardId,
                        };

                        var dashboardContainer = document.getElementById('to-send-by-campaign-container');
                        var dashboard = powerbi.embed(dashboardContainer, config);
                    }
                }
            },
            failure: function (response) {
                alert("fail");
            },
            error: function (response) {
                alert("error");
            }
        }).always(function () {
            abp.ui.clearBusy(_$Container);
        });
    };
    _widgetBase.runDelayed(function () {
        getRegistrationsByStatus("First Attempt");
    });
});
