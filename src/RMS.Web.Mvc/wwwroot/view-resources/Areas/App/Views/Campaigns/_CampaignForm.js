(function () {
    $(function () {
        var editable = $('[id="Editable"]')[0].value;

        //var _createCampaignFormFromCompanyModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'App/Campaigns/CampaignFormCompanyLookupTableModal',
        //    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Campaigns/_CampaignFormCompanyLookupTableModal.js',
        //    modalClass: 'CampaignFormCompanyLookupTableModal'
        //});

        //$('#campaignFormFromCompany').click(function () {
        //    _createCampaignFormFromCompanyModal.open();
        //});

        $('.formLocale').on('change', function () {
            PopulateFormLocaleBlocks();
        });

        function PopulateFormLocaleBlocks() {
            var selectedFormLocale = $(".formLocale option:selected");
            $('#dvFormLocaleBlock').load('/App/Forms/DisplayFormLocaleBlocks/', { formLocaleId: selectedFormLocale[0].dataset.formlocaleid, formLocaleText: selectedFormLocale[0].outerText, localeId: selectedFormLocale[0].dataset.localeid, editable: editable });
            $('#dvFormLocaleBlock').show();
        }
    });
})();