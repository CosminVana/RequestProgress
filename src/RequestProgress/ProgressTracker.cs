using System;
using System.Collections.Generic;
using System.Web;

namespace RequestProgress
{
    public class ProgressTracker
    {
        private readonly ProgressTrackerConfiguration progressTrackerConfiguration;

        private readonly IDictionary<string, TrackedRequest> trackedRequests;

        public ProgressTracker() : this(new ProgressTrackerConfiguration())
        {
        }

        public ProgressTracker(ProgressTrackerConfiguration progressTrackerConfiguration)
        {
            this.progressTrackerConfiguration = progressTrackerConfiguration;
            this.trackedRequests = new Dictionary<string, TrackedRequest>();
        }

        public void TrackRequest(HttpRequest request)
        {
            var requestIdentifier = request[progressTrackerConfiguration.RequestIdentifierUrlParameterName];
            if (requestIdentifier != null)
            {
                if (!string.IsNullOrEmpty(progressTrackerConfiguration.RetrieveProgressUrl))
                {
                    if (request.Url.Segments[request.Url.Segments.Length - 1] == progressTrackerConfiguration.RetrieveProgressUrl)
                    {
                        ReturnProgress(requestIdentifier, request);
                        return;
                    }
                }

                var contentLengthHeader = request.Headers[Constants.ContentLengthHeaderName];

                if (contentLengthHeader != null)
                {
                    if (!trackedRequests.ContainsKey(requestIdentifier))
                    {
                        var contentLength = int.Parse(contentLengthHeader);
                        var trackedRequest = new TrackedRequest() { LoadedBytes = 0, TotalBytes = contentLength };
                        trackedRequests.Add(requestIdentifier, trackedRequest);
                        TrackProgress(request, trackedRequest, requestIdentifier);
                    }
                }
            }
        }

        private void ReturnProgress(string requestIdentifier, HttpRequest request)
        {
            var progress = GetProgress(requestIdentifier);

            var response = request.RequestContext.HttpContext.Response;
            response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            response.Cache.SetValidUntilExpires(false);
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();

            response.Write(progress);
            response.Flush();
            response.End();
        }

        public int GetProgress(string requestId)
        {
            if (this.trackedRequests.ContainsKey(requestId))
            {
                var trackedRequest = this.trackedRequests[requestId];
                return trackedRequest.GetPercentage();
            }
            return -1;
        }

        private void TrackProgress(HttpRequest request, TrackedRequest trackedRequest, string requestIdentifier)
        {
            var requestStream = request.GetBufferedInputStream();

            byte[] buffer = new byte[progressTrackerConfiguration.ChunkSize];

            int readBytes = requestStream.Read(buffer, 0, progressTrackerConfiguration.ChunkSize);
            while (readBytes > 0)
            {
                trackedRequest.LoadedBytes += readBytes;
                readBytes = requestStream.Read(buffer, 0, progressTrackerConfiguration.ChunkSize);
            }
        }
    }
}
