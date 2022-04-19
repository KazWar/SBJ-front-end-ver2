(function () {
    $(function () {

        var _$messagesTable = $('#MessagesTable');
        var _messagesService = abp.services.app.messages;
		var _entityTypeFullName = 'RMS.SBJ.SystemTables.Message';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Messages.Create'),
            edit: abp.auth.hasPermission('Pages.Messages.Edit'),
            'delete': abp.auth.hasPermission('Pages.Messages.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Messages/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Messages/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditMessageModal'
        });       

		 var _viewMessageModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Messages/ViewmessageModal',
            modalClass: 'ViewMessageModal'
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

        var dataTable = _$messagesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _messagesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#MessagesTableFilter').val(),
					versionFilter: $('#VersionFilterId').val(),
					systemLevelDescriptionFilter: $('#SystemLevelDescriptionFilterId').val()
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
                                    _viewMessageModal.open({ id: data.record.message.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.message.id });                                
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
                                    entityId: data.record.message.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteMessage(data.record.message);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "message.version",
						 name: "version"   
					},
					{
						targets: 2,
						 data: "systemLevelDescription" ,
						 name: "systemLevelFk.description" 
					}
            ]
        });

        function getMessages() {
            dataTable.ajax.reload();
        }

        function deleteMessage(message) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _messagesService.delete({
                            id: message.id
                        }).done(function () {
                            getMessages(true);
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

        $('#CreateNewMessageButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _messagesService
                .getMessagesToExcel({
				filter : $('#MessagesTableFilter').val(),
					versionFilter: $('#VersionFilterId').val(),
					systemLevelDescriptionFilter: $('#SystemLevelDescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditMessageModalSaved', function () {
            getMessages();
        });

		$('#GetMessagesButton').click(function (e) {
            e.preventDefault();
            getMessages();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getMessages();
		  }
		});
    });
})();