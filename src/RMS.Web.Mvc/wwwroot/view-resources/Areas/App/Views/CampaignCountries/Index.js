(function () {
    $(function () {

        var _$campaignCountriesTable = $('#CampaignCountriesTable');
        var _campaignCountriesService = abp.services.app.campaignCountries;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CampaignCountries.Create'),
            edit: abp.auth.hasPermission('Pages.CampaignCountries.Edit'),
            'delete': abp.auth.hasPermission('Pages.CampaignCountries.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/CampaignCountries/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CampaignCountries/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditCampaignCountryModal'
                });
                   

		 var _viewCampaignCountryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CampaignCountries/ViewcampaignCountryModal',
            modalClass: 'ViewCampaignCountryModal'
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

        var dataTable = _$campaignCountriesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _campaignCountriesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CampaignCountriesTableFilter').val(),
					campaignNameFilter: $('#CampaignNameFilterId').val(),
					countryDescriptionFilter: $('#CountryDescriptionFilterId').val()
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
                                    _viewCampaignCountryModal.open({ id: data.record.campaignCountry.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.campaignCountry.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCampaignCountry(data.record.campaignCountry);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "campaignName" ,
						 name: "campaignFk.name" 
					},
					{
						targets: 3,
						 data: "countryDescription" ,
						 name: "countryFk.description" 
					}
            ]
        });

        function getCampaignCountries() {
            dataTable.ajax.reload();
        }

        function deleteCampaignCountry(campaignCountry) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _campaignCountriesService.delete({
                            id: campaignCountry.id
                        }).done(function () {
                            getCampaignCountries(true);
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

        $('#CreateNewCampaignCountryButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _campaignCountriesService
                .getCampaignCountriesToExcel({
				filter : $('#CampaignCountriesTableFilter').val(),
					campaignNameFilter: $('#CampaignNameFilterId').val(),
					countryDescriptionFilter: $('#CountryDescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCampaignCountryModalSaved', function () {
            getCampaignCountries();
        });

		$('#GetCampaignCountriesButton').click(function (e) {
            e.preventDefault();
            getCampaignCountries();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCampaignCountries();
		  }
		});
		
		
		
    });
})();
