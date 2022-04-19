(function ($) {
    app.modals.PromoLookupTableModal = function () {

        var _modalManager;

        var _promoProductsService = abp.services.app.promoProducts;
        var _$promoTable = $('#PromoTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$promoTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _promoProductsService.getAllPromoForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#PromoTableFilter').val()
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

        $('#PromoTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getPromo() {
            dataTable.ajax.reload();
        }

        $('#GetPromoButton').click(function (e) {
            e.preventDefault();
            getPromo();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getPromo();
            }
        });

    };
})(jQuery);

