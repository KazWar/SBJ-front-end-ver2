(function ($) {
    app.modals.CreateOrEditLocaleModal = function () {

        var _localesService = abp.services.app.locales;

        var _modalManager;
        var _$localeInformationForm = null;

		        var _LocalecountryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Locales/CountryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Locales/_LocaleCountryLookupTableModal.js',
            modalClass: 'CountryLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$localeInformationForm = _modalManager.getModal().find('form[name=LocaleInformationsForm]');
            _$localeInformationForm.validate();
        };

		          $('#OpenCountryLookupTableButton').click(function () {

            var locale = _$localeInformationForm.serializeFormToObject();

            _LocalecountryLookupTableModal.open({ id: locale.countryId, displayName: locale.countryCountryCode }, function (data) {
                _$localeInformationForm.find('input[name=countryCountryCode]').val(data.displayName); 
                _$localeInformationForm.find('input[name=countryId]').val(data.id); 
            });
        });
		
		$('#ClearCountryCountryCodeButton').click(function () {
                _$localeInformationForm.find('input[name=countryCountryCode]').val(''); 
                _$localeInformationForm.find('input[name=countryId]').val(''); 
        });
		


        this.save = function () {
            if (!_$localeInformationForm.valid()) {
                return;
            }
            if ($('#Locale_CountryId').prop('required') && $('#Locale_CountryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Country')));
                return;
            }

            var locale = _$localeInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _localesService.createOrEdit(
				locale
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditLocaleModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);