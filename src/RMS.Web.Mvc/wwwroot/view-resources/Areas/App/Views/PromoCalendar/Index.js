(function () {
    $(function () {

        var getRetailers = function () {
            var retailers = [];
            $.each($('#RetailerFilterId option:selected'), function () {
                retailers.push($(this).val());
            });
            return String(retailers);
        }

        var getFilter = function () {
            return {
                filter: $('#PromosTableFilter').val(),
                promocodeFilter: $('#PromocodeFilterId').val(),
                descriptionFilter: $('#DescriptionFilterId').val(),
                promoScopeFilter: $('#PromoScopeFilterId').val(),
                campaignTypeFilter: $('#CampaignTypeFilterId').val(),
                productCategoryFilter: $('#ProductCategoryFilterId').val(),
                retailerFilter: getRetailers()
            };
        }
        var calendarEl = document.getElementById('promocalendar');
        var calendar = new FullCalendar.Calendar(calendarEl, {
            initialView: 'dayGridWeek',
            headerToolbar: {
                left: 'today,dayGridWeek,dayGridMonth',
                center: 'title',
                right: 'prevYear,prev,next,nextYear'
            },
            buttonText: {
                today: 'vandaag',
                month: 'maand',
                week: 'week'
            },
            weekNumbers: true,
            firstDay: 1,
            timeZone: 'local',
            locale: 'nl',
            eventSources: [
                {
                    url: 'PromoCalendar/GetAllEvents',
                    extraParams: function () {
                        return {
                            inputFilter: JSON.stringify(getFilter())
                        }
                    }

                }
            ]
        });
        calendar.render();

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        function getPromos() {
            calendar.refetchEvents();
        }

        $('#GetPromosButton').click(function (e) {
            e.preventDefault();
            getPromos();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getPromos();
            }
        });

    });
})();
