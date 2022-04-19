using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CodeTypeTables.Exporting
{
    public class MessageVariablesExcelExporter : NpoiExcelExporterBase, IMessageVariablesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MessageVariablesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMessageVariableForViewDto> messageVariables)
        {
            return CreateExcelPackage(
                "MessageVariables.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("MessageVariables"));

                    AddHeader(
                        sheet,
                        L("Description"),
                        L("RmsTable"),
                        L("TableField")
                        );

                    AddObjects(
                        sheet, 2, messageVariables,
                        _ => _.MessageVariable.Description,
                        _ => _.MessageVariable.RmsTable,
                        _ => _.MessageVariable.TableField
                        );

					
					
                });
        }
    }
}
