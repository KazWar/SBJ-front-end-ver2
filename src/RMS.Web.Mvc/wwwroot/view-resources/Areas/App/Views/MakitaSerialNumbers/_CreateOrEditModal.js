(function ($) {
    app.modals.CreateOrEditMakitaSerialNumberModal = function () {

        var _makitaSerialNumbersService = abp.services.app.makitaSerialNumbers;

        var _modalManager;
        var _$makitaSerialNumberInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$makitaSerialNumberInformationForm = _modalManager.getModal().find('form[name=MakitaSerialNumberInformationsForm]');
            _$makitaSerialNumberInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$makitaSerialNumberInformationForm.valid()) {
                return;
            }

            var makitaSerialNumber = _$makitaSerialNumberInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _makitaSerialNumbersService.createOrEdit(
				makitaSerialNumber
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditMakitaSerialNumberModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);