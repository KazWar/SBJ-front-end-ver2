(function () {
    $(function () {
        //Handle Email variant
        $('#ShowRelatedRegistrationsByEmailSpan').click(function () {
            $('#ShowRelatedRegistrationsByEmailSpan').hide();
            $('#HideRelatedRegistrationsByEmailSpan').show();
            $('#RelatedRegistrationsByEmailTable').slideDown();
        });

        $('#HideRelatedRegistrationsByEmailSpan').click(function () {
            $('#HideRelatedRegistrationsByEmailSpan').hide();
            $('#ShowRelatedRegistrationsByEmailSpan').show();
            $('#RelatedRegistrationsByEmailTable').slideUp();
        });

        //Handle SerialNumber variant
        $('#ShowRelatedRegistrationsBySerialNumberSpan').click(function () {
            $('#ShowRelatedRegistrationsBySerialNumberSpan').hide();
            $('#HideRelatedRegistrationsBySerialNumberSpan').show();
            $('#RelatedRegistrationsBySerialNumberTable').slideDown();
        });

        $('#HideRelatedRegistrationsBySerialNumberSpan').click(function () {
            $('#HideRelatedRegistrationsBySerialNumberSpan').hide();
            $('#ShowRelatedRegistrationsBySerialNumberSpan').show();
            $('#RelatedRegistrationsBySerialNumberTable').slideUp();
        });
    });
})();