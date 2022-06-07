namespace RMS.Web.Api.V2.Models
{
    public class Section
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [ForeignKey("FormId")]
        [Required]
        public int FormId { get; set; }

        /// <summary>
        /// Unique name for the section within a form
        /// </summary>
        [Required]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Short description for the section
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The bundles which make up a section, they are
        /// collections of fields containing at least 1 field
        /// </summary>
        public Bundle[] Bundles { get; set; } = default!;
    }
}
