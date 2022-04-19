(function () {
    $(function () {

        var _$addressesTable = $('#AddressesTable');
        var _addressesService = abp.services.app.addresses;
		var _entityTypeFullName = 'RMS.SBJ.Company.Address';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Addresses.Create'),
            edit: abp.auth.hasPermission('Pages.Addresses.Edit'),
            'delete': abp.auth.hasPermission('Pages.Addresses.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Addresses/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Addresses/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditAddressModal'
        });       

		 var _viewAddressModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Addresses/ViewaddressModal',
            modalClass: 'ViewAddressModal'
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

        var dataTable = _$addressesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _addressesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#AddressesTableFilter').val(),
					addressLine1Filter: $('#AddressLine1FilterId').val(),
					addressLine2Filter: $('#AddressLine2FilterId').val(),
					postalCodeFilter: $('#PostalCodeFilterId').val(),
					cityFilter: $('#CityFilterId').val(),
					countryCountryCodeFilter: $('#CountryCountryCodeFilterId').val()
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
                                    _viewAddressModal.open({ id: data.record.address.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.address.id });                                
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
                                    entityId: data.record.address.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteAddress(data.record.address);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "address.addressLine1",
						 name: "addressLine1"   
					},
					{
						targets: 2,
						 data: "address.addressLine2",
						 name: "addressLine2"   
					},
					{
						targets: 3,
						 data: "address.postalCode",
						 name: "postalCode"   
					},
					{
						targets: 4,
						 data: "address.city",
						 name: "city"   
					},
					{
						targets: 5,
						 data: "countryCountryCode" ,
						 name: "countryFk.countryCode" 
					}
            ]
        });

        function getAddresses() {
            dataTable.ajax.reload();
        }

        function deleteAddress(address) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _addressesService.delete({
                            id: address.id
                        }).done(function () {
                            getAddresses(true);
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

        $('#CreateNewAddressButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _addressesService
                .getAddressesToExcel({
				filter : $('#AddressesTableFilter').val(),
					addressLine1Filter: $('#AddressLine1FilterId').val(),
					addressLine2Filter: $('#AddressLine2FilterId').val(),
					postalCodeFilter: $('#PostalCodeFilterId').val(),
					cityFilter: $('#CityFilterId').val(),
					countryCountryCodeFilter: $('#CountryCountryCodeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditAddressModalSaved', function () {
            getAddresses();
        });

		$('#GetAddressesButton').click(function (e) {
            e.preventDefault();
            getAddresses();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getAddresses();
		  }
		});
    });
})();