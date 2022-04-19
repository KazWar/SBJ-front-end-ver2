

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.SBJ.CodeTypeTables.Exporting;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RMS.SBJ.CodeTypeTables
{
	[AbpAuthorize(AppPermissions.Pages_MessageVariables)]
    public class MessageVariablesAppService : RMSAppServiceBase, IMessageVariablesAppService
    {
		 private readonly IRepository<MessageVariable, long> _messageVariableRepository;
		 private readonly IMessageVariablesExcelExporter _messageVariablesExcelExporter;
		 

		  public MessageVariablesAppService(IRepository<MessageVariable, long> messageVariableRepository, IMessageVariablesExcelExporter messageVariablesExcelExporter ) 
		  {
			_messageVariableRepository = messageVariableRepository;
			_messageVariablesExcelExporter = messageVariablesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetMessageVariableForViewDto>> GetAll(GetAllMessageVariablesInput input)
         {
			
			var filteredMessageVariables = _messageVariableRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter) || e.RmsTable.Contains(input.Filter) || e.TableField.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RmsTableFilter),  e => e.RmsTable == input.RmsTableFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TableFieldFilter),  e => e.TableField == input.TableFieldFilter);

			var pagedAndFilteredMessageVariables = filteredMessageVariables
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var messageVariables = from o in pagedAndFilteredMessageVariables
                         select new GetMessageVariableForViewDto() {
							MessageVariable = new MessageVariableDto
							{
                                Description = o.Description,
                                RmsTable = o.RmsTable,
                                TableField = o.TableField,
                                Id = o.Id
							}
						};

            var totalCount = await filteredMessageVariables.CountAsync();

            return new PagedResultDto<GetMessageVariableForViewDto>(
                totalCount,
                await messageVariables.ToListAsync()
            );
         }
		 
		 public async Task<GetMessageVariableForViewDto> GetMessageVariableForView(long id)
         {
            var messageVariable = await _messageVariableRepository.GetAsync(id);

            var output = new GetMessageVariableForViewDto { MessageVariable = ObjectMapper.Map<MessageVariableDto>(messageVariable) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_MessageVariables_Edit)]
		 public async Task<GetMessageVariableForEditOutput> GetMessageVariableForEdit(EntityDto<long> input)
         {
            var messageVariable = await _messageVariableRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetMessageVariableForEditOutput {MessageVariable = ObjectMapper.Map<CreateOrEditMessageVariableDto>(messageVariable)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditMessageVariableDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_MessageVariables_Create)]
		 protected virtual async Task Create(CreateOrEditMessageVariableDto input)
         {
            var messageVariable = ObjectMapper.Map<MessageVariable>(input);

			
			if (AbpSession.TenantId != null)
			{
				messageVariable.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _messageVariableRepository.InsertAsync(messageVariable);
         }

		 [AbpAuthorize(AppPermissions.Pages_MessageVariables_Edit)]
		 protected virtual async Task Update(CreateOrEditMessageVariableDto input)
         {
            var messageVariable = await _messageVariableRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, messageVariable);
         }

		 [AbpAuthorize(AppPermissions.Pages_MessageVariables_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _messageVariableRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetMessageVariablesToExcel(GetAllMessageVariablesForExcelInput input)
         {
			
			var filteredMessageVariables = _messageVariableRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter) || e.RmsTable.Contains(input.Filter) || e.TableField.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RmsTableFilter),  e => e.RmsTable == input.RmsTableFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TableFieldFilter),  e => e.TableField == input.TableFieldFilter);

			var query = (from o in filteredMessageVariables
                         select new GetMessageVariableForViewDto() { 
							MessageVariable = new MessageVariableDto
							{
                                Description = o.Description,
                                RmsTable = o.RmsTable,
                                TableField = o.TableField,
                                Id = o.Id
							}
						 });


            var messageVariableListDtos = await query.ToListAsync();

            return _messageVariablesExcelExporter.ExportToFile(messageVariableListDtos);
         }

		public async Task<List<GetMessageVariableForViewDto>> GetAllMessageVariables()
		{
			var allMessageVariables = _messageVariableRepository.GetAll();

			var messageVariables = from o in allMessageVariables
								   select new GetMessageVariableForViewDto()
								   {
									   MessageVariable = new MessageVariableDto
									   {
										   Description = o.Description,
										   RmsTable = o.RmsTable,
										   TableField = o.TableField,
										   Id = o.Id
									   }
								   };

			var totalCount = await allMessageVariables.CountAsync();

			return messageVariables.ToList();
		}
	}
}