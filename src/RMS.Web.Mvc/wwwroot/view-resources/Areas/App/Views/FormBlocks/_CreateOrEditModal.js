(function ($) {
    app.modals.CreateOrEditFormBlockModal = function () {

        var _formBlocksService = abp.services.app.formBlocks;

        var _modalManager;
        var _$formBlockInformationForm = null;

		        var _FormBlockformLocaleLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormBlocks/FormLocaleLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormBlocks/_FormBlockFormLocaleLookupTableModal.js',
            modalClass: 'FormLocaleLookupTableModal'
        });

        var formLocaleDropdown = $(".formLocale option:selected");
        $('#FormLocaleDescription').val(formLocaleDropdown[0].outerText);
        $('#FormBlock_FormLocaleId').val(formLocaleDropdown.data('formlocaleid'));

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$formBlockInformationForm = _modalManager.getModal().find('form[name=FormBlockInformationsForm]');
            _$formBlockInformationForm.validate();
        };

		          $('#OpenFormLocaleLookupTableButton').click(function () {

            var formBlock = _$formBlockInformationForm.serializeFormToObject();

            _FormBlockformLocaleLookupTableModal.open({ id: formBlock.formLocaleId, displayName: formBlock.formLocaleDescription }, function (data) {
                _$formBlockInformationForm.find('input[name=formLocaleDescription]').val(data.displayName); 
                _$formBlockInformationForm.find('input[name=formLocaleId]').val(data.id); 
            });
        });
		
		$('#ClearFormLocaleDescriptionButton').click(function () {
                _$formBlockInformationForm.find('input[name=formLocaleDescription]').val(''); 
                _$formBlockInformationForm.find('input[name=formLocaleId]').val(''); 
        });
		
        function PopulateFormLocaleBlocks() {
            $('#dvFormLocaleBlock').load('/App/Forms/DisplayFormLocaleBlocks/', { formLocaleId: formLocaleDropdown.data('formlocaleid'), formLocaleText: formLocaleDropdown[0].outerText, localeId: formLocaleDropdown.data('localeid') });
            $('#dvFormLocaleBlock').show();
        }

        this.save = function () {
            if (!_$formBlockInformationForm.valid()) {
                return;
            }
            if ($('#FormBlock_FormLocaleId').prop('required') && $('#FormBlock_FormLocaleId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('FormLocale')));
                return;
            }

            var formBlock = _$formBlockInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _formBlocksService.createOrEdit(
				formBlock
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
                 abp.event.trigger('app.createOrEditFormBlockModalSaved');
                 PopulateFormLocaleBlocks();
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);