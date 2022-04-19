(function ($) {
    app.modals.CreateOrEditCampaignCategoryModal = function () {

        var _campaignCategoriesService = abp.services.app.campaignCategories;

        var _modalManager;
        var _$campaignCategoryInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$campaignCategoryInformationForm = _modalManager.getModal().find('form[name=CampaignCategoryInformationsForm]');
            _$campaignCategoryInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$campaignCategoryInformationForm.valid()) {
                return;
            }

            var campaignCategory = _$campaignCategoryInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _campaignCategoriesService.createOrEdit(
				campaignCategory
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCampaignCategoryModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);