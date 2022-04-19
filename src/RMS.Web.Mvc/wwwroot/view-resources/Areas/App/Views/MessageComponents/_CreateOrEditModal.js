(function ($) {
    app.modals.CreateOrEditMessageComponentModal = function () {

        var _messageComponentsService = abp.services.app.messageComponents;

        var _modalManager;
        var _$messageComponentInformationForm = null;

		        var _MessageComponentmessageTypeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageComponents/MessageTypeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageComponents/_MessageComponentMessageTypeLookupTableModal.js',
            modalClass: 'MessageTypeLookupTableModal'
        });        var _MessageComponentmessageComponentTypeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageComponents/MessageComponentTypeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageComponents/_MessageComponentMessageComponentTypeLookupTableModal.js',
            modalClass: 'MessageComponentTypeLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$messageComponentInformationForm = _modalManager.getModal().find('form[name=MessageComponentInformationsForm]');
            _$messageComponentInformationForm.validate();
        };

		          $('#OpenMessageTypeLookupTableButton').click(function () {

            var messageComponent = _$messageComponentInformationForm.serializeFormToObject();

            _MessageComponentmessageTypeLookupTableModal.open({ id: messageComponent.messageTypeId, displayName: messageComponent.messageTypeName }, function (data) {
                _$messageComponentInformationForm.find('input[name=messageTypeName]').val(data.displayName); 
                _$messageComponentInformationForm.find('input[name=messageTypeId]').val(data.id); 
            });
        });
		
		$('#ClearMessageTypeNameButton').click(function () {
                _$messageComponentInformationForm.find('input[name=messageTypeName]').val(''); 
                _$messageComponentInformationForm.find('input[name=messageTypeId]').val(''); 
        });
		
        $('#OpenMessageComponentTypeLookupTableButton').click(function () {

            var messageComponent = _$messageComponentInformationForm.serializeFormToObject();

            _MessageComponentmessageComponentTypeLookupTableModal.open({ id: messageComponent.messageComponentTypeId, displayName: messageComponent.messageComponentTypeName }, function (data) {
                _$messageComponentInformationForm.find('input[name=messageComponentTypeName]').val(data.displayName); 
                _$messageComponentInformationForm.find('input[name=messageComponentTypeId]').val(data.id); 
            });
        });
		
		$('#ClearMessageComponentTypeNameButton').click(function () {
                _$messageComponentInformationForm.find('input[name=messageComponentTypeName]').val(''); 
                _$messageComponentInformationForm.find('input[name=messageComponentTypeId]').val(''); 
        });
		


        this.save = function () {
            if (!_$messageComponentInformationForm.valid()) {
                return;
            }
            if ($('#MessageComponent_MessageTypeId').prop('required') && $('#MessageComponent_MessageTypeId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('MessageType')));
                return;
            }
            if ($('#MessageComponent_MessageComponentTypeId').prop('required') && $('#MessageComponent_MessageComponentTypeId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('MessageComponentType')));
                return;
            }

            var messageComponent = _$messageComponentInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _messageComponentsService.createOrEdit(
				messageComponent
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditMessageComponentModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);