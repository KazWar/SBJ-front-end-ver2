(function ($) {
    app.modals.CreateOrEditCampaignMessageModal = function () {

        var _campaignMessagesService = abp.services.app.campaignMessages;

        var _modalManager;
        var _$campaignMessageInformationForm = null;

		        var _CampaignMessagecampaignLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignMessages/CampaignLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignMessages/_CampaignMessageCampaignLookupTableModal.js',
            modalClass: 'CampaignLookupTableModal'
        });        var _CampaignMessagemessageLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignMessages/MessageLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignMessages/_CampaignMessageMessageLookupTableModal.js',
            modalClass: 'MessageLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$campaignMessageInformationForm = _modalManager.getModal().find('form[name=CampaignMessageInformationsForm]');
            _$campaignMessageInformationForm.validate();
        };

		          $('#OpenCampaignLookupTableButton').click(function () {

            var campaignMessage = _$campaignMessageInformationForm.serializeFormToObject();

            _CampaignMessagecampaignLookupTableModal.open({ id: campaignMessage.campaignId, displayName: campaignMessage.campaignName }, function (data) {
                _$campaignMessageInformationForm.find('input[name=campaignName]').val(data.displayName); 
                _$campaignMessageInformationForm.find('input[name=campaignId]').val(data.id); 
            });
        });
		
		$('#ClearCampaignNameButton').click(function () {
                _$campaignMessageInformationForm.find('input[name=campaignName]').val(''); 
                _$campaignMessageInformationForm.find('input[name=campaignId]').val(''); 
        });
		
        $('#OpenMessageLookupTableButton').click(function () {

            var campaignMessage = _$campaignMessageInformationForm.serializeFormToObject();

            _CampaignMessagemessageLookupTableModal.open({ id: campaignMessage.messageId, displayName: campaignMessage.messageVersion }, function (data) {
                _$campaignMessageInformationForm.find('input[name=messageVersion]').val(data.displayName); 
                _$campaignMessageInformationForm.find('input[name=messageId]').val(data.id); 
            });
        });
		
		$('#ClearMessageVersionButton').click(function () {
                _$campaignMessageInformationForm.find('input[name=messageVersion]').val(''); 
                _$campaignMessageInformationForm.find('input[name=messageId]').val(''); 
        });
		


        this.save = function () {
            if (!_$campaignMessageInformationForm.valid()) {
                return;
            }
            if ($('#CampaignMessage_CampaignId').prop('required') && $('#CampaignMessage_CampaignId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Campaign')));
                return;
            }
            if ($('#CampaignMessage_MessageId').prop('required') && $('#CampaignMessage_MessageId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Message')));
                return;
            }

            var campaignMessage = _$campaignMessageInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _campaignMessagesService.createOrEdit(
				campaignMessage
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCampaignMessageModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);