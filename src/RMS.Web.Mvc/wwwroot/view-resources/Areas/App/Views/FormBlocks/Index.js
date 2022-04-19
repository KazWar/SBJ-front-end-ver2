(function () {
    $(function () {

        var _$formBlocksTable = $('#FormBlocksTable');
        var _formBlocksService = abp.services.app.formBlocks;
		var _entityTypeFullName = 'RMS.SBJ.Forms.FormBlock';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.FormBlocks.Create'),
            edit: abp.auth.hasPermission('Pages.FormBlocks.Edit'),
            'delete': abp.auth.hasPermission('Pages.FormBlocks.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/FormBlocks/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormBlocks/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditFormBlockModal'
                });

        var _chooseFormFieldsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormBlocks/ChooseFormFieldsModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FormBlocks/_ChooseFormFieldsModal.js',
            modalClass: 'ChooseFormFieldsModal'
        });

		 var _viewFormBlockModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FormBlocks/ViewformBlockModal',
            modalClass: 'ViewFormBlockModal'
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
        
        var getMaxDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z"); 
        }

        var dataTable = _$formBlocksTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _formBlocksService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#FormBlocksTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					isPurchaseRegistrationFilter: $('#IsPurchaseRegistrationFilterId').val(),
					minSortOrderFilter: $('#MinSortOrderFilterId').val(),
					maxSortOrderFilter: $('#MaxSortOrderFilterId').val(),
					formLocaleDescriptionFilter: $('#FormLocaleDescriptionFilterId').val()
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
                                    _viewFormBlockModal.open({ id: data.record.formBlock.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.formBlock.id });                                
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
                                    entityId: data.record.formBlock.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteFormBlock(data.record.formBlock);
                            }
                        },
                        {
                            text: app.localize('ChooseFormFields'),
                            action: function (data) {
                                _chooseFormFieldsModal.open({ id: data.record.formBlock.id, description: data.record.formBlock.description });
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "formBlock.description",
						 name: "description"   
					},
					{
						targets: 3,
						 data: "formBlock.isPurchaseRegistration",
						 name: "isPurchaseRegistration"  ,
						render: function (isPurchaseRegistration) {
							if (isPurchaseRegistration) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 4,
						 data: "formBlock.sortOrder",
						 name: "sortOrder"   
					},
					{
						targets: 5,
						 data: "formLocaleDescription" ,
						 name: "formLocaleFk.description" 
					}
            ]
        });

        function getFormBlocks() {
            dataTable.ajax.reload();
        }

        function deleteFormBlock(formBlock) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _formBlocksService.delete({
                            id: formBlock.id
                        }).done(function () {
                            getFormBlocks(true);
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

        $('#CreateNewFormBlockButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _formBlocksService
                .getFormBlocksToExcel({
				filter : $('#FormBlocksTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					isPurchaseRegistrationFilter: $('#IsPurchaseRegistrationFilterId').val(),
					minSortOrderFilter: $('#MinSortOrderFilterId').val(),
					maxSortOrderFilter: $('#MaxSortOrderFilterId').val(),
					formLocaleDescriptionFilter: $('#FormLocaleDescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditFormBlockModalSaved', function () {
            getFormBlocks();
        });

		$('#GetFormBlocksButton').click(function (e) {
            e.preventDefault();
            getFormBlocks();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getFormBlocks();
		  }
		});
		
		
		
    });
})();
