using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using RMS.PromoPlanner.Exporting;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using Abp.Application.Services.Dto;
using RMS.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RMS.PromoPlanner
{
    [AbpAuthorize(AppPermissions.Pages_PromoSteps)]
    public class PromoStepsAppService : RMSAppServiceBase, IPromoStepsAppService
    {
        private readonly IRepository<PromoStep> _promoStepRepository;
        private readonly IRepository<PromoStepField> _promoStepFieldRepository;
        private readonly IPromoStepsExcelExporter _promoStepsExcelExporter;
        private readonly IRepository<PromoStepField, int> _lookup_promoStepFieldRepository;

        public PromoStepsAppService(
            IRepository<PromoStep> promoStepRepository,
            IRepository<PromoStepField> promoStepFieldRepository,
            IPromoStepsExcelExporter promoStepsExcelExporter,
            IRepository<PromoStepField, int> lookup_promoStepFieldRepository)
        {
            _promoStepRepository = promoStepRepository;
            _promoStepFieldRepository = promoStepFieldRepository;
            _promoStepsExcelExporter = promoStepsExcelExporter;
            _lookup_promoStepFieldRepository = lookup_promoStepFieldRepository;
        }

        public async Task<PagedResultDto<GetPromoStepForViewDto>> GetAll(GetAllPromoStepsInput input)
        {
            var filteredPromoSteps = _promoStepRepository
                .GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredPromoSteps = filteredPromoSteps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var promoSteps = from o in pagedAndFilteredPromoSteps
                             select new GetPromoStepForViewDto()
                             {
                                 PromoStep = new PromoStepDto
                                 {
                                     Sequence = o.Sequence,
                                     Description = o.Description,
                                     Id = o.Id
                                 }
                             };

            var totalCount = await filteredPromoSteps.CountAsync();

            return new PagedResultDto<GetPromoStepForViewDto>(
                totalCount,
                await promoSteps.ToListAsync()
            );
        }

        public async Task<IReadOnlyList<CustomPromoStepForView>> GetAllAsReadOnlyList(GetAllPromoStepsInput input)
        {
            var filteredPromoSteps = _promoStepRepository
                .GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                .OrderBy(input.Sorting ?? "id asc");

            var filteredPromoStepFields = _promoStepFieldRepository.GetAll();

            var promoSteps = from o in filteredPromoSteps
                             select new CustomPromoStepForView
                             {
                                 StepId = o.Id,
                                 Sequence = o.Sequence,
                                 Description = o.Description,
                                 PromoStepFields =
                                    from psf in filteredPromoStepFields
                                    where psf.PromoStepId == o.Id
                                    select new CustomPromoStepFieldForView
                                    {
                                        FieldId = psf.Id,
                                        Sequence = psf.Sequence,
                                        FieldName = psf.Description
                                    }
                             };

            return await promoSteps.ToListAsync();
        }

        public async Task<GetPromoStepForViewDto> GetPromoStepForView(int id)
        {
            var promoStep = await _promoStepRepository.GetAsync(id);

            var output = new GetPromoStepForViewDto { PromoStep = ObjectMapper.Map<PromoStepDto>(promoStep) };

            return output;
        }

        public async Task<GetPromoStepForViewDto> GetPromoStepForViewBySequence(short sequence)
        {
            var promoStep = await _promoStepRepository.GetAll().Where(s => s.Sequence == sequence).FirstOrDefaultAsync();

            var output = new GetPromoStepForViewDto { PromoStep = ObjectMapper.Map<PromoStepDto>(promoStep) };

            return output;
        }

        public async Task<CustomPromoStepForView> GetPromoStepForViewBySequenceWithFields(short sequence)
        {
            var promoStep = await _promoStepRepository.GetAll().Where(s => s.Sequence == sequence).FirstOrDefaultAsync();
            var promoStepFields = _lookup_promoStepFieldRepository.GetAll().Where(e => e.PromoStepId == promoStep.Id);

            var output = new CustomPromoStepForView()
            {
                StepId = promoStep.Id,
                Sequence = promoStep.Sequence,
                Description = promoStep.Description,
                Status = "in progress"
            };

            var promoStepFieldsList = new List<CustomPromoStepFieldForView>();

            foreach (var promoStepField in promoStepFields)
            {
                promoStepFieldsList.Add(new CustomPromoStepFieldForView() { FieldId = promoStepField.Id, FieldName = promoStepField.Description, FieldValue = "" });
            }

            output.PromoStepFields = promoStepFieldsList.OrderBy(s => s.Sequence);

            return output;
        }

        public async Task<int> GetPromoStepTotalCount()
        {
            var promoSteps = await _promoStepRepository.GetAllListAsync();

            return promoSteps.Count;
        }

        [AbpAuthorize(AppPermissions.Pages_PromoSteps_Edit)]
        public async Task<GetPromoStepForEditOutput> GetPromoStepForEdit(EntityDto input)
        {
            var promoStep = await _promoStepRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPromoStepForEditOutput { PromoStep = ObjectMapper.Map<CreateOrEditPromoStepDto>(promoStep) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPromoStepDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PromoSteps_Create)]
        protected virtual async Task Create(CreateOrEditPromoStepDto input)
        {
            var promoStep = ObjectMapper.Map<PromoStep>(input);


            if (AbpSession.TenantId != null)
            {
                promoStep.TenantId = (int?)AbpSession.TenantId;
            }


            await _promoStepRepository.InsertAsync(promoStep);
        }

        [AbpAuthorize(AppPermissions.Pages_PromoSteps_Edit)]
        protected virtual async Task Update(CreateOrEditPromoStepDto input)
        {
            var promoStep = await _promoStepRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, promoStep);
        }

        [AbpAuthorize(AppPermissions.Pages_PromoSteps_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _promoStepRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPromoStepsToExcel(GetAllPromoStepsForExcelInput input)
        {

            var filteredPromoSteps = _promoStepRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                        .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var query = (from o in filteredPromoSteps
                         select new GetPromoStepForViewDto()
                         {
                             PromoStep = new PromoStepDto
                             {
                                 Sequence = o.Sequence,
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });


            var promoStepListDtos = await query.ToListAsync();

            return _promoStepsExcelExporter.ExportToFile(promoStepListDtos);
        }


    }
}