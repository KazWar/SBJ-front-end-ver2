(function ($) {
    app.modals.CreateOrEditCampaignTypeEventRegistrationStatusModal = function () {

        var _campaignTypeEventRegistrationStatusesService = abp.services.app.campaignTypeEventRegistrationStatuses;

        var _modalManager;
        var _$campaignTypeEventRegistrationStatusInformationForm = null;

		        var _CampaignTypeEventRegistrationStatuscampaignTypeEventLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignTypeEventRegistrationStatuses/CampaignTypeEventLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignTypeEventRegistrationStatuses/_CTERSCampaignTypeEventLookupTableModal.js',
            modalClass: 'CampaignTypeEventLookupTableModal'
        });        var _CampaignTypeEventRegistrationStatusregistrationStatusLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignTypeEventRegistrationStatuses/RegistrationStatusLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignTypeEventRegistrationStatuses/_CTERSRegistrationStatusLookupTableModal.js',
            modalClass: 'RegistrationStatusLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$campaignTypeEventRegistrationStatusInformationForm = _modalManager.getModal().find('form[name=CampaignTypeEventRegistrationStatusInformationsForm]');
            _$campaignTypeEventRegistrationStatusInformationForm.validate();
        };

		          $('#OpenCampaignTypeEventLookupTableButton').click(function () {

            var campaignTypeEventRegistrationStatus = _$campaignTypeEventRegistrationStatusInformationForm.serializeFormToObject();

            _CampaignTypeEventRegistrationStatuscampaignTypeEventLookupTableModal.open({ id: campaignTypeEventRegistrationStatus.campaignTypeEventId, displayName: campaignTypeEventRegistrationStatus.campaignTypeEventSortOrder }, function (data) {
                _$campaignTypeEventRegistrationStatusInformationForm.find('input[name=campaignTypeEventSortOrder]').val(data.displayName); 
                _$campaignTypeEventRegistrationStatusInformationForm.find('input[name=campaignTypeEventId]').val(data.id); 
            });
        });
		
		$('#ClearCampaignTypeEventSortOrderButton').click(function () {
                _$campaignTypeEventRegistrationStatusInformationForm.find('input[name=campaignTypeEventSortOrder]').val(''); 
                _$campaignTypeEventRegistrationStatusInformationForm.find('input[name=campaignTypeEventId]').val(''); 
        });
		
        $('#OpenRegistrationStatusLookupTableButton').click(function () {

            var campaignTypeEventRegistrationStatus = _$campaignTypeEventRegistrationStatusInformationForm.serializeFormToObject();

            _CampaignTypeEventRegistrationStatusregistrationStatusLookupTableModal.open({ id: campaignTypeEventRegistrationStatus.registrationStatusId, displayName: campaignTypeEventRegistrationStatus.registrationStatusDescription }, function (data) {
                _$campaignTypeEventRegistrationStatusInformationForm.find('input[name=registrationStatusDescription]').val(data.displayName); 
                _$campaignTypeEventRegistrationStatusInformationForm.find('input[name=registrationStatusId]').val(data.id); 
            });
        });
		
		$('#ClearRegistrationStatusDescriptionButton').click(function () {
                _$campaignTypeEventRegistrationStatusInformationForm.find('input[name=registrationStatusDescription]').val(''); 
                _$campaignTypeEventRegistrationStatusInformationForm.find('input[name=registrationStatusId]').val(''); 
        });
		


        this.save = function () {
            if (!_$campaignTypeEventRegistrationStatusInformationForm.valid()) {
                return;
            }
            if ($('#CampaignTypeEventRegistrationStatus_CampaignTypeEventId').prop('required') && $('#CampaignTypeEventRegistrationStatus_CampaignTypeEventId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('CampaignTypeEvent')));
                return;
            }
            if ($('#CampaignTypeEventRegistrationStatus_RegistrationStatusId').prop('required') && $('#CampaignTypeEventRegistrationStatus_RegistrationStatusId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('RegistrationStatus')));
                return;
            }

            var campaignTypeEventRegistrationStatus = _$campaignTypeEventRegistrationStatusInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _campaignTypeEventRegistrationStatusesService.createOrEdit(
				campaignTypeEventRegistrationStatus
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCampaignTypeEventRegistrationStatusModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);