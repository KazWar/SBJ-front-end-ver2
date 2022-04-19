(function () {
    $(function () {

        var _$promoStepFieldsTable = $('#PromoStepFieldsTable');
        var _promoStepFieldsService = abp.services.app.promoStepFields;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PromoStepFields.Create'),
            edit: abp.auth.hasPermission('Pages.PromoStepFields.Edit'),
            'delete': abp.auth.hasPermission('Pages.PromoStepFields.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoStepFields/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoStepFields/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPromoStepFieldModal'
        });       

		 var _viewPromoStepFieldModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoStepFields/ViewpromoStepFieldModal',
            modalClass: 'ViewPromoStepFieldModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$promoStepFieldsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _promoStepFieldsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PromoStepFieldsTableFilter').val(),
					minFormFieldIdFilter: $('#MinFormFieldIdFilterId').val(),
					maxFormFieldIdFilter: $('#MaxFormFieldIdFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					minSequenceFilter: $('#MinSequenceFilterId').val(),
					maxSequenceFilter: $('#MaxSequenceFilterId').val(),
					promoStepDescriptionFilter: $('#PromoStepDescriptionFilterId').val()
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
                                    _viewPromoStepFieldModal.open({ id: data.record.promoStepField.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.promoStepField.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePromoStepField(data.record.promoStepField);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "promoStepField.formFieldId",
						 name: "formFieldId"   
					},
					{
						targets: 2,
						 data: "promoStepField.description",
						 name: "description"   
					},
					{
						targets: 3,
						 data: "promoStepField.sequence",
						 name: "sequence"   
					},
					{
						targets: 4,
						 data: "promoStepDescription" ,
						 name: "promoStepFk.description" 
					}
            ]
        });

        function getPromoStepFields() {
            dataTable.ajax.reload();
        }

        function deletePromoStepField(promoStepField) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _promoStepFieldsService.delete({
                            id: promoStepField.id
                        }).done(function () {
                            getPromoStepFields(true);
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

        $('#CreateNewPromoStepFieldButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _promoStepFieldsService
                .getPromoStepFieldsToExcel({
				filter : $('#PromoStepFieldsTableFilter').val(),
					minFormFieldIdFilter: $('#MinFormFieldIdFilterId').val(),
					maxFormFieldIdFilter: $('#MaxFormFieldIdFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					minSequenceFilter: $('#MinSequenceFilterId').val(),
					maxSequenceFilter: $('#MaxSequenceFilterId').val(),
					promoStepDescriptionFilter: $('#PromoStepDescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPromoStepFieldModalSaved', function () {
            getPromoStepFields();
        });

		$('#GetPromoStepFieldsButton').click(function (e) {
            e.preventDefault();
            getPromoStepFields();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPromoStepFields();
		  }
		});
    });
})();