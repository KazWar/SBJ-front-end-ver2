(function ($) {
    app.modals.FormLookupTableModal = function () {

        var _modalManager;

        var _formLocalesService = abp.services.app.formLocales;
        var _$formTable = $('#FormTable');

        //Resolving the SystemLevel Id based on current page (Assuming the values for company and campaign will always be the same)
        var formSystemLevelId = $("#pageSource").val() == "CompanyForms" ? 1 : 0;

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$formTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _formLocalesService.getAllFormForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#FormTableFilter').val(),
                        currentFormPage: formSystemLevelId,
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

        $('#FormTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getForm() {
            dataTable.ajax.reload();
        }

        $('#GetFormButton').click(function (e) {
            e.preventDefault();
            getForm();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getForm();
            }
        });

    };
})(jQuery);

