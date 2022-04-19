

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.HandlingLineLogics.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.HandlingLineLogics
{
	[AbpAuthorize(AppPermissions.Pages_HandlingLineLogics)]
    public class HandlingLineLogicsAppService : RMSAppServiceBase, IHandlingLineLogicsAppService
    {
		 private readonly IRepository<HandlingLineLogic, long> _handlingLineLogicRepository;
		 

		  public HandlingLineLogicsAppService(IRepository<HandlingLineLogic, long> handlingLineLogicRepository ) 
		  {
			_handlingLineLogicRepository = handlingLineLogicRepository;
			
		  }

		 public async Task<PagedResultDto<GetHandlingLineLogicForViewDto>> GetAll(GetAllHandlingLineLogicsInput input)
         {
			
			var filteredHandlingLineLogics = _handlingLineLogicRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Operator.Contains(input.Filter))
						.WhereIf(input.MinFirstHandlingLineIdFilter != null, e => e.FirstHandlingLineId >= input.MinFirstHandlingLineIdFilter)
						.WhereIf(input.MaxFirstHandlingLineIdFilter != null, e => e.FirstHandlingLineId <= input.MaxFirstHandlingLineIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OperatorFilter),  e => e.Operator == input.OperatorFilter)
						.WhereIf(input.MinSecondHandlingLineIdFilter != null, e => e.SecondHandlingLineId >= input.MinSecondHandlingLineIdFilter)
						.WhereIf(input.MaxSecondHandlingLineIdFilter != null, e => e.SecondHandlingLineId <= input.MaxSecondHandlingLineIdFilter);

			var pagedAndFilteredHandlingLineLogics = filteredHandlingLineLogics
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var handlingLineLogics = from o in pagedAndFilteredHandlingLineLogics
                         select new GetHandlingLineLogicForViewDto() {
							HandlingLineLogic = new HandlingLineLogicDto
							{
                                FirstHandlingLineId = o.FirstHandlingLineId,
                                Operator = o.Operator,
                                SecondHandlingLineId = o.SecondHandlingLineId,
                                Id = o.Id
							}
						};

            var totalCount = await filteredHandlingLineLogics.CountAsync();

            return new PagedResultDto<GetHandlingLineLogicForViewDto>(
                totalCount,
                await handlingLineLogics.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_HandlingLineLogics_Edit)]
		 public async Task<GetHandlingLineLogicForEditOutput> GetHandlingLineLogicForEdit(EntityDto<long> input)
         {
            var handlingLineLogic = await _handlingLineLogicRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetHandlingLineLogicForEditOutput {HandlingLineLogic = ObjectMapper.Map<CreateOrEditHandlingLineLogicDto>(handlingLineLogic)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditHandlingLineLogicDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_HandlingLineLogics_Create)]
		 protected virtual async Task Create(CreateOrEditHandlingLineLogicDto input)
         {
            var handlingLineLogic = ObjectMapper.Map<HandlingLineLogic>(input);

			
			if (AbpSession.TenantId != null)
			{
				handlingLineLogic.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _handlingLineLogicRepository.InsertAsync(handlingLineLogic);
         }

		 [AbpAuthorize(AppPermissions.Pages_HandlingLineLogics_Edit)]
		 protected virtual async Task Update(CreateOrEditHandlingLineLogicDto input)
         {
            var handlingLineLogic = await _handlingLineLogicRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, handlingLineLogic);
         }

		 [AbpAuthorize(AppPermissions.Pages_HandlingLineLogics_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _handlingLineLogicRepository.DeleteAsync(input.Id);
         } 
    }
}