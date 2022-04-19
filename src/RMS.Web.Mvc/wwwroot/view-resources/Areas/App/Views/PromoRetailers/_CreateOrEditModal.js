(function ($) {
    app.modals.CreateOrEditPromoRetailerModal = function () {

        var _promoRetailersService = abp.services.app.promoRetailers;

        var _modalManager;
        var _$promoRetailerInformationForm = null;

		        var _PromoRetailerpromoLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoRetailers/PromoLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoRetailers/_PromoRetailerPromoLookupTableModal.js',
            modalClass: 'PromoLookupTableModal'
        });        var _PromoRetailerretailerLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoRetailers/RetailerLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoRetailers/_PromoRetailerRetailerLookupTableModal.js',
            modalClass: 'RetailerLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$promoRetailerInformationForm = _modalManager.getModal().find('form[name=PromoRetailerInformationsForm]');
            _$promoRetailerInformationForm.validate();
        };

		          $('#OpenPromoLookupTableButton').click(function () {

            var promoRetailer = _$promoRetailerInformationForm.serializeFormToObject();

            _PromoRetailerpromoLookupTableModal.open({ id: promoRetailer.promoId, displayName: promoRetailer.promoPromocode }, function (data) {
                _$promoRetailerInformationForm.find('input[name=promoPromocode]').val(data.displayName); 
                _$promoRetailerInformationForm.find('input[name=promoId]').val(data.id); 
            });
        });
		
		$('#ClearPromoPromocodeButton').click(function () {
                _$promoRetailerInformationForm.find('input[name=promoPromocode]').val(''); 
                _$promoRetailerInformationForm.find('input[name=promoId]').val(''); 
        });
		
        $('#OpenRetailerLookupTableButton').click(function () {

            var promoRetailer = _$promoRetailerInformationForm.serializeFormToObject();

            _PromoRetailerretailerLookupTableModal.open({ id: promoRetailer.retailerId, displayName: promoRetailer.retailerCode }, function (data) {
                _$promoRetailerInformationForm.find('input[name=retailerCode]').val(data.displayName); 
                _$promoRetailerInformationForm.find('input[name=retailerId]').val(data.id); 
            });
        });
		
		$('#ClearRetailerCodeButton').click(function () {
                _$promoRetailerInformationForm.find('input[name=retailerCode]').val(''); 
                _$promoRetailerInformationForm.find('input[name=retailerId]').val(''); 
        });
		


        this.save = function () {
            if (!_$promoRetailerInformationForm.valid()) {
                return;
            }
            if ($('#PromoRetailer_PromoId').prop('required') && $('#PromoRetailer_PromoId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Promo')));
                return;
            }
            if ($('#PromoRetailer_RetailerId').prop('required') && $('#PromoRetailer_RetailerId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Retailer')));
                return;
            }

            var promoRetailer = _$promoRetailerInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _promoRetailersService.createOrEdit(
				promoRetailer
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPromoRetailerModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);