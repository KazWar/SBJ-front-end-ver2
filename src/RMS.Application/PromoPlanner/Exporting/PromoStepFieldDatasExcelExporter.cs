using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.PromoPlanner.Exporting
{
    public class PromoStepFieldDatasExcelExporter : NpoiExcelExporterBase, IPromoStepFieldDatasExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PromoStepFieldDatasExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPromoStepFieldDataForViewDto> promoStepFieldDatas)
        {
            return CreateExcelPackage(
                "PromoStepFieldData.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("PromoStepFieldData"));

                    AddHeader(
                        sheet,
                        L("Value"),
                        (L("PromoStepField")) + L("Description"),
                        (L("PromoStepData")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, promoStepFieldDatas,
                        _ => _.PromoStepFieldData.Value,
                        _ => _.PromoStepFieldDescription,
                        _ => _.PromoStepDataDescription
                        );

					
					
                });
        }
    }
}
