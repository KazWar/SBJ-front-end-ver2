(function () {
    $(function () {

        var _$promoStepDatasTable = $('#PromoStepDatasTable');
        var _promoStepDatasService = abp.services.app.promoStepDatas;
		var _entityTypeFullName = 'RMS.PromoPlanner.PromoStepData';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PromoStepDatas.Create'),
            edit: abp.auth.hasPermission('Pages.PromoStepDatas.Edit'),
            'delete': abp.auth.hasPermission('Pages.PromoStepDatas.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoStepDatas/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoStepDatas/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPromoStepDataModal'
        });       

		 var _viewPromoStepDataModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoStepDatas/ViewpromoStepDataModal',
            modalClass: 'ViewPromoStepDataModal'
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

        var dataTable = _$promoStepDatasTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _promoStepDatasService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PromoStepDatasTableFilter').val(),
					minConfirmationDateFilter:  getDateFilter($('#MinConfirmationDateFilterId')),
					maxConfirmationDateFilter:  getDateFilter($('#MaxConfirmationDateFilterId')),
					descriptionFilter: $('#DescriptionFilterId').val(),
					promoStepDescriptionFilter: $('#PromoStepDescriptionFilterId').val(),
					promoPromocodeFilter: $('#PromoPromocodeFilterId').val()
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
                                    _viewPromoStepDataModal.open({ id: data.record.promoStepData.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.promoStepData.id });                                
                            }
                        },
                        {
                            text: app.localize('History'),
                            visible: function () {
                                return entityHistoryIsEnabled();
                            },
                            action: function (data) {
                                _entityTypeHistoryModal.open({
                                    entityTypeFullName: _entityTypeFullName,
                                    entityId: data.record.promoStepData.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePromoStepData(data.record.promoStepData);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "promoStepData.confirmationDate",
						 name: "confirmationDate" ,
					render: function (confirmationDate) {
						if (confirmationDate) {
							return moment(confirmationDate).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 2,
						 data: "promoStepData.description",
						 name: "description"   
					},
					{
						targets: 3,
						 data: "promoStepDescription" ,
						 name: "promoStepFk.description" 
					},
					{
						targets: 4,
						 data: "promoPromocode" ,
						 name: "promoFk.promocode" 
					}
            ]
        });

        function getPromoStepDatas() {
            dataTable.ajax.reload();
        }

        function deletePromoStepData(promoStepData) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _promoStepDatasService.delete({
                            id: promoStepData.id
                        }).done(function () {
                            getPromoStepDatas(true);
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

        $('#CreateNewPromoStepDataButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _promoStepDatasService
                .getPromoStepDatasToExcel({
				filter : $('#PromoStepDatasTableFilter').val(),
					minConfirmationDateFilter:  getDateFilter($('#MinConfirmationDateFilterId')),
					maxConfirmationDateFilter:  getDateFilter($('#MaxConfirmationDateFilterId')),
					descriptionFilter: $('#DescriptionFilterId').val(),
					promoStepDescriptionFilter: $('#PromoStepDescriptionFilterId').val(),
					promoPromocodeFilter: $('#PromoPromocodeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPromoStepDataModalSaved', function () {
            getPromoStepDatas();
        });

		$('#GetPromoStepDatasButton').click(function (e) {
            e.preventDefault();
            getPromoStepDatas();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPromoStepDatas();
		  }
		});
    });
})();