(function () {
    $(function () {

        var _$fieldTypesTable = $('#FieldTypesTable');
        var _fieldTypesService = abp.services.app.fieldTypes;
		var _entityTypeFullName = 'RMS.SBJ.Forms.FieldType';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.FieldTypes.Create'),
            edit: abp.auth.hasPermission('Pages.FieldTypes.Edit'),
            'delete': abp.auth.hasPermission('Pages.FieldTypes.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FieldTypes/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FieldTypes/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditFieldTypeModal'
        });       

		 var _viewFieldTypeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FieldTypes/ViewfieldTypeModal',
            modalClass: 'ViewFieldTypeModal'
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

        var dataTable = _$fieldTypesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _fieldTypesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#FieldTypesTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
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
                                action: function (data) {
                                    _viewFieldTypeModal.open({ id: data.record.fieldType.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.fieldType.id });                                
                            }
                        },
                        {
                            text: app.localize('History'),
                            visible: function () {
                                return entityHistoryIsEnabled();
                            },
                            action: function (data) {
                                _entityTypeHistoryModal.open({
                                    entityTypeFullName: _entityTypeFullName,
                                    entityId: data.record.fieldType.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteFieldType(data.record.fieldType);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "fieldType.description",
						 name: "description"   
					}
            ]
        });

        function getFieldTypes() {
            dataTable.ajax.reload();
        }

        function deleteFieldType(fieldType) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _fieldTypesService.delete({
                            id: fieldType.id
                        }).done(function () {
                            getFieldTypes(true);
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

        $('#CreateNewFieldTypeButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _fieldTypesService
                .getFieldTypesToExcel({
				filter : $('#FieldTypesTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditFieldTypeModalSaved', function () {
            getFieldTypes();
        });

		$('#GetFieldTypesButton').click(function (e) {
            e.preventDefault();
            getFieldTypes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getFieldTypes();
		  }
		});
    });
})();