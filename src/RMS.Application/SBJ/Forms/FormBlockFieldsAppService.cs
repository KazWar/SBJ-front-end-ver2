using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using RMS.Authorization;
using RMS.Dto;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.Forms.Dtos;
using RMS.SBJ.Forms.Exporting;
using RMS.SBJ.PurchaseRegistrationFields;
using RMS.SBJ.RegistrationFields;
using RMS.SBJ.Registrations.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace RMS.SBJ.Forms
{
    [AbpAuthorize(AppPermissions.Pages_FormBlockFields)]
    public class FormBlockFieldsAppService : RMSAppServiceBase, IFormBlockFieldsAppService
    {
        private readonly IRepository<FormBlockField, long> _formBlockFieldRepository;
        private readonly IFormBlockFieldsExcelExporter _formBlockFieldsExcelExporter;
        private readonly IRepository<FormField, long> _lookup_formFieldRepository;
        private readonly IRepository<FormBlock, long> _lookup_formBlockRepository;
        private readonly IRepository<FieldType, long> _lookup_fieldTypeRepository;
        private readonly IRepository<RegistrationField, long> _lookup_registrationFieldRepository;
        private readonly IRepository<PurchaseRegistrationField, long> _lookup_purchaseRegistrationFieldRepository;
        private readonly IRepository<FormFieldTranslation, long> _lookup_formFieldTranslationRepository;
        private readonly IRepository<Locale, long> _lookup_localeRepository;

        public FormBlockFieldsAppService(IRepository<FormBlockField, long> formBlockFieldRepository, 
                                         IFormBlockFieldsExcelExporter formBlockFieldsExcelExporter, 
                                         IRepository<FormField, long> lookup_formFieldRepository, 
                                         IRepository<FormBlock, long> lookup_formBlockRepository, 
                                         IRepository<FieldType, long> lookup_fieldTypeRepository,
                                         IRepository<RegistrationField, long> lookup_registrationFieldRepository,
                                         IRepository<PurchaseRegistrationField, long> lookup_purchaseRegistrationFieldRepository,
                                         IRepository<FormFieldTranslation, long> lookup_formFieldTranslationRepository,
                                         IRepository<Locale, long> lookup_localeRepository)
        {
            _formBlockFieldRepository = formBlockFieldRepository;
            _formBlockFieldsExcelExporter = formBlockFieldsExcelExporter;
            _lookup_formFieldRepository = lookup_formFieldRepository;
            _lookup_formBlockRepository = lookup_formBlockRepository;
            _lookup_fieldTypeRepository = lookup_fieldTypeRepository;
            _lookup_registrationFieldRepository = lookup_registrationFieldRepository;
            _lookup_purchaseRegistrationFieldRepository = lookup_purchaseRegistrationFieldRepository;
            _lookup_formFieldTranslationRepository = lookup_formFieldTranslationRepository;
            _lookup_localeRepository = lookup_localeRepository;
        }

        public async Task<PagedResultDto<GetFormBlockFieldForViewDto>> GetAll(GetAllFormBlockFieldsInput input)
        {
            var filteredFormBlockFields = _formBlockFieldRepository.GetAll()
                        .Include(e => e.FormFieldFk)
                        .Include(e => e.FormBlockFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinSortOrderFilter != null, e => e.SortOrder >= input.MinSortOrderFilter)
                        .WhereIf(input.MaxSortOrderFilter != null, e => e.SortOrder <= input.MaxSortOrderFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FormFieldDescriptionFilter), e => e.FormFieldFk != null && e.FormFieldFk.Description == input.FormFieldDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FormBlockDescriptionFilter), e => e.FormBlockFk != null && e.FormBlockFk.Description == input.FormBlockDescriptionFilter);

            var pagedAndFilteredFormBlockFields = filteredFormBlockFields
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var formBlockFields = from o in filteredFormBlockFields
                                  join o1 in _lookup_formFieldRepository.GetAll() on o.FormFieldId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_formBlockRepository.GetAll() on o.FormBlockId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  select new GetFormBlockFieldForViewDto()
                                  {
                                      FormBlockField = new FormBlockFieldDto
                                      {
                                          SortOrder = o.SortOrder,
                                          Id = o.Id,
                                          FormBlockId = o.FormBlockId,
                                          FormFieldId = o.FormFieldId
                                      },
                                      FormFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                                      FormBlockDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
                                  };

            var totalCount = await filteredFormBlockFields.CountAsync();

            return new PagedResultDto<GetFormBlockFieldForViewDto>(
                totalCount,
                await formBlockFields.ToListAsync()
            );
        }

        public async Task<GetFormBlockFieldForViewDto> GetFormBlockFieldForView(long id)
        {
            var formBlockField = await _formBlockFieldRepository.GetAsync(id);
            var output = new GetFormBlockFieldForViewDto { FormBlockField = ObjectMapper.Map<FormBlockFieldDto>(formBlockField) };

            if (output.FormBlockField.FormFieldId != null)
            {
                var _lookupFormField = await _lookup_formFieldRepository.FirstOrDefaultAsync((long)output.FormBlockField.FormFieldId);
                output.FormFieldDescription = _lookupFormField?.Description?.ToString();
            }

            if (output.FormBlockField.FormBlockId != null)
            {
                var _lookupFormBlock = await _lookup_formBlockRepository.FirstOrDefaultAsync((long)output.FormBlockField.FormBlockId);
                output.FormBlockDescription = _lookupFormBlock?.Description?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FormBlockFields_Edit)]
        public async Task<GetFormBlockFieldForEditOutput> GetFormBlockFieldForEdit(EntityDto<long> input)
        {
            var formBlockField = await _formBlockFieldRepository.FirstOrDefaultAsync(input.Id);
            var output = new GetFormBlockFieldForEditOutput { FormBlockField = ObjectMapper.Map<CreateOrEditFormBlockFieldDto>(formBlockField) };

            if (output.FormBlockField.FormFieldId != null)
            {
                var _lookupFormField = await _lookup_formFieldRepository.FirstOrDefaultAsync((long)output.FormBlockField.FormFieldId);
                output.FormFieldDescription = _lookupFormField?.Description?.ToString();
            }

            if (output.FormBlockField.FormBlockId != null)
            {
                var _lookupFormBlock = await _lookup_formBlockRepository.FirstOrDefaultAsync((long)output.FormBlockField.FormBlockId);
                output.FormBlockDescription = _lookupFormBlock?.Description?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFormBlockFieldDto input)
        {
            if (input.Id == null || input.Id == 0)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_FormBlockFields_Create)]
        protected virtual async Task Create(CreateOrEditFormBlockFieldDto input)
        {
            var formBlockField = ObjectMapper.Map<FormBlockField>(input);

            if (AbpSession.TenantId != null)
            {
                formBlockField.TenantId = AbpSession.TenantId;
            }

            await _formBlockFieldRepository.InsertAsync(formBlockField);
        }

        [AbpAuthorize(AppPermissions.Pages_FormBlockFields_Edit)]
        protected virtual async Task Update(CreateOrEditFormBlockFieldDto input)
        {
            var formBlockField = await _formBlockFieldRepository.FirstOrDefaultAsync((long)input.Id);

            ObjectMapper.Map(input, formBlockField);
        }

        [AbpAuthorize(AppPermissions.Pages_FormBlockFields_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            if (input != null && input.Id != 0)
                await _formBlockFieldRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFormBlockFieldsToExcel(GetAllFormBlockFieldsForExcelInput input)
        {
            var filteredFormBlockFields = _formBlockFieldRepository.GetAll()
                        .Include(e => e.FormFieldFk)
                        .Include(e => e.FormBlockFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinSortOrderFilter != null, e => e.SortOrder >= input.MinSortOrderFilter)
                        .WhereIf(input.MaxSortOrderFilter != null, e => e.SortOrder <= input.MaxSortOrderFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FormFieldDescriptionFilter), e => e.FormFieldFk != null && e.FormFieldFk.Description == input.FormFieldDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FormBlockDescriptionFilter), e => e.FormBlockFk != null && e.FormBlockFk.Description == input.FormBlockDescriptionFilter);

            var query = (from o in filteredFormBlockFields
                         join o1 in _lookup_formFieldRepository.GetAll() on o.FormFieldId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_formBlockRepository.GetAll() on o.FormBlockId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetFormBlockFieldForViewDto()
                         {
                             FormBlockField = new FormBlockFieldDto
                             {
                                 SortOrder = o.SortOrder,
                                 Id = o.Id
                             },
                             FormFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                             FormBlockDescription = s2 == null || s2.Description == null ? "" : s2.Description.ToString()
                         });


            var formBlockFieldListDtos = await query.ToListAsync();

            return _formBlockFieldsExcelExporter.ExportToFile(formBlockFieldListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_FormBlockFields)]
        public async Task<PagedResultDto<FormBlockFieldFormFieldLookupTableDto>> GetAllFormFieldForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_formFieldRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var formFieldList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<FormBlockFieldFormFieldLookupTableDto>();

            foreach (var formField in formFieldList)
            {
                lookupTableDtoList.Add(new FormBlockFieldFormFieldLookupTableDto
                {
                    Id = formField.Id,
                    DisplayName = formField.Description?.ToString()
                });
            }

            return new PagedResultDto<FormBlockFieldFormFieldLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_FormBlockFields)]
        public async Task<PagedResultDto<FormBlockFieldFormBlockLookupTableDto>> GetAllFormBlockForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_formBlockRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var formBlockList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<FormBlockFieldFormBlockLookupTableDto>();

            foreach (var formBlock in formBlockList)
            {
                lookupTableDtoList.Add(new FormBlockFieldFormBlockLookupTableDto
                {
                    Id = formBlock.Id,
                    DisplayName = formBlock.Description?.ToString()
                });
            }

            return new PagedResultDto<FormBlockFieldFormBlockLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_FormBlockFields)]
        public async Task<List<GetFormBlockFieldForViewDto>> GetAllFormBlockFields(long? localeId)
        {
            var allFormBlockFields = _formBlockFieldRepository.GetAll();
            var formBlockFields = await (from o in allFormBlockFields
                                         join o1 in _lookup_formFieldRepository.GetAll() on o.FormFieldId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()
                                         join o2 in _lookup_formBlockRepository.GetAll() on o.FormBlockId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()
                                         orderby s2.SortOrder, o.SortOrder 
                                         select new GetFormBlockFieldForViewDto()
                                         {
                                             FormBlockField = new FormBlockFieldDto
                                             {
                                                 Id = o.Id,
                                                 FormBlockId = o.FormBlockId,
                                                 FormFieldId = o.FormFieldId,
                                                 SortOrder = o.SortOrder
                                             },
                                             FormFieldDescription = s1 == null || s1.Description == null ? "" : s1.Description,
                                             FormBlockDescription = s2 == null || s2.Description == null ? "" : s2.Description,
                                             FormBlockSortOrder = s2.SortOrder 
                                         }).ToListAsync();

            if (localeId != null)
            {
                foreach (var formBlockField in formBlockFields)
                {
                    var formFieldForEdit = await GetFormBlockFieldForEdit(formBlockField.FormBlockField.FormFieldId.Value, formBlockField.FormBlockField.FormBlockId.Value, localeId.Value);

                    formBlockField.FormBlockFieldForEdit = formFieldForEdit;
                    formBlockField.FormBlockFieldForEdit.SortOrderBlock = formBlockField.FormBlockSortOrder;
                    formBlockField.FormBlockFieldForEdit.SortOrderField = formBlockField.FormBlockField.SortOrder;
                }
            }

            return formBlockFields;
        }

        [AbpAuthorize(AppPermissions.Pages_FormBlockFields)]
        public async Task<GetFormBlockFieldForEditDto> GetFormBlockFieldForEdit(long fieldId, long blockId, long localeId)
        {
            var formField = await _lookup_formFieldRepository.GetAsync(fieldId);
            var formFieldType = await _lookup_fieldTypeRepository.GetAsync(formField.FieldTypeId);
            var formFieldTranslation = await _lookup_formFieldTranslationRepository.GetAll().Where(t => t.FormFieldId == formField.Id && t.LocaleId == localeId).FirstOrDefaultAsync();
            var locale = await _lookup_localeRepository.GetAsync(localeId);

            var formFieldForEdit = new GetFormBlockFieldForEditDto()
            {
                FieldId = fieldId,
                BlockId = blockId,
                FieldTypeId = formFieldType.Id,
                FieldName = formField.FieldName,
                FieldDescription = formField.Description,
                MaxLength = formField.MaxLength,
                RequiredField = formField.Required,
                ReadOnly = formField.ReadOnly,
                RegistrationField = formField.RegistrationField ?? String.Empty,
                PurchaseRegistrationField = formField.PurchaseRegistrationField ?? String.Empty,
                CustomRegistrationFieldId = 0,
                CustomPurchaseRegistrationFieldId = 0,
                LocaleId = locale.Id,
                Locale = locale.Description,
                FieldLabelGlobal = formField.Label,
                DefaultValueGlobal = formField.DefaultValue,
                RegularExpressionGlobal = formField.RegularExpression,
                FieldLabelLocale = formFieldTranslation != null ? formFieldTranslation.Label : String.Empty,
                DefaultValueLocale = formFieldTranslation != null ? formFieldTranslation.DefaultValue : String.Empty,
                RegularExpressionLocale = formFieldTranslation != null ? formFieldTranslation.RegularExpression : String.Empty,
                AvailableFieldTypes = new Dictionary<long, string>(),
                AvailableCustomRegistrationFields = new Dictionary<long, string>(),
                AvailableCustomPurchaseRegistrationFields = new Dictionary<long, string>()
            };

            int fieldTarget = -1;

            if (!String.IsNullOrWhiteSpace(formFieldForEdit.RegistrationField))
            {
                fieldTarget = 1;
            }
            else if (!String.IsNullOrWhiteSpace(formFieldForEdit.PurchaseRegistrationField))
            {
                fieldTarget = 2;
            }

            if (fieldTarget == -1)
            {
                string[] builtInSelectors = { FieldSourceHelper.Country, 
                                              FieldSourceHelper.RetailerLocation, 
                                              FieldSourceHelper.RetailerRadioButton,
                                              FieldSourceHelper.Product,
                                              FieldSourceHelper.ProductPremiumLite,
                                              FieldSourceHelper.ProductPremiumQuantity, 
                                              FieldSourceHelper.PurchaseRegistration, 
                                              FieldSourceHelper.PurchaseRegistrationLite, 
                                              FieldSourceHelper.PurchaseRegistrationSerial, 
                                              FieldSourceHelper.IbanChecker,
                                              FieldSourceHelper.UniqueCode,
                                              FieldSourceHelper.UniqueCodeByCampaign };

                if (builtInSelectors.Contains(formFieldType.Description) || (formFieldType.Description == FieldTypeHelper.DropdownMenu && (formField.FieldName == FieldNameHelper.Country || formField.FieldName == FieldNameHelper.ProductPremium)))
                {
                    fieldTarget = 5;
                }
            }

            if (fieldTarget == -1)
            {
                if (formFieldType.Description == FieldTypeHelper.HtmlText)
                {
                    fieldTarget = 6;
                }
                else if (formFieldType.Description == FieldTypeHelper.PageSeparator)
                {
                    fieldTarget = 7;
                }
            }

            if (fieldTarget == -1)
            {
                var customRegistrationField = await _lookup_registrationFieldRepository.GetAll().Where(f => f.FormFieldId == formField.Id).FirstOrDefaultAsync();

                if (customRegistrationField != null)
                {
                    fieldTarget = 3;
                    formFieldForEdit.CustomRegistrationFieldId = customRegistrationField.Id;
                    formFieldForEdit.AvailableCustomRegistrationFields.Add(customRegistrationField.Id, customRegistrationField.Description); //only 1 choice; may not be changed anyway
                }
                else
                {
                    var customPurchaseRegistrationField = await _lookup_purchaseRegistrationFieldRepository.GetAll().Where(f => f.FormFieldId == formField.Id).FirstOrDefaultAsync();

                    if (customPurchaseRegistrationField != null)
                    {
                        fieldTarget = 4;
                        formFieldForEdit.CustomPurchaseRegistrationFieldId = customPurchaseRegistrationField.Id;
                        formFieldForEdit.AvailableCustomPurchaseRegistrationFields.Add(customPurchaseRegistrationField.Id, customPurchaseRegistrationField.Description); //only 1 choice; may not be changed anyway
                    }
                }
            }

            formFieldForEdit.FieldTarget = fieldTarget;
            formFieldForEdit.AvailableFieldTypes.Add(formFieldType.Id, formFieldType.Description); //only 1 choice; may not be changed anyway

            return formFieldForEdit;
        }

        [AbpAuthorize(AppPermissions.Pages_FormBlockFields_Edit)]
        public async Task<bool> UpdateSortOrder(UpdateSortOrderDto input)
        {
            if (input == null || input.EditedSortOrder == null)
            {
                return false;
            }

            long blockId = 0;

            foreach (var sortOrderDetails in input.EditedSortOrder)
            {
                if (sortOrderDetails.BlockId != blockId)
                {
                    blockId = sortOrderDetails.BlockId;

                    var formBlock = await _lookup_formBlockRepository.FirstOrDefaultAsync(blockId);
                    formBlock.SortOrder = sortOrderDetails.SortOrderBlock;
                    await _lookup_formBlockRepository.UpdateAsync(formBlock);
                }

                var formBlockField = await _formBlockFieldRepository.GetAll().Where(f => f.FormBlockId == sortOrderDetails.BlockId && f.FormFieldId == sortOrderDetails.FieldId).FirstOrDefaultAsync();
                formBlockField.SortOrder = sortOrderDetails.SortOrderField;
                await _formBlockFieldRepository.UpdateAsync(formBlockField);
            }

            return true;
        }

        [AbpAuthorize(AppPermissions.Pages_FormBlockFields_Edit)]
        public async Task<bool> AddFormFieldToFormBlock(long fieldId, long blockId)
        {
            int latestSortOrder = 0;           
            var currentFormFields = await _formBlockFieldRepository.GetAll().Where(f => f.FormBlockId == blockId).ToListAsync();

            if (currentFormFields.Count > 0)
            {
                latestSortOrder = currentFormFields.Select(f => f.SortOrder).Max();
            }
            
            await _formBlockFieldRepository.InsertAsync(new FormBlockField
            {
                TenantId = AbpSession.TenantId,
                CreatorUserId = AbpSession.UserId ?? 1,
                CreationTime = DateTime.Now, 
                FormFieldId = fieldId,
                FormBlockId = blockId,
                SortOrder = latestSortOrder + 1,
                IsDeleted = false
            });
            
            return true;
        }

        [AbpAuthorize(AppPermissions.Pages_FormBlockFields_Edit)]
        public async Task<bool> RemoveFormFieldFromFormBlock(long fieldId, long blockId)
        {
            var formBlockField = await _formBlockFieldRepository.GetAll().Where(f => f.FormFieldId == fieldId && f.FormBlockId == blockId).FirstOrDefaultAsync();

            if (formBlockField != null)
            {
                await _formBlockFieldRepository.HardDeleteAsync(formBlockField);
            }

            return true;
        }
    }
}