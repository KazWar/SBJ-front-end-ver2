using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.RegistrationJsonData.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.RegistrationJsonData.Exporting
{
    public class RegistrationJsonDatasExcelExporter : NpoiExcelExporterBase, IRegistrationJsonDatasExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RegistrationJsonDatasExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRegistrationJsonDataForViewDto> registrationJsonDatas)
        {
            return CreateExcelPackage(
                "RegistrationJsonData.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RegistrationJsonData"));

                    AddHeader(
                        sheet,
                        L("Data"),
                        L("DateCreated"),
                        (L("Registration")) + L("FirstName")
                        );

                    AddObjects(
                        sheet, 2, registrationJsonDatas,
                        _ => _.RegistrationJsonData.Data,
                        _ => _timeZoneConverter.Convert(_.RegistrationJsonData.DateCreated, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.RegistrationFirstName
                        );

                    for (var i = 1; i <= registrationJsonDatas.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2);
                });
        }
    }
}