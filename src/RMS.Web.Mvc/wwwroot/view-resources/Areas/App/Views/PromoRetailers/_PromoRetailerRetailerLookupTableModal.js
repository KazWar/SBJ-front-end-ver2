(function ($) {
    app.modals.RetailerLookupTableModal = function () {

        var _modalManager;

        var _promoRetailersService = abp.services.app.promoRetailers;
        var _$retailerTable = $('#RetailerTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$retailerTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _promoRetailersService.getAllRetailerForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#RetailerTableFilter').val()
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

        $('#RetailerTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getRetailer() {
            dataTable.ajax.reload();
        }

        $('#GetRetailerButton').click(function (e) {
            e.preventDefault();
            getRetailer();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getRetailer();
            }
        });

    };
})(jQuery);

