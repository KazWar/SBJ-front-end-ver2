(function () {
    $(function () {

        var _$localesTable = $('#LocalesTable');
        var _localesService = abp.services.app.locales;
		var _entityTypeFullName = 'RMS.SBJ.CodeTypeTables.Locale';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Locales.Create'),
            edit: abp.auth.hasPermission('Pages.Locales.Edit'),
            'delete': abp.auth.hasPermission('Pages.Locales.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Locales/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Locales/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditLocaleModal'
        });       

		 var _viewLocaleModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Locales/ViewlocaleModal',
            modalClass: 'ViewLocaleModal'
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

        var dataTable = _$localesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _localesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#LocalesTableFilter').val(),
					languageCodeFilter: $('#LanguageCodeFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					countryCountryCodeFilter: $('#CountryCountryCodeFilterId').val()
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
                                    _viewLocaleModal.open({ id: data.record.locale.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.locale.id });                                
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
                                    entityId: data.record.locale.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteLocale(data.record.locale);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "locale.languageCode",
						 name: "languageCode"   
					},
					{
						targets: 2,
						 data: "locale.description",
						 name: "description"   
					},
					{
						targets: 3,
						 data: "locale.isActive",
						 name: "isActive"  ,
						render: function (isActive) {
							if (isActive) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 4,
						 data: "countryCountryCode" ,
						 name: "countryFk.countryCode" 
					}
            ]
        });

        function getLocales() {
            dataTable.ajax.reload();
        }

        function deleteLocale(locale) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _localesService.delete({
                            id: locale.id
                        }).done(function () {
                            getLocales(true);
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

        $('#CreateNewLocaleButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _localesService
                .getLocalesToExcel({
				filter : $('#LocalesTableFilter').val(),
					languageCodeFilter: $('#LanguageCodeFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					countryCountryCodeFilter: $('#CountryCountryCodeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditLocaleModalSaved', function () {
            getLocales();
        });

		$('#GetLocalesButton').click(function (e) {
            e.preventDefault();
            getLocales();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getLocales();
		  }
		});
    });
})();