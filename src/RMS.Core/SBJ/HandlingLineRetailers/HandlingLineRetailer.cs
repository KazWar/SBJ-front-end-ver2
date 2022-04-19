using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.Retailers;

namespace RMS.SBJ.HandlingLineRetailers
{
    [Table("HandlingLineRetailer")]
    public class HandlingLineRetailer : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long HandlingLineId { get; set; }

        public virtual long RetailerId { get; set; }

        [ForeignKey("HandlingLineId")]
        public HandlingLine HandlingLineFk { get; set; }

        [ForeignKey("RetailerId")]
        public Retailer RetailerFk { get; set; }
    }
}