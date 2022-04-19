(function ($) {
    app.modals.CreateOrEditPromoStepDataModal = function () {

        var _promoStepDatasService = abp.services.app.promoStepDatas;

        var _modalManager;
        var _$promoStepDataInformationForm = null;

		        var _PromoStepDatapromoStepLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoStepDatas/PromoStepLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoStepDatas/_PromoStepDataPromoStepLookupTableModal.js',
            modalClass: 'PromoStepLookupTableModal'
        });        var _PromoStepDatapromoLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoStepDatas/PromoLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoStepDatas/_PromoStepDataPromoLookupTableModal.js',
            modalClass: 'PromoLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$promoStepDataInformationForm = _modalManager.getModal().find('form[name=PromoStepDataInformationsForm]');
            _$promoStepDataInformationForm.validate();
        };

		          $('#OpenPromoStepLookupTableButton').click(function () {

            var promoStepData = _$promoStepDataInformationForm.serializeFormToObject();

            _PromoStepDatapromoStepLookupTableModal.open({ id: promoStepData.promoStepId, displayName: promoStepData.promoStepDescription }, function (data) {
                _$promoStepDataInformationForm.find('input[name=promoStepDescription]').val(data.displayName); 
                _$promoStepDataInformationForm.find('input[name=promoStepId]').val(data.id); 
            });
        });
		
		$('#ClearPromoStepDescriptionButton').click(function () {
                _$promoStepDataInformationForm.find('input[name=promoStepDescription]').val(''); 
                _$promoStepDataInformationForm.find('input[name=promoStepId]').val(''); 
        });
		
        $('#OpenPromoLookupTableButton').click(function () {

            var promoStepData = _$promoStepDataInformationForm.serializeFormToObject();

            _PromoStepDatapromoLookupTableModal.open({ id: promoStepData.promoId, displayName: promoStepData.promoPromocode }, function (data) {
                _$promoStepDataInformationForm.find('input[name=promoPromocode]').val(data.displayName); 
                _$promoStepDataInformationForm.find('input[name=promoId]').val(data.id); 
            });
        });
		
		$('#ClearPromoPromocodeButton').click(function () {
                _$promoStepDataInformationForm.find('input[name=promoPromocode]').val(''); 
                _$promoStepDataInformationForm.find('input[name=promoId]').val(''); 
        });
		


        this.save = function () {
            if (!_$promoStepDataInformationForm.valid()) {
                return;
            }
            if ($('#PromoStepData_PromoStepId').prop('required') && $('#PromoStepData_PromoStepId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('PromoStep')));
                return;
            }
            if ($('#PromoStepData_PromoId').prop('required') && $('#PromoStepData_PromoId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Promo')));
                return;
            }

            var promoStepData = _$promoStepDataInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _promoStepDatasService.createOrEdit(
				promoStepData
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPromoStepDataModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);