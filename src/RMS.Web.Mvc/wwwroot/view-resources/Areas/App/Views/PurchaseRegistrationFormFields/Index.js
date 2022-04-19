(function () {
    $(function () {

        var _$purchaseRegistrationFormFieldsTable = $('#PurchaseRegistrationFormFieldsTable');
        var _purchaseRegistrationFormFieldsService = abp.services.app.purchaseRegistrationFormFields;
		var _entityTypeFullName = 'RMS.SBJ.PurchaseRegistrationFormFields.PurchaseRegistrationFormField';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PurchaseRegistrationFormFields.Create'),
            edit: abp.auth.hasPermission('Pages.PurchaseRegistrationFormFields.Edit'),
            'delete': abp.auth.hasPermission('Pages.PurchaseRegistrationFormFields.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/PurchaseRegistrationFormFields/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PurchaseRegistrationFormFields/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditPurchaseRegistrationFormFieldModal'
                });
                   

		 var _viewPurchaseRegistrationFormFieldModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PurchaseRegistrationFormFields/ViewpurchaseRegistrationFormFieldModal',
            modalClass: 'ViewPurchaseRegistrationFormFieldModal'
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

        var dataTable = _$purchaseRegistrationFormFieldsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _purchaseRegistrationFormFieldsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PurchaseRegistrationFormFieldsTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					formFieldDescriptionFilter: $('#FormFieldDescriptionFilterId').val()
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
                                    _viewPurchaseRegistrationFormFieldModal.open({ id: data.record.purchaseRegistrationFormField.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.purchaseRegistrationFormField.id });                                
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
                                    entityId: data.record.purchaseRegistrationFormField.id
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
                                deletePurchaseRegistrationFormField(data.record.purchaseRegistrationFormField);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "purchaseRegistrationFormField.description",
						 name: "description"   
					},
					{
						targets: 3,
						 data: "formFieldDescription" ,
						 name: "formFieldFk.description" 
					}
            ]
        });

        function getPurchaseRegistrationFormFields() {
            dataTable.ajax.reload();
        }

        function deletePurchaseRegistrationFormField(purchaseRegistrationFormField) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _purchaseRegistrationFormFieldsService.delete({
                            id: purchaseRegistrationFormField.id
                        }).done(function () {
                            getPurchaseRegistrationFormFields(true);
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

        $('#CreateNewPurchaseRegistrationFormFieldButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _purchaseRegistrationFormFieldsService
                .getPurchaseRegistrationFormFieldsToExcel({
				filter : $('#PurchaseRegistrationFormFieldsTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					formFieldDescriptionFilter: $('#FormFieldDescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPurchaseRegistrationFormFieldModalSaved', function () {
            getPurchaseRegistrationFormFields();
        });

		$('#GetPurchaseRegistrationFormFieldsButton').click(function (e) {
            e.preventDefault();
            getPurchaseRegistrationFormFields();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPurchaseRegistrationFormFields();
		  }
		});
		
		
		
    });
})();
