(function () {
    $(function () {

        var _$campaignCampaignTypesTable = $('#CampaignCampaignTypesTable');
        var _campaignCampaignTypesService = abp.services.app.campaignCampaignTypes;
		var _entityTypeFullName = 'RMS.SBJ.CampaignProcesses.CampaignCampaignType';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CampaignCampaignTypes.Create'),
            edit: abp.auth.hasPermission('Pages.CampaignCampaignTypes.Edit'),
            'delete': abp.auth.hasPermission('Pages.CampaignCampaignTypes.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignCampaignTypes/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignCampaignTypes/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCampaignCampaignTypeModal'
        });       

		 var _viewCampaignCampaignTypeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignCampaignTypes/ViewcampaignCampaignTypeModal',
            modalClass: 'ViewCampaignCampaignTypeModal'
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

        var dataTable = _$campaignCampaignTypesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _campaignCampaignTypesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CampaignCampaignTypesTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					campaignDescriptionFilter: $('#CampaignDescriptionFilterId').val(),
					campaignTypeNameFilter: $('#CampaignTypeNameFilterId').val()
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
                                action: function (data) {
                                    _viewCampaignCampaignTypeModal.open({ id: data.record.campaignCampaignType.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.campaignCampaignType.id });                                
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
                                    entityId: data.record.campaignCampaignType.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCampaignCampaignType(data.record.campaignCampaignType);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "campaignCampaignType.description",
						 name: "description"   
					},
					{
						targets: 3,
						 data: "campaignDescription" ,
						 name: "campaignFk.description" 
					},
					{
						targets: 4,
						 data: "campaignTypeName" ,
						 name: "campaignTypeFk.name" 
					}
            ]
        });

        function getCampaignCampaignTypes() {
            dataTable.ajax.reload();
        }

        function deleteCampaignCampaignType(campaignCampaignType) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _campaignCampaignTypesService.delete({
                            id: campaignCampaignType.id
                        }).done(function () {
                            getCampaignCampaignTypes(true);
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

        $('#CreateNewCampaignCampaignTypeButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _campaignCampaignTypesService
                .getCampaignCampaignTypesToExcel({
				filter : $('#CampaignCampaignTypesTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					campaignDescriptionFilter: $('#CampaignDescriptionFilterId').val(),
					campaignTypeNameFilter: $('#CampaignTypeNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCampaignCampaignTypeModalSaved', function () {
            getCampaignCampaignTypes();
        });

		$('#GetCampaignCampaignTypesButton').click(function (e) {
            e.preventDefault();
            getCampaignCampaignTypes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCampaignCampaignTypes();
		  }
		});
    });
})();