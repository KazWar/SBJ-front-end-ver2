namespace RMS.Web.Api.V2.Models
{
    public class Bundle
    {
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to determine which section this bundle is a part of
        /// </summary>
        [ForeignKey("SectionId")]
        [Required]
        public int SectionId { get; set; }

        /// <summary>
        /// Unique name for the bundle within
        /// </summary>
        [Required]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Short description for the field.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Determines in a form, whether a bundle can repeat, 
        /// but only if the value is higher than 1
        /// </summary>
        public int Multiples { get; set; }

        /// <summary>
        /// A collection containing at least one field
        /// </summary>
        public Field[] Fields { get; set; } = default!;
    }
}
