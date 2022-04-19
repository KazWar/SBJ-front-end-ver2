(function ($) {
    app.modals.ChooseFormFieldsModal = function () {

        var formFieldsSelected = [];
        var formFieldsAvailable = [];
        var _modalManager;
        var _$formBlockInformationForm = null;
        var selectedFormLocale = $(".formLocale option:selected");

        var formFieldDualList = new DualListbox('.formFieldDualList', {
            addButtonText: '>',
            removeButtonText: '<'
        }
        );

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

        function PopulateFormLocaleBlocks() {
            $('#dvFormLocaleBlock').load('/App/Forms/DisplayFormLocaleBlocks/', { formLocaleId: selectedFormLocale[0].dataset.formlocaleid, formLocaleText: selectedFormLocale[0].outerText, localeId: selectedFormLocale[0].dataset.localeid });
            $('#dvFormLocaleBlock').show();
        };

        this.save = function () {
            var availableList = formFieldDualList.available;
            var selectedList = formFieldDualList.selected;
            $.each(selectedList, function (i, val) {
                formFieldsSelected.push({
                    formFieldId: val.dataset.id,
                    formBlockId: $('.formFieldDualList').attr('data-formBlockId'),
                    sortOrder: i + 1,
                });
            });

            $.each(availableList, function (i, val) {
                formFieldsAvailable.push({
                    formFieldId: val.dataset.id,
                    formBlockId: $('.formFieldDualList').attr('data-formBlockId')
                });
            });

            $.ajax({
                type: 'POST',
                url: '/App/FormBlocks/UpdateFormBlockFields/',
                contentType: "application/json",
                dataType: 'json',
                data: JSON.stringify({
                    formBlockId: $('.formFieldDualList').attr('data-formBlockId'),
                    SelectedFormFields: formFieldsSelected,
                    AvailableFormFields: formFieldsAvailable
                })
            }).done(function () {
                _modalManager.close();
                PopulateFormLocaleBlocks();
                abp.notify.info(app.localize('SavedSuccessfully'));
            }).fail(function () {
                abp.notify.info('Error while saving the changes');
            });
        };
    };
})(jQuery);