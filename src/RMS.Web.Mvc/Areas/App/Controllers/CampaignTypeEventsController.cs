using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Authorization;
using RMS.SBJ.CampaignProcesses;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.SBJ.CodeTypeTables;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Web.Areas.App.Models.CampaignTypeEvents;
using RMS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CampaignTypeEvents)]
    public class CampaignTypeEventsController : RMSControllerBase
    {
        private readonly ICampaignTypeEventsAppService _campaignTypeEventsAppService;
        private readonly ICampaignTypesAppService _campaignTypesAppService;
        private readonly IProcessEventsAppService _processEventsAppService;
        private readonly IRegistrationStatusesAppService _registrationStatusesAppService;
        private readonly ICampaignTypeEventRegistrationStatusesAppService _campaignTypeEventRegistrationStatusesAppService;

        public CampaignTypeEventsController(
            ICampaignTypeEventsAppService campaignTypeEventsAppService,
            ICampaignTypesAppService campaignTypesAppService,
            IProcessEventsAppService processEventsAppService,
            IRegistrationStatusesAppService registrationStatusesAppService,
            ICampaignTypeEventRegistrationStatusesAppService campaignTypeEventRegistrationStatusesAppService
        )
        {
            _campaignTypeEventsAppService = campaignTypeEventsAppService;
            _campaignTypesAppService = campaignTypesAppService;
            _processEventsAppService = processEventsAppService;
            _registrationStatusesAppService = registrationStatusesAppService;
            _campaignTypeEventRegistrationStatusesAppService = campaignTypeEventRegistrationStatusesAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new CampaignTypeEventsViewModel
            {
                CampaignTypes = await this.GetAllAvailableCampaignTypes(),
                ProcessEvents = await this.GetAllAvailableProcessEvents(),
                RegistrationStatuses = await this.GetAllAvailableRegistrationStatuses(),
            };
            return View(model);
        }
        [HttpGet]
        public async Task<JsonResult> LoadCampaignTypeEvents(long campaignTypeId, long campaignTypeEventId)
        {
            var allCampaignTypeEvents = await _campaignTypeEventsAppService.GetAll();
            var allActiveProcessEvents = await this.GetAllAvailableProcessEvents();
            var allActiveRegistrationStatuses = await this.GetAllAvailableRegistrationStatuses();
            var allCampaignTypeEventRegistrationStatuses = await _campaignTypeEventRegistrationStatusesAppService.GetAllCampaignTypeEventRegistrationStatus();

            var filteredCampaignTypeEvents = allCampaignTypeEvents.Items.Where(i => i.CampaignTypeEvent.CampaignTypeId == campaignTypeId).Select(c => c);

            var filteredProcessEvents = allActiveProcessEvents.Items.Where(i => filteredCampaignTypeEvents.Any(c => c.CampaignTypeEvent.ProcessEventId == i.ProcessEvent.Id));
            var availableProcessEvents = allActiveProcessEvents.Items.Except(filteredProcessEvents);
            var selectedProcessEvents = filteredCampaignTypeEvents.Where(item => allActiveProcessEvents.Items.Any(y => y.ProcessEvent.Id == item.CampaignTypeEvent.ProcessEventId))
                .OrderBy(item => item.CampaignTypeEvent.SortOrder)
                .Select(events => events);
            
            var registrationStatusIdList = allCampaignTypeEventRegistrationStatuses.Items.Where(i => i.CampaignTypeEventRegistrationStatus.CampaignTypeEventId == campaignTypeEventId).Select(v => v.CampaignTypeEventRegistrationStatus.RegistrationStatusId);
            var availableRegistrationStatuses = allActiveRegistrationStatuses.Items.Where(i => !registrationStatusIdList.Contains(i.RegistrationStatus.Id));
            var selectedRegistrationStatuses = allCampaignTypeEventRegistrationStatuses.Items.Where(item => item.CampaignTypeEventRegistrationStatus.CampaignTypeEventId == campaignTypeEventId)
                .OrderBy(item => item.CampaignTypeEventRegistrationStatus.SortOrder)
                .Select(status => status);

            var model = new CampaignTypeEventsViewModel
            {
                CampaignTypes = await this.GetAllAvailableCampaignTypes(),
                AvailableProcessEvents = availableProcessEvents,
                SelectedProcessEvents = selectedProcessEvents,
                AvailableRegistrationStatuses = availableRegistrationStatuses,
                SelectedRegistrationStatus = selectedRegistrationStatuses,
            };
            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateCampaignTypeEvents([FromBody]SaveCampaignTypeEventViewModel viewModel)
        {
            //Get all active campaign type events
            var allActiveCampaignTypeEvents = await _campaignTypeEventsAppService.GetAll();

            //Get all active campaign type event registration statuses
            var allActiveCampaignTypeEventRegistrationStatuses = await _campaignTypeEventRegistrationStatusesAppService.GetAllCampaignTypeEventRegistrationStatus();

            //Get all selected Registration statuses 
            var registrationStatusesInViewModel = viewModel.Events.Where(i => i.RegistrationStatusPerProcessEvent != null).SelectMany(e => e.RegistrationStatusPerProcessEvent.ToList());

            //Filter campaign type events only for selected campaign type 
            var campaignTypeEventsPerCampaignType = allActiveCampaignTypeEvents.Items.Where(item => viewModel.CampaignTypeId == item.CampaignTypeEvent.CampaignTypeId);

            //Filter campaign type event registration statuses for selected campaign type event 
            var registrationStatusesPercampaignTypeEvents = allActiveCampaignTypeEventRegistrationStatuses.Items.Where(item => viewModel.RegistrationCampaignTypeEventId == item.CampaignTypeEventRegistrationStatus.CampaignTypeEventId);

            var processEventPresentInBoardAndDB = viewModel.Events.Where(e => campaignTypeEventsPerCampaignType.Any(item => item.CampaignTypeEvent.CampaignTypeId == e.CampaignTypeId && item.CampaignTypeEvent.ProcessEventId  == e.ProcessEventId));
            var processEventNotAvailableInBoardButPresentInDB = campaignTypeEventsPerCampaignType.Where(item => !viewModel.Events.Any(e => e.CampaignTypeId == item.CampaignTypeEvent.CampaignTypeId && e.ProcessEventId == item.CampaignTypeEvent.ProcessEventId));
            var processEventPresentInBoardButNotAvailableInDB = viewModel.Events.Where(i => !campaignTypeEventsPerCampaignType.Any(f => f.CampaignTypeEvent.CampaignTypeId == i.CampaignTypeId && f.CampaignTypeEvent.ProcessEventId == i.ProcessEventId));

            var registrationStatusesPresentInBoardAndDB = registrationStatusesInViewModel.Where(m => registrationStatusesPercampaignTypeEvents.Any(item => item.CampaignTypeEventRegistrationStatus.RegistrationStatusId == m.RegistrationStatusId && item.CampaignTypeEventRegistrationStatus.CampaignTypeEventId == m.RegistrationCampaignTypeEventId));
            var registrationStatusesNotAvailableInBoardButPresentInDB = registrationStatusesPercampaignTypeEvents
                .Where(item => !registrationStatusesInViewModel.Any(m => m.RegistrationStatusId == item.CampaignTypeEventRegistrationStatus.RegistrationStatusId && m.RegistrationCampaignTypeEventId == item.CampaignTypeEventRegistrationStatus.CampaignTypeEventId));
            var registrationStatusesPresentInBoardButNotAvailableInDB = registrationStatusesInViewModel
                .Where(i => !registrationStatusesPercampaignTypeEvents.Any(m => m.CampaignTypeEventRegistrationStatus.RegistrationStatusId == i.RegistrationStatusId && m.CampaignTypeEventRegistrationStatus.CampaignTypeEventId == i.RegistrationCampaignTypeEventId));

            if (processEventPresentInBoardAndDB.Count() > 0)
            {
                var events = processEventPresentInBoardAndDB.ToArray();
                for (var i = 0; i < events.Count(); i++)
                {
                    var campaignTypeEventId = campaignTypeEventsPerCampaignType.Where(item => item.CampaignTypeEvent.CampaignTypeId == events[i].CampaignTypeId && item.CampaignTypeEvent.ProcessEventId == events[i].ProcessEventId).Select(f => f.CampaignTypeEvent.Id).FirstOrDefault();
                    var updateEntity = new CreateOrEditCampaignTypeEventDto
                    {
                        CampaignTypeId = events[i].CampaignTypeId,
                        ProcessEventId = events[i].ProcessEventId,
                        SortOrder = Convert.ToInt32(events[i].SortOrderId),
                        Id = campaignTypeEventId,
                        IsUpdate = true
                    };
                    if (events[i].SortOrderId != campaignTypeEventsPerCampaignType.Where(item => item.CampaignTypeEvent.CampaignTypeId == events[i].CampaignTypeId && item.CampaignTypeEvent.ProcessEventId == events[i].ProcessEventId).Select(f => f.CampaignTypeEvent.SortOrder).FirstOrDefault())
                    {
                        await _campaignTypeEventsAppService.CreateOrEdit(updateEntity);
                    }
                }
            }

            if (processEventPresentInBoardButNotAvailableInDB.Count() > 0)
            {
                var events = processEventPresentInBoardButNotAvailableInDB.ToArray();
                for (var i = 0; i < events.Count(); i++)
                {
                    var createEntity = new CreateOrEditCampaignTypeEventDto
                    {
                        CampaignTypeId = events[i].CampaignTypeId,
                        ProcessEventId = events[i].ProcessEventId,
                        SortOrder = Convert.ToInt32(events[i].SortOrderId),
                        Id = 0
                    };
                    await _campaignTypeEventsAppService.CreateOrEdit(createEntity);
                }
            }

            if (processEventNotAvailableInBoardButPresentInDB.Count() > 0 || viewModel.Events.Count() == 0)
            {
                foreach (var campaignTypeEvent in processEventNotAvailableInBoardButPresentInDB)
                {
                    await _campaignTypeEventsAppService.Delete(new EntityDto<long>(campaignTypeEvent.CampaignTypeEvent.Id));
                }
            }

            if (registrationStatusesPresentInBoardAndDB.Count() > 0)
            {
                var statuses = registrationStatusesPresentInBoardAndDB.ToArray();
                for (var i = 0; i < statuses.Count(); i++)
                {
                    var campaignTypeEventRegistrationStatusId = registrationStatusesPercampaignTypeEvents.Where(item => item.CampaignTypeEventRegistrationStatus.CampaignTypeEventId == statuses[i].RegistrationCampaignTypeEventId && item.CampaignTypeEventRegistrationStatus.RegistrationStatusId == statuses[i].RegistrationStatusId).Select(item => item.CampaignTypeEventRegistrationStatus.Id).FirstOrDefault();
                    var updateRegistrationStatusEntity = new CreateOrEditCampaignTypeEventRegistrationStatusDto
                    {
                        CampaignTypeEventId = statuses[i].RegistrationCampaignTypeEventId,
                        RegistrationStatusId = statuses[i].RegistrationStatusId,
                        SortOrder = Convert.ToInt32(statuses[i].RegistrationSortOrder),
                        Id = campaignTypeEventRegistrationStatusId,
                        IsUpdate = true
                    };
                    if (statuses[i].RegistrationSortOrder != registrationStatusesPercampaignTypeEvents.Where(item => item.CampaignTypeEventRegistrationStatus.CampaignTypeEventId == statuses[i].RegistrationCampaignTypeEventId && item.CampaignTypeEventRegistrationStatus.RegistrationStatusId == statuses[i].RegistrationStatusId).Select(f => f.CampaignTypeEventRegistrationStatus.SortOrder).FirstOrDefault())
                    {
                        await _campaignTypeEventRegistrationStatusesAppService.CreateOrEdit(updateRegistrationStatusEntity);
                    }
                }
            }

            if (registrationStatusesPresentInBoardButNotAvailableInDB.Count() > 0)
            {
                var statuses = registrationStatusesPresentInBoardButNotAvailableInDB.ToArray();
                for (var i = 0; i < statuses.Count(); i++)
                {
                    var createRegistrationStatusEntity = new CreateOrEditCampaignTypeEventRegistrationStatusDto
                    {
                        CampaignTypeEventId = statuses[i].RegistrationCampaignTypeEventId,
                        RegistrationStatusId = statuses[i].RegistrationStatusId,
                        SortOrder= Convert.ToInt32(statuses[i].RegistrationSortOrder),
                        Id = statuses[i].CampaignTypeEventRegistrationStatusId
                    };
                    if (statuses[i].CampaignTypeEventRegistrationStatusId == 0)
                    {
                        await _campaignTypeEventRegistrationStatusesAppService.CreateOrEdit(createRegistrationStatusEntity);
                    }
                }
            }

            if (registrationStatusesNotAvailableInBoardButPresentInDB.Count() > 0 || registrationStatusesInViewModel.Count() == 0)
            {
                foreach (var registrationStatus in registrationStatusesNotAvailableInBoardButPresentInDB)
                {
                    await _campaignTypeEventRegistrationStatusesAppService.Delete(new EntityDto<long>(registrationStatus.CampaignTypeEventRegistrationStatus.Id));
                }
            }

            var model = new CampaignTypeEventsViewModel
            {
                CampaignTypes = await this.GetAllAvailableCampaignTypes(),
            };
            return Json(model);
        }

        private async Task<PagedResultDto<GetCampaignTypeForViewDto>> GetAllAvailableCampaignTypes()
        {
            return await this._campaignTypesAppService.GetAll(new SBJ.CodeTypeTables.Dtos.GetAllCampaignTypesInput
            {
                IsActiveFilter = 1
            });
        }

        private async Task<PagedResultDto<GetProcessEventForViewDto>> GetAllAvailableProcessEvents()
        {
            return await this._processEventsAppService.GetAll(new SBJ.CodeTypeTables.Dtos.GetAllProcessEventsInput
            {
                IsActiveFilter = 1,
            });
        }

        private async Task<PagedResultDto<GetRegistrationStatusForViewDto>> GetAllAvailableRegistrationStatuses()
        {
            return await this._registrationStatusesAppService.GetAll(new SBJ.CodeTypeTables.Dtos.GetAllRegistrationStatusesInput
            {
                IsActiveFilter = 1
            });
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CampaignTypeEvents_Create, AppPermissions.Pages_CampaignTypeEvents_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            GetCampaignTypeEventForEditOutput getCampaignTypeEventForEditOutput;

            if (id.HasValue)
            {
                getCampaignTypeEventForEditOutput = await _campaignTypeEventsAppService.GetCampaignTypeEventForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getCampaignTypeEventForEditOutput = new GetCampaignTypeEventForEditOutput
                {
                    CampaignTypeEvent = new CreateOrEditCampaignTypeEventDto()
                };
            }

            var viewModel = new CreateOrEditCampaignTypeEventModalViewModel()
            {
                CampaignTypeEvent = getCampaignTypeEventForEditOutput.CampaignTypeEvent,
                CampaignTypeName = getCampaignTypeEventForEditOutput.CampaignTypeName,
                ProcessEventName = getCampaignTypeEventForEditOutput.ProcessEventName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCampaignTypeEventModal(long id)
        {
            var getCampaignTypeEventForViewDto = await _campaignTypeEventsAppService.GetCampaignTypeEventForView(id);

            var model = new CampaignTypeEventViewModel()
            {
                CampaignTypeEvent = getCampaignTypeEventForViewDto.CampaignTypeEvent,
                CampaignTypeName = getCampaignTypeEventForViewDto.CampaignTypeName,
                ProcessEventName = getCampaignTypeEventForViewDto.ProcessEventName
            };

            return PartialView("_ViewCampaignTypeEventModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CampaignTypeEvents_Create, AppPermissions.Pages_CampaignTypeEvents_Edit)]
        public PartialViewResult CampaignTypeLookupTableModal(long? id, string displayName)
        {
            var viewModel = new CampaignTypeEventCampaignTypeLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CampaignTypeEventCampaignTypeLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_CampaignTypeEvents_Create, AppPermissions.Pages_CampaignTypeEvents_Edit)]
        public PartialViewResult ProcessEventLookupTableModal(long? id, string displayName)
        {
            var viewModel = new CampaignTypeEventProcessEventLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CampaignTypeEventProcessEventLookupTableModal", viewModel);
        }
    }
}