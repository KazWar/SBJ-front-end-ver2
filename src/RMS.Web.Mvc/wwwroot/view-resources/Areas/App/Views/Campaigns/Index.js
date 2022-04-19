(function () {
    $(function () {

        var _$campaignsTable = $('#CampaignsTable');
        var _campaignsService = abp.services.app.campaigns;
        var _entityTypeFullName = 'RMS.SBJ.CampaignProcesses.Campaign';

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Campaigns.Create'),
            edit: abp.auth.hasPermission('Pages.Campaigns.Edit'),
            'delete': abp.auth.hasPermission('Pages.Campaigns.Delete')
        };

        //var _createOrEditModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'App/Campaigns/CreateOrEditModal',
        //    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Campaigns/_CreateOrEditModal.js',
        //    modalClass: 'CreateOrEditCampaignModal'
        //});
                   
		//var _viewCampaignModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'App/Campaigns/ViewcampaignModal',
        //    modalClass: 'ViewCampaignModal'
        //});

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

        var getMaxDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z");
        }

        var dataTable = _$campaignsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _campaignsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#CampaignsTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        descriptionFilter: $('#DescriptionFilterId').val(),
                        minStartDateFilter: getDateFilter($('#MinStartDateFilterId')),
                        maxStartDateFilter: getMaxDateFilter($('#MaxStartDateFilterId')),
                        minEndDateFilter: getDateFilter($('#MinEndDateFilterId')),
                        maxEndDateFilter: getMaxDateFilter($('#MaxEndDateFilterId')),
                        minCampaignCodeFilter: $('#MinCampaignCodeFilterId').val(),
                        maxCampaignCodeFilter: $('#MaxCampaignCodeFilterId').val(),
                        externalCodeFilter: $('#ExternalCodeFilterId').val()
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
                                    window.location = "/App/Campaigns/CampaignOverview?campaignId=" + data.record.campaign.id + "&editable=false";
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                window.location = "/App/Campaigns/CreateOrEdit/" + data.record.campaign.id;
                            }
                        },
                        {
                            text: app.localize('Duplicate'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                window.location = "/App/Campaigns/Duplicate/" + data.record.campaign.id;
                            }
                        },
                        {
                            text: app.localize('History'),
                            iconStyle: 'fas fa-history mr-2',
                            visible: function () {
                                return entityHistoryIsEnabled();
                            },
                            action: function (data) {
                                _entityTypeHistoryModal.open({
                                    entityTypeFullName: _entityTypeFullName,
                                    entityId: data.record.campaign.id
                                });
                            }
						}]
                    }
                },
                {
                    targets: 2,
                    data: "campaign.name",
                    name: "name"
                },
                {
                    targets: 3,
                    data: "campaign.startDate",
                    name: "startDate",
                    render: function (startDate) {
                        if (startDate) {
                            return moment(startDate).format('DD-MM-YYYY HH:mm');
                        }
                        return "";
                    }
                },
                {
                    targets: 4,
                    data: "campaign.endDate",
                    name: "endDate",
                    render: function (endDate) {
                        if (endDate) {
                            return moment(endDate).format('DD-MM-YYYY HH:mm');
                        }
                        return "";
                    }
                },
                {
                    targets: 5,
                    data: "campaign.campaignCode",
                    name: "campaignCode"
                },
                {
                    targets: 6,
                    data: "campaign.externalCode",
                    name: "externalCode"
                }
            ]
        });

        function getCampaigns() {
            dataTable.ajax.reload();
        }

        function deleteCampaign(campaign) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _campaignsService.delete({
                            id: campaign.id
                        }).done(function () {
                            getCampaigns(true);
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

        $('#CreateNewCampaignButton').click(function () {
            _campaignsService.checkCompanySetup()
                .done(function (result) {
                    if (result === true) {
                        window.location = "/App/Campaigns/CreateOrEdit";
                    } else {
                        window.location = "/App/Campaigns/CheckCompanyAlert";
                    }
                });
        });

        $('#ExportToExcelButton').click(function () {
            _campaignsService
                .getCampaignsToExcel({
                    filter: $('#CampaignsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    descriptionFilter: $('#DescriptionFilterId').val(),
                    minStartDateFilter: getDateFilter($('#MinStartDateFilterId')),
                    maxStartDateFilter: getMaxDateFilter($('#MaxStartDateFilterId')),
                    minEndDateFilter: getDateFilter($('#MinEndDateFilterId')),
                    maxEndDateFilter: getMaxDateFilter($('#MaxEndDateFilterId')),
                    minCampaignCodeFilter: $('#MinCampaignCodeFilterId').val(),
                    maxCampaignCodeFilter: $('#MaxCampaignCodeFilterId').val(),
                    externalCodeFilter: $('#ExternalCodeFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCampaignModalSaved', function () {
            getCampaigns();
        });

        $('#GetCampaignsButton').click(function (e) {
            e.preventDefault();
            getCampaigns();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getCampaigns();
            }
        });



    });
})();
