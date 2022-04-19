(function ($) {
    app.modals.ListValueLookupTableModal = function () {

        var _modalManager;

        var _listValueTranslationsService = abp.services.app.listValueTranslations;
        var _$listValueTable = $('#ListValueTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$listValueTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _listValueTranslationsService.getAllListValueForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#ListValueTableFilter').val()
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

        $('#ListValueTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getListValue() {
            dataTable.ajax.reload();
        }

        $('#GetListValueButton').click(function (e) {
            e.preventDefault();
            getListValue();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getListValue();
            }
        });

    };
})(jQuery);

