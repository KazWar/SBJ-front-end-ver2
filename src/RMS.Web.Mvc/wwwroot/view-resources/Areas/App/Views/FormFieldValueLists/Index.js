(function () {
    $(function () {

        var _$formFieldValueListsTable = $('#FormFieldValueListsTable');
        var _formFieldValueListsService = abp.services.app.formFieldValueLists;
		var _entityTypeFullName = 'RMS.SBJ.Forms.FormFieldValueList';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.FormFieldValueLists.Create'),
            edit: abp.auth.hasPermission('Pages.FormFieldValueLists.Edit'),
            'delete': abp.auth.hasPermission('Pages.FormFieldValueLists.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormFieldValueLists/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormFieldValueLists/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditFormFieldValueListModal'
        });       

		 var _viewFormFieldValueListModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormFieldValueLists/ViewformFieldValueListModal',
            modalClass: 'ViewFormFieldValueListModal'
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

        var dataTable = _$formFieldValueListsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _formFieldValueListsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#FormFieldValueListsTableFilter').val(),
					possibleListValuesFilter: $('#PossibleListValuesFilterId').val(),
					formFieldDescriptionFilter: $('#FormFieldDescriptionFilterId').val(),
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
                                    _viewFormFieldValueListModal.open({ id: data.record.formFieldValueList.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.formFieldValueList.id });                                
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
                                    entityId: data.record.formFieldValueList.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteFormFieldValueList(data.record.formFieldValueList);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "formFieldValueList.possibleListValues",
						 name: "possibleListValues"   
					},
					{
						targets: 2,
						 data: "formFieldDescription" ,
						 name: "formFieldFk.description" 
					},
					{
						targets: 3,
						 data: "valueListDescription" ,
						 name: "valueListFk.description" 
					}
            ]
        });

        function getFormFieldValueLists() {
            dataTable.ajax.reload();
        }

        function deleteFormFieldValueList(formFieldValueList) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _formFieldValueListsService.delete({
                            id: formFieldValueList.id
                        }).done(function () {
                            getFormFieldValueLists(true);
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

        $('#CreateNewFormFieldValueListButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _formFieldValueListsService
                .getFormFieldValueListsToExcel({
				filter : $('#FormFieldValueListsTableFilter').val(),
					possibleListValuesFilter: $('#PossibleListValuesFilterId').val(),
					formFieldDescriptionFilter: $('#FormFieldDescriptionFilterId').val(),
					valueListDescriptionFilter: $('#ValueListDescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditFormFieldValueListModalSaved', function () {
            getFormFieldValueLists();
        });

		$('#GetFormFieldValueListsButton').click(function (e) {
            e.preventDefault();
            getFormFieldValueLists();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getFormFieldValueLists();
		  }
		});
    });
})();