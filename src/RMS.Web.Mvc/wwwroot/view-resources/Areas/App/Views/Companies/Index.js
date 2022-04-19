(function () {
    $(function () {

        var _$companiesTable = $('#CompaniesTable');
        var _companiesService = abp.services.app.companies;
		var _entityTypeFullName = 'RMS.SBJ.Company.Company';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Companies.Create'),
            edit: abp.auth.hasPermission('Pages.Companies.Edit'),
            'delete': abp.auth.hasPermission('Pages.Companies.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Companies/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Companies/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCompanyModal'
        });

		 var _viewCompanyModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Companies/ViewcompanyModal',
            modalClass: 'ViewCompanyModal'
        });

		        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
		        function entityHistoryIsEnabled() {
            return abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$companiesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _companiesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CompaniesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					phoneNumberFilter: $('#PhoneNumberFilterId').val(),
					emailAddressFilter: $('#EmailAddressFilterId').val(),
					bicCashBackFilter: $('#BICCashBackFilterId').val(),
					ibanCashBackFilter: $('#IBANCashBackFilterId').val(),
					addressPostalCodeFilter: $('#AddressPostalCodeFilterId').val()
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
                                    _viewCompanyModal.open({ id: data.record.company.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.company.id });
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
                                    entityId: data.record.company.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCompany(data.record.company);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "company.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "company.phoneNumber",
						 name: "phoneNumber"   
					},
					{
						targets: 3,
						 data: "company.emailAddress",
						 name: "emailAddress"   
					},
					{
						targets: 4,
						 data: "company.bicCashBack",
						 name: "bicCashBack"   
					},
					{
						targets: 5,
						 data: "company.ibanCashBack",
						 name: "ibanCashBack"   
					},
					{
						targets: 6,
						 data: "addressPostalCode" ,
						 name: "addressFk.postalCode" 
					}
            ]
        });


        function getCompanies() {
            dataTable.ajax.reload();
        }

        function deleteCompany(company) {
            abp.message.confirm(
                '',
                function (isConfirmed) {
                    if (isConfirmed) {
                        _companiesService.delete({
                            id: company.id
                        }).done(function () {
                            getCompanies(true);
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

        $('#CreateNewCompanyButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _companiesService
                .getCompaniesToExcel({
				filter : $('#CompaniesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					phoneNumberFilter: $('#PhoneNumberFilterId').val(),
					emailAddressFilter: $('#EmailAddressFilterId').val(),
					bicCashBackFilter: $('#BICCashBackFilterId').val(),
					ibanCashBackFilter: $('#IBANCashBackFilterId').val(),
					addressPostalCodeFilter: $('#AddressPostalCodeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCompanyModalSaved', function () {
            getCompanies();
        });

		$('#GetCompaniesButton').click(function (e) {
            e.preventDefault();
            getCompanies();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCompanies();
		  }
		});

    });
})();