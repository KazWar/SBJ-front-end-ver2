(function () {
    $(function () {

        var _$campaignMessagesTable = $('#CampaignMessagesTable');
        var _campaignMessagesService = abp.services.app.campaignMessages;
		var _entityTypeFullName = 'RMS.SBJ.CampaignProcesses.CampaignMessage';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CampaignMessages.Create'),
            edit: abp.auth.hasPermission('Pages.CampaignMessages.Edit'),
            'delete': abp.auth.hasPermission('Pages.CampaignMessages.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/CampaignMessages/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignMessages/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditCampaignMessageModal'
                });
                   

		 var _viewCampaignMessageModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignMessages/ViewcampaignMessageModal',
            modalClass: 'ViewCampaignMessageModal'
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
        
        var getMaxDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z"); 
        }

        var dataTable = _$campaignMessagesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _campaignMessagesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CampaignMessagesTableFilter').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					campaignNameFilter: $('#CampaignNameFilterId').val(),
					messageVersionFilter: $('#MessageVersionFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    },
                    targets: 0
                },
                {
                    width: 120,
                    targets: 1,
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
                                iconStyle: 'far fa-eye mr-2',
                                action: function (data) {
                                    _viewCampaignMessageModal.open({ id: data.record.campaignMessage.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.campaignMessage.id });                                
                            }
                        },
                        {
                            text: app.localize('History'),
                            iconStyle: 'fas fa-history mr-2',
                            visible: function () {
                                return entityHistoryIsEnabled();
                            },
                            action: function (data) {
                                _entityTypeHistoryModal.open({
                                    entityTypeFullName: _entityTypeFullName,
                                    entityId: data.record.campaignMessage.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCampaignMessage(data.record.campaignMessage);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "campaignMessage.isActive",
						 name: "isActive"  ,
						render: function (isActive) {
							if (isActive) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 3,
						 data: "campaignName" ,
						 name: "campaignFk.name" 
					},
					{
						targets: 4,
						 data: "messageVersion" ,
						 name: "messageFk.version" 
					}
            ]
        });

        function getCampaignMessages() {
            dataTable.ajax.reload();
        }

        function deleteCampaignMessage(campaignMessage) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _campaignMessagesService.delete({
                            id: campaignMessage.id
                        }).done(function () {
                            getCampaignMessages(true);
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

        $('#CreateNewCampaignMessageButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _campaignMessagesService
                .getCampaignMessagesToExcel({
				filter : $('#CampaignMessagesTableFilter').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					campaignNameFilter: $('#CampaignNameFilterId').val(),
					messageVersionFilter: $('#MessageVersionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCampaignMessageModalSaved', function () {
            getCampaignMessages();
        });

		$('#GetCampaignMessagesButton').click(function (e) {
            e.preventDefault();
            getCampaignMessages();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCampaignMessages();
		  }
		});
		
		
		
    });
})();
