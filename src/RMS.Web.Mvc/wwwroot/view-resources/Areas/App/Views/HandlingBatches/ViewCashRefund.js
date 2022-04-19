(function () {
    $(function () {
        var _handlingBatchesService = abp.services.app.handlingBatches;

        let id = $('#Id');
        let sepaInitiator = $('#SepaInitiator');

        function process() {
            abp.ui.setBusy();
            _handlingBatchesService
                .processCashRefunds(id.val(), sepaInitiator.val())
                .done(function (result) {
                    if (result !== null) {
                        app.downloadTempFile(result);
                    } else {
                        alert('SEPA could not be generated');
                    }
                }).always(function () {
                    abp.ui.clearBusy();
                });
        };

        function finish(successCallback) {
            abp.ui.setBusy();
            _handlingBatchesService
                .finishCashRefunds(id.val())
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
                .getCashRefundBatchToExcel(id.val())
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#ProcessCashRefundsButton').click(function () {           
            process();
        });

        $('#FinishCashRefundsButton').click(function () {
            finish(function () {
                window.location = "/App/HandlingBatches";
            });
        });

        $('#goBackBtn').on('click', function () {
            window.history.back();
        });
    });
})();