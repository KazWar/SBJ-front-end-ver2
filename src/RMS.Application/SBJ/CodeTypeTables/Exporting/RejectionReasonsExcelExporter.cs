using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CodeTypeTables.Exporting
{
    public class RejectionReasonsExcelExporter : NpoiExcelExporterBase, IRejectionReasonsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RejectionReasonsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRejectionReasonForViewDto> rejectionReasons)
        {
            return CreateExcelPackage(
                "RejectionReasons.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RejectionReasons"));

                    AddHeader(
                        sheet,
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, rejectionReasons,
                        _ => _.RejectionReason.Description
                        );

                });
        }
    }
}