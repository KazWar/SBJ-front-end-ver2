namespace RMS.Web.Api.V2.Models
{
    public class Field
    {
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Unique name for the field within a bundle
        /// </summary>
        [Required]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Short description for the field
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The default shown name. Has a prefix for translations of 'lbl_'
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// The default tooltip for the input field. Has a prefix for translations of 'tip_'
        /// </summary>
        public string? Tooltip { get; set; }


        /// <summary>
        /// Determines which table this field's data belongs to.
        /// </summary>
        [ForeignKey("DestinationTypeId")]
        [Required]
        public int DestinationTypeId { get; set; }

        /// <summary>
        /// Self-referencing foreign key. Links to another field for double validation,
        /// E.G confirm your mail, IBAN, etc.
        /// </summary>
        [ForeignKey("LinkId")]
        public int? LinkId { get; set; }

        /// <summary>
        /// Rules which apply to the field
        /// </summary>
        public Rule[]? Rules { get; set; }

        /// <summary>
        /// Input styling options for Quasar inputs in JSON format
        /// </summary>
        public string? Design { get; set; }

        /// <summary>
        /// Input feature options for Quasar inputs in JSON format
        /// </summary>
        public string? Features { get; set; }

        /// <summary>
        /// Input mask options for Quasar inputs in JSON format
        /// </summary>
        public string? Mask { get; set; }
    }
}
