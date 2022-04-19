(function ($) {
    app.modals.CreateOrEditProductCategoryYearPOModal = function () {

        var _productCategoryYearPOsService = abp.services.app.productCategoryYearPOs;

        var _modalManager;
        var _$productCategoryYearPOInformationForm = null;

		        var _ProductCategoryYearPOproductCategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProductCategoryYearPOs/ProductCategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProductCategoryYearPOs/_ProductCategoryYearPOProductCategoryLookupTableModal.js',
            modalClass: 'ProductCategoryLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$productCategoryYearPOInformationForm = _modalManager.getModal().find('form[name=ProductCategoryYearPOInformationsForm]');
            _$productCategoryYearPOInformationForm.validate();
        };

		          $('#OpenProductCategoryLookupTableButton').click(function () {

            var productCategoryYearPO = _$productCategoryYearPOInformationForm.serializeFormToObject();

            _ProductCategoryYearPOproductCategoryLookupTableModal.open({ id: productCategoryYearPO.productCategoryId, displayName: productCategoryYearPO.productCategoryCode }, function (data) {
                _$productCategoryYearPOInformationForm.find('input[name=productCategoryCode]').val(data.displayName); 
                _$productCategoryYearPOInformationForm.find('input[name=productCategoryId]').val(data.id); 
            });
        });
		
		$('#ClearProductCategoryCodeButton').click(function () {
                _$productCategoryYearPOInformationForm.find('input[name=productCategoryCode]').val(''); 
                _$productCategoryYearPOInformationForm.find('input[name=productCategoryId]').val(''); 
        });
		


        this.save = function () {
            if (!_$productCategoryYearPOInformationForm.valid()) {
                return;
            }
            if ($('#ProductCategoryYearPO_ProductCategoryId').prop('required') && $('#ProductCategoryYearPO_ProductCategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ProductCategory')));
                return;
            }

            var productCategoryYearPO = _$productCategoryYearPOInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _productCategoryYearPOsService.createOrEdit(
				productCategoryYearPO
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditProductCategoryYearPOModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);