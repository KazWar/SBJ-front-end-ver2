(function () {
    $(function () {
        var _handlingBatchesService = abp.services.app.handlingBatches;

        let id = $('#Id');
        let warehouseId = $('#WarehouseId');
        let orderUserId = $('#OrderUserId');
        let password = $('#Password');

        $('#ExportToExcelButton').click(function () {
            _handlingBatchesService
                .getPremiumBatchToExcel({
                    id: id.val(),
                    warehouseId: warehouseId.val(),
                    orderUserId: orderUserId.val(),
                    password: password.val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#goBackBtn').on('click', function () {
            window.history.back();
        });
    });
})();