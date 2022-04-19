(function ($) {
    app.modals.CreateOrEditValueListModal = function () {

        var _valueListsService = abp.services.app.valueLists;

        var _modalManager;
        var _$valueListInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$valueListInformationForm = _modalManager.getModal().find('form[name=ValueListInformationsForm]');
            _$valueListInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$valueListInformationForm.valid()) {
                return;
            }

            var valueList = _$valueListInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _valueListsService.createOrEdit(
				valueList
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditValueListModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);