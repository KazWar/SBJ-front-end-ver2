(function ($) {
    app.modals.AddressLookupTableModal = function () {

        var _modalManager;

        var _companiesService = abp.services.app.companies;
        var _$addressTable = $('#AddressTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$addressTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _companiesService.getAllAddressForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#AddressTableFilter').val()
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

        $('#AddressTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getAddress() {
            dataTable.ajax.reload();
        }

        $('#GetAddressButton').click(function (e) {
            e.preventDefault();
            getAddress();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getAddress();
            }
        });

    };
})(jQuery);

