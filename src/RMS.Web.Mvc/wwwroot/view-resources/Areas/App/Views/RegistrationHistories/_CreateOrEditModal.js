(function ($) {
    app.modals.CreateOrEditRegistrationHistoryModal = function () {

        var _registrationHistoriesService = abp.services.app.registrationHistories;

        var _modalManager;
        var _$registrationHistoryInformationForm = null;

		        var _RegistrationHistoryregistrationStatusLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RegistrationHistories/RegistrationStatusLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RegistrationHistories/_RegistrationHistoryRegistrationStatusLookupTableModal.js',
            modalClass: 'RegistrationStatusLookupTableModal'
        });        var _RegistrationHistoryregistrationLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RegistrationHistories/RegistrationLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RegistrationHistories/_RegistrationHistoryRegistrationLookupTableModal.js',
            modalClass: 'RegistrationLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$registrationHistoryInformationForm = _modalManager.getModal().find('form[name=RegistrationHistoryInformationsForm]');
            _$registrationHistoryInformationForm.validate();
        };

		          $('#OpenRegistrationStatusLookupTableButton').click(function () {

            var registrationHistory = _$registrationHistoryInformationForm.serializeFormToObject();

            _RegistrationHistoryregistrationStatusLookupTableModal.open({ id: registrationHistory.registrationStatusId, displayName: registrationHistory.registrationStatusStatusCode }, function (data) {
                _$registrationHistoryInformationForm.find('input[name=registrationStatusStatusCode]').val(data.displayName); 
                _$registrationHistoryInformationForm.find('input[name=registrationStatusId]').val(data.id); 
            });
        });
		
		$('#ClearRegistrationStatusStatusCodeButton').click(function () {
                _$registrationHistoryInformationForm.find('input[name=registrationStatusStatusCode]').val(''); 
                _$registrationHistoryInformationForm.find('input[name=registrationStatusId]').val(''); 
        });
		
        $('#OpenRegistrationLookupTableButton').click(function () {

            var registrationHistory = _$registrationHistoryInformationForm.serializeFormToObject();

            _RegistrationHistoryregistrationLookupTableModal.open({ id: registrationHistory.registrationId, displayName: registrationHistory.registrationFirstName }, function (data) {
                _$registrationHistoryInformationForm.find('input[name=registrationFirstName]').val(data.displayName); 
                _$registrationHistoryInformationForm.find('input[name=registrationId]').val(data.id); 
            });
        });
		
		$('#ClearRegistrationFirstNameButton').click(function () {
                _$registrationHistoryInformationForm.find('input[name=registrationFirstName]').val(''); 
                _$registrationHistoryInformationForm.find('input[name=registrationId]').val(''); 
        });
		


        this.save = function () {
            if (!_$registrationHistoryInformationForm.valid()) {
                return;
            }
            if ($('#RegistrationHistory_RegistrationStatusId').prop('required') && $('#RegistrationHistory_RegistrationStatusId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('RegistrationStatus')));
                return;
            }
            if ($('#RegistrationHistory_RegistrationId').prop('required') && $('#RegistrationHistory_RegistrationId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Registration')));
                return;
            }

            var registrationHistory = _$registrationHistoryInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _registrationHistoriesService.createOrEdit(
				registrationHistory
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditRegistrationHistoryModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);