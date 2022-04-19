(function () {
    $(function () {

        var _$listValueTranslationsTable = $('#ListValueTranslationsTable');
        var _listValueTranslationsService = abp.services.app.listValueTranslations;
		var _entityTypeFullName = 'RMS.SBJ.Forms.ListValueTranslation';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ListValueTranslations.Create'),
            edit: abp.auth.hasPermission('Pages.ListValueTranslations.Edit'),
            'delete': abp.auth.hasPermission('Pages.ListValueTranslations.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ListValueTranslations/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ListValueTranslations/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditListValueTranslationModal'
        });       

		 var _viewListValueTranslationModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ListValueTranslations/ViewlistValueTranslationModal',
            modalClass: 'ViewListValueTranslationModal'
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

        var dataTable = _$listValueTranslationsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _listValueTranslationsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ListValueTranslationsTableFilter').val(),
					keyValueFilter: $('#KeyValueFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					listValueKeyValueFilter: $('#ListValueKeyValueFilterId').val(),
					localeLanguageCodeFilter: $('#LocaleLanguageCodeFilterId').val()
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
                                    _viewListValueTranslationModal.open({ id: data.record.listValueTranslation.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.listValueTranslation.id });                                
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
                                    entityId: data.record.listValueTranslation.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteListValueTranslation(data.record.listValueTranslation);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "listValueTranslation.keyValue",
						 name: "keyValue"   
					},
					{
						targets: 2,
						 data: "listValueTranslation.description",
						 name: "description"   
					},
					{
						targets: 3,
						 data: "listValueKeyValue" ,
						 name: "listValueFk.keyValue" 
					},
					{
						targets: 4,
						 data: "localeLanguageCode" ,
						 name: "localeFk.languageCode" 
					}
            ]
        });

        function getListValueTranslations() {
            dataTable.ajax.reload();
        }

        function deleteListValueTranslation(listValueTranslation) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _listValueTranslationsService.delete({
                            id: listValueTranslation.id
                        }).done(function () {
                            getListValueTranslations(true);
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

        $('#CreateNewListValueTranslationButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _listValueTranslationsService
                .getListValueTranslationsToExcel({
				filter : $('#ListValueTranslationsTableFilter').val(),
					keyValueFilter: $('#KeyValueFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					listValueKeyValueFilter: $('#ListValueKeyValueFilterId').val(),
					localeLanguageCodeFilter: $('#LocaleLanguageCodeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditListValueTranslationModalSaved', function () {
            getListValueTranslations();
        });

		$('#GetListValueTranslationsButton').click(function (e) {
            e.preventDefault();
            getListValueTranslations();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getListValueTranslations();
		  }
		});
    });
})();