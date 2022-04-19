(function ($) {
    app.modals.CampaignTypeEventRegistrationStatusLookupTableModal = function () {

        var _modalManager;

        var _messageComponentContentsService = abp.services.app.messageComponentContents;
        var _$campaignTypeEventRegistrationStatusTable = $('#CampaignTypeEventRegistrationStatusTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$campaignTypeEventRegistrationStatusTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _messageComponentContentsService.getAllCampaignTypeEventRegistrationStatusForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#CampaignTypeEventRegistrationStatusTableFilter').val()
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

        $('#CampaignTypeEventRegistrationStatusTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getCampaignTypeEventRegistrationStatus() {
            dataTable.ajax.reload();
        }

        $('#GetCampaignTypeEventRegistrationStatusButton').click(function (e) {
            e.preventDefault();
            getCampaignTypeEventRegistrationStatus();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getCampaignTypeEventRegistrationStatus();
            }
        });

    };
})(jQuery);

