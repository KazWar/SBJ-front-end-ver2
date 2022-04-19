(function ($) {
    app.modals.CreateOrEditPurchaseRegistrationFormFieldModal = function () {

        var _purchaseRegistrationFormFieldsService = abp.services.app.purchaseRegistrationFormFields;

        var _modalManager;
        var _$purchaseRegistrationFormFieldInformationForm = null;

		        var _PurchaseRegistrationFormFieldformFieldLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PurchaseRegistrationFormFields/FormFieldLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PurchaseRegistrationFormFields/_PurchaseRegistrationFormFieldFormFieldLookupTableModal.js',
            modalClass: 'FormFieldLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$purchaseRegistrationFormFieldInformationForm = _modalManager.getModal().find('form[name=PurchaseRegistrationFormFieldInformationsForm]');
            _$purchaseRegistrationFormFieldInformationForm.validate();
        };

		          $('#OpenFormFieldLookupTableButton').click(function () {

            var purchaseRegistrationFormField = _$purchaseRegistrationFormFieldInformationForm.serializeFormToObject();

            _PurchaseRegistrationFormFieldformFieldLookupTableModal.open({ id: purchaseRegistrationFormField.formFieldId, displayName: purchaseRegistrationFormField.formFieldDescription }, function (data) {
                _$purchaseRegistrationFormFieldInformationForm.find('input[name=formFieldDescription]').val(data.displayName); 
                _$purchaseRegistrationFormFieldInformationForm.find('input[name=formFieldId]').val(data.id); 
            });
        });
		
		$('#ClearFormFieldDescriptionButton').click(function () {
                _$purchaseRegistrationFormFieldInformationForm.find('input[name=formFieldDescription]').val(''); 
                _$purchaseRegistrationFormFieldInformationForm.find('input[name=formFieldId]').val(''); 
        });
		


        this.save = function () {
            if (!_$purchaseRegistrationFormFieldInformationForm.valid()) {
                return;
            }
            if ($('#PurchaseRegistrationFormField_FormFieldId').prop('required') && $('#PurchaseRegistrationFormField_FormFieldId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('FormField')));
                return;
            }

            var purchaseRegistrationFormField = _$purchaseRegistrationFormFieldInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _purchaseRegistrationFormFieldsService.createOrEdit(
				purchaseRegistrationFormField
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPurchaseRegistrationFormFieldModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);