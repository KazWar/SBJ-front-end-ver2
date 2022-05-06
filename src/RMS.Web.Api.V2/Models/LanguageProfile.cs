namespace RMS.Web.Api.V2.Models
{
    public class LanguageProfile
    {
        public int Id { get; set; }
        public int CampaignLocaleId { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

    }

    public class Translation
    {
        public int Id { get; set; }
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;

    }

    /// <summary>
    /// Represents the type of collection for bundled translations.
    /// </summary>
    public enum CollectionTypes : int
    {
        // UI text
        System = 1,

        // E-mail text
        Message = 2,

        // Page text
        Content = 3
    }
}
