(function () {
    $(function () {

        var _$registrationStatusesTable = $('#RegistrationStatusesTable');
        var _registrationStatusesService = abp.services.app.registrationStatuses;
		var _entityTypeFullName = 'RMS.SBJ.CodeTypeTables.RegistrationStatus';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.RegistrationStatuses.Create'),
            edit: abp.auth.hasPermission('Pages.RegistrationStatuses.Edit'),
            'delete': abp.auth.hasPermission('Pages.RegistrationStatuses.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RegistrationStatuses/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RegistrationStatuses/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditRegistrationStatusModal'
        });       

		 var _viewRegistrationStatusModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RegistrationStatuses/ViewregistrationStatusModal',
            modalClass: 'ViewRegistrationStatusModal'
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

        var dataTable = _$registrationStatusesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _registrationStatusesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#RegistrationStatusesTableFilter').val(),
					statusCodeFilter: $('#StatusCodeFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val()
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
                                    _viewRegistrationStatusModal.open({ id: data.record.registrationStatus.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.registrationStatus.id });                                
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
                                    entityId: data.record.registrationStatus.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteRegistrationStatus(data.record.registrationStatus);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "registrationStatus.statusCode",
						 name: "statusCode"   
					},
					{
						targets: 2,
						 data: "registrationStatus.description",
						 name: "description"   
					},
					{
						targets: 3,
						 data: "registrationStatus.isActive",
						 name: "isActive"  ,
						render: function (isActive) {
							if (isActive) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					}
            ]
        });

        function getRegistrationStatuses() {
            dataTable.ajax.reload();
        }

        function deleteRegistrationStatus(registrationStatus) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _registrationStatusesService.delete({
                            id: registrationStatus.id
                        }).done(function () {
                            getRegistrationStatuses(true);
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

        $('#CreateNewRegistrationStatusButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _registrationStatusesService
                .getRegistrationStatusesToExcel({
				filter : $('#RegistrationStatusesTableFilter').val(),
					statusCodeFilter: $('#StatusCodeFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditRegistrationStatusModalSaved', function () {
            getRegistrationStatuses();
        });

		$('#GetRegistrationStatusesButton').click(function (e) {
            e.preventDefault();
            getRegistrationStatuses();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getRegistrationStatuses();
		  }
		});
    });
})();