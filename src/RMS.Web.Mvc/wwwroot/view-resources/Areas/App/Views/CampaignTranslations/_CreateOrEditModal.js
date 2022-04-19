(function ($) {
    app.modals.CreateOrEditCampaignTranslationModal = function () {

        var _campaignTranslationsService = abp.services.app.campaignTranslations;

        var _modalManager;
        var _$campaignTranslationInformationForm = null;

		        var _CampaignTranslationcampaignLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignTranslations/CampaignLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignTranslations/_CampaignTranslationCampaignLookupTableModal.js',
            modalClass: 'CampaignLookupTableModal'
        });        var _CampaignTranslationlocaleLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignTranslations/LocaleLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignTranslations/_CampaignTranslationLocaleLookupTableModal.js',
            modalClass: 'LocaleLookupTableModal'
        });
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$campaignTranslationInformationForm = _modalManager.getModal().find('form[name=CampaignTranslationInformationsForm]');
            _$campaignTranslationInformationForm.validate();
        };

		          $('#OpenCampaignLookupTableButton').click(function () {

            var campaignTranslation = _$campaignTranslationInformationForm.serializeFormToObject();

            _CampaignTranslationcampaignLookupTableModal.open({ id: campaignTranslation.campaignId, displayName: campaignTranslation.campaignName }, function (data) {
                _$campaignTranslationInformationForm.find('input[name=campaignName]').val(data.displayName); 
                _$campaignTranslationInformationForm.find('input[name=campaignId]').val(data.id); 
            });
        });
		
		$('#ClearCampaignNameButton').click(function () {
                _$campaignTranslationInformationForm.find('input[name=campaignName]').val(''); 
                _$campaignTranslationInformationForm.find('input[name=campaignId]').val(''); 
        });
		
        $('#OpenLocaleLookupTableButton').click(function () {

            var campaignTranslation = _$campaignTranslationInformationForm.serializeFormToObject();

            _CampaignTranslationlocaleLookupTableModal.open({ id: campaignTranslation.localeId, displayName: campaignTranslation.localeDescription }, function (data) {
                _$campaignTranslationInformationForm.find('input[name=localeDescription]').val(data.displayName); 
                _$campaignTranslationInformationForm.find('input[name=localeId]').val(data.id); 
            });
        });
		
		$('#ClearLocaleDescriptionButton').click(function () {
                _$campaignTranslationInformationForm.find('input[name=localeDescription]').val(''); 
                _$campaignTranslationInformationForm.find('input[name=localeId]').val(''); 
        });
		


        this.save = function () {
            if (!_$campaignTranslationInformationForm.valid()) {
                return;
            }
            if ($('#CampaignTranslation_CampaignId').prop('required') && $('#CampaignTranslation_CampaignId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Campaign')));
                return;
            }
            if ($('#CampaignTranslation_LocaleId').prop('required') && $('#CampaignTranslation_LocaleId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Locale')));
                return;
            }

            

            var campaignTranslation = _$campaignTranslationInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _campaignTranslationsService.createOrEdit(
				campaignTranslation
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCampaignTranslationModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);