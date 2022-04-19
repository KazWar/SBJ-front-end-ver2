(function ($) {
    app.modals.CreateOrEditProductModal = function () {

        var _productsService = abp.services.app.products;

        var _modalManager;
        var _$productInformationForm = null;

		        var _ProductproductCategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Products/ProductCategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Products/_ProductProductCategoryLookupTableModal.js',
            modalClass: 'ProductCategoryLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$productInformationForm = _modalManager.getModal().find('form[name=ProductInformationsForm]');
            _$productInformationForm.validate();
        };

		          $('#OpenProductCategoryLookupTableButton').click(function () {

            var product = _$productInformationForm.serializeFormToObject();

            _ProductproductCategoryLookupTableModal.open({ id: product.productCategoryId, displayName: product.productCategoryDescription }, function (data) {
                _$productInformationForm.find('input[name=productCategoryDescription]').val(data.displayName); 
                _$productInformationForm.find('input[name=productCategoryId]').val(data.id); 
            });
        });
		
		$('#ClearProductCategoryDescriptionButton').click(function () {
                _$productInformationForm.find('input[name=productCategoryDescription]').val(''); 
                _$productInformationForm.find('input[name=productCategoryId]').val(''); 
        });
		


        this.save = function () {
            if (!_$productInformationForm.valid()) {
                return;
            }
            if ($('#Product_ProductCategoryId').prop('required') && $('#Product_ProductCategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ProductCategory')));
                return;
            }

            var product = _$productInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _productsService.createOrEdit(
				product
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditProductModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);