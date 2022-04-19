(function ($) {
    $(function () {
        var _$promoCountryTable = $('#table__promo-country-selection');
        var _$promoInformationForm = $('form[name="PromoInformationsForm"]');

        var _promoCountryService = abp.services.app.promoCountries;
        var _promoCountryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Promos/CountryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Promos/_PromoCountryLookupTableModal.js',
            modalClass: 'CountryLookupTableModal'
        });

        var promo = _$promoInformationForm.serializeFormToObject();

        var dataTable = _$promoCountryTable.DataTable({
            paging: true,
            processing: true,
            serverSide: true,
            listAction: {
                ajaxFunction: _promoCountryService.getAllCountriesForPromo,
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
                                action: function (data) { }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () { return true; },
                                action: function (data) {
                                    _promoCountryService.delete({
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
                    data: 'countryCode',
                    name: 'CountryCode'
                },
                {
                    targets: 2,
                    data: 'countryName',
                    name: 'CountryName'
                }
            ]
        });

        $('#OpenCountryLookupTableButton').click(function () {
            var countryString = '';
            var data = dataTable.rows().data();

            data.each(function (record) {
                var countryId = record.countryId;

                if (countryString === '') {
                    countryString = countryId;
                } else {
                    countryString += '|' + countryId;
                }
            });

            _promoCountryLookupTableModal.open({ id: promo.id, displayName: countryString }, function (data) {
                _promoCountryService.createOrEdit({ promoId: promo.id, countryId: data.id })
                    .done(function () {
                        dataTable.ajax.reload();
                    });
            });
        });
    });
})(jQuery);