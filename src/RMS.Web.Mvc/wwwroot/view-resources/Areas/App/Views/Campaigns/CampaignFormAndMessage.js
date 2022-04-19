(function () {
    $(function () {

        var campaignId = $('[id="CampaignId"]')[0].value;
        var editable = $('[id="Editable"]')[0].value;

        $("#CampaignFormTab").click(function () {
            $('#dvCampaignForms').load('CampaignForms/', { campaignId: campaignId, editable: editable } );
            $('#dvCampaignForms').show();
        });

        $("#CampaignMessageTab").click(function () {
            $('#dvCampaignMessages').load('CampaignMessages/', { campaignId: campaignId });
            $('#dvCampaignMessages').show();
        });
        
    });
}) ();