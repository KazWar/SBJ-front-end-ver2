(function () {
    $(function () {

        var _$campaignTypeEventRegistrationStatusesTable = $('#CampaignTypeEventRegistrationStatusesTable');
        var _campaignTypeEventRegistrationStatusesService = abp.services.app.campaignTypeEventRegistrationStatuses;
		var _entityTypeFullName = 'RMS.SBJ.CampaignProcesses.CampaignTypeEventRegistrationStatus';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CampaignTypeEventRegistrationStatuses.Create'),
            edit: abp.auth.hasPermission('Pages.CampaignTypeEventRegistrationStatuses.Edit'),
            'delete': abp.auth.hasPermission('Pages.CampaignTypeEventRegistrationStatuses.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignTypeEventRegistrationStatuses/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignTypeEventRegistrationStatuses/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCampaignTypeEventRegistrationStatusModal'
        });       

		 var _viewCampaignTypeEventRegistrationStatusModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignTypeEventRegistrationStatuses/ViewcampaignTypeEventRegistrationStatusModal',
            modalClass: 'ViewCampaignTypeEventRegistrationStatusModal'
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

        var dataTable = _$campaignTypeEventRegistrationStatusesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _campaignTypeEventRegistrationStatusesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CampaignTypeEventRegistrationStatusesTableFilter').val(),
					minSortOrderFilter: $('#MinSortOrderFilterId').val(),
					maxSortOrderFilter: $('#MaxSortOrderFilterId').val(),
					campaignTypeEventSortOrderFilter: $('#CampaignTypeEventSortOrderFilterId').val(),
					registrationStatusDescriptionFilter: $('#RegistrationStatusDescriptionFilterId').val()
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
                                    _viewCampaignTypeEventRegistrationStatusModal.open({ id: data.record.campaignTypeEventRegistrationStatus.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.campaignTypeEventRegistrationStatus.id });                                
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
                                    entityId: data.record.campaignTypeEventRegistrationStatus.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCampaignTypeEventRegistrationStatus(data.record.campaignTypeEventRegistrationStatus);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "campaignTypeEventRegistrationStatus.sortOrder",
						 name: "sortOrder"   
					},
					{
						targets: 2,
						 data: "campaignTypeEventSortOrder" ,
						 name: "campaignTypeEventFk.sortOrder" 
					},
					{
						targets: 3,
						 data: "registrationStatusDescription" ,
						 name: "registrationStatusFk.description" 
					}
            ]
        });

        function getCampaignTypeEventRegistrationStatuses() {
            dataTable.ajax.reload();
        }

        function deleteCampaignTypeEventRegistrationStatus(campaignTypeEventRegistrationStatus) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _campaignTypeEventRegistrationStatusesService.delete({
                            id: campaignTypeEventRegistrationStatus.id
                        }).done(function () {
                            getCampaignTypeEventRegistrationStatuses(true);
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

        $('#CreateNewCampaignTypeEventRegistrationStatusButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _campaignTypeEventRegistrationStatusesService
                .getCampaignTypeEventRegistrationStatusesToExcel({
				filter : $('#CampaignTypeEventRegistrationStatusesTableFilter').val(),
					minSortOrderFilter: $('#MinSortOrderFilterId').val(),
					maxSortOrderFilter: $('#MaxSortOrderFilterId').val(),
					campaignTypeEventSortOrderFilter: $('#CampaignTypeEventSortOrderFilterId').val(),
					registrationStatusDescriptionFilter: $('#RegistrationStatusDescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCampaignTypeEventRegistrationStatusModalSaved', function () {
            getCampaignTypeEventRegistrationStatuses();
        });

		$('#GetCampaignTypeEventRegistrationStatusesButton').click(function (e) {
            e.preventDefault();
            getCampaignTypeEventRegistrationStatuses();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCampaignTypeEventRegistrationStatuses();
		  }
		});
    });
})();