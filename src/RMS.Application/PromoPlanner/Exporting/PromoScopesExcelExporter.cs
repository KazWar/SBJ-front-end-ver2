using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.PromoPlanner.Exporting
{
    public class PromoScopesExcelExporter : NpoiExcelExporterBase, IPromoScopesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PromoScopesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPromoScopeForViewDto> promoScopes)
        {
            return CreateExcelPackage(
                "PromoScopes.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("PromoScopes"));

                    AddHeader(
                        sheet,
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, promoScopes,
                        _ => _.PromoScope.Description
                        );

					
					
                });
        }
    }
}
