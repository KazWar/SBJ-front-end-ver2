namespace RMS.Web.Api.V2.Models
{
    public class LanguageProfile
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
