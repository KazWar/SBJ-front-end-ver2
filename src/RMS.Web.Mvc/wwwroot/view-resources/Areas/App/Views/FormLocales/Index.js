(function () {
    $(function () {

        var _$formLocalesTable = $('#FormLocalesTable');
        var _formLocalesService = abp.services.app.formLocales;
		var _entityTypeFullName = 'RMS.SBJ.Forms.FormLocale';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.FormLocales.Create'),
            edit: abp.auth.hasPermission('Pages.FormLocales.Edit'),
            'delete': abp.auth.hasPermission('Pages.FormLocales.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/FormLocales/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormLocales/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditFormLocaleModal'
                });
                   

		 var _viewFormLocaleModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormLocales/ViewformLocaleModal',
            modalClass: 'ViewFormLocaleModal'
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

        var dataTable = _$formLocalesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _formLocalesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#FormLocalesTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					formVersionFilter: $('#FormVersionFilterId').val(),
					localeDescriptionFilter: $('#LocaleDescriptionFilterId').val()
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
                                action: function (data) {
                                    _viewFormLocaleModal.open({ id: data.record.formLocale.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.formLocale.id });                                
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
                                    entityId: data.record.formLocale.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteFormLocale(data.record.formLocale);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "formLocale.description",
						 name: "description"   
					},
					{
						targets: 3,
						 data: "formVersion" ,
						 name: "formFk.version" 
					},
					{
						targets: 4,
						 data: "localeDescription" ,
						 name: "localeFk.description" 
					}
            ]
        });

        function getFormLocales() {
            dataTable.ajax.reload();
        }

        function deleteFormLocale(formLocale) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _formLocalesService.delete({
                            id: formLocale.id
                        }).done(function () {
                            getFormLocales(true);
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
        //    _createOrEditModal.open();
        //});        

		$('#ExportToExcelButton').click(function () {
            _formLocalesService
                .getFormLocalesToExcel({
				filter : $('#FormLocalesTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					formVersionFilter: $('#FormVersionFilterId').val(),
					localeDescriptionFilter: $('#LocaleDescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditFormLocaleModalSaved', function () {
            getFormLocales();
        });

		$('#GetFormLocalesButton').click(function (e) {
            e.preventDefault();
            getFormLocales();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getFormLocales();
		  }
		});
		
		
		
    });
})();