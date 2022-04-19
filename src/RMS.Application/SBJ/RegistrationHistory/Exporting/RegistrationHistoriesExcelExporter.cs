using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.RegistrationHistory.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.RegistrationHistory.Exporting
{
    public class RegistrationHistoriesExcelExporter : NpoiExcelExporterBase, IRegistrationHistoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RegistrationHistoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRegistrationHistoryForViewDto> registrationHistories)
        {
            return CreateExcelPackage(
                "RegistrationHistories.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RegistrationHistories"));

                    AddHeader(
                        sheet,
                        L("DateCreated"),
                        L("Remarks"),
                        L("AbpUserId"),
                        (L("RegistrationStatus")) + L("StatusCode"),
                        (L("Registration")) + L("FirstName")
                        );

                    AddObjects(
                        sheet, 2, registrationHistories,
                        _ => _timeZoneConverter.Convert(_.RegistrationHistory.DateCreated, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.RegistrationHistory.Remarks,
                        _ => _.RegistrationHistory.AbpUserId,
                        _ => _.RegistrationStatusStatusCode,
                        _ => _.RegistrationFirstName
                        );

                    for (var i = 1; i <= registrationHistories.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(1);
                });
        }
    }
}