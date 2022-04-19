(function ($) {
    app.modals.CreateOrEditMessageTypeModal = function () {

        var _messageTypesService = abp.services.app.messageTypes;

        var _modalManager;
        var _$messageTypeInformationForm = null;

		        var _MessageTypemessageLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageTypes/MessageLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageTypes/_MessageTypeMessageLookupTableModal.js',
            modalClass: 'MessageLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$messageTypeInformationForm = _modalManager.getModal().find('form[name=MessageTypeInformationsForm]');
            _$messageTypeInformationForm.validate();
        };

		          $('#OpenMessageLookupTableButton').click(function () {

            var messageType = _$messageTypeInformationForm.serializeFormToObject();

            _MessageTypemessageLookupTableModal.open({ id: messageType.messageId, displayName: messageType.messageVersion }, function (data) {
                _$messageTypeInformationForm.find('input[name=messageVersion]').val(data.displayName); 
                _$messageTypeInformationForm.find('input[name=messageId]').val(data.id); 
            });
        });
		
		$('#ClearMessageVersionButton').click(function () {
                _$messageTypeInformationForm.find('input[name=messageVersion]').val(''); 
                _$messageTypeInformationForm.find('input[name=messageId]').val(''); 
        });
		


        this.save = function () {
            if (!_$messageTypeInformationForm.valid()) {
                return;
            }
            if ($('#MessageType_MessageId').prop('required') && $('#MessageType_MessageId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Message')));
                return;
            }

            var messageType = _$messageTypeInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _messageTypesService.createOrEdit(
				messageType
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditMessageTypeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);