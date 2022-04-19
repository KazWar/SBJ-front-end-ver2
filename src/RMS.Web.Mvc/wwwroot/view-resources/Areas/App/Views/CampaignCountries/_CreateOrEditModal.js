(function ($) {
    app.modals.CreateOrEditCampaignCountryModal = function () {

        var _campaignCountriesService = abp.services.app.campaignCountries;

        var _modalManager;
        var _$campaignCountryInformationForm = null;

		        var _CampaignCountrycampaignLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignCountries/CampaignLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignCountries/_CampaignCountryCampaignLookupTableModal.js',
            modalClass: 'CampaignLookupTableModal'
        });        var _CampaignCountrycountryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignCountries/CountryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignCountries/_CampaignCountryCountryLookupTableModal.js',
            modalClass: 'CountryLookupTableModal'
        });
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$campaignCountryInformationForm = _modalManager.getModal().find('form[name=CampaignCountryInformationsForm]');
            _$campaignCountryInformationForm.validate();
        };

		          $('#OpenCampaignLookupTableButton').click(function () {

            var campaignCountry = _$campaignCountryInformationForm.serializeFormToObject();

            _CampaignCountrycampaignLookupTableModal.open({ id: campaignCountry.campaignId, displayName: campaignCountry.campaignName }, function (data) {
                _$campaignCountryInformationForm.find('input[name=campaignName]').val(data.displayName); 
                _$campaignCountryInformationForm.find('input[name=campaignId]').val(data.id); 
            });
        });
		
		$('#ClearCampaignNameButton').click(function () {
                _$campaignCountryInformationForm.find('input[name=campaignName]').val(''); 
                _$campaignCountryInformationForm.find('input[name=campaignId]').val(''); 
        });
		
        $('#OpenCountryLookupTableButton').click(function () {

            var campaignCountry = _$campaignCountryInformationForm.serializeFormToObject();

            _CampaignCountrycountryLookupTableModal.open({ id: campaignCountry.countryId, displayName: campaignCountry.countryDescription }, function (data) {
                _$campaignCountryInformationForm.find('input[name=countryDescription]').val(data.displayName); 
                _$campaignCountryInformationForm.find('input[name=countryId]').val(data.id); 
            });
        });
		
		$('#ClearCountryDescriptionButton').click(function () {
                _$campaignCountryInformationForm.find('input[name=countryDescription]').val(''); 
                _$campaignCountryInformationForm.find('input[name=countryId]').val(''); 
        });
		


        this.save = function () {
            if (!_$campaignCountryInformationForm.valid()) {
                return;
            }
            if ($('#CampaignCountry_CampaignId').prop('required') && $('#CampaignCountry_CampaignId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Campaign')));
                return;
            }
            if ($('#CampaignCountry_CountryId').prop('required') && $('#CampaignCountry_CountryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Country')));
                return;
            }

            

            var campaignCountry = _$campaignCountryInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _campaignCountriesService.createOrEdit(
				campaignCountry
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCampaignCountryModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);