using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.PromoPlanner.Exporting
{
    public class PromoStepFieldsExcelExporter : NpoiExcelExporterBase, IPromoStepFieldsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PromoStepFieldsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPromoStepFieldForViewDto> promoStepFields)
        {
            return CreateExcelPackage(
                "PromoStepFields.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("PromoStepFields"));

                    AddHeader(
                        sheet,
                        L("FormFieldId"),
                        L("Description"),
                        L("Sequence"),
                        (L("PromoStep")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, promoStepFields,
                        _ => _.PromoStepField.FormFieldId,
                        _ => _.PromoStepField.Description,
                        _ => _.PromoStepField.Sequence,
                        _ => _.PromoStepDescription
                        );

					
					
                });
        }
    }
}
