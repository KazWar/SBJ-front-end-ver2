(function ($) {
    $(function () {
        var _$retailerTable = $('#table__retailer-selection');
        var _$promoInformationForm = $('form[name="PromoInformationsForm"]');

        var _retailerService = abp.services.app.retailers;
        /*var _retailerLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Retailers/RetailerLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoRetailers/_PromoRetailerRetailerLookupTableModal.js',
            modalClass: 'RetailerLookupTableModal'
        });*/

        var retailer = _$promoInformationForm.serializeFormToObject();

        var dataTable = _$retailerTable.DataTable({
            paging: true,
            processing: true,
            serverSide: true,
            listAction: {
                ajaxFunction: _retailerService.getAllRetailersByPromoId,
                inputFilter: function () {
                    return {
                        promoId: promo.id
                    }
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    data: 'id',
                    name: 'ID'
                },
                {
                    targets: 1,
                    data: 'code',
                    name: 'RetailerCode'
                },
                {
                    targets: 2,
                    data: 'name',
                    name: 'RetailerName'
                }
            ]
        });
    });
})(jQuery);