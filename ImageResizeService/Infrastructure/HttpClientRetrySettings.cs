namespace ImageResizeService.Infrastructure
{
    public class HttpClientRetrySettings
    {
        public int MaxRetryAttempts { get; set; }
        public int TimeOutInMilliseconds { get; set; }
    }
}