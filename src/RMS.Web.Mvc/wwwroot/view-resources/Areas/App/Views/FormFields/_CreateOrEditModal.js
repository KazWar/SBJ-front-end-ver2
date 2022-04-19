(function ($) {
    app.modals.CreateOrEditFormFieldModal = function () {

        var _formFieldsService = abp.services.app.formFields;

        var _modalManager;
        var _$formFieldInformationForm = null;

		        var _FormFieldfieldTypeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormFields/FieldTypeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormFields/_FormFieldFieldTypeLookupTableModal.js',
            modalClass: 'FieldTypeLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$formFieldInformationForm = _modalManager.getModal().find('form[name=FormFieldInformationsForm]');
            _$formFieldInformationForm.validate();
        };

		          $('#OpenFieldTypeLookupTableButton').click(function () {

            var formField = _$formFieldInformationForm.serializeFormToObject();

            _FormFieldfieldTypeLookupTableModal.open({ id: formField.fieldTypeId, displayName: formField.fieldTypeDescription }, function (data) {
                _$formFieldInformationForm.find('input[name=fieldTypeDescription]').val(data.displayName); 
                _$formFieldInformationForm.find('input[name=fieldTypeId]').val(data.id); 
            });
        });
		
		$('#ClearFieldTypeDescriptionButton').click(function () {
                _$formFieldInformationForm.find('input[name=fieldTypeDescription]').val(''); 
                _$formFieldInformationForm.find('input[name=fieldTypeId]').val(''); 
        });
		


        this.save = function () {
            if (!_$formFieldInformationForm.valid()) {
                return;
            }
            if ($('#FormField_FieldTypeId').prop('required') && $('#FormField_FieldTypeId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('FieldType')));
                return;
            }

            var formField = _$formFieldInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _formFieldsService.createOrEdit(
				formField
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditFormFieldModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);