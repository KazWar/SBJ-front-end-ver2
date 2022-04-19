(function () {
    $(function () {        
        var _formFieldsService = abp.services.app.formFields;
        var _formBlockFieldsService = abp.services.app.formBlockFields;

        let unmappedFieldList = $('[name=UnmappedFieldList]');
        let fieldTargetList = $('[name=FieldTargetList]');

        let updateFormLocaleBlocks = $('[id="UpdateFormLocaleBlocks"]');
        let addFormFieldToFormBlock = $('[name="AddFormFieldToFormBlock"]');

        var editable = ($('[id="Editable"]')[0].value === 'true');
        var formFields = $('[id^="LookupFieldId__"]');

        for (let i = 0; i < formFields.length; i++)
        {
            let lookFieldId = formFields[i].value;
            let formFieldId = $('[id="FieldId__' + lookFieldId + '"]')[0].value;

            if (formFieldId > 0) {
                var fieldTarget = $('[id="FieldTarget__' + lookFieldId + '"]')[0];
                var registrationField = $('[id="RegistrationField__' + lookFieldId + '"]')[0];
                var purchaseRegistrationField = $('[id="PurchaseRegistrationField__' + lookFieldId + '"]')[0];
                var customRegistrationField = $('[id="CustomRegistrationField__' + lookFieldId + '"]')[0];
                var customPurchaseRegistrationField = $('[id="CustomPurchaseRegistrationField__' + lookFieldId + '"]')[0];
                var fieldType = $('[id="FieldType__' + lookFieldId + '"]')[0];
                var fieldName = $('[id="FieldName__' + lookFieldId + '"]')[0];
                var fieldDescription = $('[id="FieldDescription__' + lookFieldId + '"]')[0];
                var maxLength = $('[id="MaxLength__' + lookFieldId + '"]')[0];
                var requiredField = $('[id="RequiredField__' + lookFieldId + '"]')[0];
                var readOnly = $('[id="ReadOnly__' + lookFieldId + '"]')[0];
                var fieldLabelGlobal = $('[id="FieldLabelGlobal__' + lookFieldId + '"]')[0];
                var fieldLabelLocale = $('[id="FieldLabelLocale__' + lookFieldId + '"]')[0];
                var defaultValueGlobal = $('[id="DefaultValueGlobal__' + lookFieldId + '"]')[0];
                var defaultValueLocale = $('[id="DefaultValueLocale__' + lookFieldId + '"]')[0];
                var regularExpressionGlobal = $('[id="RegularExpressionGlobal__' + lookFieldId + '"]')[0];
                var regularExpressionLocale = $('[id="RegularExpressionLocale__' + lookFieldId + '"]')[0];

                let fieldTargetValue = parseInt(fieldTarget.value);

                if (fieldTargetValue !== 0) {
                    $(fieldTarget).prop('disabled', true);
                    $(fieldType).prop('disabled', true);
                    $(fieldName).prop('disabled', true);

                    if (typeof registrationField !== 'undefined') {
                        $(registrationField).prop('disabled', true);
                    }

                    if (typeof purchaseRegistrationField !== 'undefined') {
                        $(purchaseRegistrationField).prop('disabled', true);
                    }

                    if (typeof customRegistrationField !== 'undefined') {
                        $(customRegistrationField).prop('disabled', true);
                    }

                    if (typeof customPurchaseRegistrationField !== 'undefined') {
                        $(customPurchaseRegistrationField).prop('disabled', true);
                    }

                    if (editable === false) {
                        $(fieldDescription).prop('disabled', true);
                        $(maxLength).prop('disabled', true);
                        $(requiredField).prop('disabled', true);
                        $(readOnly).prop('disabled', true);
                        $(fieldLabelGlobal).prop('disabled', true);
                        $(fieldLabelLocale).prop('disabled', true);
                        $(defaultValueGlobal).prop('disabled', true);
                        $(defaultValueLocale).prop('disabled', true);
                        $(regularExpressionGlobal).prop('disabled', true);
                        $(regularExpressionLocale).prop('disabled', true);
                    }
                }              
            }
        }

        if (editable === true) {
            $(".selectedFormLocaleBlocks").sortable({
                axis: 'y',
                connectWith: ".selectedFormLocaleBlocks",
                handle: ".portlet-header",
                cancel: ".portlet-toggle",
                placeholder: "portlet-placeholder ui-corner-all",
                update: function (event, ui) {
                    rearrangeSortOrder();
                },
            });

            $(".selectedFormLocaleFields").sortable({
                axis: 'y',
                connectWith: ".selectedFormLocaleFields",
                handle: ".portlet-header",
                cancel: ".portlet-toggle",
                placeholder: "portlet-placeholder ui-corner-all",
                update: function (event, ui) {
                    rearrangeSortOrder();
                },
            });
        } else {
            var saveChanges = $('[id="SaveChanges"]')[0];
            if (!$(saveChanges).hasClass('d-none')) {
                $(saveChanges).addClass('d-none');
            }
        }

        $(".portlet")
            .addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
            .find(".portlet-header")
            .addClass("ui-widget-header ui-corner-all")
            .append("<span class='ui-icon ui-icon-closethick'></span>")
            .prepend("<span class='ui-icon ui-icon-minusthick portlet-toggle'></span>");

        //$(".availableFormLocaleBlocks").sortable({
        //    axis: 'y',
        //    connectWith: ".selectedFormLocaleBlocks,.emptySelectedFormLocaleBlocks",
        //    dropOnEmpty: true,
        //    handle: ".portlet-header",
        //    cancel: ".portlet-toggle",
        //    placeholder: "portlet-placeholder ui-corner-all",
        //    update: function (event, ui) {
        //        rearrangeSortOrder();
        //    },
        //});

        //$(".availablePortlet")
        //    .addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
        //    .find(".portlet-header")
        //    .addClass("ui-widget-header ui-corner-all")
        //    .append("<span class='ui-icon ui-icon-link'></span>");

        $(".portlet-toggle").on("click", function () {
            var icon = $(this);
            icon.toggleClass("ui-icon-minusthick ui-icon-plusthick");
            icon.closest(".portlet").find(".portlet-content").toggle();
        });

        $(".ui-icon-closethick").on('click', function () {
            var icon = $(this);
            var id = icon.closest('.portlet')[0].id;

            let lookFieldId = id.split('__')[1];
            let formFieldId = $('[id="FieldId__' + lookFieldId + '"]')[0].value;
            let formBlockId = $('[id="BlockId__' + lookFieldId + '"]')[0].value;

            saveUpdateSortOrder(function () {
                saveRemoveFormFieldFromFormBlock(formFieldId, formBlockId, function () {
                    saveUpdateFormFields(function () {
                        PopulateFormLocaleBlocks();
                    });
                });
            });
        });

        var formFieldViews = $('[name^="FormFieldView"]');

        for (let i = 0; i < formFieldViews.length; i++) {
            var formFieldView = formFieldViews[i];
            $(formFieldView).closest(".portlet").find(".portlet-header").first().children().first().toggleClass("ui-icon-minusthick ui-icon-plusthick");
            $(formFieldView).toggle();
            if (editable === false) {
                if (!$(formFieldView).closest(".portlet").find(".portlet-header").first().children().last().hasClass('d-none')) {
                    $(formFieldView).closest(".portlet").find(".portlet-header").first().children().last().addClass('d-none');
                }
            }                       
        }

        $('.emptySelectedFormLocaleBlocks').addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all");

        function PopulateFormLocaleBlocks() {
            var selectedFormLocale = $(".formLocale option:selected");
            $('#dvFormLocaleBlock').load('/App/Forms/DisplayFormLocaleBlocks/', { formLocaleId: selectedFormLocale[0].dataset.formlocaleid, formLocaleText: selectedFormLocale[0].outerText, localeId: selectedFormLocale[0].dataset.localeid });
            $('#dvFormLocaleBlock').show();
        };

        function rearrangeSortOrder() {
            var formFields = $('[id^="LookupFieldId__"]');

            let currentBlockId = 0;
            let rankBlock = 0;
            let rankField = 0;

            for (let i = 0; i < formFields.length; i++) {
                let lookFieldId = formFields[i].value;
                let formBlockId = $('[id="BlockId__' + lookFieldId + '"]')[0].value;
                let sortOrderBlock = parseInt($('[id="SortOrderBlock__' + lookFieldId + '"]')[0].value);
                let sortOrderField = parseInt($('[id="SortOrderField__' + lookFieldId + '"]')[0].value);

                if (sortOrderBlock === 0 || sortOrderField === 0) {
                    continue;
                }

                if (formBlockId !== currentBlockId) {
                    currentBlockId = formBlockId;
                    rankBlock = rankBlock + 1;
                    rankField = 1;
                }

                $('[id="SortOrderBlock__' + lookFieldId + '"]').eq(0).val(rankBlock.toString());
                $('[id="SortOrderField__' + lookFieldId + '"]').eq(0).val(rankField.toString());
                rankField = rankField + 1;
            }
        }

        function serializeUpdateFormFields() {
            var formFieldCollection = [];
            var formFields = $('[id^="LookupFieldId__"]');

            for (let i = 0; i < formFields.length; i++) {
                let lookFieldId = formFields[i].value;
                let formFieldId = $('[id="FieldId__' + lookFieldId + '"]')[0].value;

                let localeId = $('[id="LocaleId__' + lookFieldId + '"]')[0].value;
                let fieldDescription = $('[id="FieldDescription__' + lookFieldId + '"]')[0].value;
                let maxLength = $('[id="MaxLength__' + lookFieldId + '"]')[0].value;
                let requiredField = $('[id="RequiredField__' + lookFieldId + '"]')[0].checked;
                let readOnly = $('[id="ReadOnly__' + lookFieldId + '"]')[0].checked;
                let fieldLabelGlobal = $('[id="FieldLabelGlobal__' + lookFieldId + '"]')[0].value;
                let fieldLabelLocale = $('[id="FieldLabelLocale__' + lookFieldId + '"]')[0].value;
                let defaultValueGlobal = $('[id="DefaultValueGlobal__' + lookFieldId + '"]')[0].value;
                let defaultValueLocale = $('[id="DefaultValueLocale__' + lookFieldId + '"]')[0].value;
                let regularExpressionGlobal = $('[id="RegularExpressionGlobal__' + lookFieldId + '"]')[0].value;
                let regularExpressionLocale = $('[id="RegularExpressionLocale__' + lookFieldId + '"]')[0].value;

                formFieldCollection.push({
                    fieldId: formFieldId,
                    localeId: localeId,
                    fieldDescription: fieldDescription,
                    maxLength: maxLength,
                    requiredField: requiredField,
                    readOnly: readOnly,
                    fieldLabelGlobal: fieldLabelGlobal,
                    fieldLabelLocale: fieldLabelLocale,
                    defaultValueGlobal: defaultValueGlobal,
                    defaultValueLocale: defaultValueLocale,
                    regularExpressionGlobal: regularExpressionGlobal,
                    regularExpressionLocale: regularExpressionLocale
                });
            }

            return {
                editedFormFields: [...formFieldCollection]
            };
        }

        function serializeUpdateSortOrder() {
            var sortOrderCollection = [];
            var formFields = $('[id^="LookupFieldId__"]');

            for (let i = 0; i < formFields.length; i++) {
                let lookFieldId = formFields[i].value;
                let blockId = $('[id="BlockId__' + lookFieldId + '"]')[0].value;
                let fieldId = $('[id="FieldId__' + lookFieldId + '"]')[0].value;
                let sortOrderBlock = parseInt($('[id="SortOrderBlock__' + lookFieldId + '"]')[0].value);
                let sortOrderField = parseInt($('[id="SortOrderField__' + lookFieldId + '"]')[0].value);

                if (sortOrderBlock === 0 || sortOrderField === 0) {
                    continue;
                }

                sortOrderCollection.push({
                    blockId: blockId,
                    fieldId: fieldId,
                    sortOrderBlock: sortOrderBlock,
                    sortOrderField: sortOrderField
                });
            }

            return {
                editedSortOrder: [...sortOrderCollection]
            };
        }

        function saveUpdateFormFields(successCallback) {
            const updateFormFields = serializeUpdateFormFields();
            abp.ui.setBusy();
            _formFieldsService.updateFormFields(
                updateFormFields
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                abp.event.trigger('app.createOrEditCampaignModalSaved');
                if (typeof (successCallback) === 'function') {
                    successCallback();
                }
            }).always(function () {
                abp.ui.clearBusy();
            });
        }

        function saveUpdateSortOrder(successCallback) {
            const updateSortOrder = serializeUpdateSortOrder();
            abp.ui.setBusy();
            _formBlockFieldsService.updateSortOrder(
                updateSortOrder
            ).done(function () {
                if (typeof (successCallback) === 'function') {
                    successCallback();
                }
            }).always(function () {
                abp.ui.clearBusy();
            });
        }

        function saveAddFormFieldToFormBlock(formFieldId, formBlockId, successCallback) {
            abp.ui.setBusy();
            _formBlockFieldsService.addFormFieldToFormBlock(
                formFieldId,
                formBlockId
            ).done(function () {
                if (typeof (successCallback) === 'function') {
                    successCallback();
                }
            }).always(function () {
                abp.ui.clearBusy();
            });
        }

        function saveRemoveFormFieldFromFormBlock(formFieldId, formBlockId, successCallback) {
            abp.ui.setBusy();
            _formBlockFieldsService.removeFormFieldFromFormBlock(
                formFieldId,
                formBlockId
            ).done(function () {
                if (typeof (successCallback) === 'function') {
                    successCallback();
                }
            }).always(function () {
                abp.ui.clearBusy();
            });
        }

        $(unmappedFieldList).change(function (e) {
            var fieldId = e.target.value;
            var fieldViews = $('[id^="UnmappedFieldView__"]');
            var fieldButts = $('[id^="UnmappedFieldButt__"]');

            for (let i = 0; i < fieldViews.length; i++) {
                var fieldView = fieldViews[i];
                var fieldButt = fieldButts[i];
                if (!$(fieldView).hasClass('d-none')) {
                    $(fieldView).addClass('d-none');
                }
                if (!$(fieldButt).hasClass('d-none')) {
                    $(fieldButt).addClass('d-none');
                }
            }

            if (fieldId > 0) {
                var fieldView = $('[id="UnmappedFieldView__' + fieldId + '"]')[0];
                var fieldButt = $('[id="UnmappedFieldButt__' + fieldId + '"]')[0];
                $(fieldView).removeClass('d-none');
                if (editable === true) {
                    $(fieldButt).removeClass('d-none');
                }
            }
        });

        $(fieldTargetList).change(function (e) {
            var id = e.target.id;
            var targetId = parseInt(e.target.value);

            let uniqueId = id.split('__')[1];

            var registrationFieldRow = $('[id="RegistrationFieldRow__' + uniqueId + '"]');
            var purchaseRegistrationFieldRow = $('[id="PurchaseRegistrationFieldRow__' + uniqueId + '"]');
            var customRegistrationFieldRow = $('[id="CustomRegistrationFieldRow__' + uniqueId + '"]');
            var customPurchaseRegistrationFieldRow = $('[id="CustomPurchaseRegistrationFieldRow__' + uniqueId + '"]');

            if (!$(registrationFieldRow).hasClass('d-none')) {
                $(registrationFieldRow).addClass('d-none');
            }
            if (!$(purchaseRegistrationFieldRow).hasClass('d-none')) {
                $(purchaseRegistrationFieldRow).addClass('d-none');
            }
            if (!$(customRegistrationFieldRow).hasClass('d-none')) {
                $(customRegistrationFieldRow).addClass('d-none');
            }
            if (!$(customPurchaseRegistrationFieldRow).hasClass('d-none')) {
                $(customPurchaseRegistrationFieldRow).addClass('d-none');
            }

            if (targetId == 1) {
                $(registrationFieldRow).removeClass('d-none');
            } else if (targetId == 2) {
                $(purchaseRegistrationFieldRow).removeClass('d-none');
            } else if (targetId == 3) {
                $(customRegistrationFieldRow).removeClass('d-none');
            } else if (targetId == 4) {
                $(customPurchaseRegistrationFieldRow).removeClass('d-none');
            }
        });

        $(updateFormLocaleBlocks).on('click', function () {
            saveUpdateSortOrder(function () {
                saveUpdateFormFields(function () {
                    window.location = "/App/Campaigns";
                });
            });
        });

        $(addFormFieldToFormBlock).on('click', function (e) {
            var id = e.target.id;

            let lookFieldId = id.split('__')[1];
            let formFieldId = $('[id="FieldId__' + lookFieldId + '"]')[0].value;
            let formBlockId = $('[id="BlockId__' + lookFieldId + '"]')[0].value;

            saveUpdateSortOrder(function () {
                saveAddFormFieldToFormBlock(formFieldId, formBlockId, function () {
                    saveUpdateFormFields(function () {
                        PopulateFormLocaleBlocks();
                    });
                });
            });
        });

        //$('#CreateNewFormBlockButton').click(function () {
        //    _createOrEditFormBlockModal.open();
        //});
    });
})();