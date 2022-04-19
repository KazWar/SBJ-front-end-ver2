(function ($) {
    app.modals.CreateOrEditCompanyModal = function () {

        var _companiesService = abp.services.app.companies;

        var _modalManager;
        var _$companyInformationForm = null;

		        var _CompanyaddressLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Companies/AddressLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Companies/_CompanyAddressLookupTableModal.js',
            modalClass: 'AddressLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$companyInformationForm = _modalManager.getModal().find('form[name=CompanyInformationsForm]');
            _$companyInformationForm.validate();
        };

		          $('#OpenAddressLookupTableButton').click(function () {

            var company = _$companyInformationForm.serializeFormToObject();

            _CompanyaddressLookupTableModal.open({ id: company.addressId, displayName: company.addressPostalCode }, function (data) {
                _$companyInformationForm.find('input[name=addressPostalCode]').val(data.displayName); 
                _$companyInformationForm.find('input[name=addressId]').val(data.id); 
            });
        });
		
		$('#ClearAddressPostalCodeButton').click(function () {
                _$companyInformationForm.find('input[name=addressPostalCode]').val(''); 
                _$companyInformationForm.find('input[name=addressId]').val(''); 
        });
		


        this.save = function () {
            if (!_$companyInformationForm.valid()) {
                return;
            }
            if ($('#Company_AddressId').prop('required') && $('#Company_AddressId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Address')));
                return;
            }

            var company = _$companyInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _companiesService.createOrEdit(
				company
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCompanyModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);