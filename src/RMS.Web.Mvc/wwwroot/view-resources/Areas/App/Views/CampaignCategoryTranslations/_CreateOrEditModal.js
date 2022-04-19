(function ($) {
    app.modals.CreateOrEditCampaignCategoryTranslationModal = function () {

        var _campaignCategoryTranslationsService = abp.services.app.campaignCategoryTranslations;

        var _modalManager;
        var _$campaignCategoryTranslationInformationForm = null;

		        var _CampaignCategoryTranslationlocaleLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignCategoryTranslations/LocaleLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignCategoryTranslations/_CampaignCategoryTranslationLocaleLookupTableModal.js',
            modalClass: 'LocaleLookupTableModal'
        });        var _CampaignCategoryTranslationcampaignCategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignCategoryTranslations/CampaignCategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignCategoryTranslations/_CampaignCategoryTranslationCampaignCategoryLookupTableModal.js',
            modalClass: 'CampaignCategoryLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$campaignCategoryTranslationInformationForm = _modalManager.getModal().find('form[name=CampaignCategoryTranslationInformationsForm]');
            _$campaignCategoryTranslationInformationForm.validate();
        };

		          $('#OpenLocaleLookupTableButton').click(function () {

            var campaignCategoryTranslation = _$campaignCategoryTranslationInformationForm.serializeFormToObject();

            _CampaignCategoryTranslationlocaleLookupTableModal.open({ id: campaignCategoryTranslation.localeId, displayName: campaignCategoryTranslation.localeDescription }, function (data) {
                _$campaignCategoryTranslationInformationForm.find('input[name=localeDescription]').val(data.displayName); 
                _$campaignCategoryTranslationInformationForm.find('input[name=localeId]').val(data.id); 
            });
        });
		
		$('#ClearLocaleDescriptionButton').click(function () {
                _$campaignCategoryTranslationInformationForm.find('input[name=localeDescription]').val(''); 
                _$campaignCategoryTranslationInformationForm.find('input[name=localeId]').val(''); 
        });
		
        $('#OpenCampaignCategoryLookupTableButton').click(function () {

            var campaignCategoryTranslation = _$campaignCategoryTranslationInformationForm.serializeFormToObject();

            _CampaignCategoryTranslationcampaignCategoryLookupTableModal.open({ id: campaignCategoryTranslation.campaignCategoryId, displayName: campaignCategoryTranslation.campaignCategoryName }, function (data) {
                _$campaignCategoryTranslationInformationForm.find('input[name=campaignCategoryName]').val(data.displayName); 
                _$campaignCategoryTranslationInformationForm.find('input[name=campaignCategoryId]').val(data.id); 
            });
        });
		
		$('#ClearCampaignCategoryNameButton').click(function () {
                _$campaignCategoryTranslationInformationForm.find('input[name=campaignCategoryName]').val(''); 
                _$campaignCategoryTranslationInformationForm.find('input[name=campaignCategoryId]').val(''); 
        });
		


        this.save = function () {
            if (!_$campaignCategoryTranslationInformationForm.valid()) {
                return;
            }
            if ($('#CampaignCategoryTranslation_LocaleId').prop('required') && $('#CampaignCategoryTranslation_LocaleId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Locale')));
                return;
            }
            if ($('#CampaignCategoryTranslation_CampaignCategoryId').prop('required') && $('#CampaignCategoryTranslation_CampaignCategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('CampaignCategory')));
                return;
            }

            var campaignCategoryTranslation = _$campaignCategoryTranslationInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _campaignCategoryTranslationsService.createOrEdit(
				campaignCategoryTranslation
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCampaignCategoryTranslationModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);