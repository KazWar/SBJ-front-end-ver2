(function () {
    $(function () {

        var _$processEventsTable = $('#ProcessEventsTable');
        var _processEventsService = abp.services.app.processEvents;
		var _entityTypeFullName = 'RMS.SBJ.CodeTypeTables.ProcessEvent';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ProcessEvents.Create'),
            edit: abp.auth.hasPermission('Pages.ProcessEvents.Edit'),
            'delete': abp.auth.hasPermission('Pages.ProcessEvents.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProcessEvents/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProcessEvents/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProcessEventModal'
        });       

		 var _viewProcessEventModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProcessEvents/ViewprocessEventModal',
            modalClass: 'ViewProcessEventModal'
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

        var dataTable = _$processEventsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _processEventsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ProcessEventsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val()
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
                                    _viewProcessEventModal.open({ id: data.record.processEvent.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.processEvent.id });                                
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
                                    entityId: data.record.processEvent.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteProcessEvent(data.record.processEvent);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "processEvent.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "processEvent.isActive",
						 name: "isActive"  ,
						render: function (isActive) {
							if (isActive) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					}
            ]
        });

        function getProcessEvents() {
            dataTable.ajax.reload();
        }

        function deleteProcessEvent(processEvent) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _processEventsService.delete({
                            id: processEvent.id
                        }).done(function () {
                            getProcessEvents(true);
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

        $('#CreateNewProcessEventButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _processEventsService
                .getProcessEventsToExcel({
				filter : $('#ProcessEventsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditProcessEventModalSaved', function () {
            getProcessEvents();
        });

		$('#GetProcessEventsButton').click(function (e) {
            e.preventDefault();
            getProcessEvents();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getProcessEvents();
		  }
		});
    });
})();