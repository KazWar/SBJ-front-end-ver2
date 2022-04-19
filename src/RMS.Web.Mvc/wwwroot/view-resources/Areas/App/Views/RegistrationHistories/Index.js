(function () {
    $(function () {

        var _$registrationHistoriesTable = $('#RegistrationHistoriesTable');
        var _registrationHistoriesService = abp.services.app.registrationHistories;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.RegistrationHistories.Create'),
            edit: abp.auth.hasPermission('Pages.RegistrationHistories.Edit'),
            'delete': abp.auth.hasPermission('Pages.RegistrationHistories.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/RegistrationHistories/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RegistrationHistories/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditRegistrationHistoryModal'
                });
                   

		 var _viewRegistrationHistoryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/RegistrationHistories/ViewregistrationHistoryModal',
            modalClass: 'ViewRegistrationHistoryModal'
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

        var dataTable = _$registrationHistoriesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _registrationHistoriesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#RegistrationHistoriesTableFilter').val(),
					minDateCreatedFilter:  getDateFilter($('#MinDateCreatedFilterId')),
					maxDateCreatedFilter:  getMaxDateFilter($('#MaxDateCreatedFilterId')),
					remarksFilter: $('#RemarksFilterId').val(),
					minAbpUserIdFilter: $('#MinAbpUserIdFilterId').val(),
					maxAbpUserIdFilter: $('#MaxAbpUserIdFilterId').val(),
					registrationStatusStatusCodeFilter: $('#RegistrationStatusStatusCodeFilterId').val(),
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
                                    _viewRegistrationHistoryModal.open({ id: data.record.registrationHistory.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.registrationHistory.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteRegistrationHistory(data.record.registrationHistory);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "registrationHistory.dateCreated",
						 name: "dateCreated" ,
					render: function (dateCreated) {
						if (dateCreated) {
							return moment(dateCreated).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 3,
						 data: "registrationHistory.remarks",
						 name: "remarks"   
					},
					{
						targets: 4,
						 data: "registrationHistory.abpUserId",
						 name: "abpUserId"   
					},
					{
						targets: 5,
						 data: "registrationStatusStatusCode" ,
						 name: "registrationStatusFk.statusCode" 
					},
					{
						targets: 6,
						 data: "registrationFirstName" ,
						 name: "registrationFk.firstName" 
					}
            ]
        });

        function getRegistrationHistories() {
            dataTable.ajax.reload();
        }

        function deleteRegistrationHistory(registrationHistory) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _registrationHistoriesService.delete({
                            id: registrationHistory.id
                        }).done(function () {
                            getRegistrationHistories(true);
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

        $('#CreateNewRegistrationHistoryButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _registrationHistoriesService
                .getRegistrationHistoriesToExcel({
				filter : $('#RegistrationHistoriesTableFilter').val(),
					minDateCreatedFilter:  getDateFilter($('#MinDateCreatedFilterId')),
					maxDateCreatedFilter:  getMaxDateFilter($('#MaxDateCreatedFilterId')),
					remarksFilter: $('#RemarksFilterId').val(),
					minAbpUserIdFilter: $('#MinAbpUserIdFilterId').val(),
					maxAbpUserIdFilter: $('#MaxAbpUserIdFilterId').val(),
					registrationStatusStatusCodeFilter: $('#RegistrationStatusStatusCodeFilterId').val(),
					registrationFirstNameFilter: $('#RegistrationFirstNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditRegistrationHistoryModalSaved', function () {
            getRegistrationHistories();
        });

		$('#GetRegistrationHistoriesButton').click(function (e) {
            e.preventDefault();
            getRegistrationHistories();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getRegistrationHistories();
		  }
		});
		
		
		
    });
})();
