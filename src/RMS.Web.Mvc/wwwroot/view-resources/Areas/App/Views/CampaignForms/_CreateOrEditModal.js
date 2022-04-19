(function ($) {
    app.modals.CreateOrEditCampaignFormModal = function () {

        var _campaignFormsService = abp.services.app.campaignForms;

        var _modalManager;
        var _$campaignFormInformationForm = null;

		        var _CampaignFormcampaignLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignForms/CampaignLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignForms/_CampaignFormCampaignLookupTableModal.js',
            modalClass: 'CampaignLookupTableModal'
                });
        var _CampaignFormformLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignForms/FormLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignForms/_CampaignFormFormLookupTableModal.js',
            modalClass: 'FormLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$campaignFormInformationForm = _modalManager.getModal().find('form[name=CampaignFormInformationsForm]');
            _$campaignFormInformationForm.validate();
        };

		          $('#OpenCampaignLookupTableButton').click(function () {

            var campaignForm = _$campaignFormInformationForm.serializeFormToObject();

            _CampaignFormcampaignLookupTableModal.open({ id: campaignForm.campaignId, displayName: campaignForm.campaignName }, function (data) {
                _$campaignFormInformationForm.find('input[name=campaignName]').val(data.displayName); 
                _$campaignFormInformationForm.find('input[name=campaignId]').val(data.id); 
            });
        });
		
		$('#ClearCampaignNameButton').click(function () {
                _$campaignFormInformationForm.find('input[name=campaignName]').val(''); 
                _$campaignFormInformationForm.find('input[name=campaignId]').val(''); 
        });
		
        $('#OpenFormLookupTableButton').click(function () {

            var campaignForm = _$campaignFormInformationForm.serializeFormToObject();

            _CampaignFormformLookupTableModal.open({ id: campaignForm.formId, displayName: campaignForm.formVersion }, function (data) {
                _$campaignFormInformationForm.find('input[name=formVersion]').val(data.displayName); 
                _$campaignFormInformationForm.find('input[name=formId]').val(data.id); 
            });
        });
		
		$('#ClearFormVersionButton').click(function () {
                _$campaignFormInformationForm.find('input[name=formVersion]').val(''); 
                _$campaignFormInformationForm.find('input[name=formId]').val(''); 
        });
		


        this.save = function () {
            if (!_$campaignFormInformationForm.valid()) {
                return;
            }
            if ($('#CampaignForm_CampaignId').prop('required') && $('#CampaignForm_CampaignId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Campaign')));
                return;
            }
            if ($('#CampaignForm_FormId').prop('required') && $('#CampaignForm_FormId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Form')));
                return;
            }

            var campaignForm = _$campaignFormInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _campaignFormsService.createOrEdit(
				campaignForm
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCampaignFormModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);