using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace RMS.SBJ.MakitaSerialNumber
{
    [Table("MakitaSerialNumber")]
    public class MakitaSerialNumber : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string ProductCode { get; set; }

        public virtual string SerialNumber { get; set; }

        public virtual string RetailerExternalCode { get; set; }
    }
}