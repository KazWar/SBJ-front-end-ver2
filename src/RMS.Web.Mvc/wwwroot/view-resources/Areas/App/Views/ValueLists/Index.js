(function () {
    $(function () {

        var _$valueListsTable = $('#ValueListsTable');
        var _valueListsService = abp.services.app.valueLists;
		var _entityTypeFullName = 'RMS.SBJ.Forms.ValueList';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ValueLists.Create'),
            edit: abp.auth.hasPermission('Pages.ValueLists.Edit'),
            'delete': abp.auth.hasPermission('Pages.ValueLists.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ValueLists/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ValueLists/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditValueListModal'
        });       

		 var _viewValueListModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ValueLists/ViewvalueListModal',
            modalClass: 'ViewValueListModal'
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

        var dataTable = _$valueListsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _valueListsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ValueListsTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					listValueApiCallFilter: $('#ListValueApiCallFilterId').val()
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
                                    _viewValueListModal.open({ id: data.record.valueList.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.valueList.id });                                
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
                                    entityId: data.record.valueList.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteValueList(data.record.valueList);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "valueList.description",
						 name: "description"   
					},
					{
						targets: 2,
						 data: "valueList.listValueApiCall",
						 name: "listValueApiCall"   
					}
            ]
        });

        function getValueLists() {
            dataTable.ajax.reload();
        }

        function deleteValueList(valueList) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _valueListsService.delete({
                            id: valueList.id
                        }).done(function () {
                            getValueLists(true);
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

        $('#CreateNewValueListButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _valueListsService
                .getValueListsToExcel({
				filter : $('#ValueListsTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					listValueApiCallFilter: $('#ListValueApiCallFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditValueListModalSaved', function () {
            getValueLists();
        });

		$('#GetValueListsButton').click(function (e) {
            e.preventDefault();
            getValueLists();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getValueLists();
		  }
		});
    });
})();