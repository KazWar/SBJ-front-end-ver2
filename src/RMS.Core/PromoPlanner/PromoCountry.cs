using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.CodeTypeTables;

namespace RMS.PromoPlanner
{
    [Table("PromoCountry")]
    public class PromoCountry : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long PromoId { get; set; }

        public virtual long CountryId { get; set; }

        [ForeignKey("PromoId")]
        public Promo PromoFk { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }
    }
}