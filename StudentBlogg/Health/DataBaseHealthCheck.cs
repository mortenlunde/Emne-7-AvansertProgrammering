using Microsoft.Extensions.Diagnostics.HealthChecks;
using StudentBlogg.Data;
using Exception = System.Exception;

namespace StudentBlogg.Health;

public class DataBaseHealthCheck : IHealthCheck
{
    private readonly StudentBloggDbContext _dbContext;

    public DataBaseHealthCheck(StudentBloggDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            if (await _dbContext.Database.CanConnectAsync(cancellationToken))
                return HealthCheckResult.Healthy();
            
            return HealthCheckResult.Unhealthy("Can't connect to database");
            
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy("Databaseconnection failed ..!", e);
        }
    }
}