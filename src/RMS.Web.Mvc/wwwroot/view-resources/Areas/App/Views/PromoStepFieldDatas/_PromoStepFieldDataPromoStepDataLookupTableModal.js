(function ($) {
    app.modals.PromoStepDataLookupTableModal = function () {

        var _modalManager;

        var _promoStepFieldDatasService = abp.services.app.promoStepFieldDatas;
        var _$promoStepDataTable = $('#PromoStepDataTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$promoStepDataTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _promoStepFieldDatasService.getAllPromoStepDataForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#PromoStepDataTableFilter').val()
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

        $('#PromoStepDataTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getPromoStepData() {
            dataTable.ajax.reload();
        }

        $('#GetPromoStepDataButton').click(function (e) {
            e.preventDefault();
            getPromoStepData();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getPromoStepData();
            }
        });

    };
})(jQuery);

