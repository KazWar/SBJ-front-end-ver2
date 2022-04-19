using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace RMS.SBJ.Report.GeneralReports.Dtos
{
    public class GeneralReportDto : EntityDto<long>
    {
        public string CampaignName { get; set; }
        public string Locale { get; set; }
        public DateTime RegistrationTime { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BicIban { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime CurrentStatusTime { get; set; }
        public string Remarks { get; set; }
        public List<CustomField> RegistrationFields { get; set; }
        public string RejectionReason { get; set; }
        public string RejectedFields { get; set; }
        public string IncompleteReason { get; set; }
        public string IncompleteFields { get; set; }
        public string ProductPurchased { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchaseTime { get; set; }
        public string StorePurchased { get; set; }
        public string AddressPurchased { get; set; }
        public string PostalPurchased { get; set; }
        public string CityPurchased { get; set; }
        public string ProductSelected { get; set; }
        public bool ActivationcodeSelected { get; set; }
        public decimal? CashRefund { get; set; }
        public List<CustomField> PurchaseRegistrationFields { get; set; }
        public long? HandlingBatchId { get; set; }
        public DateTime HandlingBatchFinishedTime { get; set; }
    }
}
