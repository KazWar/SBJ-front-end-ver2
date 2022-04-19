(function ($) {
    app.modals.CreateOrEditRetailerModal = function () {

        var _retailersService = abp.services.app.retailers;

        var _modalManager;
        var _$retailerInformationForm = null;

		        var _RetailercountryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Retailers/CountryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Retailers/_RetailerCountryLookupTableModal.js',
            modalClass: 'CountryLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$retailerInformationForm = _modalManager.getModal().find('form[name=RetailerInformationsForm]');
            _$retailerInformationForm.validate();
        };

		          $('#OpenCountryLookupTableButton').click(function () {

            var retailer = _$retailerInformationForm.serializeFormToObject();

            _RetailercountryLookupTableModal.open({ id: retailer.countryId, displayName: retailer.countryCountryCode }, function (data) {
                _$retailerInformationForm.find('input[name=countryCountryCode]').val(data.displayName); 
                _$retailerInformationForm.find('input[name=countryId]').val(data.id); 
            });
        });
		
		$('#ClearCountryCountryCodeButton').click(function () {
                _$retailerInformationForm.find('input[name=countryCountryCode]').val(''); 
                _$retailerInformationForm.find('input[name=countryId]').val(''); 
        });
		


        this.save = function () {
            if (!_$retailerInformationForm.valid()) {
                return;
            }
            if ($('#Retailer_CountryId').prop('required') && $('#Retailer_CountryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Country')));
                return;
            }

            var retailer = _$retailerInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _retailersService.createOrEdit(
				retailer
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditRetailerModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);