(function ($) {
    app.modals.CreateOrEditPromoCountryModal = function () {

        var _promoCountriesService = abp.services.app.promoCountries;

        var _modalManager;
        var _$promoCountryInformationForm = null;

		        var _PromoCountrypromoLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoCountries/PromoLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoCountries/_PromoCountryPromoLookupTableModal.js',
            modalClass: 'PromoLookupTableModal'
        });        var _PromoCountrycountryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoCountries/CountryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoCountries/_PromoCountryCountryLookupTableModal.js',
            modalClass: 'CountryLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$promoCountryInformationForm = _modalManager.getModal().find('form[name=PromoCountryInformationsForm]');
            _$promoCountryInformationForm.validate();
        };

		          $('#OpenPromoLookupTableButton').click(function () {

            var promoCountry = _$promoCountryInformationForm.serializeFormToObject();

            _PromoCountrypromoLookupTableModal.open({ id: promoCountry.promoId, displayName: promoCountry.promoPromocode }, function (data) {
                _$promoCountryInformationForm.find('input[name=promoPromocode]').val(data.displayName); 
                _$promoCountryInformationForm.find('input[name=promoId]').val(data.id); 
            });
        });
		
		$('#ClearPromoPromocodeButton').click(function () {
                _$promoCountryInformationForm.find('input[name=promoPromocode]').val(''); 
                _$promoCountryInformationForm.find('input[name=promoId]').val(''); 
        });
		
        $('#OpenCountryLookupTableButton').click(function () {

            var promoCountry = _$promoCountryInformationForm.serializeFormToObject();

            _PromoCountrycountryLookupTableModal.open({ id: promoCountry.countryId, displayName: promoCountry.countryCountryCode }, function (data) {
                _$promoCountryInformationForm.find('input[name=countryCountryCode]').val(data.displayName); 
                _$promoCountryInformationForm.find('input[name=countryId]').val(data.id); 
            });
        });
		
		$('#ClearCountryCountryCodeButton').click(function () {
                _$promoCountryInformationForm.find('input[name=countryCountryCode]').val(''); 
                _$promoCountryInformationForm.find('input[name=countryId]').val(''); 
        });
		


        this.save = function () {
            if (!_$promoCountryInformationForm.valid()) {
                return;
            }
            if ($('#PromoCountry_PromoId').prop('required') && $('#PromoCountry_PromoId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Promo')));
                return;
            }
            if ($('#PromoCountry_CountryId').prop('required') && $('#PromoCountry_CountryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Country')));
                return;
            }

            var promoCountry = _$promoCountryInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _promoCountriesService.createOrEdit(
				promoCountry
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPromoCountryModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);