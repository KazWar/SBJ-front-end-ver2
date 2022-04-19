(function ($) {
    app.modals.CreateOrEditAddressModal = function () {

        var _addressesService = abp.services.app.addresses;

        var _modalManager;
        var _$addressInformationForm = null;

		        var _AddresscountryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Addresses/CountryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Addresses/_AddressCountryLookupTableModal.js',
            modalClass: 'CountryLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$addressInformationForm = _modalManager.getModal().find('form[name=AddressInformationsForm]');
            _$addressInformationForm.validate();
        };

		          $('#OpenCountryLookupTableButton').click(function () {

            var address = _$addressInformationForm.serializeFormToObject();

            _AddresscountryLookupTableModal.open({ id: address.countryId, displayName: address.countryCountryCode }, function (data) {
                _$addressInformationForm.find('input[name=countryCountryCode]').val(data.displayName); 
                _$addressInformationForm.find('input[name=countryId]').val(data.id); 
            });
        });
		
		$('#ClearCountryCountryCodeButton').click(function () {
                _$addressInformationForm.find('input[name=countryCountryCode]').val(''); 
                _$addressInformationForm.find('input[name=countryId]').val(''); 
        });
		


        this.save = function () {
            if (!_$addressInformationForm.valid()) {
                return;
            }
            if ($('#Address_CountryId').prop('required') && $('#Address_CountryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Country')));
                return;
            }

            var address = _$addressInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _addressesService.createOrEdit(
				address
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditAddressModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);