namespace RMS.SBJ.HandlingBatch.Helpers
{
    public static class HandlingBatchLineStatusCodeHelper
    {
        public const string Pending = "100";
        public const string InProgress = "200";
        public const string Finished = "300";
        public const string Failed = "600";
        public const string Blocked = "700";
        public const string Cancelled = "900";
    }
}
