(function () {
    $(function () {

        var _$listValuesTable = $('#ListValuesTable');
        var _listValuesService = abp.services.app.listValues;
		var _entityTypeFullName = 'RMS.SBJ.Forms.ListValue';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ListValues.Create'),
            edit: abp.auth.hasPermission('Pages.ListValues.Edit'),
            'delete': abp.auth.hasPermission('Pages.ListValues.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ListValues/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ListValues/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditListValueModal'
        });       

		 var _viewListValueModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ListValues/ViewlistValueModal',
            modalClass: 'ViewListValueModal'
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

        var dataTable = _$listValuesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _listValuesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ListValuesTableFilter').val(),
					keyValueFilter: $('#KeyValueFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					minSortOrderFilter: $('#MinSortOrderFilterId').val(),
					maxSortOrderFilter: $('#MaxSortOrderFilterId').val(),
					valueListDescriptionFilter: $('#ValueListDescriptionFilterId').val()
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
                                    _viewListValueModal.open({ id: data.record.listValue.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.listValue.id });                                
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
                                    entityId: data.record.listValue.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteListValue(data.record.listValue);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "listValue.keyValue",
						 name: "keyValue"   
					},
					{
						targets: 2,
						 data: "listValue.description",
						 name: "description"   
					},
					{
						targets: 3,
						 data: "listValue.sortOrder",
						 name: "sortOrder"   
					},
					{
						targets: 4,
						 data: "valueListDescription" ,
						 name: "valueListFk.description" 
					}
            ]
        });

        function getListValues() {
            dataTable.ajax.reload();
        }

        function deleteListValue(listValue) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _listValuesService.delete({
                            id: listValue.id
                        }).done(function () {
                            getListValues(true);
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

        $('#CreateNewListValueButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _listValuesService
                .getListValuesToExcel({
				filter : $('#ListValuesTableFilter').val(),
					keyValueFilter: $('#KeyValueFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					minSortOrderFilter: $('#MinSortOrderFilterId').val(),
					maxSortOrderFilter: $('#MaxSortOrderFilterId').val(),
					valueListDescriptionFilter: $('#ValueListDescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditListValueModalSaved', function () {
            getListValues();
        });

		$('#GetListValuesButton').click(function (e) {
            e.preventDefault();
            getListValues();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getListValues();
		  }
		});
    });
})();