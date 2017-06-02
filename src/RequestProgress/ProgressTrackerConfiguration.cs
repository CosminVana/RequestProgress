using System;

namespace RequestProgress
{
    public class ProgressTrackerConfiguration
    {
        public ProgressTrackerConfiguration()
        {
            this.RequestIdentifierUrlParameterName = Constants.DefaultRequestIdentifierUrlParameterName;
            this.ChunkSize = Constants.DefaultChunkSize;
            this.CacheLifetime = TimeSpan.FromMinutes(Constants.DefaultCacheLifetimeMinutes);
            this.RetrieveProgressUrl = Constants.DefaultRetrieveProgressUrl;
        }

        /// <summary>
        /// The name of the parameter which will be used as unique identifier of the request
        /// </summary>
        public string RequestIdentifierUrlParameterName { get; set; }

        /// <summary>
        /// The time for which a specific request can still be retrieved by the id, after it was completed
        /// </summary>
        public TimeSpan CacheLifetime { get; set; }

        /// <summary>
        /// The number of bytes to be read in one chunk. Higher number means better performance, but lower progress rate.
        /// </summary>
        public int ChunkSize { get; set; }

        /// <summary>
        /// Relative url where the progress of a request can be checked. If this parameter is empty, you will have to create a method which returns the progress
        /// </summary>
        public string RetrieveProgressUrl { get; set; }
    }
}
