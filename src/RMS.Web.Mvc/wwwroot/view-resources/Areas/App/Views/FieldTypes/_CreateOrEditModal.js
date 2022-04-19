(function ($) {
    app.modals.CreateOrEditFieldTypeModal = function () {

        var _fieldTypesService = abp.services.app.fieldTypes;

        var _modalManager;
        var _$fieldTypeInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$fieldTypeInformationForm = _modalManager.getModal().find('form[name=FieldTypeInformationsForm]');
            _$fieldTypeInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$fieldTypeInformationForm.valid()) {
                return;
            }

            var fieldType = _$fieldTypeInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _fieldTypesService.createOrEdit(
				fieldType
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditFieldTypeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);