(function () {
    $(function () {

        $('.campaignMessageLocale').on('change', function () {
            var selectedMessageLocale = $(".campaignMessageLocale option:selected");
            $('#dvMessageComponentContentTable').load('PopulateMessageComponentContent/', { messageLocaleText: selectedMessageLocale[0].outerText, localeId: selectedMessageLocale[0].dataset.localeid, campaignId: selectedMessageLocale[0].dataset.campaignid });
            $('#dvMessageComponentContentTable').show();
        });

    });
})();