namespace RMS.SBJ.HandlingBatch.Helpers
{
    public static class HandlingBatchStatusCodeHelper
    {
        public const string Pending = "100";
        public const string InProgress = "200";
        public const string ProcessedPartiallyWithFailedOrders = "210";
        public const string ProcessedPartiallyWithBlockedLines = "220";
        public const string ProcessedPartiallyWithFailedOrdersAndBlockedLines = "230";
        public const string UnprocessedBecauseOfFailedOrders = "240";
        public const string UnprocessedBecauseOfBlockedLines = "250";
        public const string UnprocessedBecauseOfFailedOrdersAndBlockedLines = "260";
        public const string Finished = "300";
        public const string Cancelled = "900";
    }
}
