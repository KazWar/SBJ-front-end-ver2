using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Forms.Exporting
{
    public class FormBlocksExcelExporter : NpoiExcelExporterBase, IFormBlocksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FormBlocksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFormBlockForViewDto> formBlocks)
        {
            return CreateExcelPackage(
                "FormBlocks.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("FormBlocks"));

                    AddHeader(
                        sheet,
                        L("Description"),
                        L("IsPurchaseRegistration"),
                        L("SortOrder"),
                        (L("FormLocale")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, formBlocks,
                        _ => _.FormBlock.Description,
                        _ => _.FormBlock.IsPurchaseRegistration,
                        _ => _.FormBlock.SortOrder,
                        _ => _.FormLocaleDescription
                        );

					
					
                });
        }
    }
}
