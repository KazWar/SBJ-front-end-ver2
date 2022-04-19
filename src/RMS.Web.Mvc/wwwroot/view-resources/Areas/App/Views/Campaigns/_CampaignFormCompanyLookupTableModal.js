(function ($) {
    app.modals.CampaignFormCompanyLookupTableModal = function () {

        var _modalManager;
        var _formsService = abp.services.app.forms;
        var _formLocalesService = abp.services.app.formLocales;
        var _$formLocaleFromCompanyTable = $('#FormLocaleTable');
        var _campaignsService = abp.services.app.campaigns;

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        var dataTable = _$formLocaleFromCompanyTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction:
            {
                ajaxFunction: _campaignsService.getAllCampaignFormFromCompanyForLookupTable,

            },
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: "<div class=\"text-center\"><input id='selectbtn' class='btn btn-success' type='button' width='25px' value='" + app.localize('Select') + "' /></div>"
                },
                {
                    autoWidth: false,
                    orderable: false,
                    targets: 1,
                    data: "displayName"
                }
            ]
        });


        $('#FormLocaleTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            var companyLocaleId = data.localeId;
            var formLocale = {
                formId: data.formId,
                localeId: data.localeId,
                description: data.displayName,
            };
            _formLocalesService.createOrEditAndGetId(
                formLocale
            ).done(function (data) {
                var formLocaleId = data;
                $.ajax({
                url: 'CampaignFormFromCompany/',
                contentType: "application/json;charset=utf-8",
                dataType: 'json',
                data: {
                    formLocaleId: data,
                    localeId: companyLocaleId
                }
                }).done(function () {
                    _modalManager.close();
                    //PopulateFormLocaleBlocks(formLocale, formLocaleId);
                    window.location.reload();
                    abp.notify.info(app.localize('AddedSuccessfully'));
                }).fail(function () {
                    abp.notify.info('Error while saving the changes');
                });
            });
            //_modalManager.setResult(data);
            _modalManager.close();
            
        });

        this.save = function () {
            if (!_$formInformationForm.valid()) {
                return;
            }
            if ($('#Form_SystemLevelId').prop('required') && $('#Form_SystemLevelId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('SystemLevel')));
                return;
            }

            var form = _$formInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _formsService.createOrEdit(
                form
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditFormModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };

        function PopulateFormLocaleBlocks(formLocale, formLocaleId) {
            $('.formLocale').append($('<option>').val(formLocaleId).text(formLocale.description).attr('data-localeId', formLocale.localeId))
            $('#dvFormLocaleBlock').load('/App/Forms/DisplayFormLocaleBlocks/', { formLocaleId: formLocaleId, formLocaleText: formLocale.description, localeId: formLocale.localeId });
            $('#dvFormLocaleBlock').show();
        };


        // #region Added for reloading the tab after adding a new form locale from company
        //TO DO: Unable to reload the tabs - try to change the nav-links href to each action method (separate partial view for each tab) and load them
        $('a[data-toggle="tab"]').on('show.bs.tab', function (e) {
            localStorage.setItem('activeTab', $(e.target).attr('href'));
            var activeTab = localStorage.getItem('activeTab');
            console.log(activeTab);
        });

        $('.close-button').click(function () {
            var activeTab = localStorage.getItem('activeTab');
            console.log(activeTab);
            if (activeTab) {
                $('#myTab a[href="' + "#pane-1" + '"]').tab('show');
            }
        });

         // #endregion
    };
})(jQuery);