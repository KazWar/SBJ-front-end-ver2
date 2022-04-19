(function ($) {
    $(function () {
        var _$productTable = $('#table__product-selection');
        var _$promoInformationForm = $('form[name="PromoInformationsForm"]');

        var _promoProductsService = abp.services.app.promoProducts;
        var _PromoProductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Promos/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Promos/_PromoProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        var promo = _$promoInformationForm.serializeFormToObject();

        var dataTable = _$productTable.DataTable({
            paging: true,
            processing: true,
            serverSide: true,
            listAction: {
                ajaxFunction: _promoProductsService.getAllProductsForPromo,
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
                                    _promoProductsService.delete({
                                        id: data.record.id
                                    }).done(function () {
                                        dataTable.ajax.reload();
                                    });
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 1,
                    data: 'description',
                    name: 'Description'
                },
                {
                    targets: 2,
                    data: 'ctnCode',
                    name: 'CTN'
                },
                {
                    targets: 3,
                    data: 'eanCode',
                    name: 'EAN'
                }
            ]
        });

        $('#OpenProductLookupTableButton').click(function () {
            var productString = '';
            var data = dataTable.rows().data();
            var productCategoryId = 0;

            Array.from(document.querySelector("#productCategory").options).forEach(function (option_element) {
                if (option_element.selected == true) {
                    productCategoryId = option_element.value;
                }
            });

            var products = [];

            data.each(function (record) {
                var productId = record.productId;

                products.push(productId);
            });

            var productString = String(products);

            _PromoProductLookupTableModal.open({ id: productCategoryId, displayName: productString }, function (data) {
                _promoProductsService.createOrEdit({ promoId: promo.id, productId: data.id })
                    .done(function () {
                        dataTable.ajax.reload();
                    });
            });
        });
    });
})(jQuery);