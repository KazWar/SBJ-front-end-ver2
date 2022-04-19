(function ($) {
    app.modals.SystemLevelLookupTableModal = function () {

        var _modalManager;

        var _messagesService = abp.services.app.messages;
        var _$systemLevelTable = $('#SystemLevelTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$systemLevelTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _messagesService.getAllSystemLevelForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#SystemLevelTableFilter').val()
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

        $('#SystemLevelTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getSystemLevel() {
            dataTable.ajax.reload();
        }

        $('#GetSystemLevelButton').click(function (e) {
            e.preventDefault();
            getSystemLevel();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getSystemLevel();
            }
        });

    };
})(jQuery);

