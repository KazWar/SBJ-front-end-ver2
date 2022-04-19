
(function ($) {
    $(function () {
        var _promosService = abp.services.app.promos;
        var _promoStepsService = abp.services.app.promoSteps;
        var _promoProductsService = abp.services.app.promoProducts;
        var _productCategoriesService = abp.services.app.productCategories;
        var _promoStepDataService = abp.services.app.promoStepDatas;
        var _promoStepFieldDataService = abp.services.app.promoStepFieldDatas;

        var _$promoInformationForm = null;

        var _PromoPromoScopeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Promos/PromoScopeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Promos/_PromoPromoScopeLookupTableModal.js',
            modalClass: 'PromoScopeLookupTableModal'
        });

        var _PromoCampaignTypeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Promos/CampaignTypeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Promos/_PromoCampaignTypeLookupTableModal.js',
            modalClass: 'CampaignTypeLookupTableModal'
        });

        var _PromoProductCategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Promos/ProductCategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Promos/_PromoProductCategoryLookupTableModal.js',
            modalClass: 'ProductCategoryLookupTableModal'
        });

        var _PromoStepFieldLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Promos/PromoStepFieldLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Promos/_PromoStepFieldLookupTableModal.js',
            modalClass: 'PromoStepFieldLookupTableModal'
        });

        _$promoInformationForm = $('form[name=PromoInformationsForm]');
        _$promoInformationForm.validate();

        let selectedCountryIds = _$promoInformationForm.find('input[name=selectedCountryIds]').val();
        let selectedPromoScopeId = _$promoInformationForm.find('input[name=promoScopeId]').val();
        let selectedCampaignTypeId = _$promoInformationForm.find('input[name=campaignTypeId]').val();
        let selectedProductCategoryId = _$promoInformationForm.find('input[name=productCategoryId]').val();

        if (selectedCountryIds) {
            let _selectedCountryIds = selectedCountryIds.split(",");

            Array.from(document.querySelector("#countries").options).forEach(function (option_element) {
                if (_selectedCountryIds.includes(option_element.value)) {
                    option_element.selected = true;
                }
                else {
                    option_element.selected = false;
                };
            });
        }

        if (selectedPromoScopeId) {
            Array.from(document.querySelector("#promoScope").options).forEach(function (option_element) {
                if (selectedPromoScopeId == option_element.value) {
                    option_element.selected = true;
                }
            });
        }

        if (selectedCampaignTypeId) {
            Array.from(document.querySelector("#campaignType").options).forEach(function (option_element) {
                if (selectedCampaignTypeId == option_element.value) {
                    option_element.selected = true;
                }
            });
        }

        if (selectedProductCategoryId) {
            Array.from(document.querySelector("#productCategory").options).forEach(function (option_element) {
                if (selectedProductCategoryId == option_element.value) {
                    option_element.selected = true;
                }
            });
        }

        function GetPoNumber(name) {
            var poNumber = null;
            var result = null;
            if (selectedProductCategoryId) {
                //abp.ui.setBusy();
                _productCategoriesService.getProductCategoryForView(selectedProductCategoryId)
                    .done(function (cb) {

                        if (cb) {
                            result = cb;
                            //if (name == "Handling") poNumber =  cb.productCateGory.POHandling;
                            //if (name == "Cashback") poNumber =  cb.productCateGory.POCashback;

                        }
                    }).always(function () {
                        abp.ui.clearBusy();
                    });


            }
            return poNumber;
        }

        $('#PoNumberHandling').click(function () {
            var result = null;
            if (selectedProductCategoryId) {
                _productCategoriesService.getProductCategoryForView(selectedProductCategoryId)
                    .done(function (cb) {

                        if (cb) {
                            result = cb;
                        }
                    });
            }
        });

        $('#PoNumberCashback').click(function () {
            var po = GetPoNumber("Cashback");
            alert(po);
        });



        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        $('#OpenPromoScopeLookupTableButton').click(function () {

            var promo = _$promoInformationForm.serializeFormToObject();

            _PromoPromoScopeLookupTableModal.open({ id: promo.promoScopeId, displayName: promo.promoScopeDescription }, function (data) {
                _$promoInformationForm.find('input[name=promoScopeDescription]').val(data.displayName);
                _$promoInformationForm.find('input[name=promoScopeId]').val(data.id);
            });
        });

        $('#ClearPromoScopeDescriptionButton').click(function () {
            _$promoInformationForm.find('input[name=promoScopeDescription]').val('');
            _$promoInformationForm.find('input[name=promoScopeId]').val('');
        });

        $('#OpenCampaignTypeLookupTableButton').click(function () {

            var promo = _$promoInformationForm.serializeFormToObject();

            _PromoCampaignTypeLookupTableModal.open({ id: promo.campaignTypeId, displayName: promo.campaignTypeName }, function (data) {
                _$promoInformationForm.find('input[name=campaignTypeName]').val(data.displayName);
                _$promoInformationForm.find('input[name=campaignTypeId]').val(data.id);
            });
        });

        $('#ClearCampaignTypeNameButton').click(function () {
            _$promoInformationForm.find('input[name=campaignTypeName]').val('');
            _$promoInformationForm.find('input[name=campaignTypeId]').val('');
        });

        $('#OpenProductCategoryLookupTableButton').click(function () {

            var promo = _$promoInformationForm.serializeFormToObject();

            _PromoProductCategoryLookupTableModal.open({ id: promo.productCategoryId, displayName: promo.productCategoryDescription }, function (data) {
                _$promoInformationForm.find('input[name=productCategoryDescription]').val(data.displayName);
                _$promoInformationForm.find('input[name=productCategoryId]').val(data.id);
            });
        });

        $('#ClearProductCategoryDescriptionButton').click(function () {
            _$promoInformationForm.find('input[name=productCategoryDescription]').val('');
            _$promoInformationForm.find('input[name=productCategoryId]').val('');
        });

        $('#CancelButton').click(function () {
            window.location.href = '/App/Promos';
        });

        $('.btn-confirm-field-value').click(function (e) {
            let promoStepId = $(e.target).data('promostep-id');
            let sequenceNumber = $(e.target).data('promostep-sequence');
            let promoStep = $('.row__promostep')[sequenceNumber - 1];
            let fields = $(promoStep).find('.input__field-value');

            let values = [];
            // Make field values read-only
            // Push ConfirmationDate to [PromoStepData]
            // Push field values to [PromoStepFieldData]
            for (let i = 0; i < fields.length; i++) {
                let field = $(fields[i]);
                let promoStepFieldId = $(field).data('promostepfield-id');

                values.push({
                    promoStepFieldId: promoStepFieldId,
                    value: field[0].value
                });
            }

            let promoId = new URL(window.location.href).searchParams.get('id');
            let confirmationDate = new Date().toISOString();
            let description = $(promoStep).find('.promostep__description').data('promostep-description');

            abp.ui.setBusy();

            _promoStepDataService.createOrEdit({
                confirmationDate: confirmationDate,
                description: description,
                promoStepId: promoStepId,
                promoId: promoId
            }).done(function (cb) {
                if (cb === null || !cb) return;
                // now that confirmation date has been created/inserted, we need to push the values to [PromoStepFieldData]
                for (let i = 0; i < values.length; i++) {
                    _promoStepFieldDataService.createOrEdit({
                        value: values[i].value,
                        promoStepFieldId: values[i].promoStepFieldId,
                        promoStepDataId: cb
                    }).done(function (callback) {
                        console.log('Finished createOrEdit call in PromoStepFieldDataService. Callback:', callback);
                    }).always(function () {
                        abp.ui.clearBusy();
                    });
                }
            });
        });

        $('#SaveButton').click(function () {
            if (!_$promoInformationForm.valid()) {
                return;
            }

            var promo = _$promoInformationForm.serializeFormToObject();

            if (!promo.id) {
                //new promo: give promocode a temporary value...
                promo.promocode = Math.random();
            }

            let selectedCountryIds = [];

            Array.from(document.querySelector("#countries").options).forEach(function (option_element) {
                let is_option_selected = option_element.selected;

                if (is_option_selected) {
                    let option_value = option_element.value;
                    selectedCountryIds.push(option_value);
                }
            });

            promo.selectedCountries = selectedCountryIds;

            Array.from(document.querySelector("#promoScope").options).forEach(function (option_element) {
                if (option_element.selected == true) {
                    promo.promoScopeId = option_element.value;
                }
            });

            Array.from(document.querySelector("#campaignType").options).forEach(function (option_element) {
                if (option_element.selected == true) {
                    promo.campaignTypeId = option_element.value;
                }
            });

            Array.from(document.querySelector("#productCategory").options).forEach(function (option_element) {
                if (option_element.selected == true) {
                    promo.productCategoryId = option_element.value;
                }
            });

            promo.promoSteps = [];

            /* Get values for promosteps */
            var promoStepRows = $('.row__promostep');

            for (let i = 0; i < promoStepRows.length; i++) {
                let row = $(promoStepRows[i]);
                let promoStepId = $(row[0]).data('promostep-id');
                let promoStepSequence = $(row[0]).data('promostep-sequence');
                let promoStepDescription = $(row[0]).find('.promostep__description').data('promostep-description');

                promo.promoSteps.push({
                    stepId: promoStepId,
                    sequence: promoStepSequence,
                    description: promoStepDescription,
                    promoStepFields: []
                });

                var promoStepFields = $($(row[0]).find('.promostep__promostep-fields li'));

                for (let j = 0; j < promoStepFields.length; j++) {
                    let fieldRow = $(promoStepFields[j]);
                    let promoStepFieldId = $($(fieldRow[0]).find('.promostep__field')).data('promostepfield-id');
                    let fieldName = $($(fieldRow[0]).find('.promostep__field')).text().trim();
                    let fieldValue = $($(fieldRow[0]).find('.input__field-value')).val().trim();

                    promo.promoSteps[i].promoStepFields.push({
                        fieldId: promoStepFieldId,
                        fieldName: fieldName,
                        fieldValue: fieldValue
                    });
                }
            }

            abp.ui.setBusy();

            $.ajax({
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                url: 'SavePromo',
                data: JSON.stringify({ promo: promo, selectedCountryIds: selectedCountryIds }),
                complete: function (data) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    abp.ui.clearBusy();
                    window.location.href = "/App/Promos";
                }
            });

            /*
            _promosService.customCreateOrEdit({ promo: promo, selectedCountryIds: selectedCountryIds }).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                window.location.href = "/App/Promos";
            }).always(function () {
                abp.ui.clearBusy();
            });*/
        });

    });
})(jQuery);