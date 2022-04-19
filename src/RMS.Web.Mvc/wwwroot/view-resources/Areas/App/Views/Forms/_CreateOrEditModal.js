(function ($) {
    app.modals.CreateOrEditFormModal = function () {

        var _formsService = abp.services.app.forms;

        var _modalManager;
        var _$formInformationForm = null;

		        var _FormsystemLevelLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Forms/SystemLevelLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Forms/_FormSystemLevelLookupTableModal.js',
            modalClass: 'SystemLevelLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$formInformationForm = _modalManager.getModal().find('form[name=FormInformationsForm]');
            _$formInformationForm.validate();
        };

		          $('#OpenSystemLevelLookupTableButton').click(function () {

            var form = _$formInformationForm.serializeFormToObject();

            _FormsystemLevelLookupTableModal.open({ id: form.systemLevelId, displayName: form.systemLevelDescription }, function (data) {
                _$formInformationForm.find('input[name=systemLevelDescription]').val(data.displayName); 
                _$formInformationForm.find('input[name=systemLevelId]').val(data.id); 
            });
        });
		
		$('#ClearSystemLevelDescriptionButton').click(function () {
                _$formInformationForm.find('input[name=systemLevelDescription]').val(''); 
                _$formInformationForm.find('input[name=systemLevelId]').val(''); 
        });
		


        this.save = function () {
            if (!_$formInformationForm.valid()) {
                return;
            }
            if ($('#Form_SystemLevelId').prop('required') && $('#Form_SystemLevelId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('SystemLevel')));
                return;
            }

            var form = _$formInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _formsService.createOrEdit(
				form
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditFormModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);