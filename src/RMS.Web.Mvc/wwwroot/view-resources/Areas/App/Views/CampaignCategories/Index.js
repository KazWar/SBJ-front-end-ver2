(function () {
    $(function () {

        var _$campaignCategoriesTable = $('#CampaignCategoriesTable');
        var _campaignCategoriesService = abp.services.app.campaignCategories;
		var _entityTypeFullName = 'RMS.SBJ.CodeTypeTables.CampaignCategory';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CampaignCategories.Create'),
            edit: abp.auth.hasPermission('Pages.CampaignCategories.Edit'),
            'delete': abp.auth.hasPermission('Pages.CampaignCategories.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignCategories/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignCategories/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCampaignCategoryModal'
        });       

		 var _viewCampaignCategoryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignCategories/ViewcampaignCategoryModal',
            modalClass: 'ViewCampaignCategoryModal'
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

        var dataTable = _$campaignCategoriesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _campaignCategoriesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CampaignCategoriesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					minSortOrderFilter: $('#MinSortOrderFilterId').val(),
					maxSortOrderFilter: $('#MaxSortOrderFilterId').val()
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
                                    _viewCampaignCategoryModal.open({ id: data.record.campaignCategory.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.campaignCategory.id });                                
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
                                    entityId: data.record.campaignCategory.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCampaignCategory(data.record.campaignCategory);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "campaignCategory.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "campaignCategory.isActive",
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
						 data: "campaignCategory.sortOrder",
						 name: "sortOrder"   
					}
            ]
        });

        function getCampaignCategories() {
            dataTable.ajax.reload();
        }

        function deleteCampaignCategory(campaignCategory) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _campaignCategoriesService.delete({
                            id: campaignCategory.id
                        }).done(function () {
                            getCampaignCategories(true);
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

        $('#CreateNewCampaignCategoryButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _campaignCategoriesService
                .getCampaignCategoriesToExcel({
				filter : $('#CampaignCategoriesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					minSortOrderFilter: $('#MinSortOrderFilterId').val(),
					maxSortOrderFilter: $('#MaxSortOrderFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCampaignCategoryModalSaved', function () {
            getCampaignCategories();
        });

		$('#GetCampaignCategoriesButton').click(function (e) {
            e.preventDefault();
            getCampaignCategories();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCampaignCategories();
		  }
		});
    });
})();