(function () {
    $(function () {

        var _$messageComponentTypesTable = $('#MessageComponentTypesTable');
        var _messageComponentTypesService = abp.services.app.messageComponentTypes;
		var _entityTypeFullName = 'RMS.SBJ.CodeTypeTables.MessageComponentType';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.MessageComponentTypes.Create'),
            edit: abp.auth.hasPermission('Pages.MessageComponentTypes.Edit'),
            'delete': abp.auth.hasPermission('Pages.MessageComponentTypes.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageComponentTypes/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageComponentTypes/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditMessageComponentTypeModal'
        });       

		 var _viewMessageComponentTypeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageComponentTypes/ViewmessageComponentTypeModal',
            modalClass: 'ViewMessageComponentTypeModal'
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

        var dataTable = _$messageComponentTypesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _messageComponentTypesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#MessageComponentTypesTableFilter').val(),
					nameFilter: $('#NameFilterId').val()
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
                                    _viewMessageComponentTypeModal.open({ id: data.record.messageComponentType.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.messageComponentType.id });                                
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
                                    entityId: data.record.messageComponentType.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteMessageComponentType(data.record.messageComponentType);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "messageComponentType.name",
						 name: "name"   
					}
            ]
        });

        function getMessageComponentTypes() {
            dataTable.ajax.reload();
        }

        function deleteMessageComponentType(messageComponentType) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _messageComponentTypesService.delete({
                            id: messageComponentType.id
                        }).done(function () {
                            getMessageComponentTypes(true);
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

        $('#CreateNewMessageComponentTypeButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _messageComponentTypesService
                .getMessageComponentTypesToExcel({
				filter : $('#MessageComponentTypesTableFilter').val(),
					nameFilter: $('#NameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditMessageComponentTypeModalSaved', function () {
            getMessageComponentTypes();
        });

		$('#GetMessageComponentTypesButton').click(function (e) {
            e.preventDefault();
            getMessageComponentTypes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getMessageComponentTypes();
		  }
		});
    });
})();