namespace RequestProgress
{
    public class TrackedRequest
    {
        public int LoadedBytes { get; set; }

        public int TotalBytes { get; set; }

        public int GetPercentage()
        {
            return TotalBytes > 0 ? (LoadedBytes * 100) / TotalBytes : -1;
        }
    }
}
