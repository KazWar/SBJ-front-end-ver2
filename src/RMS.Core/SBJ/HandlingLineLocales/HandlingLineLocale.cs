using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.CodeTypeTables;

namespace RMS.SBJ.HandlingLineLocales
{
    [Table("HandlingLineLocale")]
    public class HandlingLineLocale : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long HandlingLineId { get; set; }

        public virtual long LocaleId { get; set; }

        [ForeignKey("HandlingLineId")]
        public HandlingLine HandlingLineFk { get; set; }

        [ForeignKey("LocaleId")]
        public Locale LocaleFk { get; set; }
    }
}