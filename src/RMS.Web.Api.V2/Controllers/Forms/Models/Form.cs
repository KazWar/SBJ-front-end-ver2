namespace RMS.Web.Api.V2.Models
{
    public class Form
    {
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Unique name for the form
        /// </summary>
        [Required]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Short description for the form
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Foreign key to determine which locale this form is a part of.
        /// </summary>
        [ForeignKey("Locale")]
        [Required]
        public string Locale { get; set; } = default!;

        /// <summary>
        /// Sections of a form E.G first step personal information, 2nd step product registration, etc.
        /// </summary>
        [Required]
        public Section[] Sections { get; set; } = default!;
    }
}
