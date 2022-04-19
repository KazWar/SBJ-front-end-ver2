(function () {
    $(function () {

        $('#WaitForExportToFinishText').hide();

        var _generalReportService = abp.services.app.generalReport;

        var ddlCampaigns = $('#CampaignDescriptionFilterId');
        var selectedCampaign = 1;

        $.ajax({
            url: 'Campaigns/GetCampaignDropdownContent',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify({
                activeCampaignsOnly: 'false'
            }),
            success: function (result) {

                $.each(result, function () {
                    ddlCampaigns.append('<option value="' + this.campaign.id + '">' + this.campaign.name + '</option>');
                });
            },
            error: function (error) {
                console.log("Error call to controller - error: " + error);
            }
        });

        $('#StartDateFilterId').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'DD-MM-YYYY',
            defaultDate: new Date()
        });

        $('#EndDateFilterId').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'DD-MM-YYYY',
            defaultDate: new Date().setDate(new Date().getDate() + 1)
        });

        var getStartDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var getEndDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        $('#CampaignDescriptionFilterId').on('change', updateSelectedCampaign);

        function updateSelectedCampaign() {
            selectedCampaign = $('#CampaignDescriptionFilterId').val();
        }

        $('#ExportToExcelButton').click(function () {
            if (getStartDateFilter($('#StartDateFilterId')) <= getEndDateFilter($('#EndDateFilterId'))) {
                $('#WaitForExportToFinishText').show();
                $('#ExportToExcelButton').prop('disabled', true);

                _generalReportService
                    .getGeneralReportToExcel({
                        campaignFilter: selectedCampaign,
                        startDateFilter: getStartDateFilter($('#StartDateFilterId')),
                        endDateFilter: getEndDateFilter($('#EndDateFilterId'))
                    })
                    .done(function (result) {
                        if (result.fileType != null) {
                            app.downloadTempFile(result);
                        }
                        else {
                            abp.message.error('Something went wrong with creating the Excel export.', 'Internal Server Error');
                        }
                        
                        $('#ExportToExcelButton').prop('disabled', false);
                        $('#WaitForExportToFinishText').hide();
                    });
            }
            else {
                abp.message.warn('The start date has to be before or on the end date.');
            }
        });
    });
})();