(function ($) {
    app.modals.CreateOrEditProductCategoryModal = function () {

        var _productCategoriesService = abp.services.app.productCategories;

        var _modalManager;
        var _$productCategoryInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$productCategoryInformationForm = _modalManager.getModal().find('form[name=ProductCategoryInformationsForm]');
            _$productCategoryInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$productCategoryInformationForm.valid()) {
                return;
            }

            var productCategory = _$productCategoryInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _productCategoriesService.createOrEdit(
				productCategory
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditProductCategoryModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);