(function ($) {
    $(function () {
        var _$promoRetailerTable = $('#table__promo-retailer-selection');
        var _$promoInformationForm = $('form[name="PromoInformationsForm"]');

        var _promoRetailerService = abp.services.app.promoRetailers;
        var _promoRetailerLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Promos/RetailerLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Promos/_PromoRetailerLookupTableModal.js',
            modalClass: 'RetailerLookupTableModal'
        });

        var promo = _$promoInformationForm.serializeFormToObject();

        var dataTable = _$promoRetailerTable.DataTable({
            paging: true,
            processing: true,
            serverSide: true,
            listAction: {
                ajaxFunction: _promoRetailerService.getAllRetailersForPromo,
                inputFilter: function () {
                    return {
                        promoId: promo.id
                    }
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            // View button needs to be present (even if not visible) or it breaks
                            {
                                text: app.localize('View'),
                                visible: function () { return false; },
                                action: function (data) {}
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () { return true; },
                                action: function (data) {
                                    _promoRetailerService.delete({
                                        id: data.record.id
                                    }).done(function () {
                                        dataTable.ajax.reload();
                                    })
                                }
                            }]
                    }
                },
                {
                    targets: 1,
                    data: 'retailerName',
                    name: 'RetailerName'
                },
                {
                    targets: 2,
                    data: 'retailerCountry',
                    name: 'RetailerCountry'
                },
                {
                    targets: 3,
                    data: 'retailerCode',
                    name: 'RetailerCode'
                }
            ]
        });

        $('#OpenRetailerLookupTableButton').click(function () {
            
            var data = dataTable.rows().data();

            console.log('data click', data);

            var retailers = [];
            data.each(function (record) {
                var retailerId = record.retailerId;

                retailers.push(retailerId);
            });
            var retailerString = String(retailers);

            _promoRetailerLookupTableModal.open({ id: promo.id, displayName: retailerString }, function (data) {
                _promoRetailerService.createOrEdit({ promoId: promo.id, retailerId: data.id })
                    .done(function () {
                        dataTable.ajax.reload();
                    });
            });
        });
    });
})(jQuery);