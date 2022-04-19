(function ($) {
    app.modals.ProductCategoryLookupTableModal = function () {

        var _modalManager;

        var _productCategoryYearPOsService = abp.services.app.productCategoryYearPOs;
        var _$productCategoryTable = $('#ProductCategoryTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$productCategoryTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productCategoryYearPOsService.getAllProductCategoryForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#ProductCategoryTableFilter').val()
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

        $('#ProductCategoryTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getProductCategory() {
            dataTable.ajax.reload();
        }

        $('#GetProductCategoryButton').click(function (e) {
            e.preventDefault();
            getProductCategory();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getProductCategory();
            }
        });

    };
})(jQuery);

