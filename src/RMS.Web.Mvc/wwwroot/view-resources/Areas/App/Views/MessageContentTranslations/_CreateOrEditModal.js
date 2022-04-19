(function ($) {
    app.modals.CreateOrEditMessageContentTranslationModal = function () {

        var _messageContentTranslationsService = abp.services.app.messageContentTranslations;

        var _modalManager;
        var _$messageContentTranslationInformationForm = null;

		        var _MessageContentTranslationlocaleLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageContentTranslations/LocaleLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageContentTranslations/_MessageContentTranslationLocaleLookupTableModal.js',
            modalClass: 'LocaleLookupTableModal'
        });        var _MessageContentTranslationmessageComponentContentLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageContentTranslations/MessageComponentContentLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageContentTranslations/_MessageContentTranslationMessageComponentContentLookupTableModal.js',
            modalClass: 'MessageComponentContentLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$messageContentTranslationInformationForm = _modalManager.getModal().find('form[name=MessageContentTranslationInformationsForm]');
            _$messageContentTranslationInformationForm.validate();
        };

		          $('#OpenLocaleLookupTableButton').click(function () {

            var messageContentTranslation = _$messageContentTranslationInformationForm.serializeFormToObject();

            _MessageContentTranslationlocaleLookupTableModal.open({ id: messageContentTranslation.localeId, displayName: messageContentTranslation.localeDescription }, function (data) {
                _$messageContentTranslationInformationForm.find('input[name=localeDescription]').val(data.displayName); 
                _$messageContentTranslationInformationForm.find('input[name=localeId]').val(data.id); 
            });
        });
		
		$('#ClearLocaleDescriptionButton').click(function () {
                _$messageContentTranslationInformationForm.find('input[name=localeDescription]').val(''); 
                _$messageContentTranslationInformationForm.find('input[name=localeId]').val(''); 
        });
		
        $('#OpenMessageComponentContentLookupTableButton').click(function () {

            var messageContentTranslation = _$messageContentTranslationInformationForm.serializeFormToObject();

            _MessageContentTranslationmessageComponentContentLookupTableModal.open({ id: messageContentTranslation.messageComponentContentId, displayName: messageContentTranslation.messageComponentContentContent }, function (data) {
                _$messageContentTranslationInformationForm.find('input[name=messageComponentContentContent]').val(data.displayName); 
                _$messageContentTranslationInformationForm.find('input[name=messageComponentContentId]').val(data.id); 
            });
        });
		
		$('#ClearMessageComponentContentContentButton').click(function () {
                _$messageContentTranslationInformationForm.find('input[name=messageComponentContentContent]').val(''); 
                _$messageContentTranslationInformationForm.find('input[name=messageComponentContentId]').val(''); 
        });
		


        this.save = function () {
            if (!_$messageContentTranslationInformationForm.valid()) {
                return;
            }
            if ($('#MessageContentTranslation_LocaleId').prop('required') && $('#MessageContentTranslation_LocaleId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Locale')));
                return;
            }
            if ($('#MessageContentTranslation_MessageComponentContentId').prop('required') && $('#MessageContentTranslation_MessageComponentContentId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('MessageComponentContent')));
                return;
            }

            var messageContentTranslation = _$messageContentTranslationInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _messageContentTranslationsService.createOrEdit(
				messageContentTranslation
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditMessageContentTranslationModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);