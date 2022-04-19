(function ($) {
    app.modals.CreateOrEditMessageComponentTypeModal = function () {

        var _messageComponentTypesService = abp.services.app.messageComponentTypes;

        var _modalManager;
        var _$messageComponentTypeInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$messageComponentTypeInformationForm = _modalManager.getModal().find('form[name=MessageComponentTypeInformationsForm]');
            _$messageComponentTypeInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$messageComponentTypeInformationForm.valid()) {
                return;
            }

            var messageComponentType = _$messageComponentTypeInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _messageComponentTypesService.createOrEdit(
				messageComponentType
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditMessageComponentTypeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);