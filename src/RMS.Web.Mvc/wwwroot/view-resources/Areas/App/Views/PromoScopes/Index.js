(function () {
    $(function () {

        var _$promoScopesTable = $('#PromoScopesTable');
        var _promoScopesService = abp.services.app.promoScopes;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PromoScopes.Create'),
            edit: abp.auth.hasPermission('Pages.PromoScopes.Edit'),
            'delete': abp.auth.hasPermission('Pages.PromoScopes.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoScopes/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoScopes/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPromoScopeModal'
        });       

		 var _viewPromoScopeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoScopes/ViewpromoScopeModal',
            modalClass: 'ViewPromoScopeModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$promoScopesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _promoScopesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PromoScopesTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val()
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
                                    _viewPromoScopeModal.open({ id: data.record.promoScope.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.promoScope.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePromoScope(data.record.promoScope);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "promoScope.description",
						 name: "description"   
					}
            ]
        });

        function getPromoScopes() {
            dataTable.ajax.reload();
        }

        function deletePromoScope(promoScope) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _promoScopesService.delete({
                            id: promoScope.id
                        }).done(function () {
                            getPromoScopes(true);
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

        $('#CreateNewPromoScopeButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _promoScopesService
                .getPromoScopesToExcel({
				filter : $('#PromoScopesTableFilter').val(),
					descriptionFilter: $('#DescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPromoScopeModalSaved', function () {
            getPromoScopes();
        });

		$('#GetPromoScopesButton').click(function (e) {
            e.preventDefault();
            getPromoScopes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPromoScopes();
		  }
		});
    });
})();