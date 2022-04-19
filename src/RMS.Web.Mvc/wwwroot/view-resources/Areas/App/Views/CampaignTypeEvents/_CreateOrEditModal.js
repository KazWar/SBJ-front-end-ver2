(function ($) {
    app.modals.CreateOrEditCampaignTypeEventModal = function () {

        var _campaignTypeEventsService = abp.services.app.campaignTypeEvents;

        var _modalManager;
        var _$campaignTypeEventInformationForm = null;

		        var _CampaignTypeEventcampaignTypeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignTypeEvents/CampaignTypeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignTypeEvents/_CampaignTypeEventCampaignTypeLookupTableModal.js',
            modalClass: 'CampaignTypeLookupTableModal'
        });        var _CampaignTypeEventprocessEventLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignTypeEvents/ProcessEventLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignTypeEvents/_CampaignTypeEventProcessEventLookupTableModal.js',
            modalClass: 'ProcessEventLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$campaignTypeEventInformationForm = _modalManager.getModal().find('form[name=CampaignTypeEventInformationsForm]');
            _$campaignTypeEventInformationForm.validate();
        };

		          $('#OpenCampaignTypeLookupTableButton').click(function () {

            var campaignTypeEvent = _$campaignTypeEventInformationForm.serializeFormToObject();

            _CampaignTypeEventcampaignTypeLookupTableModal.open({ id: campaignTypeEvent.campaignTypeId, displayName: campaignTypeEvent.campaignTypeName }, function (data) {
                _$campaignTypeEventInformationForm.find('input[name=campaignTypeName]').val(data.displayName); 
                _$campaignTypeEventInformationForm.find('input[name=campaignTypeId]').val(data.id); 
            });
        });
		
		$('#ClearCampaignTypeNameButton').click(function () {
                _$campaignTypeEventInformationForm.find('input[name=campaignTypeName]').val(''); 
                _$campaignTypeEventInformationForm.find('input[name=campaignTypeId]').val(''); 
        });
		
        $('#OpenProcessEventLookupTableButton').click(function () {

            var campaignTypeEvent = _$campaignTypeEventInformationForm.serializeFormToObject();

            _CampaignTypeEventprocessEventLookupTableModal.open({ id: campaignTypeEvent.processEventId, displayName: campaignTypeEvent.processEventName }, function (data) {
                _$campaignTypeEventInformationForm.find('input[name=processEventName]').val(data.displayName); 
                _$campaignTypeEventInformationForm.find('input[name=processEventId]').val(data.id); 
            });
        });
		
		$('#ClearProcessEventNameButton').click(function () {
                _$campaignTypeEventInformationForm.find('input[name=processEventName]').val(''); 
                _$campaignTypeEventInformationForm.find('input[name=processEventId]').val(''); 
        });
		


        this.save = function () {
            if (!_$campaignTypeEventInformationForm.valid()) {
                return;
            }
            if ($('#CampaignTypeEvent_CampaignTypeId').prop('required') && $('#CampaignTypeEvent_CampaignTypeId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('CampaignType')));
                return;
            }
            if ($('#CampaignTypeEvent_ProcessEventId').prop('required') && $('#CampaignTypeEvent_ProcessEventId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ProcessEvent')));
                return;
            }

            var campaignTypeEvent = _$campaignTypeEventInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _campaignTypeEventsService.createOrEdit(
				campaignTypeEvent
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCampaignTypeEventModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);