(function ($) {
    app.modals.CreateOrEditCampaignCampaignTypeModal = function () {

        var _campaignCampaignTypesService = abp.services.app.campaignCampaignTypes;

        var _modalManager;
        var _$campaignCampaignTypeInformationForm = null;

		        var _CampaignCampaignTypecampaignLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignCampaignTypes/CampaignLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignCampaignTypes/_CampaignCampaignTypeCampaignLookupTableModal.js',
            modalClass: 'CampaignLookupTableModal'
        });        var _CampaignCampaignTypecampaignTypeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignCampaignTypes/CampaignTypeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignCampaignTypes/_CampaignCampaignTypeCampaignTypeLookupTableModal.js',
            modalClass: 'CampaignTypeLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$campaignCampaignTypeInformationForm = _modalManager.getModal().find('form[name=CampaignCampaignTypeInformationsForm]');
            _$campaignCampaignTypeInformationForm.validate();
        };

		          $('#OpenCampaignLookupTableButton').click(function () {

            var campaignCampaignType = _$campaignCampaignTypeInformationForm.serializeFormToObject();

            _CampaignCampaignTypecampaignLookupTableModal.open({ id: campaignCampaignType.campaignId, displayName: campaignCampaignType.campaignDescription }, function (data) {
                _$campaignCampaignTypeInformationForm.find('input[name=campaignDescription]').val(data.displayName); 
                _$campaignCampaignTypeInformationForm.find('input[name=campaignId]').val(data.id); 
            });
        });
		
		$('#ClearCampaignDescriptionButton').click(function () {
                _$campaignCampaignTypeInformationForm.find('input[name=campaignDescription]').val(''); 
                _$campaignCampaignTypeInformationForm.find('input[name=campaignId]').val(''); 
        });
		
        $('#OpenCampaignTypeLookupTableButton').click(function () {

            var campaignCampaignType = _$campaignCampaignTypeInformationForm.serializeFormToObject();

            _CampaignCampaignTypecampaignTypeLookupTableModal.open({ id: campaignCampaignType.campaignTypeId, displayName: campaignCampaignType.campaignTypeName }, function (data) {
                _$campaignCampaignTypeInformationForm.find('input[name=campaignTypeName]').val(data.displayName); 
                _$campaignCampaignTypeInformationForm.find('input[name=campaignTypeId]').val(data.id); 
            });
        });
		
		$('#ClearCampaignTypeNameButton').click(function () {
                _$campaignCampaignTypeInformationForm.find('input[name=campaignTypeName]').val(''); 
                _$campaignCampaignTypeInformationForm.find('input[name=campaignTypeId]').val(''); 
        });
		


        this.save = function () {
            if (!_$campaignCampaignTypeInformationForm.valid()) {
                return;
            }
            if ($('#CampaignCampaignType_CampaignId').prop('required') && $('#CampaignCampaignType_CampaignId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Campaign')));
                return;
            }
            if ($('#CampaignCampaignType_CampaignTypeId').prop('required') && $('#CampaignCampaignType_CampaignTypeId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('CampaignType')));
                return;
            }

            var campaignCampaignType = _$campaignCampaignTypeInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _campaignCampaignTypesService.createOrEdit(
				campaignCampaignType
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCampaignCampaignTypeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);