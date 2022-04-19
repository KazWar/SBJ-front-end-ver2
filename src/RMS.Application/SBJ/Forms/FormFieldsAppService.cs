using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using RMS.Authorization;
using RMS.Dto;
using RMS.SBJ.Forms.Dtos;
using RMS.SBJ.Forms.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace RMS.SBJ.Forms
{
    [AbpAuthorize(AppPermissions.Pages_FormFields)]
    public class FormFieldsAppService : RMSAppServiceBase, IFormFieldsAppService
    {
        private readonly IRepository<FormField, long> _formFieldRepository;
        private readonly IRepository<FormFieldTranslation, long> _formFieldTranslationRepository;
        private readonly IFormFieldsExcelExporter _formFieldsExcelExporter;
        private readonly IRepository<FieldType, long> _lookup_fieldTypeRepository;

        public FormFieldsAppService(IRepository<FormField, long> formFieldRepository, 
                                    IRepository<FormFieldTranslation, long> formFieldTranslationRepository, 
                                    IFormFieldsExcelExporter formFieldsExcelExporter, 
                                    IRepository<FieldType, long> lookup_fieldTypeRepository)
        {
            _formFieldRepository = formFieldRepository;
            _formFieldTranslationRepository = formFieldTranslationRepository;
            _formFieldsExcelExporter = formFieldsExcelExporter;
            _lookup_fieldTypeRepository = lookup_fieldTypeRepository;

        }

        public async Task<PagedResultDto<GetFormFieldForViewDto>> GetAll(GetAllFormFieldsInput input)
        {

            var filteredFormFields = _formFieldRepository.GetAll()
                        .Include(e => e.FieldTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter) || e.Label.Contains(input.Filter) || e.DefaultValue.Contains(input.Filter) || e.InputMask.Contains(input.Filter) || e.RegularExpression.Contains(input.Filter) || e.ValidationApiCall.Contains(input.Filter) || e.RegistrationField.Contains(input.Filter) || e.PurchaseRegistrationField.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LabelFilter), e => e.Label == input.LabelFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DefaultValueFilter), e => e.DefaultValue == input.DefaultValueFilter)
                        .WhereIf(input.MinMaxLengthFilter != null, e => e.MaxLength >= input.MinMaxLengthFilter)
                        .WhereIf(input.MaxMaxLengthFilter != null, e => e.MaxLength <= input.MaxMaxLengthFilter)
                        .WhereIf(input.RequiredFilter.HasValue && input.RequiredFilter > -1, e => (input.RequiredFilter == 1 && e.Required) || (input.RequiredFilter == 0 && !e.Required))
                        .WhereIf(input.ReadOnlyFilter.HasValue && input.ReadOnlyFilter > -1, e => (input.ReadOnlyFilter == 1 && e.ReadOnly) || (input.ReadOnlyFilter == 0 && !e.ReadOnly))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InputMaskFilter), e => e.InputMask == input.InputMaskFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegularExpressionFilter), e => e.RegularExpression == input.RegularExpressionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ValidationApiCallFilter), e => e.ValidationApiCall == input.ValidationApiCallFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationFieldFilter), e => e.RegistrationField == input.RegistrationFieldFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PurchaseRegistrationFieldFilter), e => e.PurchaseRegistrationField == input.PurchaseRegistrationFieldFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FieldTypeDescriptionFilter), e => e.FieldTypeFk != null && e.FieldTypeFk.Description == input.FieldTypeDescriptionFilter);

            var pagedAndFilteredFormFields = filteredFormFields
                .OrderBy(input.Sorting ?? "id asc");

            var formFields = from o in pagedAndFilteredFormFields
                             join o1 in _lookup_fieldTypeRepository.GetAll() on o.FieldTypeId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             select new GetFormFieldForViewDto()
                             {
                                 FormField = new FormFieldDto
                                 {
                                     Description = o.Description,
                                     Label = o.Label,
                                     DefaultValue = o.DefaultValue,
                                     MaxLength = o.MaxLength,
                                     Required = o.Required,
                                     ReadOnly = o.ReadOnly,
                                     InputMask = o.InputMask,
                                     RegularExpression = o.RegularExpression,
                                     ValidationApiCall = o.ValidationApiCall,
                                     RegistrationField = o.RegistrationField,
                                     PurchaseRegistrationField = o.PurchaseRegistrationField,
                                     Id = o.Id
                                 },
                                 FieldTypeDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
                             };

            var totalCount = await filteredFormFields.CountAsync();

            return new PagedResultDto<GetFormFieldForViewDto>(
                totalCount,
                await formFields.ToListAsync()
            );
        }

        public async Task<GetFormFieldForViewDto> GetFormFieldForView(long id)
        {
            var formField = await _formFieldRepository.GetAsync(id);

            var output = new GetFormFieldForViewDto { FormField = ObjectMapper.Map<FormFieldDto>(formField) };

            var _lookupFieldType = await _lookup_fieldTypeRepository.FirstOrDefaultAsync((long)output.FormField.FieldTypeId);
            output.FieldTypeDescription = _lookupFieldType?.Description?.ToString();

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FormFields_Edit)]
        public async Task<GetFormFieldForEditOutput> GetFormFieldForEdit(EntityDto<long> input)
        {
            var formField = await _formFieldRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFormFieldForEditOutput { FormField = ObjectMapper.Map<CreateOrEditFormFieldDto>(formField) };

            var _lookupFieldType = await _lookup_fieldTypeRepository.FirstOrDefaultAsync((long)output.FormField.FieldTypeId);
            output.FieldTypeDescription = _lookupFieldType?.Description?.ToString();

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFormFieldDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_FormFields_Create)]
        protected virtual async Task Create(CreateOrEditFormFieldDto input)
        {
            var formField = ObjectMapper.Map<FormField>(input);

            if (AbpSession.TenantId != null)
            {
                formField.TenantId = (int?)AbpSession.TenantId;
            }

            await _formFieldRepository.InsertAsync(formField);
        }

        [AbpAuthorize(AppPermissions.Pages_FormFields_Edit)]
        protected virtual async Task Update(CreateOrEditFormFieldDto input)
        {
            var formField = await _formFieldRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, formField);
        }

        [AbpAuthorize(AppPermissions.Pages_FormFields_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _formFieldRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFormFieldsToExcel(GetAllFormFieldsForExcelInput input)
        {

            var filteredFormFields = _formFieldRepository.GetAll()
                .Include(e => e.FieldTypeFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter) || e.Label.Contains(input.Filter) || e.DefaultValue.Contains(input.Filter) || e.InputMask.Contains(input.Filter) || e.RegularExpression.Contains(input.Filter) || e.ValidationApiCall.Contains(input.Filter) || e.RegistrationField.Contains(input.Filter) ||  e.PurchaseRegistrationField.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.LabelFilter), e => e.Label == input.LabelFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DefaultValueFilter), e => e.DefaultValue == input.DefaultValueFilter)
                .WhereIf(input.MinMaxLengthFilter != null, e => e.MaxLength >= input.MinMaxLengthFilter)
                .WhereIf(input.MaxMaxLengthFilter != null, e => e.MaxLength <= input.MaxMaxLengthFilter)
                .WhereIf(input.RequiredFilter.HasValue && input.RequiredFilter > -1, e => (input.RequiredFilter == 1 && e.Required) || (input.RequiredFilter == 0 && !e.Required))
                .WhereIf(input.ReadOnlyFilter.HasValue && input.ReadOnlyFilter > -1, e => (input.ReadOnlyFilter == 1 && e.ReadOnly) || (input.ReadOnlyFilter == 0 && !e.ReadOnly))
                .WhereIf(!string.IsNullOrWhiteSpace(input.InputMaskFilter), e => e.InputMask == input.InputMaskFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.RegularExpressionFilter), e => e.RegularExpression == input.RegularExpressionFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ValidationApiCallFilter), e => e.ValidationApiCall == input.ValidationApiCallFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.RegistrationFieldFilter), e => e.RegistrationField == input.RegistrationFieldFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.PurchaseRegistrationFieldFilter), e => e.PurchaseRegistrationField == input.PurchaseRegistrationFieldFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.FieldTypeDescriptionFilter), e => e.FieldTypeFk != null && e.FieldTypeFk.Description == input.FieldTypeDescriptionFilter);

            var query = (from o in filteredFormFields
                         join o1 in _lookup_fieldTypeRepository.GetAll() on o.FieldTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         select new GetFormFieldForViewDto()
                         {
                             FormField = new FormFieldDto
                             {
                                 Description = o.Description,
                                 Label = o.Label,
                                 DefaultValue = o.DefaultValue,
                                 MaxLength = o.MaxLength,
                                 Required = o.Required,
                                 ReadOnly = o.ReadOnly,
                                 InputMask = o.InputMask,
                                 RegularExpression = o.RegularExpression,
                                 ValidationApiCall = o.ValidationApiCall,
                                 RegistrationField = o.RegistrationField,
                                 PurchaseRegistrationField = o.PurchaseRegistrationField,
                                 Id = o.Id
                             },
                             FieldTypeDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
                         });


            var formFieldListDtos = await query.ToListAsync();

            return _formFieldsExcelExporter.ExportToFile(formFieldListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_FormFields)]
        public async Task<PagedResultDto<FormFieldFieldTypeLookupTableDto>> GetAllFieldTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_fieldTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var fieldTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<FormFieldFieldTypeLookupTableDto>();
            foreach (var fieldType in fieldTypeList)
            {
                lookupTableDtoList.Add(new FormFieldFieldTypeLookupTableDto
                {
                    Id = fieldType.Id,
                    DisplayName = fieldType.Description?.ToString()
                });
            }

            return new PagedResultDto<FormFieldFieldTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        //Created a new method to get and return all the form field entities from the database without any filterations
        //EXISTING: The existing GetAll() return paged and sorted list based on the input passed
        public async Task<List<GetFormFieldForViewDto>> GetAllFormFields()
        {

            var allFormFields = _formFieldRepository.GetAll();

            var formFields = from o in allFormFields
                             join o1 in _lookup_fieldTypeRepository.GetAll() on o.FieldTypeId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             select new GetFormFieldForViewDto()
                             {
                                 FormField = new FormFieldDto
                                 {
                                     Description = o.Description,
                                     Label = o.Label,
                                     DefaultValue = o.DefaultValue,
                                     MaxLength = o.MaxLength,
                                     Required = o.Required,
                                     ReadOnly = o.ReadOnly,
                                     InputMask = o.InputMask,
                                     RegularExpression = o.RegularExpression,
                                     ValidationApiCall = o.ValidationApiCall,
                                     RegistrationField = o.RegistrationField,
                                     PurchaseRegistrationField = o.PurchaseRegistrationField,
                                     
                                     Id = o.Id
                                 },
                                 FieldTypeDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString()
                             };

            var totalCount = await allFormFields.CountAsync();

            return await formFields.ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_FormFields_Edit)]
        public async Task<bool> UpdateFormFields(UpdateFormFieldsDto input)
        {
            if (input == null || input.EditedFormFields == null)
            {
                return false;
            }

            foreach (var editedFormField in input.EditedFormFields)
            {
                var formField = await _formFieldRepository.GetAsync(editedFormField.FieldId);
                var formFieldTranslation = await _formFieldTranslationRepository.GetAll().Where(t => t.FormFieldId == editedFormField.FieldId && t.LocaleId == editedFormField.LocaleId).FirstOrDefaultAsync();

                formField.Description = editedFormField.FieldDescription;
                formField.MaxLength = editedFormField.MaxLength;
                formField.Required = editedFormField.RequiredField;
                formField.ReadOnly = editedFormField.ReadOnly;
                formField.Label = editedFormField.FieldLabelGlobal;
                formField.DefaultValue = editedFormField.DefaultValueGlobal;
                formField.RegularExpression = editedFormField.RegularExpressionGlobal;

                if (!String.IsNullOrWhiteSpace(editedFormField.FieldDescription) && !String.IsNullOrWhiteSpace(editedFormField.FieldLabelGlobal))
                {
                    await _formFieldRepository.UpdateAsync(formField);
                }

                if (!String.IsNullOrWhiteSpace(editedFormField.FieldDescription) && !String.IsNullOrWhiteSpace(editedFormField.FieldLabelLocale))
                {
                    if (formFieldTranslation != null)
                    {
                        formFieldTranslation.Label = editedFormField.FieldLabelLocale;
                        formFieldTranslation.DefaultValue = editedFormField.DefaultValueLocale;
                        formFieldTranslation.RegularExpression = editedFormField.RegularExpressionLocale;

                        await _formFieldTranslationRepository.UpdateAsync(formFieldTranslation);
                    }
                    else
                    {
                        await _formFieldTranslationRepository.InsertAsync(new FormFieldTranslation()
                        {
                            FormFieldId = editedFormField.FieldId,
                            LocaleId = editedFormField.LocaleId,
                            Label = editedFormField.FieldLabelLocale,
                            DefaultValue = editedFormField.DefaultValueLocale,
                            RegularExpression = editedFormField.RegularExpressionLocale,
                            TenantId = AbpSession.TenantId,
                            CreatorUserId = AbpSession.UserId ?? 1,
                            CreationTime = DateTime.Now
                        });
                    }
                }
            }

            return true;
        }
    }
}