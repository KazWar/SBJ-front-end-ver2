
(function () {
    $(function () {
        var _$registrationsTable = $('#RegistrationsTable');
        var _registrationsService = abp.services.app.registrations;
        var dataTable;

        $('#ShowAdvancedFiltersSpan').hide();
        $('#HideAdvancedFiltersSpan').show();
        $('#AdvacedAuditFiltersArea').slideDown();

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        const cookieContent = getCookie('registration_filter');
        if (cookieContent) {
            const cookieFilter = JSON.parse(cookieContent);

            $('#RegistrationsTableFilter').val(cookieFilter.filter);
            $('#FirstNameFilterId').val(cookieFilter.firstNameFilter);
            $('#LastNameFilterId').val(cookieFilter.lastNameFilter);
            $('#CityFilterId').val(cookieFilter.cityFilter);
            $('#EmailAddressFilterId').val(cookieFilter.emailAddressFilter);
            $('#SerialNumberFilterId').val(cookieFilter.serialNumberFilter);

            if (cookieFilter.registrationStatusFilter) {
                $('#RegistrationStatusFilterId').val(cookieFilter.registrationStatusFilter.split(','));
            }
            $('#ActiveCampaignsOnlyFilterId').prop('checked', cookieFilter.activeCampaignsOnlyFilter);
        }

        populateCampaignDropdownContent();

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Registrations.Create'),
            edit: abp.auth.hasPermission('Pages.Registrations.Edit'),
            'delete': abp.auth.hasPermission('Pages.Registrations.Delete')
        };

        var _viewRegistrationModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Registrations/ViewregistrationModal',
            modalClass: 'ViewRegistrationModal'
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

        $('#RegistrationStatusFilterId').on('change', getRegistrations);
        $('#CampaignDescriptionFilterId').on('change', getRegistrations);

        function getRegistrations() {
            dataTable.ajax.reload();
        }

        function deleteRegistration(registration) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _registrationsService.delete({
                            id: registration.id
                        }).done(function () {
                            getRegistrations(true);
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

        $('#ExportToExcelButton').click(function () {
            _registrationsService
                .getRegistrationsToExcel({
                    filter: $('#RegistrationsTableFilter').val(),
                    firstNameFilter: $('#FirstNameFilterId').val(),
                    lastNameFilter: $('#LastNameFilterId').val(),
                    streetFilter: $('#StreetFilterId').val(),
                    postalCodeFilter: $('#PostalCodeFilterId').val(),
                    cityFilter: $('#CityFilterId').val(),
                    emailAddressFilter: $('#EmailAddressFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        function setFilterCookie(filter) {
            if (getCookie('registration_filter')) {
                document.cookie = "registration_filter= ; expires = Thu, 01 Jan 1970 00:00:00 GMT; path=/App;";
            }

            const dateString = new Date(Date.now() + 864e8).toUTCString();
            const cookieString = `registration_filter=${filter}; expires=${dateString}; path=/App;`;

            document.cookie = cookieString;
        }

        function getCookie(c_name) {
            if (document.cookie.length) {
                let c_start = document.cookie.indexOf(c_name + '=');

                if (c_start !== -1) {
                    c_start += c_name.length + 1;

                    let c_end = document.cookie.indexOf(';', c_start);
                    if (c_end === -1) {
                        c_end = document.cookie.length;
                    }

                    return unescape(document.cookie.substring(c_start, c_end));
                }
            }

            return '';
        }

        function getRegistrationStatuses() {
            let registrationStatuses = [];

            $.each($('#RegistrationStatusFilterId option:selected'), function () {
                registrationStatuses.push($(this).val());
            });

            return String(registrationStatuses);
        }

        function getFilters() {

            const x = {
                filter: $('#RegistrationsTableFilter').val(),
                firstNameFilter: $('#FirstNameFilterId').val(),
                lastNameFilter: $('#LastNameFilterId').val(),
                cityFilter: $('#CityFilterId').val(),
                emailAddressFilter: $('#EmailAddressFilterId').val(),
                campaignDescriptionFilter: $('#CampaignDescriptionFilterId').val(),
                serialNumberFilter: $('#SerialNumberFilterId').val(),
                registrationStatusFilter: getRegistrationStatuses(),
                activeCampaignsOnlyFilter: $('#ActiveCampaignsOnlyFilterId').prop('checked')
            };
            
            return x;
        }

        function populateCampaignDropdownContent() {
            var ddlCampaigns = $('#CampaignDescriptionFilterId');

            $.ajax({
                url: 'Campaigns/GetCampaignDropdownContent',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    activeCampaignsOnly: $('#ActiveCampaignsOnlyFilterId').is(':checked')
                }),
                success: function (result) {

                    $.each(result, function () {
                        var cookieContent = getCookie('registration_filter');
                        if (cookieContent && this.campaign.id == JSON.parse(cookieContent).campaignDescriptionFilter) {
                            ddlCampaigns.append('<option value="' + this.campaign.id + '" selected>' + this.campaign.name + '</option>');
                        }
                        else {
                            ddlCampaigns.append('<option value="' + this.campaign.id + '">' + this.campaign.name + '</option>');
                        }
                    });

                    dataTable = _$registrationsTable.DataTable({
                        paging: true,
                        serverSide: true,
                        stateSave: true,
                        processing: true,
                        listAction: {
                            ajaxFunction: _registrationsService.getAll,
                            inputFilter: function () {
                                return getFilters()
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
                                            iconStyle: 'far fa-eye mr-2',
                                            action: function (data) {
                                                window.location = "/App/Registrations/ViewRegistration/" + data.record.registration.id;
                                            }
                                        },
                                        {
                                            text: app.localize('Edit'),
                                            iconStyle: 'far fa-edit mr-2',
                                            visible: function (data) {
                                                //return _permissions.edit && (
                                                //    data.record.registrationStatusStatusCode !== '300' &&
                                                //    data.record.registrationStatusStatusCode !== '400');

                                                return _permissions.edit;
                                            },
                                            action: function (data) {
                                                window.location = "/App/Registrations/CreateOrEdit/" + data.record.registration.id;
                                            }
                                        }]
                                }
                            },
                            {
                                targets: 1,
                                data: "registrationStatusStatusCode",
                                name: "registrationStatusStatusCode",
                                orderable: false,
                                render: function (registrationStatusStatusCode) {
                                    let renderString;

                                    switch (registrationStatusStatusCode) {
                                        case '200':
                                            renderString = '<i class="fas fa-check-circle color-approved"></i>';
                                            break;
                                        case '500':
                                            renderString = '<i class="far fa-dot-circle color-incomplete"></i>';
                                            break;
                                        case '999':
                                            renderString = '<i class="fas fa-times-circle color-rejected"></i>';
                                            break;
                                        case '300':
                                            renderString = '<i class="fas fa-spinner color-progress"></i>';
                                            break;
                                        case '400':
                                            renderString = '<i class="fas fa-warehouse color-sent"></i>';
                                            break;
                                        case '110':
                                            renderString = '<i class="fas fa-file-invoice-dollar color-awaiting-invoice-check"></i>';
                                            break;
                                        default:
                                            renderString = '<i class="far fa-circle color-pending"></i>';
                                            break;
                                    }

                                    return renderString;
                                }
                            },
                            {
                                targets: 2,
                                orderable: false,
                                data: "externalCode",
                                name: "externalCode"
                            },
                            {
                                targets: 3,
                                orderable: false,
                                data: "productCode",
                                name: "productCode"
                            },
                            {
                                targets: 4,
                                orderable: false,
                                data: "registration.id",
                                name: "registrationId"
                            },
                            {
                                targets: 5,
                                orderable: false,
                                data: "registration.firstName",
                                name: "firstName"
                            },
                            {
                                targets: 6,
                                orderable: false,
                                data: "registration.lastName",
                                name: "lastName"
                            },
                            {
                                targets: 7,
                                orderable: false,
                                data: "registration.city",
                                name: "city"
                            },
                            {
                                targets: 8,
                                orderable: false,
                                data: "registration.emailAddress",
                                name: "emailAddress"
                            },
                            {
                                targets: 9,
                                orderable: false,
                                data: "dateCreated",
                                name: "dateCreated"
                            }
                        ]
                    });
                },
                error: function (error) {
                    //console.log("Error call to controller - error: " + error);
                }
            });
        }

        abp.event.on('app.createOrEditRegistrationModalSaved', function () {
            getRegistrations();
        });

        $('#ActiveCampaignsOnlyFilterId').change(function () {
            const jsonContent = JSON.stringify(getFilters());
            setFilterCookie(jsonContent);

            var ddlCampaigns = $('#CampaignDescriptionFilterId');
            $.each(ddlCampaigns.find('option'), function () {
                if (this.value != -1) {
                    this.remove();
                }
            });

            populateCampaignDropdownContent();
            getRegistrations();
        });

        $('#AdvacedAuditFiltersArea input').change(function () {
            const jsonContent = JSON.stringify(getFilters());
            setFilterCookie(jsonContent);
        });

        $('#AdvacedAuditFiltersArea select').change(function () {
            const jsonContent = JSON.stringify(getFilters());
            setFilterCookie(jsonContent);
        });

        $('#RegistrationsTableFilter').change(function () {
            const jsonContent = JSON.stringify(getFilters());
            setFilterCookie(jsonContent);
        });

        $('#ResetFilter').click(function () {
            const emptyJson = `
                {
                    "filter": "",
                    "firstNameFilter": "",
                    "lastNameFilter": "",
                    "cityFilter": "",
                    "emailAddressFilter": "",
                    "campaignDescriptionFilter": "",
                    "serialNumberFilter": "",
                    "registrationStatusFilter": "",
                    "activeCampaignsOnlyFilter": "true"
                }`;

            setFilterCookie(emptyJson);
            location.reload();
        });

        $('#GetRegistrationsButton').click(function (e) {
            e.preventDefault();
            getRegistrations();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getRegistrations();
            }
        });
    });
})();
