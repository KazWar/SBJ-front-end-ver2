(function ($) {
    app.modals.CreateOrEditFormLocaleModal = function () {

        var _formLocalesService = abp.services.app.formLocales;

        var _modalManager;
        var _$formLocaleInformationForm = null;

		        var _FormLocaleformLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormLocales/FormLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormLocales/_FormLocaleFormLookupTableModal.js',
            modalClass: 'FormLookupTableModal'
        });        var _FormLocalelocaleLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormLocales/LocaleLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormLocales/_FormLocaleLocaleLookupTableModal.js',
            modalClass: 'LocaleLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$formLocaleInformationForm = _modalManager.getModal().find('form[name=FormLocaleInformationsForm]');
            _$formLocaleInformationForm.validate();
        };

		          $('#OpenFormLookupTableButton').click(function () {

            var formLocale = _$formLocaleInformationForm.serializeFormToObject();

            _FormLocaleformLookupTableModal.open({ id: formLocale.formId, displayName: formLocale.formVersion }, function (data) {
                _$formLocaleInformationForm.find('input[name=formVersion]').val(data.displayName); 
                _$formLocaleInformationForm.find('input[name=formId]').val(data.id); 
            });
        });
		
		$('#ClearFormVersionButton').click(function () {
                _$formLocaleInformationForm.find('input[name=formVersion]').val(''); 
                _$formLocaleInformationForm.find('input[name=formId]').val(''); 
        });
		
        $('#OpenLocaleLookupTableButton').click(function () {

            var formLocale = _$formLocaleInformationForm.serializeFormToObject();

            _FormLocalelocaleLookupTableModal.open({ id: formLocale.localeId, displayName: formLocale.localeDescription }, function (data) {
                _$formLocaleInformationForm.find('input[name=localeDescription]').val(data.displayName); 
                _$formLocaleInformationForm.find('input[name=localeId]').val(data.id); 
                _$formLocaleInformationForm.find('input[name=description]').val(data.displayName).attr('disabled', true);
            });
        });
		
		$('#ClearLocaleDescriptionButton').click(function () {
                _$formLocaleInformationForm.find('input[name=localeDescription]').val(''); 
                _$formLocaleInformationForm.find('input[name=localeId]').val(''); 
        });
		


        this.save = function () {
            if (!_$formLocaleInformationForm.valid()) {
                return;
            }
            if ($('#FormLocale_FormId').prop('required') && $('#FormLocale_FormId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Form')));
                return;
            }
            if ($('#FormLocale_LocaleId').prop('required') && $('#FormLocale_LocaleId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Locale')));
                return;
            }

            var formLocale = _$formLocaleInformationForm.serializeFormToObject();

			 _modalManager.setBusy(true);
			 _formLocalesService.createOrEdit(
				formLocale
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
                 abp.event.trigger('app.createOrEditFormLocaleModalSaved');
                 window.location.reload();
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);