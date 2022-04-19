(function () {
    $(function () {
        var _$handlingBatchesTable = $('#HandlingBatchesTable');
        var _handlingBatchesService = abp.services.app.handlingBatches;

        let warehouseId = $('#WarehouseId');
        let orderUserId = $('#OrderUserId');
        let password = $('#Password');

        let createPremiumButton = $('#CreatePremiumButton');
        let createCashRefundButton = $('#CreateCashRefundButton');
        let createActivationCodeButton = $('#CreateActivationCodeButton');

        let processPremiumsButton = $('#ProcessPremiumsButton');
        let processWarehouseButton = $('#ProcessWarehouseButton');
        let processSendgridButton = $('#ProcessSendgridButton');

        let premiumResultLabel = $('#PremiumResultLabel');

        $('#ShowAdvancedFiltersSpan').hide();
        $('#HideAdvancedFiltersSpan').show();
        $('#AdvancedAuditFiltersArea').slideDown();

        //$('.date-picker').datetimepicker({
        //    locale: abp.localization.currentLanguage.name,
        //    format: 'DD-MM-YYYY',
        //    defaultDate: new Date()
        //});

        $('#MinDateCreatedFilterId').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'DD-MM-YYYY',
            defaultDate: new Date().setMonth(new Date().getMonth() - 1)
        });

        $('#MaxDateCreatedFilterId').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'DD-MM-YYYY',
            defaultDate: new Date()
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.HandlingBatches.Create'),
            edit: abp.auth.hasPermission('Pages.HandlingBatches.Edit'),
            'delete': abp.auth.hasPermission('Pages.HandlingBatches.Delete')
        };
               
	    var _viewHandlingBatchModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HandlingBatches/ViewhandlingBatchModal',
            modalClass: 'ViewHandlingBatchModal'
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

        var getCampaignTypeFilter = function () {
            let campaignTypes = [];

            $.each($('#CampaignTypeFilterId option:selected'), function () {
                campaignTypes.push($(this).val());
            });

            return String(campaignTypes);
        }

        var getHandlingBatchStatusFilter = function () {
            let handlingBatchStatuses = [];

            $.each($('#HandlingBatchStatusFilterId option:selected'), function () {
                handlingBatchStatuses.push($(this).val());
            });

            return String(handlingBatchStatuses);
        }

        var dataTable = _$handlingBatchesTable.DataTable({
            paging: true,
            serverSide: true,
            stateSave: true,
            processing: true,
            listAction: {
                ajaxFunction: _handlingBatchesService.getAll,
                inputFilter: function () {
                    return {
					    filter: $('#HandlingBatchesTableFilter').val(),
					    minDateCreatedFilter:  getDateFilter($('#MinDateCreatedFilterId')),
					    maxDateCreatedFilter:  getMaxDateFilter($('#MaxDateCreatedFilterId')),
					    remarksFilter: $('#RemarksFilterId').val(),
                        campaignTypeFilter: getCampaignTypeFilter(),
                        handlingBatchStatusFilter: getHandlingBatchStatusFilter()
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
                                iconStyle: 'far fa-eye mr-2',
                                action: function (data) {
                                    if (data.record.handlingBatch.campaignTypeCode === "PM") {
                                        window.location = "/App/HandlingBatches/ViewPremium/" + data.record.handlingBatch.id;
                                    }
                                    else if (data.record.handlingBatch.campaignTypeCode === "CR") {
                                        window.location = "/App/HandlingBatches/ViewCashRefund/" + data.record.handlingBatch.id;
                                    }
                                    else if (data.record.handlingBatch.campaignTypeCode === "AC") {
                                        window.location = "/App/HandlingBatches/ViewActivationCode/" + data.record.handlingBatch.id;
                                    }
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return false;
                            },
                            action: function (data) {
                            window.location="/App/HandlingBatches/CreateOrEdit/" + data.record.handlingBatch.id;                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return false;
                            },
                            action: function (data) {
                                deleteHandlingBatch(data.record.handlingBatch);
                            }
                        }]
                    }
                },
                {
                    targets: 1,
                    data: "handlingBatch.id",
                    name: "id"
                },
				{
					targets: 2,
					data: "campaignTypeName" ,
					name: "campaignTypeFk.name" 
				},
                {
                    targets: 3,
                    data: "handlingBatch.dateCreated",
                    name: "dateCreated",
                    render: function (dateCreated) {
                            if (dateCreated) {
                                return moment(dateCreated).format('DD-MM-YYYY HH:mm');
                            }
                            return "";
                    }
                },
                {
                    targets: 4,
                    data: "handlingBatchStatusStatusDescription",
                    name: "handlingBatchStatusFk.statusDescription"
                }
            ]
        });

        $('#CampaignTypeFilterId').on('change', function () { getHandlingBatches(); })
        $('#HandlingBatchStatusFilterId').on('change', function () { getHandlingBatches(); })
        $('.date-picker').on('dp.change', function () { getHandlingBatches(); })

        function getHandlingBatches() {
            dataTable.ajax.reload();
        }

        function deleteHandlingBatch(handlingBatch) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _handlingBatchesService.delete({
                            id: handlingBatch.id
                        }).done(function () {
                            getHandlingBatches();
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        function disableDashboard(type) {
            if (type === 'PM') {
                $(processPremiumsButton).css('background-color', 'yellow');
            }
            else if (type === 'WH') {
                $(processWarehouseButton).css('background-color', 'yellow');
            }
            else if (type === 'SG') {
                $(processSendgridButton).css('background-color', 'yellow');
            }

            $(processPremiumsButton).prop('disabled', true);
            $(processPremiumsButton).css('cursor', 'default');

            $(processWarehouseButton).prop('disabled', true);
            $(processWarehouseButton).css('cursor', 'default');

            $(processSendgridButton).prop('disabled', true);
            $(processSendgridButton).css('cursor', 'default');
            
            $(createPremiumButton).css('pointer-events', 'none');
            $(createPremiumButton).css('cursor', 'default');
            
            $(createCashRefundButton).css('pointer-events', 'none');
            $(createCashRefundButton).css('cursor', 'default');
            
            $(createActivationCodeButton).css('pointer-events', 'none');
            $(createActivationCodeButton).css('cursor', 'default');
        }

        function enableDashboard(type) {
            if (type === 'PM') {
                $(processPremiumsButton).css('background-color', '');
            }
            else if (type === 'WH') {
                $(processWarehouseButton).css('background-color', '');
            }
            else if (type === 'SG') {
                $(processSendgridButton).css('background-color', '');
            }

            $(processPremiumsButton).prop('disabled', false);
            $(processPremiumsButton).css('cursor', 'pointer');

            $(processWarehouseButton).prop('disabled', false);
            $(processWarehouseButton).css('cursor', 'pointer');

            $(processSendgridButton).prop('disabled', false);
            $(processSendgridButton).css('cursor', 'pointer');
            
            $(createPremiumButton).css('pointer-events', 'auto');
            $(createPremiumButton).css('cursor', 'pointer');
            
            $(createCashRefundButton).css('pointer-events', 'auto');
            $(createCashRefundButton).css('cursor', 'pointer');
            
            $(createActivationCodeButton).css('pointer-events', 'auto');
            $(createActivationCodeButton).css('cursor', 'pointer');
        }

		$('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvancedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvancedAuditFiltersArea').slideUp();
        });       

		//$('#ExportToExcelButton').click(function () {
        //          _handlingBatchesService
        //              .getHandlingBatchesToExcel({
		//		filter : $('#HandlingBatchesTableFilter').val(),
		//			minDateCreatedFilter:  getDateFilter($('#MinDateCreatedFilterId')),
		//			maxDateCreatedFilter:  getMaxDateFilter($('#MaxDateCreatedFilterId')),
		//			remarksFilter: $('#RemarksFilterId').val(),
		//			campaignTypeNameFilter: $('#CampaignTypeNameFilterId').val(),
		//			handlingBatchStatusStatusDescriptionFilter: $('#HandlingBatchStatusStatusDescriptionFilterId').val()
		//		})
        //              .done(function (result) {
        //                  app.downloadTempFile(result);
        //              });
        //      });

        $(processPremiumsButton).click(function () {
            disableDashboard('PM');
            premiumResultLabel.text('Working on it...');
            _handlingBatchesService
                .processPremiums(warehouseId.val(), orderUserId.val(), password.val())
                .done(function () {
                    premiumResultLabel.text(app.localize('ProcessPremiums') + ' is ready');
                    enableDashboard('PM');
                    getHandlingBatches();
                });
        });

        $(processWarehouseButton).click(function () {
            disableDashboard('WH');
            premiumResultLabel.text('Working on it...');
            _handlingBatchesService
                .scanWarehouseStatus(warehouseId.val(), orderUserId.val(), password.val())
                .done(function () {
                    premiumResultLabel.text(app.localize('ScanWarehouseStatus') + ' is ready');
                    enableDashboard('WH');
                });
        });

        $(processSendgridButton).click(function () {
            disableDashboard('SG');
            premiumResultLabel.text('Working on it...');
            _handlingBatchesService
                .scanSendgridStatus()
                .done(function () {
                    premiumResultLabel.text(app.localize('ScanSendgridStatus') + ' is ready');
                    enableDashboard('SG');
                });
        });

        abp.event.on('app.createOrEditHandlingBatchModalSaved', function () {
            getHandlingBatches();
        });

		$('#GetHandlingBatchesButton').click(function (e) {
            e.preventDefault();
            getHandlingBatches();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getHandlingBatches();
		  }
		});						
    });
})();
