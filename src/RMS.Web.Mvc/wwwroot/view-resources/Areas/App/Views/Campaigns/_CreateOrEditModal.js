(function ($) {
    app.modals.CreateOrEditCampaignModal = function () {

        var _modalManager;
        var _$campaignInformationForm = null;
        var selectedCampaignCampaignTypeList = [];
        var unselectedCampaignCampaignTypeList = [];

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$campaignInformationForm = _modalManager.getModal().find('form[name=CampaignInformationsForm]');
            _$campaignInformationForm.validate({
                rules: {
                    'campaignTypeIdList[]': {
                        minlength: 1
                    }
                },
                highlight: function (element) {
                    $(element).closest('.form-control').addClass('has-error');
                },
                unhighlight: function (element) {
                    $(element).closest('.form-control').removeClass('has-error');
                },
                messages: {
                    'campaignTypeIdList[]': "Please select at least one campaign Type"
                },
            });
        };

        this.save = function () {
            if (!_$campaignInformationForm.valid()) {
                return;
            }

            var campaign = _$campaignInformationForm.serializeFormToObject();
            var selected = $('#CampaignTypeIdList option:selected');
            var unselected = $('#CampaignTypeIdList option:not(:selected)');
            var campaignId = _$campaignInformationForm.find('input[name=id]').val();

            $.each(selected, function (index, value) {
                selectedCampaignCampaignTypeList.push({
                    CampaignTypeName: value.outerText,
                    CampaignTypeId: $(value).attr("id"),
                    CampaignId: campaignId,
                    CampaignCampaignTypeId: $(value).data("campaign-campaign-type-id"),
                    IsSelected: true
                });
            });

            $.each(unselected, function (i, value) {
                unselectedCampaignCampaignTypeList.push({
                    CampaignCampaignTypeId: $(value).data("campaign-campaign-type-id"),
                });
            });

            var campaignFormViewModel = {
                CampaignForm:
                    campaignForm = {
                        FormId: _$campaignInformationForm.find('input[name=campaignformFormId]').val(),
                        Id: _$campaignInformationForm.find('input[name=campaignformId]').val()
                    }
            };

            var campaignMessageViewModel = {
                CampaignMessage: campaignMessage = {
                    MessageId: _$campaignInformationForm.find('input[name=campaignmessageMessageId]').val(),
                    Id: _$campaignInformationForm.find('input[name=campaignmessageId]').val()
                }
            };

            $.ajax({
                type: 'POST',
                url: '/App/Campaigns/CreateCampaign/',
                contentType: "application/json;charset=utf-8",
                dataType: 'json',
                data: JSON.stringify({
                    campaign: campaign,
                    selectedCampaignCampaignType: selectedCampaignCampaignTypeList,
                    unselectedCampaignCampaignType: unselectedCampaignCampaignTypeList,
                    campaignFormViewModel: campaignFormViewModel,
                    campaignMessageViewModel: campaignMessageViewModel
                })
            }).done(function(){
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditCampaignModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);