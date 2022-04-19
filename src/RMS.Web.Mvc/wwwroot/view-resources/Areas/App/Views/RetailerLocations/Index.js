(function () {
    $(function () {

        var _$retailerLocationsTable = $('#RetailerLocationsTable');
        var _retailerLocationsService = abp.services.app.retailerLocations;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.RetailerLocations.Create'),
            edit: abp.auth.hasPermission('Pages.RetailerLocations.Edit'),
            'delete': abp.auth.hasPermission('Pages.RetailerLocations.Delete')
        };

               

		 var _viewRetailerLocationModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RetailerLocations/ViewretailerLocationModal',
            modalClass: 'ViewRetailerLocationModal'
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

        var dataTable = _$retailerLocationsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _retailerLocationsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#RetailerLocationsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					retailerNameFilter: $('#RetailerNameFilterId').val(),
					addressAddressLine1Filter: $('#AddressAddressLine1FilterId').val()
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
                                    window.location="/App/RetailerLocations/ViewRetailerLocation/" + data.record.retailerLocation.id;
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            window.location="/App/RetailerLocations/CreateOrEdit/" + data.record.retailerLocation.id;                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteRetailerLocation(data.record.retailerLocation);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "retailerLocation.name",
						 name: "name"   
					},
					{
						targets: 3,
						 data: "retailerName" ,
						 name: "retailerFk.name" 
					},
					{
						targets: 4,
						 data: "addressAddressLine1" ,
						 name: "addressFk.addressLine1" 
					}
            ]
        });

        function getRetailerLocations() {
            dataTable.ajax.reload();
        }

        function deleteRetailerLocation(retailerLocation) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _retailerLocationsService.delete({
                            id: retailerLocation.id
                        }).done(function () {
                            getRetailerLocations(true);
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

                

		$('#ExportToExcelButton').click(function () {
            _retailerLocationsService
                .getRetailerLocationsToExcel({
				filter : $('#RetailerLocationsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					retailerNameFilter: $('#RetailerNameFilterId').val(),
					addressAddressLine1Filter: $('#AddressAddressLine1FilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditRetailerLocationModalSaved', function () {
            getRetailerLocations();
        });

		$('#GetRetailerLocationsButton').click(function (e) {
            e.preventDefault();
            getRetailerLocations();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getRetailerLocations();
		  }
		});
		
		
		
    });
})();
