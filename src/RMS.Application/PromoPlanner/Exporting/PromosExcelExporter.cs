using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.PromoPlanner.Exporting
{
    public class PromosExcelExporter : NpoiExcelExporterBase, IPromosExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PromosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPromoForViewDto> promos)
        {
            return CreateExcelPackage(
                "Promos.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Promos"));

                    AddHeader(
                        sheet,
                        L("Promocode"),
                        L("Description"),
                        L("CampaignType"),
                        L("PromoStart"),
                        L("PromoEnd"),
                        L("PromoScope"),
                        L("ProductCategory")
                        );

                    AddObjects(
                        sheet, 2, promos,
                        _ => _.Promo.Promocode,
                        _ => _.Promo.Description,
                         _ => _.CampaignTypeName,
                       _ => _timeZoneConverter.Convert(_.Promo.PromoStart, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Promo.PromoEnd, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.PromoScopeDescription,
                        _ => _.ProductCategoryDescription
                        );


                    for (var i = 1; i <= promos.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3); for (var i = 1; i <= promos.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(4);
                });
        }
    }
}
