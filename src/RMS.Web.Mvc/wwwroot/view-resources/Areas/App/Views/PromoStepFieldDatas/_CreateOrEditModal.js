(function ($) {
    app.modals.CreateOrEditPromoStepFieldDataModal = function () {

        var _promoStepFieldDatasService = abp.services.app.promoStepFieldDatas;

        var _modalManager;
        var _$promoStepFieldDataInformationForm = null;

		        var _PromoStepFieldDatapromoStepFieldLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoStepFieldDatas/PromoStepFieldLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoStepFieldDatas/_PromoStepFieldDataPromoStepFieldLookupTableModal.js',
            modalClass: 'PromoStepFieldLookupTableModal'
        });        var _PromoStepFieldDatapromoStepDataLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoStepFieldDatas/PromoStepDataLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoStepFieldDatas/_PromoStepFieldDataPromoStepDataLookupTableModal.js',
            modalClass: 'PromoStepDataLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$promoStepFieldDataInformationForm = _modalManager.getModal().find('form[name=PromoStepFieldDataInformationsForm]');
            _$promoStepFieldDataInformationForm.validate();
        };

		          $('#OpenPromoStepFieldLookupTableButton').click(function () {

            var promoStepFieldData = _$promoStepFieldDataInformationForm.serializeFormToObject();

            _PromoStepFieldDatapromoStepFieldLookupTableModal.open({ id: promoStepFieldData.promoStepFieldId, displayName: promoStepFieldData.promoStepFieldDescription }, function (data) {
                _$promoStepFieldDataInformationForm.find('input[name=promoStepFieldDescription]').val(data.displayName); 
                _$promoStepFieldDataInformationForm.find('input[name=promoStepFieldId]').val(data.id); 
            });
        });
		
		$('#ClearPromoStepFieldDescriptionButton').click(function () {
                _$promoStepFieldDataInformationForm.find('input[name=promoStepFieldDescription]').val(''); 
                _$promoStepFieldDataInformationForm.find('input[name=promoStepFieldId]').val(''); 
        });
		
        $('#OpenPromoStepDataLookupTableButton').click(function () {

            var promoStepFieldData = _$promoStepFieldDataInformationForm.serializeFormToObject();

            _PromoStepFieldDatapromoStepDataLookupTableModal.open({ id: promoStepFieldData.promoStepDataId, displayName: promoStepFieldData.promoStepDataDescription }, function (data) {
                _$promoStepFieldDataInformationForm.find('input[name=promoStepDataDescription]').val(data.displayName); 
                _$promoStepFieldDataInformationForm.find('input[name=promoStepDataId]').val(data.id); 
            });
        });
		
		$('#ClearPromoStepDataDescriptionButton').click(function () {
                _$promoStepFieldDataInformationForm.find('input[name=promoStepDataDescription]').val(''); 
                _$promoStepFieldDataInformationForm.find('input[name=promoStepDataId]').val(''); 
        });
		


        this.save = function () {
            if (!_$promoStepFieldDataInformationForm.valid()) {
                return;
            }
            if ($('#PromoStepFieldData_PromoStepFieldId').prop('required') && $('#PromoStepFieldData_PromoStepFieldId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('PromoStepField')));
                return;
            }
            if ($('#PromoStepFieldData_PromoStepDataId').prop('required') && $('#PromoStepFieldData_PromoStepDataId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('PromoStepData')));
                return;
            }

            var promoStepFieldData = _$promoStepFieldDataInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _promoStepFieldDatasService.createOrEdit(
				promoStepFieldData
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPromoStepFieldDataModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);