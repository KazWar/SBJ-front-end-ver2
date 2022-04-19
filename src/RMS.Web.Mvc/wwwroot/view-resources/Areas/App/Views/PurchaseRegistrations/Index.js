(function () {
    $(function () {

        var _$purchaseRegistrationsTable = $('#PurchaseRegistrationsTable');
        var _purchaseRegistrationsService = abp.services.app.purchaseRegistrations;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PurchaseRegistrations.Create'),
            edit: abp.auth.hasPermission('Pages.PurchaseRegistrations.Edit'),
            'delete': abp.auth.hasPermission('Pages.PurchaseRegistrations.Delete')
        };

               

		 var _viewPurchaseRegistrationModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PurchaseRegistrations/ViewpurchaseRegistrationModal',
            modalClass: 'ViewPurchaseRegistrationModal'
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

        var dataTable = _$purchaseRegistrationsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _purchaseRegistrationsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PurchaseRegistrationsTableFilter').val(),
					minQuantityFilter: $('#MinQuantityFilterId').val(),
					maxQuantityFilter: $('#MaxQuantityFilterId').val(),
					minTotalAmountFilter: $('#MinTotalAmountFilterId').val(),
					maxTotalAmountFilter: $('#MaxTotalAmountFilterId').val(),
					minPurchaseDateFilter:  getDateFilter($('#MinPurchaseDateFilterId')),
					maxPurchaseDateFilter:  getMaxDateFilter($('#MaxPurchaseDateFilterId')),
					invoiceImageFilter: $('#InvoiceImageFilterId').val(),
					registrationFirstNameFilter: $('#RegistrationFirstNameFilterId').val(),
					productCtnFilter: $('#ProductCtnFilterId').val(),
					handlingLineCustomerCodeFilter: $('#HandlingLineCustomerCodeFilterId').val(),
					retailerLocationNameFilter: $('#RetailerLocationNameFilterId').val()
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
                                    window.location="/App/PurchaseRegistrations/ViewPurchaseRegistration/" + data.record.purchaseRegistration.id;
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            window.location="/App/PurchaseRegistrations/CreateOrEdit/" + data.record.purchaseRegistration.id;                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePurchaseRegistration(data.record.purchaseRegistration);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "purchaseRegistration.quantity",
						 name: "quantity"   
					},
					{
						targets: 3,
						 data: "purchaseRegistration.totalAmount",
						 name: "totalAmount"   
					},
					{
						targets: 4,
						 data: "purchaseRegistration.purchaseDate",
						 name: "purchaseDate" ,
					render: function (purchaseDate) {
						if (purchaseDate) {
							return moment(purchaseDate).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 5,
						 data: "purchaseRegistration.invoiceImage",
						 name: "invoiceImage"   
					},
					{
						targets: 6,
						 data: "registrationFirstName" ,
						 name: "registrationFk.firstName" 
					},
					{
						targets: 7,
						 data: "productCtn" ,
						 name: "productFk.ctn" 
					},
					{
						targets: 8,
						 data: "handlingLineCustomerCode" ,
						 name: "handlingLineFk.customerCode" 
					},
					{
						targets: 9,
						 data: "retailerLocationName" ,
						 name: "retailerLocationFk.name" 
					}
            ]
        });

        function getPurchaseRegistrations() {
            dataTable.ajax.reload();
        }

        function deletePurchaseRegistration(purchaseRegistration) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _purchaseRegistrationsService.delete({
                            id: purchaseRegistration.id
                        }).done(function () {
                            getPurchaseRegistrations(true);
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
            _purchaseRegistrationsService
                .getPurchaseRegistrationsToExcel({
				filter : $('#PurchaseRegistrationsTableFilter').val(),
					minQuantityFilter: $('#MinQuantityFilterId').val(),
					maxQuantityFilter: $('#MaxQuantityFilterId').val(),
					minTotalAmountFilter: $('#MinTotalAmountFilterId').val(),
					maxTotalAmountFilter: $('#MaxTotalAmountFilterId').val(),
					minPurchaseDateFilter:  getDateFilter($('#MinPurchaseDateFilterId')),
					maxPurchaseDateFilter:  getMaxDateFilter($('#MaxPurchaseDateFilterId')),
					invoiceImageFilter: $('#InvoiceImageFilterId').val(),
					registrationFirstNameFilter: $('#RegistrationFirstNameFilterId').val(),
					productCtnFilter: $('#ProductCtnFilterId').val(),
					handlingLineCustomerCodeFilter: $('#HandlingLineCustomerCodeFilterId').val(),
					retailerLocationNameFilter: $('#RetailerLocationNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPurchaseRegistrationModalSaved', function () {
            getPurchaseRegistrations();
        });

		$('#GetPurchaseRegistrationsButton').click(function (e) {
            e.preventDefault();
            getPurchaseRegistrations();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPurchaseRegistrations();
		  }
		});
		
		
		
    });
})();
