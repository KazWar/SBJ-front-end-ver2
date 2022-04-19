(function ($) {
    app.modals.CreateOrEditRejectionReasonModal = function () {

        var _rejectionReasonsService = abp.services.app.rejectionReasons;

        var _modalManager;
        var _$rejectionReasonInformationForm = null;



        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$rejectionReasonInformationForm = _modalManager.getModal().find('form[name=RejectionReasonInformationsForm]');
            _$rejectionReasonInformationForm.validate();
        };



        this.save = function () {
            if (!_$rejectionReasonInformationForm.valid()) {
                return;
            }

            var rejectionReason = _$rejectionReasonInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _rejectionReasonsService.createOrEdit(
                rejectionReason
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditRejectionReasonModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);