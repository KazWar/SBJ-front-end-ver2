(function ($) {
    app.modals.PromoStepFieldLookupTableModal = function () {

        var _modalManager;

        var _promoStepFieldDatasService = abp.services.app.promoStepFieldDatas;
        var _$promoStepFieldTable = $('#PromoStepFieldTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$promoStepFieldTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _promoStepFieldDatasService.getAllPromoStepFieldForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#PromoStepFieldTableFilter').val()
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

        $('#PromoStepFieldTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getPromoStepField() {
            dataTable.ajax.reload();
        }

        $('#GetPromoStepFieldButton').click(function (e) {
            e.preventDefault();
            getPromoStepField();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getPromoStepField();
            }
        });

    };
})(jQuery);

