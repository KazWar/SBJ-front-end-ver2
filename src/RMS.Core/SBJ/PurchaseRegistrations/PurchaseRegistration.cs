using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.Registrations;
using RMS.SBJ.Products;
using RMS.SBJ.HandlingLines;
using RMS.SBJ.RetailerLocations;
using System.Collections.Generic;
using RMS.SBJ.PurchaseRegistrationFieldDatas;

namespace RMS.SBJ.PurchaseRegistrations
{
    [Table("PurchaseRegistration")]
    public class PurchaseRegistration : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int Quantity { get; set; }

        public virtual decimal TotalAmount { get; set; }

        public virtual DateTime PurchaseDate { get; set; }

        public virtual string InvoiceImage { get; set; }

        public virtual string InvoiceImagePath { get; set; }

        public virtual long HandlingLineId { get; set; }

        public virtual long ProductId { get; set; }

        public virtual long RegistrationId { get; set; }

        public virtual long RetailerLocationId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        [ForeignKey("HandlingLineId")]
        public HandlingLine HandlingLineFk { get; set; }

        [ForeignKey("RegistrationId")]
        public Registration RegistrationFk { get; set; }

        [ForeignKey("RetailerLocationId")]
        public RetailerLocation RetailerLocationFk { get; set; }

        public virtual ICollection<PurchaseRegistrationFieldData> PurchaseRegistrationFieldData { get; set; }
    }
}