(function () {
    $(function () {

        var _$systemLevelsTable = $('#SystemLevelsTable');
        var _systemLevelsService = abp.services.app.systemLevels;
		var _entityTypeFullName = 'RMS.SBJ.SystemTables.SystemLevel';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.SystemLevels.Create'),
            edit: abp.auth.hasPermission('Pages.SystemLevels.Edit'),
            'delete': abp.auth.hasPermission('Pages.SystemLevels.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SystemLevels/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/SystemLevels/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditSystemLevelModal'
        });       

		 var _viewSystemLevelModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SystemLevels/ViewsystemLevelModal',
            modalClass: 'ViewSystemLevelModal'
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

        var dataTable = _$systemLevelsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _systemLevelsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#SystemLevelsTableFilter').val(),
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
                                    _viewSystemLevelModal.open({ id: data.record.systemLevel.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.systemLevel.id });                                
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
                                    entityId: data.record.systemLevel.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteSystemLevel(data.record.systemLevel);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "systemLevel.description",
						 name: "description"   
					}
            ]
        });

        function getSystemLevels() {
            dataTable.ajax.reload();
        }

        function deleteSystemLevel(systemLevel) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _systemLevelsService.delete({
                            id: systemLevel.id
                        }).done(function () {
                            getSystemLevels(true);
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

        $('#CreateNewSystemLevelButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _systemLevelsService
                .getSystemLevelsToExcel({
				filter : $('#SystemLevelsTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditSystemLevelModalSaved', function () {
            getSystemLevels();
        });

		$('#GetSystemLevelsButton').click(function (e) {
            e.preventDefault();
            getSystemLevels();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getSystemLevels();
		  }
		});
    });
})();