(function ($) {
    app.modals.CreateOrEditPromoScopeModal = function () {

        var _promoScopesService = abp.services.app.promoScopes;

        var _modalManager;
        var _$promoScopeInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$promoScopeInformationForm = _modalManager.getModal().find('form[name=PromoScopeInformationsForm]');
            _$promoScopeInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$promoScopeInformationForm.valid()) {
                return;
            }

            var promoScope = _$promoScopeInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _promoScopesService.createOrEdit(
				promoScope
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPromoScopeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);