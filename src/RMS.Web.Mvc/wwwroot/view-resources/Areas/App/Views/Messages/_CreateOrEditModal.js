(function ($) {
    app.modals.CreateOrEditMessageModal = function () {

        var _messagesService = abp.services.app.messages;

        var _modalManager;
        var _$messageInformationForm = null;

		        var _MessagesystemLevelLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Messages/SystemLevelLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Messages/_MessageSystemLevelLookupTableModal.js',
            modalClass: 'SystemLevelLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$messageInformationForm = _modalManager.getModal().find('form[name=MessageInformationsForm]');
            _$messageInformationForm.validate();
        };

		          $('#OpenSystemLevelLookupTableButton').click(function () {

            var message = _$messageInformationForm.serializeFormToObject();

            _MessagesystemLevelLookupTableModal.open({ id: message.systemLevelId, displayName: message.systemLevelDescription }, function (data) {
                _$messageInformationForm.find('input[name=systemLevelDescription]').val(data.displayName); 
                _$messageInformationForm.find('input[name=systemLevelId]').val(data.id); 
            });
        });
		
		$('#ClearSystemLevelDescriptionButton').click(function () {
                _$messageInformationForm.find('input[name=systemLevelDescription]').val(''); 
                _$messageInformationForm.find('input[name=systemLevelId]').val(''); 
        });
		


        this.save = function () {
            if (!_$messageInformationForm.valid()) {
                return;
            }
            if ($('#Message_SystemLevelId').prop('required') && $('#Message_SystemLevelId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('SystemLevel')));
                return;
            }

            var message = _$messageInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _messagesService.createOrEdit(
				message
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditMessageModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);