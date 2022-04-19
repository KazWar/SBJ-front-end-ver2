(function ($) {
    app.modals.CreateOrEditCountryModal = function () {

        var _countriesService = abp.services.app.countries;

        var _modalManager;
        var _$countryInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$countryInformationForm = _modalManager.getModal().find('form[name=CountryInformationsForm]');
            _$countryInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$countryInformationForm.valid()) {
                return;
            }

            var country = _$countryInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _countriesService.createOrEdit(
				country
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCountryModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);