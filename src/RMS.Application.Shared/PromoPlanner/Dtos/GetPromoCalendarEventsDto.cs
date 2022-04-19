using System;

namespace RMS.PromoPlanner.Dtos
{
    public class GetPromoCalendarEventsDto
    {
        public Int64 id { get; set; }
        public String title { get; set; }
        public String start { get; set; }
        public String end { get; set; }
        public bool allDay { get; set; }
        public string url { get; set; }
        public string backgroundColor { get; set; }
    }
}
