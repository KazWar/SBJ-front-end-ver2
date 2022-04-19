(function ($) {
    app.modals.MessageComponentLookupTableModal = function () {

        var _modalManager;

        var _messageComponentContentsService = abp.services.app.messageComponentContents;
        var _$messageComponentTable = $('#MessageComponentTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$messageComponentTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _messageComponentContentsService.getAllMessageComponentForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#MessageComponentTableFilter').val()
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

        $('#MessageComponentTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getMessageComponent() {
            dataTable.ajax.reload();
        }

        $('#GetMessageComponentButton').click(function (e) {
            e.preventDefault();
            getMessageComponent();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getMessageComponent();
            }
        });

    };
})(jQuery);

