(function ($) {
    app.modals.RegistrationStatusLookupTableModal = function () {

        var _modalManager;

        var _registrationHistoriesService = abp.services.app.registrationHistories;
        var _$registrationStatusTable = $('#RegistrationStatusTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$registrationStatusTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _registrationHistoriesService.getAllRegistrationStatusForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#RegistrationStatusTableFilter').val()
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

        $('#RegistrationStatusTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getRegistrationStatus() {
            dataTable.ajax.reload();
        }

        $('#GetRegistrationStatusButton').click(function (e) {
            e.preventDefault();
            getRegistrationStatus();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getRegistrationStatus();
            }
        });

    };
})(jQuery);

