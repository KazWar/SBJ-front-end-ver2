(function () {
    $(function () {

        var _$formFieldTranslationsTable = $('#FormFieldTranslationsTable');
        var _formFieldTranslationsService = abp.services.app.formFieldTranslations;
		var _entityTypeFullName = 'RMS.SBJ.Forms.FormFieldTranslation';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.FormFieldTranslations.Create'),
            edit: abp.auth.hasPermission('Pages.FormFieldTranslations.Edit'),
            'delete': abp.auth.hasPermission('Pages.FormFieldTranslations.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormFieldTranslations/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormFieldTranslations/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditFormFieldTranslationModal'
        });       

		 var _viewFormFieldTranslationModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormFieldTranslations/ViewformFieldTranslationModal',
            modalClass: 'ViewFormFieldTranslationModal'
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

        var dataTable = _$formFieldTranslationsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _formFieldTranslationsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#FormFieldTranslationsTableFilter').val(),
					labelFilter: $('#LabelFilterId').val(),
					defaultValueFilter: $('#DefaultValueFilterId').val(),
					regularExpressionFilter: $('#RegularExpressionFilterId').val(),
					formFieldDescriptionFilter: $('#FormFieldDescriptionFilterId').val(),
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
                                    _viewFormFieldTranslationModal.open({ id: data.record.formFieldTranslation.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.formFieldTranslation.id });                                
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
                                    entityId: data.record.formFieldTranslation.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteFormFieldTranslation(data.record.formFieldTranslation);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "formFieldTranslation.label",
						 name: "label"   
					},
					{
						targets: 2,
						 data: "formFieldTranslation.defaultValue",
						 name: "defaultValue"   
					},
					{
						targets: 3,
						 data: "formFieldTranslation.regularExpression",
						 name: "regularExpression"   
					},
					{
						targets: 4,
						 data: "formFieldDescription" ,
						 name: "formFieldFk.description" 
					},
					{
						targets: 5,
						 data: "localeLanguageCode" ,
						 name: "localeFk.languageCode" 
					}
            ]
        });

        function getFormFieldTranslations() {
            dataTable.ajax.reload();
        }

        function deleteFormFieldTranslation(formFieldTranslation) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _formFieldTranslationsService.delete({
                            id: formFieldTranslation.id
                        }).done(function () {
                            getFormFieldTranslations(true);
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

        $('#CreateNewFormFieldTranslationButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _formFieldTranslationsService
                .getFormFieldTranslationsToExcel({
				filter : $('#FormFieldTranslationsTableFilter').val(),
					labelFilter: $('#LabelFilterId').val(),
					defaultValueFilter: $('#DefaultValueFilterId').val(),
					regularExpressionFilter: $('#RegularExpressionFilterId').val(),
					formFieldDescriptionFilter: $('#FormFieldDescriptionFilterId').val(),
					localeLanguageCodeFilter: $('#LocaleLanguageCodeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditFormFieldTranslationModalSaved', function () {
            getFormFieldTranslations();
        });

		$('#GetFormFieldTranslationsButton').click(function (e) {
            e.preventDefault();
            getFormFieldTranslations();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getFormFieldTranslations();
		  }
		});
    });
})();