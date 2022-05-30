namespace RMS.Web.Api.V2.Controllers.Campaigns.Models
{
    /// <summary>
    /// Full campaign data model, used for active campaigns.
    /// Inherits from CampaignLite.
    /// </summary>
    public class Campaign : CampaignLite
    {
        [Key]
        [Required]
        public int FormId { get; set; }

        [Key]
        [Required]
        public int Code { get; set; }

        [Key]
        [Required]
        public CampaignCategory Category { get; set; } = default!;

        public string? BannerUrl { get; set; }
    }

    /// <summary>
    /// Which campaign type it is.
    /// </summary>
    public class CampaignCategory
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        public string? Description { get; set; }
    }
}

