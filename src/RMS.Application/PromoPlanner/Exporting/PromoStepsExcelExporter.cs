using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.PromoPlanner.Exporting
{
    public class PromoStepsExcelExporter : NpoiExcelExporterBase, IPromoStepsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PromoStepsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPromoStepForViewDto> promoSteps)
        {
            return CreateExcelPackage(
                "PromoSteps.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("PromoSteps"));

                    AddHeader(
                        sheet,
                        L("Sequence"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, promoSteps,
                        _ => _.PromoStep.Sequence,
                        _ => _.PromoStep.Description
                        );

					
					
                });
        }
    }
}
