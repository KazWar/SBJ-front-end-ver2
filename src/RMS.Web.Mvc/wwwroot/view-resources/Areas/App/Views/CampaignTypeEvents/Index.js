(function () {
    $(function () {

        var _$campaignTypeEventsTable = $('#CampaignTypeEventsTable');
        var _campaignTypeEventsService = abp.services.app.campaignTypeEvents;
		var _entityTypeFullName = 'RMS.SBJ.CampaignProcesses.CampaignTypeEvent';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CampaignTypeEvents.Create'),
            edit: abp.auth.hasPermission('Pages.CampaignTypeEvents.Edit'),
            'delete': abp.auth.hasPermission('Pages.CampaignTypeEvents.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignTypeEvents/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignTypeEvents/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCampaignTypeEventModal'
        });       

		 var _viewCampaignTypeEventModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignTypeEvents/ViewcampaignTypeEventModal',
            modalClass: 'ViewCampaignTypeEventModal'
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

        var dataTable = _$campaignTypeEventsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _campaignTypeEventsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CampaignTypeEventsTableFilter').val(),
					minSortOrderFilter: $('#MinSortOrderFilterId').val(),
					maxSortOrderFilter: $('#MaxSortOrderFilterId').val(),
					campaignTypeNameFilter: $('#CampaignTypeNameFilterId').val(),
					processEventNameFilter: $('#ProcessEventNameFilterId').val()
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
                                    _viewCampaignTypeEventModal.open({ id: data.record.campaignTypeEvent.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.campaignTypeEvent.id });                                
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
                                    entityId: data.record.campaignTypeEvent.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCampaignTypeEvent(data.record.campaignTypeEvent);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "campaignTypeEvent.sortOrder",
						 name: "sortOrder"   
					},
					{
						targets: 2,
						 data: "campaignTypeName" ,
						 name: "campaignTypeFk.name" 
					},
					{
						targets: 3,
						 data: "processEventName" ,
						 name: "processEventFk.name" 
					}
            ]
        });

        function getCampaignTypeEvents() {
            dataTable.ajax.reload();
        }

        function deleteCampaignTypeEvent(campaignTypeEvent) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _campaignTypeEventsService.delete({
                            id: campaignTypeEvent.id
                        }).done(function () {
                            getCampaignTypeEvents(true);
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

        $('#CreateNewCampaignTypeEventButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _campaignTypeEventsService
                .getCampaignTypeEventsToExcel({
				filter : $('#CampaignTypeEventsTableFilter').val(),
					minSortOrderFilter: $('#MinSortOrderFilterId').val(),
					maxSortOrderFilter: $('#MaxSortOrderFilterId').val(),
					campaignTypeNameFilter: $('#CampaignTypeNameFilterId').val(),
					processEventNameFilter: $('#ProcessEventNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCampaignTypeEventModalSaved', function () {
            getCampaignTypeEvents();
        });

		$('#GetCampaignTypeEventsButton').click(function (e) {
            e.preventDefault();
            getCampaignTypeEvents();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCampaignTypeEvents();
		  }
		});
    });
})();