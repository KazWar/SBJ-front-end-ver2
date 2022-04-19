(function () {
    $(function () {

        //$('#WaitForExportToFinishText').hide();

        var _$costReportTable = $('#CostReportTable');

        var ddlCampaigns = $('#CampaignDescriptionFilterId');
        var selectedCampaign = 1;

        var costReportData;
        var costReportList;

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

        var dataTable = _$costReportTable.DataTable({
            paging: false,
            serverSide: false,
            processing: false,
            scrollX: true,
            responsive: false,
            data: [{
                'year': '',
                'month': '',
                'newRegistrations': '',
                'completeRegistrations': '',
                'extraHandlingRegistrations': '',
                'paymentBatches': '',
                'paymentsSent': '',
                'activationCodeBatches': '',
                'activationCodesSent': '',
                'premiumBatches': '',
                'premiumsSent': ''
            }],
            columnDefs: [
                {
                    targets: 0,
                    data: "year"
                },
                {
                    targets: 1,
                    data: "month"
                },
                {
                    targets: 2,
                    data: "newRegistrations"
                },
                {
                    targets: 3,
                    data: "completeRegistrations"
                },
                {
                    targets: 4,
                    data: "extraHandlingRegistrations"
                },
                {
                    targets: 5,
                    data: "paymentBatches"
                },
                {
                    targets: 6,
                    data: "paymentsSent"
                },
                {
                    targets: 7,
                    data: "activationCodeBatches"
                },
                {
                    targets: 8,
                    data: "activationCodesSent"
                },
                {
                    targets: 9,
                    data: "premiumBatches"
                },
                {
                    targets: 10,
                    data: "premiumsSent"
                }
            ]
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

        $('#GetCostReportButton').click(function (e) {
            if (dataTable != null) {
                e.preventDefault();
                getCostReport();
            }
        });

        function getCostReport() {
            $.ajax({
                url: 'CostReport/GetCostReport',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    campaignId: selectedCampaign,
                    startDateFilter: getStartDateFilter($('#StartDateFilterId')),
                    endDateFilter: getEndDateFilter($('#EndDateFilterId'))
                }),
                success: function (result) {
                    costReportData = result;
                    $('#CampaignCodeLabel').text(costReportData.campaignCode);
                    $('#CampaignNameLabel').text(costReportData.campaignName);
                    $('#CampaignStartDateLabel').text(costReportData.campaignStart);
                    $('#CampaignEndDateLabel').text(costReportData.campaignEnd);

                    costReportList = costReportData.monthlyTotals;
                    dataTable.clear().rows.add(costReportList).draw();
                    sumTotalsFooter();
                },
                error: function (error) {
                    console.log("Error call to controller - error: " + error);
                }
            });
        }

        function sumTotalsFooter() {
            if (costReportList != null) {
                for (let i = 2; i < dataTable.columns().header().length; i++) {
                    var column = dataTable.column(i);
                    var sum = column.data().reduce(function (a, b) { return a + b; });
                    $(column.footer()).html(sum);
                }
            }
        }

        $('#ExportToExcelButton').click(function () {
            if (getStartDateFilter($('#StartDateFilterId')) <= getEndDateFilter($('#EndDateFilterId'))) {
                $('#ExportToExcelButton').prop('disabled', true);

                $.ajax({
                    url: 'CostReport/GetCostReportToExcel',
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    data: JSON.stringify({
                        campaignId: selectedCampaign,
                        startDateFilter: getStartDateFilter($('#StartDateFilterId')),
                        endDateFilter: getEndDateFilter($('#EndDateFilterId'))
                    }),
                    success: function (result) {
                        if (result.fileType != null) {
                            app.downloadTempFile(result);
                        }
                        else {
                            abp.message.error('Something went wrong with creating the Excel export.', 'Internal Server Error');
                        }

                        $('#ExportToExcelButton').prop('disabled', false);
                    },
                    error: function (error) {
                        console.log("Error call to controller - error: " + error);
                    }
                });
            }
            else {
                abp.message.warn('The start date has to be before or on the end date.');
            }
        });
    });
})();