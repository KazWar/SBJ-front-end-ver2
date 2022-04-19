(function ($) {
    app.modals.ProductLookupTableModal = function () {

        var _modalManager;

        var _purchaseRegistrationsService = abp.services.app.purchaseRegistrations;
        var _$productTable = $('#ProductTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$productTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _purchaseRegistrationsService.getAllProductForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#ProductTableFilter').val()
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

        $('#ProductTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getProduct() {
            dataTable.ajax.reload();
        }

        $('#GetProductButton').click(function (e) {
            e.preventDefault();
            getProduct();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getProduct();
            }
        });

    };
})(jQuery);

