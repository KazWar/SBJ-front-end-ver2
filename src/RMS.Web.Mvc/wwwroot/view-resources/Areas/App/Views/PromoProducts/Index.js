(function () {
    $(function () {

        var _$promoProductsTable = $('#PromoProductsTable');
        var _promoProductsService = abp.services.app.promoProducts;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PromoProducts.Create'),
            edit: abp.auth.hasPermission('Pages.PromoProducts.Edit'),
            'delete': abp.auth.hasPermission('Pages.PromoProducts.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoProducts/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoProducts/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPromoProductModal'
        });       

		 var _viewPromoProductModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoProducts/ViewpromoProductModal',
            modalClass: 'ViewPromoProductModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$promoProductsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _promoProductsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PromoProductsTableFilter').val(),
					promoPromocodeFilter: $('#PromoPromocodeFilterId').val(),
					productCtnFilter: $('#ProductCtnFilterId').val()
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
                                    _viewPromoProductModal.open({ id: data.record.promoProduct.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.promoProduct.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePromoProduct(data.record.promoProduct);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "promoPromocode" ,
						 name: "promoFk.promocode" 
					},
					{
						targets: 2,
						 data: "productCtn" ,
						 name: "productFk.ctn" 
					}
            ]
        });

        function getPromoProducts() {
            dataTable.ajax.reload();
        }

        function deletePromoProduct(promoProduct) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _promoProductsService.delete({
                            id: promoProduct.id
                        }).done(function () {
                            getPromoProducts(true);
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

        $('#CreateNewPromoProductButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _promoProductsService
                .getPromoProductsToExcel({
				filter : $('#PromoProductsTableFilter').val(),
					promoPromocodeFilter: $('#PromoPromocodeFilterId').val(),
					productCtnFilter: $('#ProductCtnFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPromoProductModalSaved', function () {
            getPromoProducts();
        });

		$('#GetPromoProductsButton').click(function (e) {
            e.preventDefault();
            getPromoProducts();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPromoProducts();
		  }
		});
    });
})();