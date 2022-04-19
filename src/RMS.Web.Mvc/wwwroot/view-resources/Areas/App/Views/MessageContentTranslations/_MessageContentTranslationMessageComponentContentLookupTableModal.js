(function ($) {
    app.modals.MessageComponentContentLookupTableModal = function () {

        var _modalManager;

        var _messageContentTranslationsService = abp.services.app.messageContentTranslations;
        var _$messageComponentContentTable = $('#MessageComponentContentTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$messageComponentContentTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _messageContentTranslationsService.getAllMessageComponentContentForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#MessageComponentContentTableFilter').val()
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

        $('#MessageComponentContentTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getMessageComponentContent() {
            dataTable.ajax.reload();
        }

        $('#GetMessageComponentContentButton').click(function (e) {
            e.preventDefault();
            getMessageComponentContent();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getMessageComponentContent();
            }
        });

    };
})(jQuery);

