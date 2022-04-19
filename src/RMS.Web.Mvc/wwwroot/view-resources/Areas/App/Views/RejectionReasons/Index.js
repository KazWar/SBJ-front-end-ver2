(function () {
    $(function () {

        var _$rejectionReasonsTable = $('#RejectionReasonsTable');
        var _rejectionReasonsService = abp.services.app.rejectionReasons;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.RejectionReasons.Create'),
            edit: abp.auth.hasPermission('Pages.RejectionReasons.Edit'),
            'delete': abp.auth.hasPermission('Pages.RejectionReasons.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RejectionReasons/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RejectionReasons/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditRejectionReasonModal'
        });


        var _viewRejectionReasonModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RejectionReasons/ViewrejectionReasonModal',
            modalClass: 'ViewRejectionReasonModal'
        });




        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var getMaxDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z");
        }

        var dataTable = _$rejectionReasonsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rejectionReasonsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#RejectionReasonsTableFilter').val(),
                        descriptionFilter: $('#DescriptionFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    },
                    targets: 0
                },
                {
                    width: 120,
                    targets: 1,
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
                                iconStyle: 'far fa-eye mr-2',
                                action: function (data) {
                                    _viewRejectionReasonModal.open({ id: data.record.rejectionReason.id });
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.rejectionReason.id });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                iconStyle: 'far fa-trash-alt mr-2',
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteRejectionReason(data.record.rejectionReason);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "rejectionReason.description",
                    name: "description"
                }
            ]
        });

        function getRejectionReasons() {
            dataTable.ajax.reload();
        }

        function deleteRejectionReason(rejectionReason) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _rejectionReasonsService.delete({
                            id: rejectionReason.id
                        }).done(function () {
                            getRejectionReasons(true);
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

        $('#CreateNewRejectionReasonButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _rejectionReasonsService
                .getRejectionReasonsToExcel({
                    filter: $('#RejectionReasonsTableFilter').val(),
                    descriptionFilter: $('#DescriptionFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditRejectionReasonModalSaved', function () {
            getRejectionReasons();
        });

        $('#GetRejectionReasonsButton').click(function (e) {
            e.preventDefault();
            getRejectionReasons();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getRejectionReasons();
            }
        });



    });
})();
