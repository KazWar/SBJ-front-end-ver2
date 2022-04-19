﻿(function ($) {
    app.modals.LocaleLookupTableModal = function () {

        var _modalManager;

        var _formLocalesService = abp.services.app.formLocales;
        var _$localeTable = $('#LocaleTable');

        //Resolving the SystemLevel Id based on current page (Assuming the values for company and campaign will always be the same)
        var formSystemLevelId = $("#pageSource").val() == "CompanyForms" ? 1 : 2;

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$localeTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _formLocalesService.getAllLocaleForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#LocaleTableFilter').val(),
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

