(function ($) {
    app.modals.CreateOrEditCampaignTypeModal = function () {

        var _campaignTypesService = abp.services.app.campaignTypes;

        var _modalManager;
        var _$campaignTypeInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$campaignTypeInformationForm = _modalManager.getModal().find('form[name=CampaignTypeInformationsForm]');
            _$campaignTypeInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$campaignTypeInformationForm.valid()) {
                return;
            }

            var campaignType = _$campaignTypeInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _campaignTypesService.createOrEdit(
				campaignType
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCampaignTypeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);