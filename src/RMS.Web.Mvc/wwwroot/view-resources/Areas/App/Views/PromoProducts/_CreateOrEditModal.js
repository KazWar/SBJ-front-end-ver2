(function ($) {
    app.modals.CreateOrEditPromoProductModal = function () {

        var _promoProductsService = abp.services.app.promoProducts;

        var _modalManager;
        var _$promoProductInformationForm = null;

        var _PromoProductpromoLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoProducts/PromoLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoProducts/_PromoProductPromoLookupTableModal.js',
            modalClass: 'PromoLookupTableModal'
        });

        var _PromoProductproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoProducts/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoProducts/_PromoProductProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$promoProductInformationForm = _modalManager.getModal().find('form[name=PromoProductInformationsForm]');
            _$promoProductInformationForm.validate();
        };

        $('#OpenPromoLookupTableButton').click(function () {

            var promoProduct = _$promoProductInformationForm.serializeFormToObject();

            _PromoProductpromoLookupTableModal.open({ id: promoProduct.promoId, displayName: promoProduct.promoPromocode }, function (data) {
                _$promoProductInformationForm.find('input[name=promoPromocode]').val(data.displayName);
                _$promoProductInformationForm.find('input[name=promoId]').val(data.id);
            });
        });

        $('#ClearPromoPromocodeButton').click(function () {
            _$promoProductInformationForm.find('input[name=promoPromocode]').val('');
            _$promoProductInformationForm.find('input[name=promoId]').val('');
        });

        $('#OpenProductLookupTableButton').click(function () {

            var promoProduct = _$promoProductInformationForm.serializeFormToObject();

            _PromoProductproductLookupTableModal.open({ id: promoProduct.productId, displayName: promoProduct.productCtn }, function (data) {
                _$promoProductInformationForm.find('input[name=productCtn]').val(data.displayName);
                _$promoProductInformationForm.find('input[name=productId]').val(data.id);
            });
        });

        $('#ClearProductCtnButton').click(function () {
            _$promoProductInformationForm.find('input[name=productCtn]').val('');
            _$promoProductInformationForm.find('input[name=productId]').val('');
        });



        this.save = function () {
            if (!_$promoProductInformationForm.valid()) {
                return;
            }
            if ($('#PromoProduct_PromoId').prop('required') && $('#PromoProduct_PromoId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Promo')));
                return;
            }
            if ($('#PromoProduct_ProductId').prop('required') && $('#PromoProduct_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }

            var promoProduct = _$promoProductInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _promoProductsService.createOrEdit(
                promoProduct
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditPromoProductModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);