(function ($) {
    app.modals.CreateOrEditMessageComponentContentModal = function () {
        var _messageComponentContentsService = abp.services.app.messageComponentContents;
        var _modalManager;
        var _$messageComponentContentInformationForm = null;

        var _MessageComponentContentmessageComponentLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageComponentContents/MessageComponentLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageComponentContents/_MessageComponentContentMessageComponentLookupTableModal.js',
            modalClass: 'MessageComponentLookupTableModal'
        });
        var _MessageComponentContentcampaignTypeEventRegistrationStatusLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageComponentContents/CampaignTypeEventRegistrationStatusLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageComponentContents/_MessageComponentContentCTERSLookupTableModal.js',
            modalClass: 'CampaignTypeEventRegistrationStatusLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$messageComponentContentInformationForm = _modalManager.getModal().find('form[name=MessageComponentContentInformationsForm]');
            _$messageComponentContentInformationForm.validate();
        };

        $('#OpenMessageComponentLookupTableButton').click(function () {

            var messageComponentContent = _$messageComponentContentInformationForm.serializeFormToObject();

            _MessageComponentContentmessageComponentLookupTableModal.open({ id: messageComponentContent.messageComponentId, displayName: messageComponentContent.messageComponentIsActive }, function (data) {
                _$messageComponentContentInformationForm.find('input[name=messageComponentIsActive]').val(data.displayName);
                _$messageComponentContentInformationForm.find('input[name=messageComponentId]').val(data.id);
            });
        });

        $('#ClearMessageComponentIsActiveButton').click(function () {
            _$messageComponentContentInformationForm.find('input[name=messageComponentIsActive]').val('');
            _$messageComponentContentInformationForm.find('input[name=messageComponentId]').val('');
        });

        $('#OpenCampaignTypeEventRegistrationStatusLookupTableButton').click(function () {

            var messageComponentContent = _$messageComponentContentInformationForm.serializeFormToObject();

            _MessageComponentContentcampaignTypeEventRegistrationStatusLookupTableModal.open({ id: messageComponentContent.campaignTypeEventRegistrationStatusId, displayName: messageComponentContent.campaignTypeEventRegistrationStatusSortOrder }, function (data) {
                _$messageComponentContentInformationForm.find('input[name=campaignTypeEventRegistrationStatusSortOrder]').val(data.displayName);
                _$messageComponentContentInformationForm.find('input[name=campaignTypeEventRegistrationStatusId]').val(data.id);
            });
        });

        $('#ClearCampaignTypeEventRegistrationStatusSortOrderButton').click(function () {
            _$messageComponentContentInformationForm.find('input[name=campaignTypeEventRegistrationStatusSortOrder]').val('');
            _$messageComponentContentInformationForm.find('input[name=campaignTypeEventRegistrationStatusId]').val('');
        });

        this.save = function () {
            if (!_$messageComponentContentInformationForm.valid()) {
                return;
            }
            if ($('#MessageComponentContent_MessageComponentId').prop('required') && $('#MessageComponentContent_MessageComponentId').val() === '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('MessageComponent')));
                return;
            }
            if ($('#MessageComponentContent_CampaignTypeEventRegistrationStatusId').prop('required') && $('#MessageComponentContent_CampaignTypeEventRegistrationStatusId').val() === '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('CampaignTypeEventRegistrationStatus')));
                return;
            }

            var messageComponentContent = _$messageComponentContentInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _messageComponentContentsService.createOrEdit(
                messageComponentContent
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditMessageComponentContentModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);