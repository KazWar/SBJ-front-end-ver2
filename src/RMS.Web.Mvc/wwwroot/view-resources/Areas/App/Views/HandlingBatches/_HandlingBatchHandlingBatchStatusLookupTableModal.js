(function ($) {
    app.modals.HandlingBatchStatusLookupTableModal = function () {

        var _modalManager;

        var _handlingBatchesService = abp.services.app.handlingBatches;
        var _$handlingBatchStatusTable = $('#HandlingBatchStatusTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$handlingBatchStatusTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _handlingBatchesService.getAllHandlingBatchStatusForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#HandlingBatchStatusTableFilter').val()
                    };
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: "<div class=\"text-center\"><input id='selectbtn' class='btn btn-success' type='button' width='25px' value='" + app.localize('Select') + "' /></div>"
                },
                {
                    autoWidth: false,
                    orderable: false,
                    targets: 1,
                    data: "displayName"
                }
            ]
        });

        $('#HandlingBatchStatusTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getHandlingBatchStatus() {
            dataTable.ajax.reload();
        }

        $('#GetHandlingBatchStatusButton').click(function (e) {
            e.preventDefault();
            getHandlingBatchStatus();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getHandlingBatchStatus();
            }
        });

    };
})(jQuery);

