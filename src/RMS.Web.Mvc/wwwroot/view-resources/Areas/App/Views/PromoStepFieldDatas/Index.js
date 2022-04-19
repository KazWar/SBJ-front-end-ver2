(function () {
    $(function () {

        var _$promoStepFieldDatasTable = $('#PromoStepFieldDatasTable');
        var _promoStepFieldDatasService = abp.services.app.promoStepFieldDatas;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PromoStepFieldDatas.Create'),
            edit: abp.auth.hasPermission('Pages.PromoStepFieldDatas.Edit'),
            'delete': abp.auth.hasPermission('Pages.PromoStepFieldDatas.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoStepFieldDatas/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoStepFieldDatas/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPromoStepFieldDataModal'
        });       

		 var _viewPromoStepFieldDataModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoStepFieldDatas/ViewpromoStepFieldDataModal',
            modalClass: 'ViewPromoStepFieldDataModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$promoStepFieldDatasTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _promoStepFieldDatasService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PromoStepFieldDatasTableFilter').val(),
					valueFilter: $('#ValueFilterId').val(),
					promoStepFieldDescriptionFilter: $('#PromoStepFieldDescriptionFilterId').val(),
					promoStepDataDescriptionFilter: $('#PromoStepDataDescriptionFilterId').val()
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
                                    _viewPromoStepFieldDataModal.open({ id: data.record.promoStepFieldData.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.promoStepFieldData.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePromoStepFieldData(data.record.promoStepFieldData);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "promoStepFieldData.value",
						 name: "value"   
					},
					{
						targets: 2,
						 data: "promoStepFieldDescription" ,
						 name: "promoStepFieldFk.description" 
					},
					{
						targets: 3,
						 data: "promoStepDataDescription" ,
						 name: "promoStepDataFk.description" 
					}
            ]
        });

        function getPromoStepFieldDatas() {
            dataTable.ajax.reload();
        }

        function deletePromoStepFieldData(promoStepFieldData) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _promoStepFieldDatasService.delete({
                            id: promoStepFieldData.id
                        }).done(function () {
                            getPromoStepFieldDatas(true);
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

        $('#CreateNewPromoStepFieldDataButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _promoStepFieldDatasService
                .getPromoStepFieldDatasToExcel({
				filter : $('#PromoStepFieldDatasTableFilter').val(),
					valueFilter: $('#ValueFilterId').val(),
					promoStepFieldDescriptionFilter: $('#PromoStepFieldDescriptionFilterId').val(),
					promoStepDataDescriptionFilter: $('#PromoStepDataDescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPromoStepFieldDataModalSaved', function () {
            getPromoStepFieldDatas();
        });

		$('#GetPromoStepFieldDatasButton').click(function (e) {
            e.preventDefault();
            getPromoStepFieldDatas();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPromoStepFieldDatas();
		  }
		});
    });
})();