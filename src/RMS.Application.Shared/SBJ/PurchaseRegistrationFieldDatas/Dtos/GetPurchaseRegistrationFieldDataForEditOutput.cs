using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.PurchaseRegistrationFieldDatas.Dtos
{
    public class GetPurchaseRegistrationFieldDataForEditOutput
    {
		public CreateOrEditPurchaseRegistrationFieldDataDto PurchaseRegistrationFieldData { get; set; }

		public string PurchaseRegistrationFormFieldDescription { get; set;}

		public string PurchaseRegistrationInvoiceImage { get; set;}


    }
}