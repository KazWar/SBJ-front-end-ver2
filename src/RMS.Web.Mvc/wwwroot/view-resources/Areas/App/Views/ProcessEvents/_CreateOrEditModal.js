(function ($) {
    app.modals.CreateOrEditProcessEventModal = function () {

        var _processEventsService = abp.services.app.processEvents;

        var _modalManager;
        var _$processEventInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$processEventInformationForm = _modalManager.getModal().find('form[name=ProcessEventInformationsForm]');
            _$processEventInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$processEventInformationForm.valid()) {
                return;
            }

            var processEvent = _$processEventInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _processEventsService.createOrEdit(
				processEvent
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditProcessEventModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);