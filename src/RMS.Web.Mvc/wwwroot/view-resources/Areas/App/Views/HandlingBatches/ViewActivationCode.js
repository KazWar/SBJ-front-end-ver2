(function () {
    $(function () {
        var _handlingBatchesService = abp.services.app.handlingBatches;

        let id = $('#Id');

        function process(successCallback) {
            abp.ui.setBusy();
            _handlingBatchesService
                .processActivationCodes(id.val())
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    abp.event.trigger('app.createOrEditHandlingBatchModalSaved');

                    if (typeof (successCallback) === 'function') {
                        successCallback();
                    }
                }).always(function () {
                    abp.ui.clearBusy();
                });
        };

        $('#ExportToExcelButton').click(function () {
            _handlingBatchesService
                .getActivationCodeBatchToExcel(id.val())
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#ProcessActivationCodesButton').click(function () {
            process(function () {
                window.location = "/App/HandlingBatches";
            });
        });

        $('#goBackBtn').on('click', function () {
            window.history.back();
        });
    });
})();