(function () {
    $(function () {
        var _purchaseRegistrationsService = abp.services.app.purchaseRegistrations;

        var _$purchaseRegistrationInformationForm = $('form[name=PurchaseRegistrationInformationsForm]');
        _$purchaseRegistrationInformationForm.validate();

		        var _PurchaseRegistrationregistrationLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PurchaseRegistrations/RegistrationLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PurchaseRegistrations/_PurchaseRegistrationRegistrationLookupTableModal.js',
            modalClass: 'RegistrationLookupTableModal'
        });        var _PurchaseRegistrationproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PurchaseRegistrations/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PurchaseRegistrations/_PurchaseRegistrationProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });        var _PurchaseRegistrationhandlingLineLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PurchaseRegistrations/HandlingLineLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PurchaseRegistrations/_PurchaseRegistrationHandlingLineLookupTableModal.js',
            modalClass: 'HandlingLineLookupTableModal'
        });        var _PurchaseRegistrationretailerLocationLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PurchaseRegistrations/RetailerLocationLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PurchaseRegistrations/_PurchaseRegistrationRetailerLocationLookupTableModal.js',
            modalClass: 'RetailerLocationLookupTableModal'
        });
   
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
      
	            $('#OpenRegistrationLookupTableButton').click(function () {

            var purchaseRegistration = _$purchaseRegistrationInformationForm.serializeFormToObject();

            _PurchaseRegistrationregistrationLookupTableModal.open({ id: purchaseRegistration.registrationId, displayName: purchaseRegistration.registrationFirstName }, function (data) {
                _$purchaseRegistrationInformationForm.find('input[name=registrationFirstName]').val(data.displayName); 
                _$purchaseRegistrationInformationForm.find('input[name=registrationId]').val(data.id); 
            });
        });
		
		$('#ClearRegistrationFirstNameButton').click(function () {
                _$purchaseRegistrationInformationForm.find('input[name=registrationFirstName]').val(''); 
                _$purchaseRegistrationInformationForm.find('input[name=registrationId]').val(''); 
        });
		
        $('#OpenProductLookupTableButton').click(function () {

            var purchaseRegistration = _$purchaseRegistrationInformationForm.serializeFormToObject();

            _PurchaseRegistrationproductLookupTableModal.open({ id: purchaseRegistration.productId, displayName: purchaseRegistration.productCtn }, function (data) {
                _$purchaseRegistrationInformationForm.find('input[name=productCtn]').val(data.displayName); 
                _$purchaseRegistrationInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductCtnButton').click(function () {
                _$purchaseRegistrationInformationForm.find('input[name=productCtn]').val(''); 
                _$purchaseRegistrationInformationForm.find('input[name=productId]').val(''); 
        });
		
        $('#OpenHandlingLineLookupTableButton').click(function () {

            var purchaseRegistration = _$purchaseRegistrationInformationForm.serializeFormToObject();

            _PurchaseRegistrationhandlingLineLookupTableModal.open({ id: purchaseRegistration.handlingLineId, displayName: purchaseRegistration.handlingLineCustomerCode }, function (data) {
                _$purchaseRegistrationInformationForm.find('input[name=handlingLineCustomerCode]').val(data.displayName); 
                _$purchaseRegistrationInformationForm.find('input[name=handlingLineId]').val(data.id); 
            });
        });
		
		$('#ClearHandlingLineCustomerCodeButton').click(function () {
                _$purchaseRegistrationInformationForm.find('input[name=handlingLineCustomerCode]').val(''); 
                _$purchaseRegistrationInformationForm.find('input[name=handlingLineId]').val(''); 
        });
		
        $('#OpenRetailerLocationLookupTableButton').click(function () {

            var purchaseRegistration = _$purchaseRegistrationInformationForm.serializeFormToObject();

            _PurchaseRegistrationretailerLocationLookupTableModal.open({ id: purchaseRegistration.retailerLocationId, displayName: purchaseRegistration.retailerLocationName }, function (data) {
                _$purchaseRegistrationInformationForm.find('input[name=retailerLocationName]').val(data.displayName); 
                _$purchaseRegistrationInformationForm.find('input[name=retailerLocationId]').val(data.id); 
            });
        });
		
		$('#ClearRetailerLocationNameButton').click(function () {
                _$purchaseRegistrationInformationForm.find('input[name=retailerLocationName]').val(''); 
                _$purchaseRegistrationInformationForm.find('input[name=retailerLocationId]').val(''); 
        });
		


        function save(successCallback) {
            if (!_$purchaseRegistrationInformationForm.valid()) {
                return;
            }
            if ($('#PurchaseRegistration_RegistrationId').prop('required') && $('#PurchaseRegistration_RegistrationId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Registration')));
                return;
            }
            if ($('#PurchaseRegistration_ProductId').prop('required') && $('#PurchaseRegistration_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }
            if ($('#PurchaseRegistration_HandlingLineId').prop('required') && $('#PurchaseRegistration_HandlingLineId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('HandlingLine')));
                return;
            }
            if ($('#PurchaseRegistration_RetailerLocationId').prop('required') && $('#PurchaseRegistration_RetailerLocationId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('RetailerLocation')));
                return;
            }

            var purchaseRegistration = _$purchaseRegistrationInformationForm.serializeFormToObject();
			
			 abp.ui.setBusy();
			 _purchaseRegistrationsService.createOrEdit(
				purchaseRegistration
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               abp.event.trigger('app.createOrEditPurchaseRegistrationModalSaved');
               
               if(typeof(successCallback)==='function'){
                    successCallback();
               }
			 }).always(function () {
			    abp.ui.clearBusy();
			});
        };
        
        function clearForm(){
            _$purchaseRegistrationInformationForm[0].reset();
        }
        
        $('#saveBtn').click(function(){
            save(function(){
                window.location="/App/PurchaseRegistrations";
            });
        });
        
        $('#saveAndNewBtn').click(function(){
            save(function(){
                if (!$('input[name=id]').val()) {//if it is create page
                   clearForm();
                }
            });
        });
    });
})();