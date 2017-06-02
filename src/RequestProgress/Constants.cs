namespace RequestProgress
{
    internal class Constants
    {
        public const string ContentTypeHeaderName = "Content-Type";
        public const string ContentLengthHeaderName = "Content-Length";
        public const string DefaultRequestIdentifierUrlParameterName = "ProgressTrackerRequestId";
        public const string DefaultRetrieveProgressUrl = "GetRequestProgress";
        public const int DefaultChunkSize = 64;
        public const int DefaultCacheLifetimeMinutes = 1;
    }
}
