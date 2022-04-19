using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace RMS.PromoPlanner
{
    [Table("PromoScope")]
    public class PromoScope : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Description { get; set; }
    }
}