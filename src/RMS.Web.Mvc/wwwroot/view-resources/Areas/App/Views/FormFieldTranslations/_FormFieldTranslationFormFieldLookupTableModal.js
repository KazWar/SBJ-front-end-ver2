(function ($) {
    app.modals.FormFieldLookupTableModal = function () {

        var _modalManager;

        var _formFieldTranslationsService = abp.services.app.formFieldTranslations;
        var _$formFieldTable = $('#FormFieldTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$formFieldTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _formFieldTranslationsService.getAllFormFieldForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#FormFieldTableFilter').val()
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

        $('#FormFieldTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getFormField() {
            dataTable.ajax.reload();
        }

        $('#GetFormFieldButton').click(function (e) {
            e.preventDefault();
            getFormField();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getFormField();
            }
        });

    };
})(jQuery);

