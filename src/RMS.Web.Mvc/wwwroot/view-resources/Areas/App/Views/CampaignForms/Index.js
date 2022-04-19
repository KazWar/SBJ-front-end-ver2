(function () {
    $(function () {

        var _$campaignFormsTable = $('#CampaignFormsTable');
        var _campaignFormsService = abp.services.app.campaignForms;
		var _entityTypeFullName = 'RMS.SBJ.CampaignProcesses.CampaignForm';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CampaignForms.Create'),
            edit: abp.auth.hasPermission('Pages.CampaignForms.Edit'),
            'delete': abp.auth.hasPermission('Pages.CampaignForms.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/CampaignForms/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignForms/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditCampaignFormModal'
                });
                   

		 var _viewCampaignFormModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignForms/ViewcampaignFormModal',
            modalClass: 'ViewCampaignFormModal'
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

        var dataTable = _$campaignFormsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _campaignFormsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CampaignFormsTableFilter').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					campaignNameFilter: $('#CampaignNameFilterId').val(),
					formVersionFilter: $('#FormVersionFilterId').val()
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
                                    _viewCampaignFormModal.open({ id: data.record.campaignForm.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.campaignForm.id });                                
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
                                    entityId: data.record.campaignForm.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCampaignForm(data.record.campaignForm);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "campaignForm.isActive",
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
						 data: "formVersion" ,
						 name: "formFk.version" 
					}
            ]
        });

        function getCampaignForms() {
            dataTable.ajax.reload();
        }

        function deleteCampaignForm(campaignForm) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _campaignFormsService.delete({
                            id: campaignForm.id
                        }).done(function () {
                            getCampaignForms(true);
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

        $('#CreateNewCampaignFormButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _campaignFormsService
                .getCampaignFormsToExcel({
				filter : $('#CampaignFormsTableFilter').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					campaignNameFilter: $('#CampaignNameFilterId').val(),
					formVersionFilter: $('#FormVersionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCampaignFormModalSaved', function () {
            getCampaignForms();
        });

		$('#GetCampaignFormsButton').click(function (e) {
            e.preventDefault();
            getCampaignForms();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCampaignForms();
		  }
        });


		
    });
})();