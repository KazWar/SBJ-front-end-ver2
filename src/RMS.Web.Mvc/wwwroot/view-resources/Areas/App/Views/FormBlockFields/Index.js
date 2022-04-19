(function () {
    $(function () {

        var _$formBlockFieldsTable = $('#FormBlockFieldsTable');
        var _formBlockFieldsService = abp.services.app.formBlockFields;
		var _entityTypeFullName = 'RMS.SBJ.Forms.FormBlockField';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.FormBlockFields.Create'),
            edit: abp.auth.hasPermission('Pages.FormBlockFields.Edit'),
            'delete': abp.auth.hasPermission('Pages.FormBlockFields.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormBlockFields/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormBlockFields/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditFormBlockFieldModal'
        });       

		 var _viewFormBlockFieldModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormBlockFields/ViewformBlockFieldModal',
            modalClass: 'ViewFormBlockFieldModal'
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

        var dataTable = _$formBlockFieldsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _formBlockFieldsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#FormBlockFieldsTableFilter').val(),
					minSortOrderFilter: $('#MinSortOrderFilterId').val(),
					maxSortOrderFilter: $('#MaxSortOrderFilterId').val(),
					formFieldDescriptionFilter: $('#FormFieldDescriptionFilterId').val(),
					formBlockDescriptionFilter: $('#FormBlockDescriptionFilterId').val()
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
                                    _viewFormBlockFieldModal.open({ id: data.record.formBlockField.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.formBlockField.id });                                
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
                                    entityId: data.record.formBlockField.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteFormBlockField(data.record.formBlockField);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "formBlockField.sortOrder",
						 name: "sortOrder"   
					},
					{
						targets: 2,
						 data: "formFieldDescription" ,
						 name: "formFieldFk.description" 
					},
					{
						targets: 3,
						 data: "formBlockDescription" ,
						 name: "formBlockFk.description" 
					}
            ]
        });

        function getFormBlockFields() {
            dataTable.ajax.reload();
        }

        function deleteFormBlockField(formBlockField) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _formBlockFieldsService.delete({
                            id: formBlockField.id
                        }).done(function () {
                            getFormBlockFields(true);
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

        $('#CreateNewFormBlockFieldButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _formBlockFieldsService
                .getFormBlockFieldsToExcel({
				filter : $('#FormBlockFieldsTableFilter').val(),
					minSortOrderFilter: $('#MinSortOrderFilterId').val(),
					maxSortOrderFilter: $('#MaxSortOrderFilterId').val(),
					formFieldDescriptionFilter: $('#FormFieldDescriptionFilterId').val(),
					formBlockDescriptionFilter: $('#FormBlockDescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditFormBlockFieldModalSaved', function () {
            getFormBlockFields();
        });

		$('#GetFormBlockFieldsButton').click(function (e) {
            e.preventDefault();
            getFormBlockFields();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getFormBlockFields();
		  }
		});
    });
})();