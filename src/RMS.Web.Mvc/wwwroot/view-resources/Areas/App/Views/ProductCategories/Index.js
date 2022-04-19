(function () {
    $(function () {



        var _$productCategoriesTable = $('#ProductCategoriesTable');
        var _productCategoriesService = abp.services.app.productCategories;
		var _entityTypeFullName = 'RMS.SBJ.Products.ProductCategory';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ProductCategories.Create'),
            edit: abp.auth.hasPermission('Pages.ProductCategories.Edit'),
            'delete': abp.auth.hasPermission('Pages.ProductCategories.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/ProductCategories/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProductCategories/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditProductCategoryModal'
                });
                   

		 var _viewProductCategoryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProductCategories/ViewproductCategoryModal',
            modalClass: 'ViewProductCategoryModal'
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

      

        var dataTable = _$productCategoriesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productCategoriesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ProductCategoriesTableFilter').val(),
					codeFilter: $('#CodeFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					pOHandlingFilter: $('#POHandlingFilterId').val(),
					pOCashbackFilter: $('#POCashbackFilterId').val(),
					colorFilter: $('#ColorFilterId').val()
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
                                    console.log(data);
                                    _viewProductCategoryModal.open({ id: data.record.productCategory.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.productCategory.id });                                
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
                                    entityId: data.record.productCategory.id
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
                                deleteProductCategory(data.record.productCategory);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "productCategory.code",
						 name: "code"   
					},
					{
						targets: 3,
						 data: "productCategory.description",
						 name: "description"   
					},
					{
						targets: 4,
						 data: "productCategory.poHandling",
						 name: "poHandling"   
					},
					{
					   targets: 5,
						 data: "productCategory.poCashback",
						 name: "poCashback"   
					},
					{
                       targets: 6,
						data: "productCategory.color",
                        name: "color",
                        render: function (data, type, row, meta) {
                            return '<div class="minicolors minicolors-theme-bootstrap minicolors-position-bottom" disabled><input type="text" disabled style="background-color:transparent; border-style:none;" name="color" class="form-control colorpicker minicolors-input" data-control="hue" value="' + data + '" color="' + data + '" size="7"/><span disabled class="minicolors-swatch minicolors-sprite minicolors-input-swatch"><span disabled class="minicolors-swatch-color" style="background-color:' + data + '; opacity: 1;"></span></span></div>'
                        
                        }
                         
                    }
            ]
        });


            
        

        function getProductCategories() {
            dataTable.ajax.reload();
        }

        function deleteProductCategory(productCategory) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _productCategoriesService.delete({
                            id: productCategory.id
                        }).done(function () {
                            getProductCategories(true);
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

        $('#CreateNewProductCategoryButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _productCategoriesService
                .getProductCategoriesToExcel({
				filter : $('#ProductCategoriesTableFilter').val(),
					codeFilter: $('#CodeFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					pOHandlingFilter: $('#POHandlingFilterId').val(),
					pOCashbackFilter: $('#POCashbackFilterId').val(),
					colorFilter: $('#ColorFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditProductCategoryModalSaved', function () {
            getProductCategories();
        });

		$('#GetProductCategoriesButton').click(function (e) {
            e.preventDefault();
            getProductCategories();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getProductCategories();
		  }
		});
		
		
		
    });

})();
