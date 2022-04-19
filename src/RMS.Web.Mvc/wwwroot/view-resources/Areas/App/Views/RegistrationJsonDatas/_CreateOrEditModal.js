(function ($) {
    app.modals.CreateOrEditRegistrationJsonDataModal = function () {

        var _registrationJsonDatasService = abp.services.app.registrationJsonDatas;

        var _modalManager;
        var _$registrationJsonDataInformationForm = null;

		        var _RegistrationJsonDataregistrationLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RegistrationJsonDatas/RegistrationLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RegistrationJsonDatas/_RegistrationJsonDataRegistrationLookupTableModal.js',
            modalClass: 'RegistrationLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$registrationJsonDataInformationForm = _modalManager.getModal().find('form[name=RegistrationJsonDataInformationsForm]');
            _$registrationJsonDataInformationForm.validate();
        };

		          $('#OpenRegistrationLookupTableButton').click(function () {

            var registrationJsonData = _$registrationJsonDataInformationForm.serializeFormToObject();

            _RegistrationJsonDataregistrationLookupTableModal.open({ id: registrationJsonData.registrationId, displayName: registrationJsonData.registrationFirstName }, function (data) {
                _$registrationJsonDataInformationForm.find('input[name=registrationFirstName]').val(data.displayName); 
                _$registrationJsonDataInformationForm.find('input[name=registrationId]').val(data.id); 
            });
        });
		
		$('#ClearRegistrationFirstNameButton').click(function () {
                _$registrationJsonDataInformationForm.find('input[name=registrationFirstName]').val(''); 
                _$registrationJsonDataInformationForm.find('input[name=registrationId]').val(''); 
        });
		


        this.save = function () {
            if (!_$registrationJsonDataInformationForm.valid()) {
                return;
            }
            if ($('#RegistrationJsonData_RegistrationId').prop('required') && $('#RegistrationJsonData_RegistrationId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Registration')));
                return;
            }

            var registrationJsonData = _$registrationJsonDataInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _registrationJsonDatasService.createOrEdit(
				registrationJsonData
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditRegistrationJsonDataModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);