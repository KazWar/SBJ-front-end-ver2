(function ($) {
    app.modals.CampaignTypeEventLookupTableModal = function () {

        var _modalManager;

        var _campaignTypeEventRegistrationStatusesService = abp.services.app.campaignTypeEventRegistrationStatuses;
        var _$campaignTypeEventTable = $('#CampaignTypeEventTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$campaignTypeEventTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _campaignTypeEventRegistrationStatusesService.getAllCampaignTypeEventForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#CampaignTypeEventTableFilter').val()
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

        $('#CampaignTypeEventTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getCampaignTypeEvent() {
            dataTable.ajax.reload();
        }

        $('#GetCampaignTypeEventButton').click(function (e) {
            e.preventDefault();
            getCampaignTypeEvent();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getCampaignTypeEvent();
            }
        });

    };
})(jQuery);

