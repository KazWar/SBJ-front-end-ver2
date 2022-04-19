(function ($) {
    app.modals.PromoStepLookupTableModal = function () {

        var _modalManager;

        var _promoStepDatasService = abp.services.app.promoStepDatas;
        var _$promoStepTable = $('#PromoStepTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$promoStepTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _promoStepDatasService.getAllPromoStepForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#PromoStepTableFilter').val()
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

        $('#PromoStepTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getPromoStep() {
            dataTable.ajax.reload();
        }

        $('#GetPromoStepButton').click(function (e) {
            e.preventDefault();
            getPromoStep();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getPromoStep();
            }
        });

    };
})(jQuery);

