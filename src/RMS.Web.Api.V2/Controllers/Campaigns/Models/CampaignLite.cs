namespace RMS.Web.Api.V2.Controllers.Campaigns.Models
{
    /// <summary>
    /// Partial campaign data model, used to supply minimal data for the
    /// front-end for expired and up-and-coming campaigns.
    /// </summary>
    public class CampaignLite
    {
        [Required]
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        [Required]
        public string StartDate { get; set; } = default!;

        [Required]
        public string EndDate { get; set; } = default!;

        public string? ThumbnailUrl { get; set; }

        public string? ConditionsUrl { get; set; }
    }
}
