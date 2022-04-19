(function () {
    $(function () {

        var _$campaignTypesTable = $('#CampaignTypesTable');
        var _campaignTypesService = abp.services.app.campaignTypes;
		var _entityTypeFullName = 'RMS.SBJ.CodeTypeTables.CampaignType';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CampaignTypes.Create'),
            edit: abp.auth.hasPermission('Pages.CampaignTypes.Edit'),
            'delete': abp.auth.hasPermission('Pages.CampaignTypes.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignTypes/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignTypes/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCampaignTypeModal'
        });       

		 var _viewCampaignTypeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignTypes/ViewcampaignTypeModal',
            modalClass: 'ViewCampaignTypeModal'
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

        var dataTable = _$campaignTypesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _campaignTypesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CampaignTypesTableFilter').val(),
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
                                    _viewCampaignTypeModal.open({ id: data.record.campaignType.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.campaignType.id });                                
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
                                    entityId: data.record.campaignType.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCampaignType(data.record.campaignType);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "campaignType.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "campaignType.isActive",
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

        function getCampaignTypes() {
            dataTable.ajax.reload();
        }

        function deleteCampaignType(campaignType) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _campaignTypesService.delete({
                            id: campaignType.id
                        }).done(function () {
                            getCampaignTypes(true);
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

        $('#CreateNewCampaignTypeButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _campaignTypesService
                .getCampaignTypesToExcel({
				filter : $('#CampaignTypesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCampaignTypeModalSaved', function () {
            getCampaignTypes();
        });

		$('#GetCampaignTypesButton').click(function (e) {
            e.preventDefault();
            getCampaignTypes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCampaignTypes();
		  }
		});
    });
})();