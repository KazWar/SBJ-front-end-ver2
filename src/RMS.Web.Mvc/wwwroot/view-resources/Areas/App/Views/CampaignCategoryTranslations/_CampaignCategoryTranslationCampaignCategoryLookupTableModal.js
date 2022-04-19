(function ($) {
    app.modals.CampaignCategoryLookupTableModal = function () {

        var _modalManager;

        var _campaignCategoryTranslationsService = abp.services.app.campaignCategoryTranslations;
        var _$campaignCategoryTable = $('#CampaignCategoryTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$campaignCategoryTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _campaignCategoryTranslationsService.getAllCampaignCategoryForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#CampaignCategoryTableFilter').val()
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

        $('#CampaignCategoryTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getCampaignCategory() {
            dataTable.ajax.reload();
        }

        $('#GetCampaignCategoryButton').click(function (e) {
            e.preventDefault();
            getCampaignCategory();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getCampaignCategory();
            }
        });

    };
})(jQuery);

