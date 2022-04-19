using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.PromoPlanner.Exporting
{
    public class PromoStepDatasExcelExporter : NpoiExcelExporterBase, IPromoStepDatasExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PromoStepDatasExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPromoStepDataForViewDto> promoStepDatas)
        {
            return CreateExcelPackage(
                "PromoStepData.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("PromoStepData"));

                    AddHeader(
                        sheet,
                        L("ConfirmationDate"),
                        L("Description"),
                        (L("PromoStep")) + L("Description"),
                        (L("Promo")) + L("Promocode")
                        );

                    AddObjects(
                        sheet, 2, promoStepDatas,
                        _ => _timeZoneConverter.Convert(_.PromoStepData.ConfirmationDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.PromoStepData.Description,
                        _ => _.PromoStepDescription,
                        _ => _.PromoPromocode
                        );

					
					for (var i = 1; i <= promoStepDatas.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(1);
                });
        }
    }
}
