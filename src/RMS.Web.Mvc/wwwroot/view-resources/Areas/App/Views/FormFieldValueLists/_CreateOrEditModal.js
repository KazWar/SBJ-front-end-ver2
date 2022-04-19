(function ($) {
    app.modals.CreateOrEditFormFieldValueListModal = function () {

        var _formFieldValueListsService = abp.services.app.formFieldValueLists;

        var _modalManager;
        var _$formFieldValueListInformationForm = null;

		        var _FormFieldValueListformFieldLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormFieldValueLists/FormFieldLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormFieldValueLists/_FormFieldValueListFormFieldLookupTableModal.js',
            modalClass: 'FormFieldLookupTableModal'
        });        var _FormFieldValueListvalueListLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormFieldValueLists/ValueListLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormFieldValueLists/_FormFieldValueListValueListLookupTableModal.js',
            modalClass: 'ValueListLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$formFieldValueListInformationForm = _modalManager.getModal().find('form[name=FormFieldValueListInformationsForm]');
            _$formFieldValueListInformationForm.validate();
        };

		          $('#OpenFormFieldLookupTableButton').click(function () {

            var formFieldValueList = _$formFieldValueListInformationForm.serializeFormToObject();

            _FormFieldValueListformFieldLookupTableModal.open({ id: formFieldValueList.formFieldId, displayName: formFieldValueList.formFieldDescription }, function (data) {
                _$formFieldValueListInformationForm.find('input[name=formFieldDescription]').val(data.displayName); 
                _$formFieldValueListInformationForm.find('input[name=formFieldId]').val(data.id); 
            });
        });
		
		$('#ClearFormFieldDescriptionButton').click(function () {
                _$formFieldValueListInformationForm.find('input[name=formFieldDescription]').val(''); 
                _$formFieldValueListInformationForm.find('input[name=formFieldId]').val(''); 
        });
		
        $('#OpenValueListLookupTableButton').click(function () {

            var formFieldValueList = _$formFieldValueListInformationForm.serializeFormToObject();

            _FormFieldValueListvalueListLookupTableModal.open({ id: formFieldValueList.valueListId, displayName: formFieldValueList.valueListDescription }, function (data) {
                _$formFieldValueListInformationForm.find('input[name=valueListDescription]').val(data.displayName); 
                _$formFieldValueListInformationForm.find('input[name=valueListId]').val(data.id); 
            });
        });
		
		$('#ClearValueListDescriptionButton').click(function () {
                _$formFieldValueListInformationForm.find('input[name=valueListDescription]').val(''); 
                _$formFieldValueListInformationForm.find('input[name=valueListId]').val(''); 
        });
		


        this.save = function () {
            if (!_$formFieldValueListInformationForm.valid()) {
                return;
            }
            if ($('#FormFieldValueList_FormFieldId').prop('required') && $('#FormFieldValueList_FormFieldId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('FormField')));
                return;
            }
            if ($('#FormFieldValueList_ValueListId').prop('required') && $('#FormFieldValueList_ValueListId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ValueList')));
                return;
            }

            var formFieldValueList = _$formFieldValueListInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _formFieldValueListsService.createOrEdit(
				formFieldValueList
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditFormFieldValueListModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);