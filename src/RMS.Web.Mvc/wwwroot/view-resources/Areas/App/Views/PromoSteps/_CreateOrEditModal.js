(function ($) {
    app.modals.CreateOrEditPromoStepModal = function () {

        var _promoStepsService = abp.services.app.promoSteps;

        var _modalManager;
        var _$promoStepInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$promoStepInformationForm = _modalManager.getModal().find('form[name=PromoStepInformationsForm]');
            _$promoStepInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$promoStepInformationForm.valid()) {
                return;
            }

            var promoStep = _$promoStepInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _promoStepsService.createOrEdit(
				promoStep
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPromoStepModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);