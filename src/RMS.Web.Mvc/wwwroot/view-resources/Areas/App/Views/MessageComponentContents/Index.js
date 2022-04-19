(function () {
    $(function () {
        var _$messageComponentContentsTable = $('#MessageComponentContentsTable');
        var _messageComponentContentsService = abp.services.app.messageComponentContents;
        var _entityTypeFullName = 'RMS.SBJ.CampaignProcesses.MessageComponentContent';

        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.MessageComponentContents.Create'),
            edit: abp.auth.hasPermission('Pages.MessageComponentContents.Edit'),
            delete: abp.auth.hasPermission('Pages.MessageComponentContents.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageComponentContents/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageComponentContents/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditMessageComponentContentModal'
        });

        var _viewMessageComponentContentModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/MessageComponentContents/ViewmessageComponentContentModal',
            modalClass: 'ViewMessageComponentContentModal'
        });

        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        $('.campaignMessageLocale').on('change', function () {
            PopulateMessageComponentContent();
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() === null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$messageComponentContentsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _messageComponentContentsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#MessageComponentContentsTableFilter').val(),
                        contentFilter: $('#ContentFilterId').val(),
                        messageComponentIsActiveFilter: $('#MessageComponentIsActiveFilterId').val(),
                        campaignTypeEventRegistrationStatusSortOrderFilter: $('#CampaignTypeEventRegistrationStatusSortOrderFilterId').val()
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
                                    _viewMessageComponentContentModal.open({ id: data.record.messageComponentContent.id });
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.messageComponentContent.id });
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
                                        entityId: data.record.messageComponentContent.id
                                    });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteMessageComponentContent(data.record.messageComponentContent);
                                }
                            }]
                    }
                },
                {
                    targets: 1,
                    data: "messageComponentContent.content",
                    name: "content"
                },
                {
                    targets: 2,
                    data: "messageComponentIsActive",
                    name: "messageComponentFk.isActive"
                },
                {
                    targets: 3,
                    data: "campaignTypeEventRegistrationStatusSortOrder",
                    name: "campaignTypeEventRegistrationStatusFk.sortOrder"
                }
            ]
        });

        function getMessageComponentContents() {
            dataTable.ajax.reload();
        }

        function PopulateMessageComponentContent() {
            var selectedMessageLocale = $(".campaignMessageLocale option:selected");
            $('#dvMessageComponentContent').load('/App/MessageComponentContents/PopulateMessageComponentContent/', { localeId: selectedMessageLocale[0].dataset.localeid });
            $('#dvMessageComponentContent').show();
        }

        function createEntityModal() {
            new app.ModalManager({
                viewUrl: abp.appPath + 'App/MessageComponentContents/CreateEntityModal',
                scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/MessageComponentContents/_CreateEntityModal.js',
                modalClass: 'CreateMessageComponentContentEntityModal'
            }).open();
        }

        $('.btn__message-component-contents--add').click(function () {
            createEntityModal();
        });

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

        $('#CreateNewMessageComponentContentButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _messageComponentContentsService
                .getMessageComponentContentsToExcel({
                    filter: $('#MessageComponentContentsTableFilter').val(),
                    contentFilter: $('#ContentFilterId').val(),
                    messageComponentIsActiveFilter: $('#MessageComponentIsActiveFilterId').val(),
                    campaignTypeEventRegistrationStatusSortOrderFilter: $('#CampaignTypeEventRegistrationStatusSortOrderFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createMessageComponentContentModalSaved', function () {
            getMessageComponentContents();
        });

        abp.event.on('app.editMessageComponentContentModalSaved', function () {
            getMessageComponentContents();
        });

        $('#GetMessageComponentContentsButton').click(function (e) {
            e.preventDefault();
            getMessageComponentContents();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getMessageComponentContents();
            }
        });
    });
})();