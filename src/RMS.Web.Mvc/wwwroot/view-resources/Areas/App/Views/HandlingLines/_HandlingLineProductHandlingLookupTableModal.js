(function ($) {
    app.modals.ProductHandlingLookupTableModal = function () {

        var _modalManager;

        var _handlingLinesService = abp.services.app.handlingLines;
        var _$productHandlingTable = $('#ProductHandlingTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$productHandlingTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _handlingLinesService.getAllProductHandlingForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#ProductHandlingTableFilter').val()
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

        $('#ProductHandlingTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getProductHandling() {
            dataTable.ajax.reload();
        }

        $('#GetProductHandlingButton').click(function (e) {
            e.preventDefault();
            getProductHandling();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getProductHandling();
            }
        });

    };
})(jQuery);

