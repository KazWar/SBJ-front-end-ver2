(function ($) {
    app.modals.FieldTypeLookupTableModal = function () {

        var _modalManager;

        var _formFieldsService = abp.services.app.formFields;
        var _$fieldTypeTable = $('#FieldTypeTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$fieldTypeTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _formFieldsService.getAllFieldTypeForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#FieldTypeTableFilter').val()
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

        $('#FieldTypeTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getFieldType() {
            dataTable.ajax.reload();
        }

        $('#GetFieldTypeButton').click(function (e) {
            e.preventDefault();
            getFieldType();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getFieldType();
            }
        });

    };
})(jQuery);

