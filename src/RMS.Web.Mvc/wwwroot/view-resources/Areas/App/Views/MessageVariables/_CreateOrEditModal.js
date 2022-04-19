(function ($) {
    app.modals.CreateOrEditMessageVariableModal = function () {

        var _messageVariablesService = abp.services.app.messageVariables;

        var _modalManager;
        var _$messageVariableInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$messageVariableInformationForm = _modalManager.getModal().find('form[name=MessageVariableInformationsForm]');
            _$messageVariableInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$messageVariableInformationForm.valid()) {
                return;
            }

            var messageVariable = _$messageVariableInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _messageVariablesService.createOrEdit(
				messageVariable
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditMessageVariableModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);