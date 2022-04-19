(function () {
    $(function () {

        var _$formFieldsTable = $('#FormFieldsTable');
        var _formFieldsService = abp.services.app.formFields;
        var _entityTypeFullName = 'RMS.SBJ.Forms.FormField';

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.FormFields.Create'),
            edit: abp.auth.hasPermission('Pages.FormFields.Edit'),
            'delete': abp.auth.hasPermission('Pages.FormFields.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/FormFields/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormFields/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditFormFieldModal'
                });
                   

		 var _viewFormFieldModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormFields/ViewformFieldModal',
            modalClass: 'ViewFormFieldModal'
        });

        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }
        
        var getMaxDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z"); 
        }

        var dataTable = _$formFieldsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _formFieldsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#FormFieldsTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					labelFilter: $('#LabelFilterId').val(),
					defaultValueFilter: $('#DefaultValueFilterId').val(),
					minMaxLengthFilter: $('#MinMaxLengthFilterId').val(),
					maxMaxLengthFilter: $('#MaxMaxLengthFilterId').val(),
					requiredFilter: $('#RequiredFilterId').val(),
					readOnlyFilter: $('#ReadOnlyFilterId').val(),
					inputMaskFilter: $('#InputMaskFilterId').val(),
					regularExpressionFilter: $('#RegularExpressionFilterId').val(),
					validationApiCallFilter: $('#ValidationApiCallFilterId').val(),
					registrationFieldFilter: $('#RegistrationFieldFilterId').val(),
					purchaseRegistrationFieldFilter: $('#PurchaseRegistrationFieldFilterId').val(),
					isPurchaseRegistrationFilter: $('#IsPurchaseRegistrationFilterId').val(),
					fieldTypeDescriptionFilter: $('#FieldTypeDescriptionFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    },
                    targets: 0
                },
                {
                    width: 120,
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('View'),
                                iconStyle: 'far fa-eye mr-2',
                                action: function (data) {
                                    _viewFormFieldModal.open({ id: data.record.formField.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.formField.id });                                
                            }
                        },
                        {
                            text: app.localize('History'),
                            iconStyle: 'fas fa-history mr-2',
                            visible: function () {
                                return entityHistoryIsEnabled();
                            },
                            action: function (data) {
                                _entityTypeHistoryModal.open({
                                    entityTypeFullName: _entityTypeFullName,
                                    entityId: data.record.formField.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteFormField(data.record.formField);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "formField.description",
						 name: "description"   
					},
					{
						targets: 3,
						 data: "formField.label",
						 name: "label"   
					},
					{
						targets: 4,
						 data: "formField.defaultValue",
						 name: "defaultValue"   
					},
					{
						targets: 5,
						 data: "formField.maxLength",
						 name: "maxLength"   
					},
					{
						targets: 6,
						 data: "formField.required",
						 name: "required"  ,
						render: function (required) {
							if (required) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 7,
						 data: "formField.readOnly",
						 name: "readOnly"  ,
						render: function (readOnly) {
							if (readOnly) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 8,
						 data: "formField.inputMask",
						 name: "inputMask"   
					},
					{
						targets: 9,
						 data: "formField.regularExpression",
						 name: "regularExpression"   
					},
					{
						targets: 10,
						 data: "formField.validationApiCall",
						 name: "validationApiCall"   
					},
					{
						targets: 11,
						 data: "formField.registrationField",
						 name: "registrationField"   
					},
					{
						targets: 12,
						 data: "formField.purchaseRegistrationField",
						 name: "purchaseRegistrationField"   
					},
					{
						targets: 13,
						 data: "formField.isPurchaseRegistration",
						 name: "isPurchaseRegistration"  ,
						render: function (isPurchaseRegistration) {
							if (isPurchaseRegistration) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 14,
						 data: "fieldTypeDescription" ,
						 name: "fieldTypeFk.description" 
					}
            ]
        });

        function getFormFields() {
            dataTable.ajax.reload();
        }

        function deleteFormField(formField) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _formFieldsService.delete({
                            id: formField.id
                        }).done(function () {
                            getFormFields(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewFormFieldButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _formFieldsService
                .getFormFieldsToExcel({
				filter : $('#FormFieldsTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					labelFilter: $('#LabelFilterId').val(),
					defaultValueFilter: $('#DefaultValueFilterId').val(),
					minMaxLengthFilter: $('#MinMaxLengthFilterId').val(),
					maxMaxLengthFilter: $('#MaxMaxLengthFilterId').val(),
					requiredFilter: $('#RequiredFilterId').val(),
					readOnlyFilter: $('#ReadOnlyFilterId').val(),
					inputMaskFilter: $('#InputMaskFilterId').val(),
					regularExpressionFilter: $('#RegularExpressionFilterId').val(),
					validationApiCallFilter: $('#ValidationApiCallFilterId').val(),
					registrationFieldFilter: $('#RegistrationFieldFilterId').val(),
					purchaseRegistrationFieldFilter: $('#PurchaseRegistrationFieldFilterId').val(),
					isPurchaseRegistrationFilter: $('#IsPurchaseRegistrationFilterId').val(),
					fieldTypeDescriptionFilter: $('#FieldTypeDescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditFormFieldModalSaved', function () {
            getFormFields();
        });

        $('#GetFormFieldsButton').click(function (e) {
            e.preventDefault();
            getFormFields();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getFormFields();
		  }
		});
		
		
		
    });
})();
