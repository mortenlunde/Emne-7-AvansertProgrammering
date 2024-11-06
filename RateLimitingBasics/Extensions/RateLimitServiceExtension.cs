using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace RateLimitingBasics.Extensions;

public static class RateLimitServiceExtension
{
    public static void AddMyRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddFixedWindowLimiter("fixed", o =>
            {
                o.Window = TimeSpan.FromSeconds(10);
                o.PermitLimit = 2;
                o.QueueLimit = 2;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });
            options.AddSlidingWindowLimiter("sliding", o =>
            {
                o.Window = TimeSpan.FromSeconds(10);
                o.SegmentsPerWindow = 2;
                o.PermitLimit = 2;
                o.QueueLimit = 2;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });
            options.AddTokenBucketLimiter("token_bucket", o =>
            {
                o.TokenLimit = 10;
                o.ReplenishmentPeriod = TimeSpan.FromMinutes(1);
                o.TokensPerPeriod = 10;
            });
            options.AddConcurrencyLimiter("concurrency", o =>
            {
                o.PermitLimit = 5;
            });
        });
    }
}