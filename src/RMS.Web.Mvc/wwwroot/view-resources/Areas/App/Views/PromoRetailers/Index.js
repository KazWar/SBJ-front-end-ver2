(function () {
    $(function () {

        var _$promoRetailersTable = $('#PromoRetailersTable');
        var _promoRetailersService = abp.services.app.promoRetailers;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PromoRetailers.Create'),
            edit: abp.auth.hasPermission('Pages.PromoRetailers.Edit'),
            'delete': abp.auth.hasPermission('Pages.PromoRetailers.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoRetailers/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoRetailers/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPromoRetailerModal'
        });       

		 var _viewPromoRetailerModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoRetailers/ViewpromoRetailerModal',
            modalClass: 'ViewPromoRetailerModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$promoRetailersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _promoRetailersService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PromoRetailersTableFilter').val(),
					promoPromocodeFilter: $('#PromoPromocodeFilterId').val(),
					retailerCodeFilter: $('#RetailerCodeFilterId').val()
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
                                    _viewPromoRetailerModal.open({ id: data.record.promoRetailer.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.promoRetailer.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePromoRetailer(data.record.promoRetailer);
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
						 data: "retailerCode" ,
						 name: "retailerFk.code" 
					}
            ]
        });

        function getPromoRetailers() {
            dataTable.ajax.reload();
        }

        function deletePromoRetailer(promoRetailer) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _promoRetailersService.delete({
                            id: promoRetailer.id
                        }).done(function () {
                            getPromoRetailers(true);
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

        $('#CreateNewPromoRetailerButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _promoRetailersService
                .getPromoRetailersToExcel({
				filter : $('#PromoRetailersTableFilter').val(),
					promoPromocodeFilter: $('#PromoPromocodeFilterId').val(),
					retailerCodeFilter: $('#RetailerCodeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPromoRetailerModalSaved', function () {
            getPromoRetailers();
        });

		$('#GetPromoRetailersButton').click(function (e) {
            e.preventDefault();
            getPromoRetailers();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPromoRetailers();
		  }
		});
    });
})();