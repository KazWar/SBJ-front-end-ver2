(function ($) {
    app.modals.MessageLookupTableModal = function () {

        var _modalManager;

        var _messageTypesService = abp.services.app.messageTypes;
        var _$messageTable = $('#MessageTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$messageTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _messageTypesService.getAllMessageForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#MessageTableFilter').val()
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

        $('#MessageTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getMessage() {
            dataTable.ajax.reload();
        }

        $('#GetMessageButton').click(function (e) {
            e.preventDefault();
            getMessage();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getMessage();
            }
        });

    };
})(jQuery);

