(function () {
    $(function () {

        var _makitaCampaignsService = abp.services.app.makitaCampaigns;
        var _productsService = abp.services.app.products;
        var makitaCampaignId = $('#MakitaCampaignProductsFilter');

        function getMakitaCampaigns() {

            return
        }

        _makitaCampaignsService.isTenantMakita()
            .done(function (result) {
                if (result === false) {
                    window.location = "/App/MakitaCampaigns/OnlyWorksForMakitaTenant"
                }
            });

        console.log("makitaCampaignId", makitaCampaignId.val());

        $('#GetMakitaCampaignsButton').click(function () {

            abp.ui.setBusy();
            _makitaCampaignsService
                .updateMakitaCampaigns()
                .done(function (result) {
                    if (result !== null) {
                        console.log("good result");
                    } else {
                        console.log("bad result");
                    }
                }).always(function () {
                    abp.ui.clearBusy();
                });

        });

        $('#GetMakitaRetailersButton').click(function () {

            abp.ui.setBusy();
            _makitaCampaignsService
                .updateMakitaRetailers()
                .done(function (result) {
                    if (result !== null) {
                        console.log("good result");
                    } else {
                        console.log("bad result");
                    }
                }).always(function () {
                    abp.ui.clearBusy();
                });

        });


        $('#GetMakitaCampaignProductsButton').click(function () {
            //console.log("value campaignProductButton", makitaCampaignId.val());
            abp.ui.setBusy();
            _makitaCampaignsService
                .updateMakitaCampaignProducts()
                .done(function (result) {
                    if (result !== null) {
                        console.log("good result");
                    } else {
                        console.log("bad result");
                    }
                }).always(function () {
                    abp.ui.clearBusy();
                });

        });




    });
})();
