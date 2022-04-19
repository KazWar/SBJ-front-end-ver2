(function () {
    $(function () {
        var _registrationsService = abp.services.app.registrations;
        var _$registrationInformationForm = $('form[name=RegistrationInformationsForm]');
        var loggedUserId = abp.session.userId;

        _$registrationInformationForm.validate();

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        //Initialization shizzle...
        let lockChangeEvent = $('[id="LockChangeEvent"]');
        let statusChangedEvent = $('[id="StatusChangeEvent"]');
        let lowWrapper = $('#lowWrapper');
        let wrapperRegistrationApproved = $('.row__registration-approved');
        let wrapperRegistrationIncomplete = $('.row__registration-incomplete');
        let wrapperRegistrationRejected = $('.row__registration-rejected');
        let wrapperIncompleteReason = $('.wrapper__warning-choose-incomplete');
        let wrapperRejectionReason = $('.wrapper__warning-choose-rejection');
        let choiceApproval = $('.switch__registration-approval input[type="checkbox"]');
        let choiceApproved = $('#switch__choice-registration-approved');
        let choiceIncomplete = $('#switch__choice-registration-incomplete');
        let choiceRejected = $('#switch__choice-registration-rejected');
        let choiceIncompleteReason = $('.wrapper__warning-choose-incomplete select');
        let choiceRejectionReason = $('.wrapper__warning-choose-rejection select');
        let saveButton = $('#saveBtn');
        let goBackButton = $('#goBackBtn');
        let showHideMessagePreview = $('.showHideMessagePreview');
        let messagePreviewTable = $('.messagePreviewTable');

        $(wrapperRegistrationApproved).addClass('d-none');
        $(wrapperRegistrationIncomplete).addClass('d-none');
        $(wrapperRegistrationRejected).addClass('d-none');
        $(wrapperIncompleteReason).addClass('d-none');
        $(wrapperRejectionReason).addClass('d-none');

        $(choiceApproved).prop('checked', false);
        $(choiceIncomplete).prop('checked', false);
        $(choiceRejected).prop('checked', false);

        $(messagePreviewTable).addClass('d-none');
        $(showHideMessagePreview).text(app.localize('Show') + '...');

        const statusCode = $('[id="StatusCode"]')[0].value;
        
        if (statusCode === '200') {
            //Approved
            $(wrapperRegistrationApproved).removeClass('d-none');
            $(choiceApproved).prop('checked', true);
            $(saveButton).prop('disabled', false);
        } else if (statusCode === '500') {
            //Incomplete
            $(wrapperRegistrationIncomplete).removeClass('d-none');
            $(wrapperRegistrationRejected).removeClass('d-none');
            $(choiceIncomplete).prop('checked', true);
            $(choiceRejected).prop('checked', false);
            $(wrapperIncompleteReason).removeClass('d-none');
            incompleteReasonSaveability();
        } else if (statusCode === '999') {
            //Rejected
            $(wrapperRegistrationRejected).removeClass('d-none');
            $(wrapperRegistrationIncomplete).removeClass('d-none');
            $(choiceRejected).prop('checked', true);
            $(choiceIncomplete).prop('checked', false);
            $(wrapperRejectionReason).removeClass('d-none');
            rejectedReasonSaveability();
        } else if (statusCode === '300' || statusCode === '400') {
            //In Progress or Sent
            $(lowWrapper).addClass('d-none');
            $(saveButton).prop('disabled', false);
        } else {
            //Pending (or some WaitingFor... status)
            $(wrapperRegistrationApproved).removeClass('d-none');
            $(choiceApproved).prop('checked', false);
            $(saveButton).prop('disabled', false);
        }

        $(lockChangeEvent).eq(0).val('0');
        $(statusChangedEvent).eq(0).val('0');
        
        //End of initialization shizzle...

        $(choiceApproved).change(function (e) {
            const lockChangeEventValue = $(lockChangeEvent)[0].value;
            if (Number(lockChangeEventValue) === 1) {
                return e;
            }

            $(lockChangeEvent).eq(0).val('1');
            $(statusChangedEvent).eq(0).val('1');
            /*if (e.target.checked) {
                $(saveButton).prop('disabled', false);
            } else {
                $(saveButton).prop('disabled', true);
            }*/

            $(saveButton).prop('disabled', false);
            $(lockChangeEvent).eq(0).val('0');
        });

        $(choiceIncomplete).change(function (e) {
            const lockChangeEventValue = $(lockChangeEvent)[0].value;
            if (Number(lockChangeEventValue) === 1) {
                return e;
            }

            $(lockChangeEvent).eq(0).val('1');
            $(statusChangedEvent).eq(0).val('1');
            if (e.target.checked) {
                $(choiceRejected).prop('checked', false);
                $(saveButton).prop('disabled', false);
                $(wrapperIncompleteReason).removeClass('d-none');
                $(wrapperRejectionReason).addClass('d-none');
                $('.dropdown__rejection-reason')[0].selectedIndex = 0;

                incompleteReasonSaveability();
            } else {
                $(choiceRejected).prop('checked', true);
                $(saveButton).prop('disabled', true);
                $(wrapperIncompleteReason).addClass('d-none');
                $(wrapperRejectionReason).removeClass('d-none');
                $('.dropdown__incomplete-reason')[0].selectedIndex = 0;

                rejectedReasonSaveability();
            }

            $(lockChangeEvent).eq(0).val('0');
        });

        $(choiceRejected).change(function (e) {
            const lockChangeEventValue = $(lockChangeEvent)[0].value;
            if (Number(lockChangeEventValue) === 1) {
                return e;
            }

            $(lockChangeEvent).eq(0).val('1');
            $(statusChangedEvent).eq(0).val('1');
            if (e.target.checked) {
                $(choiceIncomplete).prop('checked', false);
                $(saveButton).prop('disabled', true);
                $(wrapperRejectionReason).removeClass('d-none');
                $(wrapperIncompleteReason).addClass('d-none');
                $('.dropdown__incomplete-reason')[0].selectedIndex = 0;

                rejectedReasonSaveability();
            } else {
                $(choiceIncomplete.prop('checked', true));
                $(saveButton).prop('disabled', false);
                $(wrapperRejectionReason).addClass('d-none');
                $(wrapperIncompleteReason).removeClass('d-none');
                $('.dropdown__rejection-reason')[0].selectedIndex = 0;

                incompleteReasonSaveability();
            }

            $(lockChangeEvent).eq(0).val('0');
        });

        $(choiceIncompleteReason).change(function () {
            const lockChangeEventValue = $(lockChangeEvent)[0].value;
            if (Number(lockChangeEventValue) === 1) {
                return e;
            }

            $(lockChangeEvent).eq(0).val('1');

            incompleteReasonSaveability();

            $(lockChangeEvent).eq(0).val('0');
        });

        $(choiceRejectionReason).change(function () {
            const lockChangeEventValue = $(lockChangeEvent)[0].value;
            if (Number(lockChangeEventValue) === 1) {
                return e;
            }

            $(lockChangeEvent).eq(0).val('1');
            
            rejectedReasonSaveability();

            $(lockChangeEvent).eq(0).val('0');
        });

        $(choiceApproval).change(function () {
            const lockChangeEventValue = $(lockChangeEvent)[0].value;
            if (Number(lockChangeEventValue) === 1) {
                return e;
            }

            $(lockChangeEvent).eq(0).val('1');
            if (!$(wrapperRegistrationApproved).hasClass('d-none')) {
                $(wrapperRegistrationApproved).addClass('d-none');
            }
            if (!$(wrapperRegistrationIncomplete).hasClass('d-none')) {
                $(wrapperRegistrationIncomplete).addClass('d-none');
            }
            if (!$(wrapperRegistrationRejected).hasClass('d-none')) {
                $(wrapperRegistrationRejected).addClass('d-none');
            }
            if (!$(wrapperRejectionReason).hasClass('d-none')) {
                $(wrapperRejectionReason).addClass('d-none');
            }
            if (!$(wrapperIncompleteReason).hasClass('d-none')) {
                $(wrapperIncompleteReason).addClass('d-none');
            }

            $(choiceApproved).prop('checked', false);
            $(choiceIncomplete).prop('checked', false);
            $(choiceRejected).prop('checked', false);

            $('.dropdown__incomplete-reason').val(-1);
            $('.dropdown__rejection-reason').val(-1);

            $(saveButton).prop('disabled', true);

            const switches = $('.switch__registration-approval input[type="checkbox"]:not(#switch__choice-registration-rejected):not(#switch__choice-registration-incomplete)');
            let allAreChecked = true;

            for (let i = 0; i < switches.length; i++) {
                if (!switches[i].checked) {
                    allAreChecked = false;
                    break;
                }
            }

            if (allAreChecked) {
                $(wrapperRegistrationApproved).removeClass('d-none');
                $(saveButton).prop('disabled', false);
            } else {
                $(wrapperRegistrationIncomplete).removeClass('d-none');
                $(wrapperRegistrationRejected).removeClass('d-none');
            }

            $(lockChangeEvent).eq(0).val('0');
        });

        function incompleteReasonSaveability() {
            const selectedObj = $('.dropdown__incomplete-reason option:selected')[0];
            const selectedValue = $(selectedObj).val();

            if (Number(selectedValue) === -1) {
                $(saveButton).prop('disabled', true);
            } else {
                $(saveButton).prop('disabled', false);
            }
        }

        function rejectedReasonSaveability() {
            const selectedObj = $('.dropdown__rejection-reason option:selected')[0];
            const selectedValue = $(selectedObj).val();

            if (Number(selectedValue) === -1) {
                $(saveButton).prop('disabled', true);
            } else {
                $(saveButton).prop('disabled', false);
            }
        }

        function handleFileSelect(event) {
            let f = event.target.files[0];
            let reader = new FileReader();

            reader.onload = (function (file) {
                return function (e) {
                    const binaryData = e.target.result;
                    const base64String = window.btoa(binaryData);
                }
            })(f);

            reader.readAsBinaryString(f);
        }

        function serializeRegistration() {
            var registrationId = $('[id="RegistrationId"]')[0].value;
            var dataFields = $('[id^="FieldId__"]');
            var dataLineFields = $('[id^="LookupLineFieldId__"]');

            var formFieldCollection = [];

            for (let i = 0; i < dataFields.length; i++) {
                //get fieldId
                let fieldId = dataFields[i].value;

                var dataField_FieldType = $('[id="FieldType__' + fieldId + '"]')[0].value;
                var dataField_FieldLabel = $('[id="FieldLabel__' + fieldId + '"]')[0].value;
                var dataField_FieldSource = $('[id="FieldSource__' + fieldId + '"]')[0].value;
                var dataField_RegistrationField = $('[id="RegistrationField__' + fieldId + '"]')[0].value;
                var dataField_PurchaseRegistrationField = $('[id="PurchaseRegistrationField__' + fieldId + '"]')[0].value;
                var dataField_FallbackFieldId = $('[id="FallbackFieldId__' + fieldId + '"]')[0].value;

                var dataFieldValue = null;

                if (dataField_FieldType === 'FileUploader') {
                    const imgSrcContents = $('[id="' + dataField_FieldType + '__' + fieldId + '__preview"] img').attr('src');
                    dataFieldValue = imgSrcContents;
                } else if (dataField_FieldType === 'CheckBox') {
                    dataFieldValue = $('[id="' + dataField_FieldType + '__' + fieldId + '"]')[0].checked;
                } else if (dataField_FieldType === 'IbanChecker') {
                    dataFieldValue = $('[id="' + dataField_FieldType + '__' + fieldId + '__iban"]')[0].value + '|' + $('[id="' + dataField_FieldType + '__' + fieldId + '__bic"]')[0].value;
                } else if (dataField_FieldType === 'RadioButton') {
                    let radioChecked = $('[name="' + dataField_FieldType + '__' + fieldId + '"]:checked')[0];
                    if (radioChecked !== undefined) {
                        dataFieldValue = radioChecked.value;
                    }
                } else {
                    dataFieldValue = $('[id="' + dataField_FieldType + '__' + fieldId + '"]')[0].value;
                }

                let rejectedObj = $('[id="FieldRejected__' + dataField_FieldType + '__' + fieldId + '"]');
                var dataField_isRejected = $(rejectedObj)[0].checked === false;

                formFieldCollection.push({
                    fieldId: fieldId,
                    fieldType: dataField_FieldType,
                    fieldLabel: dataField_FieldLabel,
                    fieldValue: dataFieldValue,
                    fieldSource: dataField_FieldSource,
                    isRejected: dataField_isRejected,
                    registrationField: dataField_RegistrationField,
                    purchaseRegistrationField: dataField_PurchaseRegistrationField,
                    fallbackFieldId: dataField_FallbackFieldId
                });
            }

            for (let i = 0; i < dataLineFields.length; i++) {
                //get lineId+fieldId
                let lineFieldId = dataLineFields[i].value;

                var dataLineField_FieldId = $('[id="LineFieldId__' + lineFieldId + '"]')[0].value;
                var dataLineField_FieldType = $('[id="LineFieldType__' + lineFieldId + '"]')[0].value;
                var dataLineField_FieldLabel = $('[id="LineFieldLabel__' + lineFieldId + '"]')[0].value;
                var dataLineField_FieldSource = $('[id="LineFieldSource__' + lineFieldId + '"]')[0].value;
                var dataLineField_RegistrationField = $('[id="LineRegistrationField__' + lineFieldId + '"]')[0].value;
                var dataLineField_PurchaseRegistrationField = $('[id="LinePurchaseRegistrationField__' + lineFieldId + '"]')[0].value;
                var dataLineField_FieldLineId = $('[id="LineFieldLineId__' + lineFieldId + '"]')[0].value;
                var dataLineField_FallbackFieldId = $('[id="LineFallbackFieldId__' + lineFieldId + '"]')[0].value;

                var dataLineFieldValue = null;

                if (dataLineField_FieldType === 'FileUploader') {
                    const imgSrcContents = $('[id="Line' + dataLineField_FieldType + '__' + lineFieldId + '__preview"] img').attr('src');
                    dataLineFieldValue = imgSrcContents;
                } else if (dataLineField_FieldType === 'CheckBox') {
                    dataLineFieldValue = $('[id="Line' + dataLineField_FieldType + '__' + lineFieldId + '"]')[0].checked;
                } else if (dataLineField_FieldType === 'IbanChecker') {
                    dataLineFieldValue = $('[id="Line' + dataLineField_FieldType + '__' + lineFieldId + '__iban"]')[0].value + '|' + $('[id="Line' + dataLineField_FieldType + '__' + lineFieldId + '__bic"]')[0].value;
                } else if (dataLineField_FieldType === 'RadioButton') {
                    let radioChecked = $('[name="Line' + dataLineField_FieldType + '__' + lineFieldId + '"]:checked')[0];
                    if (radioChecked !== undefined) {
                        dataLineFieldValue = radioChecked.value;
                    }
                } else {
                    dataLineFieldValue = $('[id="Line' + dataLineField_FieldType + '__' + lineFieldId + '"]')[0].value;
                }

                let rejectedObj = $('[id="LineFieldRejected__' + dataLineField_FieldType + '__' + lineFieldId + '"]');
                var dataLineField_isRejected = $(rejectedObj)[0].checked === false;

                formFieldCollection.push({
                    fieldId: dataLineField_FieldId,
                    fieldType: dataLineField_FieldType,
                    fieldLabel: dataLineField_FieldLabel,
                    fieldValue: dataLineFieldValue,
                    fieldSource: dataLineField_FieldSource,
                    isRejected: dataLineField_isRejected,
                    registrationField: dataLineField_RegistrationField,
                    purchaseRegistrationField: dataLineField_PurchaseRegistrationField,
                    fieldLineId: dataLineField_FieldLineId,
                    fallbackFieldId: dataLineField_FallbackFieldId
                });
            }

            const i_selectedObj = $('.dropdown__incomplete-reason option:selected')[0];
            const i_selectedValue = $(i_selectedObj).val();

            const r_selectedObj = $('.dropdown__rejection-reason option:selected')[0];
            const r_selectedValue = $(r_selectedObj).val();

            return {
                registrationId: registrationId,
                formFields: [...formFieldCollection],
                selectedIncompleteReasonId: Number(i_selectedValue),
                selectedRejectionReasonId: Number(r_selectedValue),
                isApproved: $(choiceApproved)[0].checked
            };
        }

        function save(successCallback) {
            const registration = serializeRegistration();
            abp.ui.setBusy();
            _registrationsService.saveRegistrationForProcessing(
                registration
            ).done(function () {
                const statusChangedEventValue = $(statusChangedEvent)[0].value;
                abp.notify.info(app.localize('SavedSuccessfully'));
                abp.event.trigger('app.createOrEditRegistrationModalSaved');

                if (Number(statusChangedEventValue) === 1) {
                    _registrationsService.composeRegistrationStatusMessaging(registration.registrationId);
                }
                if (typeof (successCallback) === 'function') {
                    successCallback();
                }
            }).always(function () {
                abp.ui.clearBusy();
            });
        };

        $(goBackButton).on('click', function () {
            window.history.back();
        });

        $(saveButton).click(function () {
            save(function () {
                window.location = "/App/Registrations";
            });
        });

        $(showHideMessagePreview).click(function () {
            if ($(messagePreviewTable).hasClass('d-none')) {
                $(messagePreviewTable).removeClass('d-none');
                $(showHideMessagePreview).text(app.localize('Hide') + '...');
            } else {
                $(messagePreviewTable).addClass('d-none');
                $(showHideMessagePreview).text(app.localize('Show') + '...');
            }
        });
    });
})();
