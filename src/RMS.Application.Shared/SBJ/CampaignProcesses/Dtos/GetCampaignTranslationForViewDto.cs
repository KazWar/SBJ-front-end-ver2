namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetCampaignTranslationForViewDto
    {
        public CampaignTranslationDto CampaignTranslation { get; set; }

        public string CampaignName { get; set; }

        public string LocaleDescription { get; set; }

    }
}