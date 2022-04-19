(function ($) {
    app.modals.CampaignTypeLookupTableModal = function () {

        var _modalManager;

        var _promosService = abp.services.app.promos;
        var _$campaignTypeTable = $('#CampaignTypeTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$campaignTypeTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _promosService.getAllCampaignTypeForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#CampaignTypeTableFilter').val()
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

        $('#CampaignTypeTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getCampaignType() {
            dataTable.ajax.reload();
        }

        $('#GetCampaignTypeButton').click(function (e) {
            e.preventDefault();
            getCampaignType();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getCampaignType();
            }
        });

    };
})(jQuery);

