(function ($) {
    app.modals.CreateOrEditPromoStepFieldModal = function () {

        var _promoStepFieldsService = abp.services.app.promoStepFields;

        var _modalManager;
        var _$promoStepFieldInformationForm = null;

		        var _PromoStepFieldpromoStepLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoStepFields/PromoStepLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoStepFields/_PromoStepFieldPromoStepLookupTableModal.js',
            modalClass: 'PromoStepLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$promoStepFieldInformationForm = _modalManager.getModal().find('form[name=PromoStepFieldInformationsForm]');
            _$promoStepFieldInformationForm.validate();
        };

		          $('#OpenPromoStepLookupTableButton').click(function () {

            var promoStepField = _$promoStepFieldInformationForm.serializeFormToObject();

            _PromoStepFieldpromoStepLookupTableModal.open({ id: promoStepField.promoStepId, displayName: promoStepField.promoStepDescription }, function (data) {
                _$promoStepFieldInformationForm.find('input[name=promoStepDescription]').val(data.displayName); 
                _$promoStepFieldInformationForm.find('input[name=promoStepId]').val(data.id); 
            });
        });
		
		$('#ClearPromoStepDescriptionButton').click(function () {
                _$promoStepFieldInformationForm.find('input[name=promoStepDescription]').val(''); 
                _$promoStepFieldInformationForm.find('input[name=promoStepId]').val(''); 
        });
		


        this.save = function () {
            if (!_$promoStepFieldInformationForm.valid()) {
                return;
            }
            if ($('#PromoStepField_PromoStepId').prop('required') && $('#PromoStepField_PromoStepId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('PromoStep')));
                return;
            }

            var promoStepField = _$promoStepFieldInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _promoStepFieldsService.createOrEdit(
				promoStepField
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPromoStepFieldModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);