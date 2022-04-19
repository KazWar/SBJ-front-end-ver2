(function ($) {
    app.modals.ValueListLookupTableModal = function () {

        var _modalManager;

        var _listValuesService = abp.services.app.listValues;
        var _$valueListTable = $('#ValueListTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$valueListTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _listValuesService.getAllValueListForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#ValueListTableFilter').val()
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

        $('#ValueListTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getValueList() {
            dataTable.ajax.reload();
        }

        $('#GetValueListButton').click(function (e) {
            e.preventDefault();
            getValueList();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getValueList();
            }
        });

    };
})(jQuery);

