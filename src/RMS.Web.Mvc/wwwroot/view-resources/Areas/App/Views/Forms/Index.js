(function () {
    $(function () {
        var _$formsTable = $('#FormsTable');
        var _formsService = abp.services.app.forms;
        var _entityTypeFullName = 'RMS.SBJ.Forms.Form';

        $('.formLocale').on('change', function () {
            PopulateFormLocaleBlocks();
        });

        function PopulateFormLocaleBlocks() {
            var selectedFormLocale = $(".formLocale option:selected");
            $('#dvFormLocaleBlock').load('/App/Forms/DisplayFormLocaleBlocks/', { formLocaleId: selectedFormLocale[0].dataset.formlocaleid, formLocaleText: selectedFormLocale[0].outerText, localeId: selectedFormLocale[0].dataset.localeid });
            $('#dvFormLocaleBlock').show();
        }

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Forms.Create'),
            edit: abp.auth.hasPermission('Pages.Forms.Edit'),
            'delete': abp.auth.hasPermission('Pages.Forms.Delete')
        };

        var _createOrEditFormLocaleModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormLocales/CreateOrEditModal?category=CompanyForms',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormLocales/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditFormLocaleModal'
        });

        var _createOrEditFormModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Forms/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Forms/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditFormModal'
        });

        var _viewFormModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Forms/ViewformModal',
            modalClass: 'ViewFormModal'
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

        var dataTable = _$formsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _formsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#FormsTableFilter').val(),
                        versionFilter: $('#VersionFilterId').val(),
                        systemLevelDescriptionFilter: $('#SystemLevelDescriptionFilterId').val()
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
                                    _viewFormModal.open({ id: data.record.form.id });
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditFormModal.open({ id: data.record.form.id });
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
                                        entityId: data.record.form.id
                                    });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteForm(data.record.form);
                                }
                            }]
                    }
                },
                {
                    targets: 1,
                    data: "form.version",
                    name: "version"
                },
                {
                    targets: 2,
                    data: "systemLevelDescription",
                    name: "systemLevelFk.description"
                }
            ]
        });

        function getForms() {
            dataTable.ajax.reload();
        }

        function deleteForm(form) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _formsService.delete({
                            id: form.id
                        }).done(function () {
                            getForms(true);
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

        //$('#CreateNewFormLocaleButton').click(function () {
        //    _createOrEditFormLocaleModal.open();
        //});

        $('#CreateNewFormButton').click(function () {
            _createOrEditFormModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _formsService
                .getFormsToExcel({
                    filter: $('#FormsTableFilter').val(),
                    versionFilter: $('#VersionFilterId').val(),
                    systemLevelDescriptionFilter: $('#SystemLevelDescriptionFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditFormModalSaved', function () {
            getForms();
        });

        $('#GetFormsButton').click(function (e) {
            e.preventDefault();
            getForms();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getForms();
            }
        });



  

    });
})();