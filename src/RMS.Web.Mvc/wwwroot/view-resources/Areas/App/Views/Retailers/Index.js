(function () {
    $(function () {

        var _$retailersTable = $('#RetailersTable');
        var _retailersService = abp.services.app.retailers;
        var _entityTypeFullName = 'RMS.SBJ.Retailers.Retailer';
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Retailers.Create'),
            edit: abp.auth.hasPermission('Pages.Retailers.Edit'),
            'delete': abp.auth.hasPermission('Pages.Retailers.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/Retailers/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Retailers/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditRetailerModal'
                });
                   

		 var _viewRetailerModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Retailers/ViewretailerModal',
            modalClass: 'ViewRetailerModal'
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

        var dataTable = _$retailersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _retailersService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#RetailersTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					codeFilter: $('#CodeFilterId').val(),
					countryCountryCodeFilter: $('#CountryCountryCodeFilterId').val()
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
                                    _viewRetailerModal.open({ id: data.record.retailer.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.retailer.id });                                
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
                                    entityId: data.record.retailer.id
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
                                deleteRetailer(data.record.retailer);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "retailer.name",
						 name: "name"   
					},
					{
						targets: 3,
						 data: "retailer.code",
						 name: "code"   
					},
					{
						targets: 4,
						 data: "countryCountryCode" ,
						 name: "countryFk.countryCode" 
					}
            ]
        });

        function getRetailers() {
            dataTable.ajax.reload();
        }

        function deleteRetailer(retailer) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _retailersService.delete({
                            id: retailer.id
                        }).done(function () {
                            getRetailers(true);
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

        $('#CreateNewRetailerButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _retailersService
                .getRetailersToExcel({
				filter : $('#RetailersTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					codeFilter: $('#CodeFilterId').val(),
					countryCountryCodeFilter: $('#CountryCountryCodeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditRetailerModalSaved', function () {
            getRetailers();
        });

		$('#GetRetailersButton').click(function (e) {
            e.preventDefault();
            getRetailers();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getRetailers();
		  }
		});
		
		
		
    });
})();
