(function ($) {
    app.modals.CreateOrEditListValueTranslationModal = function () {

        var _listValueTranslationsService = abp.services.app.listValueTranslations;

        var _modalManager;
        var _$listValueTranslationInformationForm = null;

		        var _ListValueTranslationlistValueLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ListValueTranslations/ListValueLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ListValueTranslations/_ListValueTranslationListValueLookupTableModal.js',
            modalClass: 'ListValueLookupTableModal'
        });        var _ListValueTranslationlocaleLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ListValueTranslations/LocaleLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ListValueTranslations/_ListValueTranslationLocaleLookupTableModal.js',
            modalClass: 'LocaleLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$listValueTranslationInformationForm = _modalManager.getModal().find('form[name=ListValueTranslationInformationsForm]');
            _$listValueTranslationInformationForm.validate();
        };

		          $('#OpenListValueLookupTableButton').click(function () {

            var listValueTranslation = _$listValueTranslationInformationForm.serializeFormToObject();

            _ListValueTranslationlistValueLookupTableModal.open({ id: listValueTranslation.listValueId, displayName: listValueTranslation.listValueKeyValue }, function (data) {
                _$listValueTranslationInformationForm.find('input[name=listValueKeyValue]').val(data.displayName); 
                _$listValueTranslationInformationForm.find('input[name=listValueId]').val(data.id); 
            });
        });
		
		$('#ClearListValueKeyValueButton').click(function () {
                _$listValueTranslationInformationForm.find('input[name=listValueKeyValue]').val(''); 
                _$listValueTranslationInformationForm.find('input[name=listValueId]').val(''); 
        });
		
        $('#OpenLocaleLookupTableButton').click(function () {

            var listValueTranslation = _$listValueTranslationInformationForm.serializeFormToObject();

            _ListValueTranslationlocaleLookupTableModal.open({ id: listValueTranslation.localeId, displayName: listValueTranslation.localeLanguageCode }, function (data) {
                _$listValueTranslationInformationForm.find('input[name=localeLanguageCode]').val(data.displayName); 
                _$listValueTranslationInformationForm.find('input[name=localeId]').val(data.id); 
            });
        });
		
		$('#ClearLocaleLanguageCodeButton').click(function () {
                _$listValueTranslationInformationForm.find('input[name=localeLanguageCode]').val(''); 
                _$listValueTranslationInformationForm.find('input[name=localeId]').val(''); 
        });
		


        this.save = function () {
            if (!_$listValueTranslationInformationForm.valid()) {
                return;
            }
            if ($('#ListValueTranslation_ListValueId').prop('required') && $('#ListValueTranslation_ListValueId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ListValue')));
                return;
            }
            if ($('#ListValueTranslation_LocaleId').prop('required') && $('#ListValueTranslation_LocaleId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Locale')));
                return;
            }

            var listValueTranslation = _$listValueTranslationInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _listValueTranslationsService.createOrEdit(
				listValueTranslation
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditListValueTranslationModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);