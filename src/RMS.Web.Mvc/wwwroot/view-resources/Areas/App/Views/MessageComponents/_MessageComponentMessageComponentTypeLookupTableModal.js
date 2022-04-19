(function ($) {
    app.modals.MessageComponentTypeLookupTableModal = function () {

        var _modalManager;

        var _messageComponentsService = abp.services.app.messageComponents;
        var _$messageComponentTypeTable = $('#MessageComponentTypeTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$messageComponentTypeTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _messageComponentsService.getAllMessageComponentTypeForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#MessageComponentTypeTableFilter').val()
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

        $('#MessageComponentTypeTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getMessageComponentType() {
            dataTable.ajax.reload();
        }

        $('#GetMessageComponentTypeButton').click(function (e) {
            e.preventDefault();
            getMessageComponentType();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getMessageComponentType();
            }
        });

    };
})(jQuery);

