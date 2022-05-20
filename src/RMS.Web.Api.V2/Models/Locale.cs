namespace RMS.Web.Api.V2.Models
{
    public class Locale
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Key]
        [Required]
        public string Code { get; set; } = default!;

        [Key]
        [Required]
        public string LanguageCode { get; set; } = default!;

        [Key]
        [Required]
        public string CountryCode { get; set; } = default!;

        [Required]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
