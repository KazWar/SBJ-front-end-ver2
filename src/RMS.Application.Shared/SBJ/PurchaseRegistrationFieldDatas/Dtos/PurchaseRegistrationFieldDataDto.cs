
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.PurchaseRegistrationFieldDatas.Dtos
{
    public class PurchaseRegistrationFieldDataDto : EntityDto<long>
    {

		 public long PurchaseRegistrationFieldId { get; set; }

		 		 public long PurchaseRegistrationId { get; set; }

		 
    }
}