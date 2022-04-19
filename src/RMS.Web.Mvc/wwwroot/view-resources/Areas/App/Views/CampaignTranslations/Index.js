(function () {
    $(function () {

        var _$campaignTranslationsTable = $('#CampaignTranslationsTable');
        var _campaignTranslationsService = abp.services.app.campaignTranslations;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CampaignTranslations.Create'),
            edit: abp.auth.hasPermission('Pages.CampaignTranslations.Edit'),
            'delete': abp.auth.hasPermission('Pages.CampaignTranslations.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/CampaignTranslations/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignTranslations/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditCampaignTranslationModal'
                });
                   

		 var _viewCampaignTranslationModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignTranslations/ViewcampaignTranslationModal',
            modalClass: 'ViewCampaignTranslationModal'
        });

		
		

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

        var dataTable = _$campaignTranslationsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _campaignTranslationsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CampaignTranslationsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					campaignNameFilter: $('#CampaignNameFilterId').val(),
					localeDescriptionFilter: $('#LocaleDescriptionFilterId').val()
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
                                    _viewCampaignTranslationModal.open({ id: data.record.campaignTranslation.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.campaignTranslation.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCampaignTranslation(data.record.campaignTranslation);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "campaignTranslation.name",
						 name: "name"   
					},
					{
						targets: 3,
						 data: "campaignTranslation.description",
						 name: "description"   
					},
					{
						targets: 4,
						 data: "campaignName" ,
						 name: "campaignFk.name" 
					},
					{
						targets: 5,
						 data: "localeDescription" ,
						 name: "localeFk.description" 
					}
            ]
        });

        function getCampaignTranslations() {
            dataTable.ajax.reload();
        }

        function deleteCampaignTranslation(campaignTranslation) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _campaignTranslationsService.delete({
                            id: campaignTranslation.id
                        }).done(function () {
                            getCampaignTranslations(true);
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

        $('#CreateNewCampaignTranslationButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _campaignTranslationsService
                .getCampaignTranslationsToExcel({
				filter : $('#CampaignTranslationsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					campaignNameFilter: $('#CampaignNameFilterId').val(),
					localeDescriptionFilter: $('#LocaleDescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCampaignTranslationModalSaved', function () {
            getCampaignTranslations();
        });

		$('#GetCampaignTranslationsButton').click(function (e) {
            e.preventDefault();
            getCampaignTranslations();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCampaignTranslations();
		  }
		});
		
		
		
    });
})();
