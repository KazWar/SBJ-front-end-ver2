namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetMessageComponentContentForViewDto
    {
		public MessageComponentContentDto MessageComponentContent { get; set; }

		public string MessageComponentIsActive { get; set;}

		public string CampaignTypeEventRegistrationStatusSortOrder { get; set;}

        public string MessageType { get; set; }

        public string MessageComponentType { get; set; }
    }
}