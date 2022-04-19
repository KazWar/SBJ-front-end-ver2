﻿(function ($) {
    app.modals.CampaignLookupTableModal = function () {

        var _modalManager;

        var _campaignTranslationsService = abp.services.app.campaignTranslations;
        var _$campaignTable = $('#CampaignTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$campaignTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _campaignTranslationsService.getAllCampaignForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#CampaignTableFilter').val()
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

        $('#CampaignTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getCampaign() {
            dataTable.ajax.reload();
        }

        $('#GetCampaignButton').click(function (e) {
            e.preventDefault();
            getCampaign();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getCampaign();
            }
        });

    };
})(jQuery);

