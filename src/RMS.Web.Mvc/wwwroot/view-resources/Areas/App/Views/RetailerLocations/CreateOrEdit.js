(function () {
    $(function () {
        var _retailerLocationsService = abp.services.app.retailerLocations;

        var _$retailerLocationInformationForm = $('form[name=RetailerLocationInformationsForm]');
        _$retailerLocationInformationForm.validate();

		        var _RetailerLocationretailerLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RetailerLocations/RetailerLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RetailerLocations/_RetailerLocationRetailerLookupTableModal.js',
            modalClass: 'RetailerLookupTableModal'
        });        var _RetailerLocationaddressLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RetailerLocations/AddressLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RetailerLocations/_RetailerLocationAddressLookupTableModal.js',
            modalClass: 'AddressLookupTableModal'
        });
   
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
      
	            $('#OpenRetailerLookupTableButton').click(function () {

            var retailerLocation = _$retailerLocationInformationForm.serializeFormToObject();

            _RetailerLocationretailerLookupTableModal.open({ id: retailerLocation.retailerId, displayName: retailerLocation.retailerName }, function (data) {
                _$retailerLocationInformationForm.find('input[name=retailerName]').val(data.displayName); 
                _$retailerLocationInformationForm.find('input[name=retailerId]').val(data.id); 
            });
        });
		
		$('#ClearRetailerNameButton').click(function () {
                _$retailerLocationInformationForm.find('input[name=retailerName]').val(''); 
                _$retailerLocationInformationForm.find('input[name=retailerId]').val(''); 
        });
		
        $('#OpenAddressLookupTableButton').click(function () {

            var retailerLocation = _$retailerLocationInformationForm.serializeFormToObject();

            _RetailerLocationaddressLookupTableModal.open({ id: retailerLocation.addressId, displayName: retailerLocation.addressAddressLine1 }, function (data) {
                _$retailerLocationInformationForm.find('input[name=addressAddressLine1]').val(data.displayName); 
                _$retailerLocationInformationForm.find('input[name=addressId]').val(data.id); 
            });
        });
		
		$('#ClearAddressAddressLine1Button').click(function () {
                _$retailerLocationInformationForm.find('input[name=addressAddressLine1]').val(''); 
                _$retailerLocationInformationForm.find('input[name=addressId]').val(''); 
        });
		


        function save(successCallback) {
            if (!_$retailerLocationInformationForm.valid()) {
                return;
            }
            if ($('#RetailerLocation_RetailerId').prop('required') && $('#RetailerLocation_RetailerId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Retailer')));
                return;
            }
            if ($('#RetailerLocation_AddressId').prop('required') && $('#RetailerLocation_AddressId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Address')));
                return;
            }

            var retailerLocation = _$retailerLocationInformationForm.serializeFormToObject();
			
			 abp.ui.setBusy();
			 _retailerLocationsService.createOrEdit(
				retailerLocation
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               abp.event.trigger('app.createOrEditRetailerLocationModalSaved');
               
               if(typeof(successCallback)==='function'){
                    successCallback();
               }
			 }).always(function () {
			    abp.ui.clearBusy();
			});
        };
        
        function clearForm(){
            _$retailerLocationInformationForm[0].reset();
        }
        
        $('#saveBtn').click(function(){
            save(function(){
                window.location="/App/RetailerLocations";
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