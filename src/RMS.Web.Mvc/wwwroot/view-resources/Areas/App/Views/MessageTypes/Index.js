(function () {
    $(function () {

        var _$messageTypesTable = $('#MessageTypesTable');
        var _messageTypesService = abp.services.app.messageTypes;
		var _entityTypeFullName = 'RMS.SBJ.CodeTypeTables.MessageType';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.MessageTypes.Create'),
            edit: abp.auth.hasPermission('Pages.MessageTypes.Edit'),
            'delete': abp.auth.hasPermission('Pages.MessageTypes.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageTypes/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageTypes/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditMessageTypeModal'
        });       

		 var _viewMessageTypeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageTypes/ViewmessageTypeModal',
            modalClass: 'ViewMessageTypeModal'
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

        var dataTable = _$messageTypesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _messageTypesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#MessageTypesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					sourceFilter: $('#SourceFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					messageVersionFilter: $('#MessageVersionFilterId').val()
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
                                    _viewMessageTypeModal.open({ id: data.record.messageType.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.messageType.id });                                
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
                                    entityId: data.record.messageType.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteMessageType(data.record.messageType);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "messageType.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "messageType.source",
						 name: "source"   
					},
					{
						targets: 3,
						 data: "messageType.isActive",
						 name: "isActive"  ,
						render: function (isActive) {
							if (isActive) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 4,
						 data: "messageVersion" ,
						 name: "messageFk.version" 
					}
            ]
        });

        function getMessageTypes() {
            dataTable.ajax.reload();
        }

        function deleteMessageType(messageType) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _messageTypesService.delete({
                            id: messageType.id
                        }).done(function () {
                            getMessageTypes(true);
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

        $('#CreateNewMessageTypeButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _messageTypesService
                .getMessageTypesToExcel({
				filter : $('#MessageTypesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					sourceFilter: $('#SourceFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					messageVersionFilter: $('#MessageVersionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditMessageTypeModalSaved', function () {
            getMessageTypes();
        });

		$('#GetMessageTypesButton').click(function (e) {
            e.preventDefault();
            getMessageTypes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getMessageTypes();
		  }
		});
    });
})();