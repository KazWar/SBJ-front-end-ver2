(function () {
    $(function () {

        var _$promoCountriesTable = $('#PromoCountriesTable');
        var _promoCountriesService = abp.services.app.promoCountries;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PromoCountries.Create'),
            edit: abp.auth.hasPermission('Pages.PromoCountries.Edit'),
            'delete': abp.auth.hasPermission('Pages.PromoCountries.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoCountries/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PromoCountries/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPromoCountryModal'
        });       

		 var _viewPromoCountryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PromoCountries/ViewpromoCountryModal',
            modalClass: 'ViewPromoCountryModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$promoCountriesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _promoCountriesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PromoCountriesTableFilter').val(),
					promoPromocodeFilter: $('#PromoPromocodeFilterId').val(),
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
                                    _viewPromoCountryModal.open({ id: data.record.promoCountry.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.promoCountry.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePromoCountry(data.record.promoCountry);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "promoPromocode" ,
						 name: "promoFk.promocode" 
					},
					{
						targets: 2,
						 data: "countryCountryCode" ,
						 name: "countryFk.countryCode" 
					}
            ]
        });

        function getPromoCountries() {
            dataTable.ajax.reload();
        }

        function deletePromoCountry(promoCountry) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _promoCountriesService.delete({
                            id: promoCountry.id
                        }).done(function () {
                            getPromoCountries(true);
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

        $('#CreateNewPromoCountryButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _promoCountriesService
                .getPromoCountriesToExcel({
				filter : $('#PromoCountriesTableFilter').val(),
					promoPromocodeFilter: $('#PromoPromocodeFilterId').val(),
					countryCountryCodeFilter: $('#CountryCountryCodeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPromoCountryModalSaved', function () {
            getPromoCountries();
        });

		$('#GetPromoCountriesButton').click(function (e) {
            e.preventDefault();
            getPromoCountries();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPromoCountries();
		  }
		});
    });
})();