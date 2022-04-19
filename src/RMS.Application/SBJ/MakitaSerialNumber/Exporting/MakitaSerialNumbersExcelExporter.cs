using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.MakitaSerialNumber.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.MakitaSerialNumber.Exporting
{
    public class MakitaSerialNumbersExcelExporter : NpoiExcelExporterBase, IMakitaSerialNumbersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MakitaSerialNumbersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMakitaSerialNumberForViewDto> makitaSerialNumbers)
        {
            return CreateExcelPackage(
                "MakitaSerialNumbers.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("MakitaSerialNumbers"));

                    AddHeader(
                        sheet,
                        L("ProductCode"),
                        L("SerialNumber"),
                        L("RetailerExternalCode")
                        );

                    AddObjects(
                        sheet, 2, makitaSerialNumbers,
                        _ => _.MakitaSerialNumber.ProductCode,
                        _ => _.MakitaSerialNumber.SerialNumber,
                        _ => _.MakitaSerialNumber.RetailerExternalCode
                        );

                });
        }
    }
}