(function () {
    $(function () {

        var _$messageComponentsTable = $('#MessageComponentsTable');
        var _messageComponentsService = abp.services.app.messageComponents;
		var _entityTypeFullName = 'RMS.SBJ.CampaignProcesses.MessageComponent';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.MessageComponents.Create'),
            edit: abp.auth.hasPermission('Pages.MessageComponents.Edit'),
            'delete': abp.auth.hasPermission('Pages.MessageComponents.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageComponents/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageComponents/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditMessageComponentModal'
        });       

		 var _viewMessageComponentModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageComponents/ViewmessageComponentModal',
            modalClass: 'ViewMessageComponentModal'
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

        var dataTable = _$messageComponentsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _messageComponentsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#MessageComponentsTableFilter').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					messageTypeNameFilter: $('#MessageTypeNameFilterId').val(),
					messageComponentTypeNameFilter: $('#MessageComponentTypeNameFilterId').val()
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
                                    _viewMessageComponentModal.open({ id: data.record.messageComponent.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.messageComponent.id });                                
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
                                    entityId: data.record.messageComponent.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteMessageComponent(data.record.messageComponent);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "messageComponent.isActive",
						 name: "isActive"  ,
						render: function (isActive) {
							if (isActive) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 2,
						 data: "messageTypeName" ,
						 name: "messageTypeFk.name" 
					},
					{
						targets: 3,
						 data: "messageComponentTypeName" ,
						 name: "messageComponentTypeFk.name" 
					}
            ]
        });

        function getMessageComponents() {
            dataTable.ajax.reload();
        }

        function deleteMessageComponent(messageComponent) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _messageComponentsService.delete({
                            id: messageComponent.id
                        }).done(function () {
                            getMessageComponents(true);
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

        $('#CreateNewMessageComponentButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _messageComponentsService
                .getMessageComponentsToExcel({
				filter : $('#MessageComponentsTableFilter').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					messageTypeNameFilter: $('#MessageTypeNameFilterId').val(),
					messageComponentTypeNameFilter: $('#MessageComponentTypeNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditMessageComponentModalSaved', function () {
            getMessageComponents();
        });

		$('#GetMessageComponentsButton').click(function (e) {
            e.preventDefault();
            getMessageComponents();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getMessageComponents();
		  }
		});
    });
})();