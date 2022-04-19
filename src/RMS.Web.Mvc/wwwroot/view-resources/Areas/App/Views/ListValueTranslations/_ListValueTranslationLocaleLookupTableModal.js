(function ($) {
    app.modals.LocaleLookupTableModal = function () {

        var _modalManager;

        var _listValueTranslationsService = abp.services.app.listValueTranslations;
        var _$localeTable = $('#LocaleTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$localeTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _listValueTranslationsService.getAllLocaleForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#LocaleTableFilter').val()
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

        $('#LocaleTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getLocale() {
            dataTable.ajax.reload();
        }

        $('#GetLocaleButton').click(function (e) {
            e.preventDefault();
            getLocale();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getLocale();
            }
        });

    };
})(jQuery);

