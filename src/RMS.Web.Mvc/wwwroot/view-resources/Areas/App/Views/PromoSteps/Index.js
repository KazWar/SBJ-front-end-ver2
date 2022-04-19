﻿(function () {
    $(function () {

        var _$promoStepsTable = $('#PromoStepsTable');
        var _promoStepsService = abp.services.app.promoSteps;
		var _entityTypeFullName = 'RMS.PromoPlanner.PromoStep';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PromoSteps.Create'),
            edit: abp.auth.hasPermission('Pages.PromoSteps.Edit'),
            'delete': abp.auth.hasPermission('Pages.PromoSteps.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoSteps/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoSteps/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPromoStepModal'
        });       

		 var _viewPromoStepModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoSteps/ViewpromoStepModal',
            modalClass: 'ViewPromoStepModal'
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

        var dataTable = _$promoStepsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _promoStepsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PromoStepsTableFilter').val(),
					minSequenceFilter: $('#MinSequenceFilterId').val(),
					maxSequenceFilter: $('#MaxSequenceFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val()
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
                                    _viewPromoStepModal.open({ id: data.record.promoStep.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.promoStep.id });                                
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
                                    entityId: data.record.promoStep.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePromoStep(data.record.promoStep);
                            }
                        }]
                    }
                },
				{
					targets: 1,
						data: "promoStep.sequence",
						name: "sequence"   
				},
				{
					targets: 2,
						data: "promoStep.description",
						name: "description"   
				}
            ]
        });

        function getPromoSteps() {
            dataTable.ajax.reload();
        }

        function deletePromoStep(promoStep) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _promoStepsService.delete({
                            id: promoStep.id
                        }).done(function () {
                            getPromoSteps(true);
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

        $('#CreateNewPromoStepButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _promoStepsService
                .getPromoStepsToExcel({
				filter : $('#PromoStepsTableFilter').val(),
					minSequenceFilter: $('#MinSequenceFilterId').val(),
					maxSequenceFilter: $('#MaxSequenceFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPromoStepModalSaved', function () {
            getPromoSteps();
        });

		$('#GetPromoStepsButton').click(function (e) {
            e.preventDefault();
            getPromoSteps();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPromoSteps();
		  }
		});
    });
})();