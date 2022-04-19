(function () {
    $(function () {
        var _$promosTable = $('#PromosTable');
        var _promosService = abp.services.app.promos;
        var _entityTypeFullName = 'RMS.PromoPlanner.Promo';

        function setFilterCookie(filter) {
            const dateString = new Date(Date.now() + 864e8).toUTCString();
            const cookieString = `promo_filter=${filter}; expires=${dateString}; path=/;`
            document.cookie = cookieString;
        }

        function getCookie(c_name) {
            if (document.cookie.length > 0) {
                c_start = document.cookie.indexOf(c_name + '=');
                if (c_start !== -1) {
                    c_start = c_start + c_name.length + 1;
                    c_end = document.cookie.indexOf(';', c_start);

                    if (c_end === -1) {
                        c_end = document.cookie.length;
                    }

                    return unescape(document.cookie.substring(c_start, c_end));
                }
            }

            return '';
        }

        $('#ShowAdvancedFiltersSpan').hide();
        $('#HideAdvancedFiltersSpan').show();
        $('#AdvacedAuditFiltersArea').slideDown();

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Promos.Create'),
            edit: abp.auth.hasPermission('Pages.Promos.Edit'),
            'delete': abp.auth.hasPermission('Pages.Promos.Delete')
        };

        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();

        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        //Set filters from Cookie
        var cookieContent = getCookie('promo_filter');
        if (cookieContent) {
            const cookieFilter = JSON.parse(cookieContent);

            $('#PromosTableFilter').val(cookieFilter.filter);
            $('#PromocodeFilterId').val(cookieFilter.promocodeFilter);
            $('#DescriptionFilterId').val(cookieFilter.descriptionFilter);

            if (cookieFilter.minPromoStartFilter) {
                $('#MinPromoStartFilterId').val(moment(cookieFilter.minPromoStartFilter).format('L'));
            }
            if (cookieFilter.maxPromoStartFilter) {
                $('#MaxPromoStartFilterId').val(moment(cookieFilter.maxPromoStartFilter).format('L'));
            }
            if (cookieFilter.minPromoEndFilter) {
                $('#MinPromoEndFilterId').val(moment(cookieFilter.minPromoEndFilter).format('L'));
            }
            if (cookieFilter.maxPromoEndFilter) {
                $('#MaxPromoEndFilterId').val(moment(cookieFilter.maxPromoEndFilter).format('L'));
            }

            $('#PromoScopeFilterId').val(cookieFilter.promoScopeFilter);
            $('#CampaignTypeFilterId').val(cookieFilter.campaignTypeFilter);
            $('#ProductCategoryFilterId').val(cookieFilter.productCategoryFilter);

            //Set RetailerList Selected Values  
            if (cookieFilter.retailerFilter) {
                $('#RetailerFilterId').val(cookieFilter.retailerFilter.split(','));
            }
        }

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });


        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var getRetailers = function () {
            var retailers = [];

            $.each($('#RetailerFilterId option:selected'), function () {
                retailers.push($(this).val());
            });

            return String(retailers);
        }

        var getFilter = function () {
            return {
                filter: $('#PromosTableFilter').val(),
                promocodeFilter: $('#PromocodeFilterId').val(),
                descriptionFilter: $('#DescriptionFilterId').val(),
                minPromoStartFilter: getDateFilter($('#MinPromoStartFilterId')),
                maxPromoStartFilter: getDateFilter($('#MaxPromoStartFilterId')),
                minPromoEndFilter: getDateFilter($('#MinPromoEndFilterId')),
                maxPromoEndFilter: getDateFilter($('#MaxPromoEndFilterId')),
                promoScopeFilter: $('#PromoScopeFilterId').val(),
                campaignTypeFilter: $('#CampaignTypeFilterId').val(),
                productCategoryFilter: $('#ProductCategoryFilterId').val(),
                retailerFilter: getRetailers()
            };
        }

        var dataTable = _$promosTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            stateSave: true,
            listAction: {
                ajaxFunction: _promosService.getAll,
                inputFilter: function () {
                    return getFilter()
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    data: '<i class="fa fa-plus"></i>',
                    orderable: false,
                    width: 50,
                    autoWidth: false,
                    defaultContent: '<i class="fa fa-plus"></i>',
                    className: 'details-control expand-options text-center d-xxl-none'
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
                                action: function (data) {
                                    window.location.href = "/App/Promos/ViewPromo?id=" + data.record.promo.id;
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    window.location.href = "/App/Promos/CreateOrEdit?id=" + data.record.promo.id;
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
                                        entityId: data.record.promo.id
                                    });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deletePromo(data.record.promo);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "promo.promocode",
                    name: "promocode"
                },
                {
                    targets: 3,
                    data: "promo.description",
                    name: "description",
                    width: "20%",
                    className: 'promo-description'
                },
                {
                    targets: 4,
                    data: "status",
                    name: "status",
                    orderable: false,
                    render: function (status) {
                        var color = "";
                        switch (status) {
                            case "Pending":
                                color = "text-info";
                                break;
                            case "Running":
                                color = "text-success";
                                break;
                            case "Finished":
                                color = "text-warning";
                                break;
                            default:
                                color = "purple-seance";
                        }
                        var htmlstring = "<span class='" + color + "'>" + status + "</span>";
                        return htmlstring;
                    }
                },
                {
                    targets: 5,
                    data: "campaignTypeName",
                    name: "campaignTypeFk.name"
                },
                {
                    targets: 6,
                    data: "promo.promoStart",
                    name: "promoStart",
                    render: function (promoStart) {
                        if (promoStart) {
                            return moment(promoStart).format('L');
                        }
                        return "";
                    }

                },
                {
                    targets: 7,
                    data: "promo.promoEnd",
                    name: "promoEnd",
                    render: function (promoEnd) {
                        if (promoEnd) {
                            return moment(promoEnd).format('L');
                        }
                        return "";
                    }

                },
                {
                    targets: 8,
                    data: "promoProgress",
                    name: "promoProgress",
                    orderable: false,
                    render: function (promoProgress) {

                        var htmlstring = "";
                        $.each(promoProgress, function (index, step) {

                            var confirmed = step.confirmed;
                            var description = step.description;
                            if (confirmed) {
                                htmlstring += "<i class='fa fa-check fa-fw text-success' title='" + description + "' ></i>"
                            }
                            else {
                                htmlstring += "<i class='fa fa-times fa-fw text-danger' title='" + description + "' ></i>"
                            }

                        });
                        return htmlstring;
                    }
                }
            ]
        });

        function getPromos() {
            dataTable.ajax.reload();
        }

        function deletePromo(promo) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _promosService.delete({
                            id: promo.id
                        }).done(function () {
                            getPromos(true);
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

        $('#SaveFilter').click(function () {
            setFilterCookie(JSON.stringify(getFilter()));
        });

        $('#ResetFilter').click(function () {
            var emptyJson = '{"filter":"","promocodeFilter":"","descriptionFilter":"","minPromoStartFilter":null,"maxPromoStartFilter":null,"minPromoEndFilter":null,"maxPromoEndFilter":null,"promoScopeFilter":null,"campaignTypeFilter":null,"productCategoryFilter":null,"retailerFilter":""}';
            setFilterCookie(emptyJson);
            location.reload();
        });

        $('#CreateNewPromoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _promosService
                .getPromosToExcel({
                    filter: $('#PromosTableFilter').val(),
                    promocodeFilter: $('#PromocodeFilterId').val(),
                    descriptionFilter: $('#DescriptionFilterId').val(),
                    minPromoStartFilter: getDateFilter($('#MinPromoStartFilterId')),
                    maxPromoStartFilter: getDateFilter($('#MaxPromoStartFilterId')),
                    minPromoEndFilter: getDateFilter($('#MinPromoEndFilterId')),
                    maxPromoEndFilter: getDateFilter($('#MaxPromoEndFilterId')),
                    promoScopeFilter: $('#PromoScopeFilterId').val(),
                    campaignTypeFilter: $('#CampaignTypeFilterId').val(),
                    productCategoryFilter: $('#ProductCategoryFilterId').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPromoModalSaved', function () {
            getPromos();
        });

        $('#GetPromosButton').click(function (e) {
            e.preventDefault();
            getPromos();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getPromos();
            }
        });
    });
})();