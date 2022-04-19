(function () {
    $(function () {

        var _$handlingLinesTable = $('#HandlingLinesTable');
        var _handlingLinesService = abp.services.app.handlingLines;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.HandlingLines.Create'),
            edit: abp.auth.hasPermission('Pages.HandlingLines.Edit'),
            'delete': abp.auth.hasPermission('Pages.HandlingLines.Delete')
        };

               

		 var _viewHandlingLineModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HandlingLines/ViewhandlingLineModal',
            modalClass: 'ViewHandlingLineModal'
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

        var dataTable = _$handlingLinesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _handlingLinesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#HandlingLinesTableFilter').val(),
					minMinimumPurchaseAmountFilter: $('#MinMinimumPurchaseAmountFilterId').val(),
					maxMinimumPurchaseAmountFilter: $('#MaxMinimumPurchaseAmountFilterId').val(),
					minMaximumPurchaseAmountFilter: $('#MinMaximumPurchaseAmountFilterId').val(),
					maxMaximumPurchaseAmountFilter: $('#MaxMaximumPurchaseAmountFilterId').val(),
					customerCodeFilter: $('#CustomerCodeFilterId').val(),
					minAmountFilter: $('#MinAmountFilterId').val(),
					maxAmountFilter: $('#MaxAmountFilterId').val(),
					fixedFilter: $('#FixedFilterId').val(),
					activationCodeFilter: $('#ActivationCodeFilterId').val(),
					minQuantityFilter: $('#MinQuantityFilterId').val(),
					maxQuantityFilter: $('#MaxQuantityFilterId').val(),
					campaignTypeNameFilter: $('#CampaignTypeNameFilterId').val(),
					productHandlingDescriptionFilter: $('#ProductHandlingDescriptionFilterId').val()
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
                                    window.location="/App/HandlingLines/ViewHandlingLine/" + data.record.handlingLine.id;
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            window.location="/App/HandlingLines/CreateOrEdit/" + data.record.handlingLine.id;                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteHandlingLine(data.record.handlingLine);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "handlingLine.minimumPurchaseAmount",
						 name: "minimumPurchaseAmount"   
					},
					{
						targets: 3,
						 data: "handlingLine.maximumPurchaseAmount",
						 name: "maximumPurchaseAmount"   
					},
					{
						targets: 4,
						 data: "handlingLine.customerCode",
						 name: "customerCode"   
					},
					{
						targets: 5,
						 data: "handlingLine.amount",
						 name: "amount"   
					},
					{
						targets: 6,
						 data: "handlingLine.fixed",
						 name: "fixed"  ,
						render: function (fixed) {
							if (fixed) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 7,
						 data: "handlingLine.activationCode",
						 name: "activationCode"  ,
						render: function (activationCode) {
							if (activationCode) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 8,
						 data: "handlingLine.quantity",
						 name: "quantity"   
					},
					{
						targets: 9,
						 data: "campaignTypeName" ,
						 name: "campaignTypeFk.name" 
					},
					{
						targets: 10,
						 data: "productHandlingDescription" ,
						 name: "productHandlingFk.description" 
					}
            ]
        });

        function getHandlingLines() {
            dataTable.ajax.reload();
        }

        function deleteHandlingLine(handlingLine) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _handlingLinesService.delete({
                            id: handlingLine.id
                        }).done(function () {
                            getHandlingLines(true);
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
            _handlingLinesService
                .getHandlingLinesToExcel({
				filter : $('#HandlingLinesTableFilter').val(),
					minMinimumPurchaseAmountFilter: $('#MinMinimumPurchaseAmountFilterId').val(),
					maxMinimumPurchaseAmountFilter: $('#MaxMinimumPurchaseAmountFilterId').val(),
					minMaximumPurchaseAmountFilter: $('#MinMaximumPurchaseAmountFilterId').val(),
					maxMaximumPurchaseAmountFilter: $('#MaxMaximumPurchaseAmountFilterId').val(),
					customerCodeFilter: $('#CustomerCodeFilterId').val(),
					minAmountFilter: $('#MinAmountFilterId').val(),
					maxAmountFilter: $('#MaxAmountFilterId').val(),
					fixedFilter: $('#FixedFilterId').val(),
					activationCodeFilter: $('#ActivationCodeFilterId').val(),
					minQuantityFilter: $('#MinQuantityFilterId').val(),
					maxQuantityFilter: $('#MaxQuantityFilterId').val(),
					campaignTypeNameFilter: $('#CampaignTypeNameFilterId').val(),
					productHandlingDescriptionFilter: $('#ProductHandlingDescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditHandlingLineModalSaved', function () {
            getHandlingLines();
        });

		$('#GetHandlingLinesButton').click(function (e) {
            e.preventDefault();
            getHandlingLines();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getHandlingLines();
		  }
		});
		
		
		
    });
})();
