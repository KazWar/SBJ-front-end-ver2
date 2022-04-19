(function ($) {
    app.modals.CreateOrEditFormBlockFieldModal = function () {

        var _formBlockFieldsService = abp.services.app.formBlockFields;

        var _modalManager;
        var _$formBlockFieldInformationForm = null;

		        var _FormBlockFieldformFieldLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormBlockFields/FormFieldLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormBlockFields/_FormBlockFieldFormFieldLookupTableModal.js',
            modalClass: 'FormFieldLookupTableModal'
        });        var _FormBlockFieldformBlockLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormBlockFields/FormBlockLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormBlockFields/_FormBlockFieldFormBlockLookupTableModal.js',
            modalClass: 'FormBlockLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$formBlockFieldInformationForm = _modalManager.getModal().find('form[name=FormBlockFieldInformationsForm]');
            _$formBlockFieldInformationForm.validate();
        };

		          $('#OpenFormFieldLookupTableButton').click(function () {

            var formBlockField = _$formBlockFieldInformationForm.serializeFormToObject();

            _FormBlockFieldformFieldLookupTableModal.open({ id: formBlockField.formFieldId, displayName: formBlockField.formFieldDescription }, function (data) {
                _$formBlockFieldInformationForm.find('input[name=formFieldDescription]').val(data.displayName); 
                _$formBlockFieldInformationForm.find('input[name=formFieldId]').val(data.id); 
            });
        });
		
		$('#ClearFormFieldDescriptionButton').click(function () {
                _$formBlockFieldInformationForm.find('input[name=formFieldDescription]').val(''); 
                _$formBlockFieldInformationForm.find('input[name=formFieldId]').val(''); 
        });
		
        $('#OpenFormBlockLookupTableButton').click(function () {

            var formBlockField = _$formBlockFieldInformationForm.serializeFormToObject();

            _FormBlockFieldformBlockLookupTableModal.open({ id: formBlockField.formBlockId, displayName: formBlockField.formBlockDescription }, function (data) {
                _$formBlockFieldInformationForm.find('input[name=formBlockDescription]').val(data.displayName); 
                _$formBlockFieldInformationForm.find('input[name=formBlockId]').val(data.id); 
            });
        });
		
		$('#ClearFormBlockDescriptionButton').click(function () {
                _$formBlockFieldInformationForm.find('input[name=formBlockDescription]').val(''); 
                _$formBlockFieldInformationForm.find('input[name=formBlockId]').val(''); 
        });
		


        this.save = function () {
            if (!_$formBlockFieldInformationForm.valid()) {
                return;
            }
            if ($('#FormBlockField_FormFieldId').prop('required') && $('#FormBlockField_FormFieldId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('FormField')));
                return;
            }
            if ($('#FormBlockField_FormBlockId').prop('required') && $('#FormBlockField_FormBlockId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('FormBlock')));
                return;
            }

            var formBlockField = _$formBlockFieldInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _formBlockFieldsService.createOrEdit(
				formBlockField
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditFormBlockFieldModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);