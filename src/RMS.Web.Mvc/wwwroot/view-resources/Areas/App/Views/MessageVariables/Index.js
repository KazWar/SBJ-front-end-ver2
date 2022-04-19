(function () {
    $(function () {

        var _$messageVariablesTable = $('#MessageVariablesTable');
        var _messageVariablesService = abp.services.app.messageVariables;
		var _entityTypeFullName = 'RMS.SBJ.CodeTypeTables.MessageVariable';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.MessageVariables.Create'),
            edit: abp.auth.hasPermission('Pages.MessageVariables.Edit'),
            'delete': abp.auth.hasPermission('Pages.MessageVariables.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageVariables/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageVariables/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditMessageVariableModal'
        });       

		 var _viewMessageVariableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageVariables/ViewmessageVariableModal',
            modalClass: 'ViewMessageVariableModal'
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

        var dataTable = _$messageVariablesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _messageVariablesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#MessageVariablesTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					rmsTableFilter: $('#RmsTableFilterId').val(),
					tableFieldFilter: $('#TableFieldFilterId').val()
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
                                    _viewMessageVariableModal.open({ id: data.record.messageVariable.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.messageVariable.id });                                
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
                                    entityId: data.record.messageVariable.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteMessageVariable(data.record.messageVariable);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "messageVariable.description",
						 name: "description"   
					},
					{
						targets: 2,
						 data: "messageVariable.rmsTable",
						 name: "rmsTable"   
					},
					{
						targets: 3,
						 data: "messageVariable.tableField",
						 name: "tableField"   
					}
            ]
        });

        function getMessageVariables() {
            dataTable.ajax.reload();
        }

        function deleteMessageVariable(messageVariable) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _messageVariablesService.delete({
                            id: messageVariable.id
                        }).done(function () {
                            getMessageVariables(true);
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

        $('#CreateNewMessageVariableButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _messageVariablesService
                .getMessageVariablesToExcel({
				filter : $('#MessageVariablesTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					rmsTableFilter: $('#RmsTableFilterId').val(),
					tableFieldFilter: $('#TableFieldFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditMessageVariableModalSaved', function () {
            getMessageVariables();
        });

		$('#GetMessageVariablesButton').click(function (e) {
            e.preventDefault();
            getMessageVariables();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getMessageVariables();
		  }
		});
    });
})();