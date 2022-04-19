(function ($) {
    app.modals.FormBlockLookupTableModal = function () {

        var _modalManager;

        var _formBlockFieldsService = abp.services.app.formBlockFields;
        var _$formBlockTable = $('#FormBlockTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$formBlockTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _formBlockFieldsService.getAllFormBlockForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#FormBlockTableFilter').val()
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

        $('#FormBlockTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getFormBlock() {
            dataTable.ajax.reload();
        }

        $('#GetFormBlockButton').click(function (e) {
            e.preventDefault();
            getFormBlock();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getFormBlock();
            }
        });

    };
})(jQuery);

