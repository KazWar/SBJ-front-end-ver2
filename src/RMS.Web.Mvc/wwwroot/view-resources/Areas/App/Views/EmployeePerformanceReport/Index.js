(function () {
    $(function () {

        var _$employeePerformanceReportTable = $('#EmployeePerformanceReportTable');
        var _employeePerformanceReportService = abp.services.app.employeePerformanceReport;
        var dataTable;

        _employeePerformanceReportService.checkIfStoredProcedureExists()
            .done(function (result) {
                if (result === false) {
                    window.location = "/App/EmployeePerformanceReport/StoredProcedureNotFound";
                }
                else {
                    dataTable = _$employeePerformanceReportTable.DataTable({
                        paging: true,
                        stateSave: true,
                        processing: true,
                        listAction: {
                            ajaxFunction: _employeePerformanceReportService.getAll,
                            inputFilter: function () {
                                return {
                                    startDateFilter: getStartDateFilter($('#StartDateFilterId')),
                                    endDateFilter: getEndDateFilter($('#EndDateFilterId'))
                                };
                            }
                        },
                        columnDefs: [
                            {
                                targets: 0,
                                data: "employeePerformanceReport.id"
                            },
                            {
                                targets: 1,
                                data: "employeePerformanceReport.userName"
                            },
                            {
                                targets: 2,
                                data: "employeePerformanceReport.datum"
                            },
                            {
                                targets: 3,
                                data: "employeePerformanceReport.aantal"
                            }
                        ]
                    });
                }
            });

        $('#StartDateFilterId').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'DD-MM-YYYY',
            defaultDate: new Date()
        });

        $('#EndDateFilterId').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'DD-MM-YYYY',
            defaultDate: new Date().setDate(new Date().getDate() + 1)
        });

        var getStartDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var getEndDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        $('#ExportToExcelButton').click(function () {
            _employeePerformanceReportService
                .getEmployeePerformanceReportToExcel({
                    startDateFilter: getStartDateFilter($('#StartDateFilterId')),
                    endDateFilter: getEndDateFilter($('#EndDateFilterId'))
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });
        
        function getEmployeePerformanceReport() {
            if (getStartDateFilter($('#StartDateFilterId')) < getEndDateFilter($('#EndDateFilterId'))) {
                dataTable.ajax.reload();
            }
            else {
                abp.message.warn('The start date has to be before the end date.');
            }
        }

        $('#GetEmployeePerformanceReportButton').click(function (e) {
            if (dataTable != null) {
                e.preventDefault();
                getEmployeePerformanceReport();
            }
        });

        $(document).keypress(function (e) {
            if (e.which === 13 && dataTable != null) {
                getEmployeePerformanceReport();
            }
        });
    });
})();