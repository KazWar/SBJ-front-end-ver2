(function ($) {
    app.modals.HandlingLineLookupTableModal = function () {

        var _modalManager;

        var _purchaseRegistrationsService = abp.services.app.purchaseRegistrations;
        var _$handlingLineTable = $('#HandlingLineTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$handlingLineTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _purchaseRegistrationsService.getAllHandlingLineForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#HandlingLineTableFilter').val()
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

        $('#HandlingLineTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getHandlingLine() {
            dataTable.ajax.reload();
        }

        $('#GetHandlingLineButton').click(function (e) {
            e.preventDefault();
            getHandlingLine();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getHandlingLine();
            }
        });

    };
})(jQuery);

