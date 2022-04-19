(function ($) {
    app.modals.CreateOrEditSystemLevelModal = function () {

        var _systemLevelsService = abp.services.app.systemLevels;

        var _modalManager;
        var _$systemLevelInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$systemLevelInformationForm = _modalManager.getModal().find('form[name=SystemLevelInformationsForm]');
            _$systemLevelInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$systemLevelInformationForm.valid()) {
                return;
            }

            var systemLevel = _$systemLevelInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _systemLevelsService.createOrEdit(
				systemLevel
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditSystemLevelModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);