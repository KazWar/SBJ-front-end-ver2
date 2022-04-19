(function ($) {
    app.modals.PromoScopeLookupTableModal = function () {

        var _modalManager;

        var _promosService = abp.services.app.promos;
        var _$promoScopeTable = $('#PromoScopeTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$promoScopeTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _promosService.getAllPromoScopeForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#PromoScopeTableFilter').val()
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

        $('#PromoScopeTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getPromoScope() {
            dataTable.ajax.reload();
        }

        $('#GetPromoScopeButton').click(function (e) {
            e.preventDefault();
            getPromoScope();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getPromoScope();
            }
        });

    };
})(jQuery);

