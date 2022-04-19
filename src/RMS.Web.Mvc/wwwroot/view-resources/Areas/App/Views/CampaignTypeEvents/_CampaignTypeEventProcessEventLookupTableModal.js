(function ($) {
    app.modals.ProcessEventLookupTableModal = function () {

        var _modalManager;

        var _campaignTypeEventsService = abp.services.app.campaignTypeEvents;
        var _$processEventTable = $('#ProcessEventTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$processEventTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _campaignTypeEventsService.getAllProcessEventForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#ProcessEventTableFilter').val()
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

        $('#ProcessEventTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getProcessEvent() {
            dataTable.ajax.reload();
        }

        $('#GetProcessEventButton').click(function (e) {
            e.preventDefault();
            getProcessEvent();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getProcessEvent();
            }
        });

    };
})(jQuery);

