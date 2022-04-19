(function ($) {
    app.modals.CreateOrEditRegistrationStatusModal = function () {

        var _registrationStatusesService = abp.services.app.registrationStatuses;

        var _modalManager;
        var _$registrationStatusInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$registrationStatusInformationForm = _modalManager.getModal().find('form[name=RegistrationStatusInformationsForm]');
            _$registrationStatusInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$registrationStatusInformationForm.valid()) {
                return;
            }

            var registrationStatus = _$registrationStatusInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _registrationStatusesService.createOrEdit(
				registrationStatus
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditRegistrationStatusModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);