(function ($) {
    app.modals.CreateOrEditFormFieldTranslationModal = function () {

        var _formFieldTranslationsService = abp.services.app.formFieldTranslations;

        var _modalManager;
        var _$formFieldTranslationInformationForm = null;

		        var _FormFieldTranslationformFieldLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormFieldTranslations/FormFieldLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormFieldTranslations/_FormFieldTranslationFormFieldLookupTableModal.js',
            modalClass: 'FormFieldLookupTableModal'
        });        var _FormFieldTranslationlocaleLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormFieldTranslations/LocaleLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormFieldTranslations/_FormFieldTranslationLocaleLookupTableModal.js',
            modalClass: 'LocaleLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$formFieldTranslationInformationForm = _modalManager.getModal().find('form[name=FormFieldTranslationInformationsForm]');
            _$formFieldTranslationInformationForm.validate();
        };

		          $('#OpenFormFieldLookupTableButton').click(function () {

            var formFieldTranslation = _$formFieldTranslationInformationForm.serializeFormToObject();

            _FormFieldTranslationformFieldLookupTableModal.open({ id: formFieldTranslation.formFieldId, displayName: formFieldTranslation.formFieldDescription }, function (data) {
                _$formFieldTranslationInformationForm.find('input[name=formFieldDescription]').val(data.displayName); 
                _$formFieldTranslationInformationForm.find('input[name=formFieldId]').val(data.id); 
            });
        });
		
		$('#ClearFormFieldDescriptionButton').click(function () {
                _$formFieldTranslationInformationForm.find('input[name=formFieldDescription]').val(''); 
                _$formFieldTranslationInformationForm.find('input[name=formFieldId]').val(''); 
        });
		
        $('#OpenLocaleLookupTableButton').click(function () {

            var formFieldTranslation = _$formFieldTranslationInformationForm.serializeFormToObject();

            _FormFieldTranslationlocaleLookupTableModal.open({ id: formFieldTranslation.localeId, displayName: formFieldTranslation.localeLanguageCode }, function (data) {
                _$formFieldTranslationInformationForm.find('input[name=localeLanguageCode]').val(data.displayName); 
                _$formFieldTranslationInformationForm.find('input[name=localeId]').val(data.id); 
            });
        });
		
		$('#ClearLocaleLanguageCodeButton').click(function () {
                _$formFieldTranslationInformationForm.find('input[name=localeLanguageCode]').val(''); 
                _$formFieldTranslationInformationForm.find('input[name=localeId]').val(''); 
        });
		


        this.save = function () {
            if (!_$formFieldTranslationInformationForm.valid()) {
                return;
            }
            if ($('#FormFieldTranslation_FormFieldId').prop('required') && $('#FormFieldTranslation_FormFieldId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('FormField')));
                return;
            }
            if ($('#FormFieldTranslation_LocaleId').prop('required') && $('#FormFieldTranslation_LocaleId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Locale')));
                return;
            }

            var formFieldTranslation = _$formFieldTranslationInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _formFieldTranslationsService.createOrEdit(
				formFieldTranslation
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditFormFieldTranslationModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);