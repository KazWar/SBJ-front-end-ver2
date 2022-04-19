(function () {
    $(function () {

        var _$projectManagersTable = $('#ProjectManagersTable');
        var _projectManagersService = abp.services.app.projectManagers;
		var _entityTypeFullName = 'RMS.SBJ.Company.ProjectManager';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ProjectManagers.Create'),
            edit: abp.auth.hasPermission('Pages.ProjectManagers.Edit'),
            'delete': abp.auth.hasPermission('Pages.ProjectManagers.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProjectManagers/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProjectManagers/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProjectManagerModal'
        });       

		 var _viewProjectManagerModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProjectManagers/ViewprojectManagerModal',
            modalClass: 'ViewProjectManagerModal'
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

        var dataTable = _$projectManagersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _projectManagersService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ProjectManagersTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					phoneNumberFilter: $('#PhoneNumberFilterId').val(),
					emailAddressFilter: $('#EmailAddressFilterId').val(),
					addressPostalCodeFilter: $('#AddressPostalCodeFilterId').val()
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
                                    _viewProjectManagerModal.open({ id: data.record.projectManager.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.projectManager.id });                                
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
                                    entityId: data.record.projectManager.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteProjectManager(data.record.projectManager);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "projectManager.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "projectManager.phoneNumber",
						 name: "phoneNumber"   
					},
					{
						targets: 3,
						 data: "projectManager.emailAddress",
						 name: "emailAddress"   
					},
					{
						targets: 4,
						 data: "addressPostalCode" ,
						 name: "addressFk.postalCode" 
					}
            ]
        });

        function getProjectManagers() {
            dataTable.ajax.reload();
        }

        function deleteProjectManager(projectManager) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _projectManagersService.delete({
                            id: projectManager.id
                        }).done(function () {
                            getProjectManagers(true);
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

        $('#CreateNewProjectManagerButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _projectManagersService
                .getProjectManagersToExcel({
				filter : $('#ProjectManagersTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					phoneNumberFilter: $('#PhoneNumberFilterId').val(),
					emailAddressFilter: $('#EmailAddressFilterId').val(),
					addressPostalCodeFilter: $('#AddressPostalCodeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditProjectManagerModalSaved', function () {
            getProjectManagers();
        });

		$('#GetProjectManagersButton').click(function (e) {
            e.preventDefault();
            getProjectManagers();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getProjectManagers();
		  }
		});
    });
})();