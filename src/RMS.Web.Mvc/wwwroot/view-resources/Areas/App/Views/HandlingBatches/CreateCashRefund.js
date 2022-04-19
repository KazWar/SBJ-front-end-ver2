(function () {
    $(function () {
        var _handlingBatchesService = abp.services.app.handlingBatches;
        var _$handlingBatchInformationForm = $('form[name=HandlingBatchInformationsForm]');

        _$handlingBatchInformationForm.validate();

        let inputBatchable = $('.batchable');
        let hiddenBatchableRegistrations = $('#BatchableRegistrations');
        let saveButton = $('#saveBtn');

        function getCampaignBatchablesAsString() {
            var inputBatchables = $('[id^="batchable_"]');
            var campaignBatchables = [];

            for (let i = 0; i < inputBatchables.length; i++) {
                var campaignId = inputBatchables[i].id.split('_')[1];

                campaignBatchables.push({
                    campaignId: Number(campaignId),
                    batchableRegistrationsCount: Number(inputBatchables[i].value)
                });
            }

            return JSON.stringify(campaignBatchables);
        }

        $(inputBatchable).change(function (e) {
            var campaignBatchablesAsString = getCampaignBatchablesAsString();

            abp.ui.setBusy();
            _handlingBatchesService.getInformationForNewCashRefundBatch(
                { campaignBatchables: campaignBatchablesAsString }
            ).done(function (result) {
                hiddenBatchableRegistrations.val(result.totalBatchableRegistrations);

                for (let i = 0; i < result.campaignInformation.length; i++) {
                    var campaignInfo = result.campaignInformation[i];
                    var campaignId = campaignInfo.campaignId;

                    var inputBatchable = $('[id="batchable_' + campaignId + '"]');
                    var labelApproved = $('[id="approved_' + campaignId + '"]');
                    var labelRefund = $('[id="refund_' + campaignId + '"]');

                    inputBatchable.val(campaignInfo.batchableRegistrationsCount);
                    labelApproved.text(campaignInfo.approvedRegistrationsCount);
                    labelRefund.text(campaignInfo.refundAmount.toFixed(2));
                }

                var labelTotalBatchable = $('[id="totalbatchable"]');
                var labelTotalApproved = $('[id="totalapproved"]');
                var labelTotalRefund = $('[id="totalrefund"]');

                labelTotalBatchable.text(result.totalBatchableRegistrationsCount);
                labelTotalApproved.text(result.totalApprovedRegistrationsCount);
                labelTotalRefund.text(result.totalRefundAmount.toFixed(2));

                var totalRow = $('[id="totalrow"]');
                var labelConclusion = $('[id="conclusion"]');

                if (result.totalBatchableRegistrationsCount > 0) {
                    totalRow.css("background-color", "lightgreen");
                    labelConclusion.text("ready to go into batch");
                    $(saveButton).prop('disabled', false);
                }
                else {
                    totalRow.css("background-color", "lightyellow");
                    labelConclusion.text("no registrations picked");
                    $(saveButton).prop('disabled', true);
                }
            }).always(function () {
                abp.ui.clearBusy();
            });
        });

        function save(successCallback) {
            if (!_$handlingBatchInformationForm.valid()) {
                return;
            }

            abp.ui.setBusy();
            _handlingBatchesService.createAndEnqueueNewCashRefundBatch(
                hiddenBatchableRegistrations.val().split(',')
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                abp.event.trigger('app.createOrEditHandlingBatchModalSaved');

                if (typeof (successCallback) === 'function') {
                    successCallback();
                }
            }).always(function () {
                abp.ui.clearBusy();
            });
        };

        function clearForm() {
            _$handlingBatchInformationForm[0].reset();
        }

        $('#saveBtn').click(function () {
            save(function () {
                window.location = "/App/HandlingBatches";
            });
        });

        $('#goBackBtn').on('click', function () {
            window.history.back();
        });
    });
})();