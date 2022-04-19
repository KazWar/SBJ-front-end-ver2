(function () {
    $(function () {

        var _$makitaSerialNumbersTable = $('#MakitaSerialNumbersTable');
        var _makitaSerialNumbersService = abp.services.app.makitaSerialNumbers;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.MakitaSerialNumbers.Create'),
            edit: abp.auth.hasPermission('Pages.MakitaSerialNumbers.Edit'),
            'delete': abp.auth.hasPermission('Pages.MakitaSerialNumbers.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/MakitaSerialNumbers/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MakitaSerialNumbers/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditMakitaSerialNumberModal'
                });
                   

		 var _viewMakitaSerialNumberModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MakitaSerialNumbers/ViewmakitaSerialNumberModal',
            modalClass: 'ViewMakitaSerialNumberModal'
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

        var dataTable = _$makitaSerialNumbersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _makitaSerialNumbersService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#MakitaSerialNumbersTableFilter').val(),
					productCodeFilter: $('#ProductCodeFilterId').val(),
					serialNumberFilter: $('#SerialNumberFilterId').val(),
					retailerExternalCodeFilter: $('#RetailerExternalCodeFilterId').val()
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
                                    _viewMakitaSerialNumberModal.open({ id: data.record.makitaSerialNumber.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.makitaSerialNumber.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteMakitaSerialNumber(data.record.makitaSerialNumber);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "makitaSerialNumber.productCode",
						 name: "productCode"   
					},
					{
						targets: 3,
						 data: "makitaSerialNumber.serialNumber",
						 name: "serialNumber"   
					},
					{
						targets: 4,
						 data: "makitaSerialNumber.retailerExternalCode",
						 name: "retailerExternalCode"   
					}
            ]
        });

        function getMakitaSerialNumbers() {
            dataTable.ajax.reload();
        }

        function deleteMakitaSerialNumber(makitaSerialNumber) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _makitaSerialNumbersService.delete({
                            id: makitaSerialNumber.id
                        }).done(function () {
                            getMakitaSerialNumbers(true);
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

        $('#CreateNewMakitaSerialNumberButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _makitaSerialNumbersService
                .getMakitaSerialNumbersToExcel({
				filter : $('#MakitaSerialNumbersTableFilter').val(),
					productCodeFilter: $('#ProductCodeFilterId').val(),
					serialNumberFilter: $('#SerialNumberFilterId').val(),
					retailerExternalCodeFilter: $('#RetailerExternalCodeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditMakitaSerialNumberModalSaved', function () {
            getMakitaSerialNumbers();
        });

		$('#GetMakitaSerialNumbersButton').click(function (e) {
            e.preventDefault();
            getMakitaSerialNumbers();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getMakitaSerialNumbers();
		  }
		});
		
		
		
    });
})();
