namespace NYC360.Infrastructure.RSS;

public class RssIngestionSettings
{
    public int IntervalMinutes { get; set; } = 10;
    public int TimeoutSeconds { get; set; } = 10;
    public int RetryCount { get; set; } = 2;
    public int RetryBackoffMilliseconds { get; set; } = 500;
}
