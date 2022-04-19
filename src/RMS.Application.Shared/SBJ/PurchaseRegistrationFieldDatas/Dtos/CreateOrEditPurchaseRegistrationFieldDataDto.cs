
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.PurchaseRegistrationFieldDatas.Dtos
{
    public class CreateOrEditPurchaseRegistrationFieldDataDto : EntityDto<long?>
    {

		 public long PurchaseRegistrationFieldId { get; set; }
		 
		 		 public long PurchaseRegistrationId { get; set; }
		 
		 
    }
}