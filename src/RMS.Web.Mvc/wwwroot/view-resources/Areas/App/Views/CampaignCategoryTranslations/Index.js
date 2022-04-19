(function () {
    $(function () {

        var _$campaignCategoryTranslationsTable = $('#CampaignCategoryTranslationsTable');
        var _campaignCategoryTranslationsService = abp.services.app.campaignCategoryTranslations;
		var _entityTypeFullName = 'RMS.SBJ.CodeTypeTables.CampaignCategoryTranslation';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CampaignCategoryTranslations.Create'),
            edit: abp.auth.hasPermission('Pages.CampaignCategoryTranslations.Edit'),
            'delete': abp.auth.hasPermission('Pages.CampaignCategoryTranslations.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignCategoryTranslations/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignCategoryTranslations/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCampaignCategoryTranslationModal'
        });       

		 var _viewCampaignCategoryTranslationModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignCategoryTranslations/ViewcampaignCategoryTranslationModal',
            modalClass: 'ViewCampaignCategoryTranslationModal'
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

        var dataTable = _$campaignCategoryTranslationsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _campaignCategoryTranslationsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CampaignCategoryTranslationsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					localeDescriptionFilter: $('#LocaleDescriptionFilterId').val(),
					campaignCategoryNameFilter: $('#CampaignCategoryNameFilterId').val()
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
                                    _viewCampaignCategoryTranslationModal.open({ id: data.record.campaignCategoryTranslation.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.campaignCategoryTranslation.id });                                
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
                                    entityId: data.record.campaignCategoryTranslation.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCampaignCategoryTranslation(data.record.campaignCategoryTranslation);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "campaignCategoryTranslation.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "localeDescription" ,
						 name: "localeFk.description" 
					},
					{
						targets: 3,
						 data: "campaignCategoryName" ,
						 name: "campaignCategoryFk.name" 
					}
            ]
        });

        function getCampaignCategoryTranslations() {
            dataTable.ajax.reload();
        }

        function deleteCampaignCategoryTranslation(campaignCategoryTranslation) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _campaignCategoryTranslationsService.delete({
                            id: campaignCategoryTranslation.id
                        }).done(function () {
                            getCampaignCategoryTranslations(true);
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

        $('#CreateNewCampaignCategoryTranslationButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _campaignCategoryTranslationsService
                .getCampaignCategoryTranslationsToExcel({
				filter : $('#CampaignCategoryTranslationsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					localeDescriptionFilter: $('#LocaleDescriptionFilterId').val(),
					campaignCategoryNameFilter: $('#CampaignCategoryNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCampaignCategoryTranslationModalSaved', function () {
            getCampaignCategoryTranslations();
        });

		$('#GetCampaignCategoryTranslationsButton').click(function (e) {
            e.preventDefault();
            getCampaignCategoryTranslations();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCampaignCategoryTranslations();
		  }
		});
    });
})();