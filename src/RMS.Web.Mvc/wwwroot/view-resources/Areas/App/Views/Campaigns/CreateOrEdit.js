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

            var selectedCampaignTypes = $('#CampaignTypeIdList option:selected');
            var selectedCampaignTypeIds = [];

            var selectedLocales = $('#LocaleIdList option:selected');
            var selectedLocaleIds = [];

            $.each(selectedCampaignTypes, function (index, value) {
                selectedCampaignTypeIds.push($(value).attr("id"));
            });

            $.each(selectedLocales, function (index, value) {
                selectedLocaleIds.push($(value).attr("id"));
            });

            _campaignsService.createOrEditCustomized(
                campaign,
                JSON.stringify(selectedCampaignTypeIds),
                JSON.stringify(selectedLocaleIds),
                null
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