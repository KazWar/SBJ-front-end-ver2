(function ($) {
    app.modals.RegistrationLookupTableModal = function () {

        var _modalManager;

        var _registrationHistoriesService = abp.services.app.registrationHistories;
        var _$registrationTable = $('#RegistrationTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$registrationTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _registrationHistoriesService.getAllRegistrationForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#RegistrationTableFilter').val()
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

        $('#RegistrationTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getRegistration() {
            dataTable.ajax.reload();
        }

        $('#GetRegistrationButton').click(function (e) {
            e.preventDefault();
            getRegistration();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getRegistration();
            }
        });

    };
})(jQuery);

