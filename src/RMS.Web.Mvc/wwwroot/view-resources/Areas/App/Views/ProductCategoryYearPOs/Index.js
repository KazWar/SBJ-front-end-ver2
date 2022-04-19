(function () {
    $(function () {

        var _$productCategoryYearPOsTable = $('#ProductCategoryYearPOsTable');
        var _productCategoryYearPOsService = abp.services.app.productCategoryYearPOs;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ProductCategoryYearPOs.Create'),
            edit: abp.auth.hasPermission('Pages.ProductCategoryYearPOs.Edit'),
            'delete': abp.auth.hasPermission('Pages.ProductCategoryYearPOs.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProductCategoryYearPOs/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProductCategoryYearPOs/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProductCategoryYearPOModal'
        });       

		 var _viewProductCategoryYearPOModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProductCategoryYearPOs/ViewproductCategoryYearPOModal',
            modalClass: 'ViewProductCategoryYearPOModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$productCategoryYearPOsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productCategoryYearPOsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ProductCategoryYearPOsTableFilter').val(),
					minYearFilter: $('#MinYearFilterId').val(),
					maxYearFilter: $('#MaxYearFilterId').val(),
					pONumberHandlingFilter: $('#PONumberHandlingFilterId').val(),
					pONumberCashbackFilter: $('#PONumberCashbackFilterId').val(),
					productCategoryCodeFilter: $('#ProductCategoryCodeFilterId').val()
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
                                    _viewProductCategoryYearPOModal.open({ id: data.record.productCategoryYearPO.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.productCategoryYearPO.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteProductCategoryYearPO(data.record.productCategoryYearPO);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "productCategoryYearPO.year",
						 name: "year"   
					},
					{
						targets: 2,
						 data: "productCategoryYearPO.poNumberHandling",
						 name: "poNumberHandling"   
					},
					{
						targets: 3,
						 data: "productCategoryYearPO.poNumberCashback",
						 name: "poNumberCashback"   
					},
					{
						targets: 4,
						 data: "productCategoryCode" ,
						 name: "productCategoryFk.code" 
					}
            ]
        });

        function getProductCategoryYearPOs() {
            dataTable.ajax.reload();
        }

        function deleteProductCategoryYearPO(productCategoryYearPO) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _productCategoryYearPOsService.delete({
                            id: productCategoryYearPO.id
                        }).done(function () {
                            getProductCategoryYearPOs(true);
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

        $('#CreateNewProductCategoryYearPOButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _productCategoryYearPOsService
                .getProductCategoryYearPOsToExcel({
				filter : $('#ProductCategoryYearPOsTableFilter').val(),
					minYearFilter: $('#MinYearFilterId').val(),
					maxYearFilter: $('#MaxYearFilterId').val(),
					pONumberHandlingFilter: $('#PONumberHandlingFilterId').val(),
					pONumberCashbackFilter: $('#PONumberCashbackFilterId').val(),
					productCategoryCodeFilter: $('#ProductCategoryCodeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditProductCategoryYearPOModalSaved', function () {
            getProductCategoryYearPOs();
        });

		$('#GetProductCategoryYearPOsButton').click(function (e) {
            e.preventDefault();
            getProductCategoryYearPOs();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getProductCategoryYearPOs();
		  }
		});
    });
})();