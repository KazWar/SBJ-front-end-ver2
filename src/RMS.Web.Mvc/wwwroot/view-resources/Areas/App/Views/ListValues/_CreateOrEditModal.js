(function ($) {
    app.modals.CreateOrEditListValueModal = function () {

        var _listValuesService = abp.services.app.listValues;

        var _modalManager;
        var _$listValueInformationForm = null;

		        var _ListValuevalueListLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ListValues/ValueListLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ListValues/_ListValueValueListLookupTableModal.js',
            modalClass: 'ValueListLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$listValueInformationForm = _modalManager.getModal().find('form[name=ListValueInformationsForm]');
            _$listValueInformationForm.validate();
        };

		          $('#OpenValueListLookupTableButton').click(function () {

            var listValue = _$listValueInformationForm.serializeFormToObject();

            _ListValuevalueListLookupTableModal.open({ id: listValue.valueListId, displayName: listValue.valueListDescription }, function (data) {
                _$listValueInformationForm.find('input[name=valueListDescription]').val(data.displayName); 
                _$listValueInformationForm.find('input[name=valueListId]').val(data.id); 
            });
        });
		
		$('#ClearValueListDescriptionButton').click(function () {
                _$listValueInformationForm.find('input[name=valueListDescription]').val(''); 
                _$listValueInformationForm.find('input[name=valueListId]').val(''); 
        });
		


        this.save = function () {
            if (!_$listValueInformationForm.valid()) {
                return;
            }
            if ($('#ListValue_ValueListId').prop('required') && $('#ListValue_ValueListId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ValueList')));
                return;
            }

            var listValue = _$listValueInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _listValuesService.createOrEdit(
				listValue
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditListValueModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);