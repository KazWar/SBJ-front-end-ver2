(function () {
    $(function () {

        var _$registrationJsonDatasTable = $('#RegistrationJsonDatasTable');
        var _registrationJsonDatasService = abp.services.app.registrationJsonDatas;
		var _entityTypeFullName = 'RMS.SBJ.RegistrationJsonData.RegistrationJsonData';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.RegistrationJsonDatas.Create'),
            edit: abp.auth.hasPermission('Pages.RegistrationJsonDatas.Edit'),
            'delete': abp.auth.hasPermission('Pages.RegistrationJsonDatas.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/RegistrationJsonDatas/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RegistrationJsonDatas/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditRegistrationJsonDataModal'
                });
                   

		 var _viewRegistrationJsonDataModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RegistrationJsonDatas/ViewregistrationJsonDataModal',
            modalClass: 'ViewRegistrationJsonDataModal'
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

        var dataTable = _$registrationJsonDatasTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _registrationJsonDatasService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#RegistrationJsonDatasTableFilter').val(),
					dataFilter: $('#DataFilterId').val(),
					minDateCreatedFilter:  getDateFilter($('#MinDateCreatedFilterId')),
					maxDateCreatedFilter:  getMaxDateFilter($('#MaxDateCreatedFilterId')),
					registrationFirstNameFilter: $('#RegistrationFirstNameFilterId').val()
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
                                    _viewRegistrationJsonDataModal.open({ id: data.record.registrationJsonData.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.registrationJsonData.id });                                
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
                                    entityId: data.record.registrationJsonData.id
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
                                deleteRegistrationJsonData(data.record.registrationJsonData);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "registrationJsonData.data",
						 name: "data"   
					},
					{
						targets: 3,
						 data: "registrationJsonData.dateCreated",
						 name: "dateCreated" ,
					render: function (dateCreated) {
						if (dateCreated) {
							return moment(dateCreated).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 4,
						 data: "registrationFirstName" ,
						 name: "registrationFk.firstName" 
					}
            ]
        });

        function getRegistrationJsonDatas() {
            dataTable.ajax.reload();
        }

        function deleteRegistrationJsonData(registrationJsonData) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _registrationJsonDatasService.delete({
                            id: registrationJsonData.id
                        }).done(function () {
                            getRegistrationJsonDatas(true);
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

        $('#CreateNewRegistrationJsonDataButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _registrationJsonDatasService
                .getRegistrationJsonDatasToExcel({
				filter : $('#RegistrationJsonDatasTableFilter').val(),
					dataFilter: $('#DataFilterId').val(),
					minDateCreatedFilter:  getDateFilter($('#MinDateCreatedFilterId')),
					maxDateCreatedFilter:  getMaxDateFilter($('#MaxDateCreatedFilterId')),
					registrationFirstNameFilter: $('#RegistrationFirstNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditRegistrationJsonDataModalSaved', function () {
            getRegistrationJsonDatas();
        });

		$('#GetRegistrationJsonDatasButton').click(function (e) {
            e.preventDefault();
            getRegistrationJsonDatas();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getRegistrationJsonDatas();
		  }
		});
		
		
		
    });
})();
