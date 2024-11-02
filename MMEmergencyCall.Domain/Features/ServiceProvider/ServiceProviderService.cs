using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MMEmergencyCall.Domain.Features.ServiceProvider;

public class ServiceProviderService
{
    private readonly ILogger<ServiceProviderService> _logger;
    private readonly AppDbContext _db;

    public ServiceProviderService(ILogger<ServiceProviderService> logger, AppDbContext context)
    {
        _logger = logger;
        _db = context;
    }

    public async Task<ServiceProviderResponseModel> GetServiceProviders()
    {
        try
        {
            var serviceProviders = await _db.ServiceProviders.AsNoTracking().ToListAsync();
            return new ServiceProviderResponseModel(
                Result<List<Databases.AppDbContextModels.ServiceProvider>>.Success(serviceProviders));
        }
        catch (Exception ex)
        {
            string message = "An error occurred while getting the service providers: " + ex.Message;
            _logger.LogError(message);
            return new ServiceProviderResponseModel(
                Result<List<Databases.AppDbContextModels.ServiceProvider>>.Failure(message));
        }
    }

}
