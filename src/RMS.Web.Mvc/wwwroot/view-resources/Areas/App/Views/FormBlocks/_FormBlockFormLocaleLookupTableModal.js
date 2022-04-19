(function ($) {
    app.modals.FormLocaleLookupTableModal = function () {

        var _modalManager;

        var _formBlocksService = abp.services.app.formBlocks;
        var _$formLocaleTable = $('#FormLocaleTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$formLocaleTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _formBlocksService.getAllFormLocaleForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#FormLocaleTableFilter').val()
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

        $('#FormLocaleTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getFormLocale() {
            dataTable.ajax.reload();
        }

        $('#GetFormLocaleButton').click(function (e) {
            e.preventDefault();
            getFormLocale();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getFormLocale();
            }
        });

    };
})(jQuery);

