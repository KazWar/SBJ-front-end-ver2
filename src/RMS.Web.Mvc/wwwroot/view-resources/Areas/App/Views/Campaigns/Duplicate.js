(function () {
    $(function () {
        var _campaignsService = abp.services.app.campaigns;
        var _$campaignInformationForm = $('form[name=CampaignInformationsForm]');

        let saveButton = $('#saveBtn');
        let goBackButton = $('#goBackBtn');

        function save(successCallback) {           
            if (!_$campaignInformationForm.valid()) {
                return;
            }

            var campaign = _$campaignInformationForm.serializeFormToObject();
            var duplicateFromId = _$campaignInformationForm.find('input[name=did]').val();

            _campaignsService.createOrEditCustomized(
                campaign,
                null,
                null,
                duplicateFromId
            ).done(function (result) {
                _$campaignInformationForm.find('input[name=id]').val(result);

                abp.notify.info(app.localize('SavedSuccessfully'));
                abp.event.trigger('app.createOrEditCampaignModalSaved');

                if (typeof (successCallback) === 'function') {
                    successCallback();
                }
            }).always(function () {
                abp.ui.clearBusy();
            });
        };

        $(saveButton).click(function () {
            save(function () {
                window.location = "/App/Campaigns/CampaignOverview?campaignId=" + _$campaignInformationForm.find('input[name=id]').val() + "&editable=true";
            });
        });

        $(goBackButton).on('click', function () {
            //window.history.back();
            window.location = "/App/Campaigns";
        });
    });
})();