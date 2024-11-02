using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MMEmergencyCall.Databases.AppDbContextModels;
using MMEmergencyCall.Domain.Features.Register;

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

    public async Task<ServiceProviderListResponseModel> GetServiceProviders()
    {
        try
        {
            var serviceProviders = await _db.ServiceProviders.AsNoTracking().ToListAsync();
            var response = new ServiceProviderListResponseModel(
                           Result<List<Databases.AppDbContextModels.ServiceProvider>>
                           .Success(serviceProviders));

            return response;
        }
        catch (Exception ex)
        {
            string message = "An error occurred while getting the service providers: " + ex.Message;
            _logger.LogError(message);
            return new ServiceProviderListResponseModel(
                Result<List<Databases.AppDbContextModels.ServiceProvider>>.Failure(message));
        }
    }

    public async Task<ServiceProviderResponseModel> GetServiceProviderById(int id)
    {
        try
        {
            var serviceProvider = await _db.ServiceProviders.AsNoTracking().FirstOrDefaultAsync(x => x.ProviderId == id);

            if(serviceProvider is null)
            {
                return new ServiceProviderResponseModel(
                           Result<Databases.AppDbContextModels.ServiceProvider>
                           .Success(serviceProvider,"Service Provider with Id "+ id + " not found." ));
            }

            var response = new ServiceProviderResponseModel(
                           Result<Databases.AppDbContextModels.ServiceProvider>
                           .Success(serviceProvider));
            return response;
        }
        catch (Exception ex)
        {

            string message = "An error occurred while getting the service provider by id: " + ex.Message;
            _logger.LogError(message);
            return new ServiceProviderResponseModel(
                Result<Databases.AppDbContextModels.ServiceProvider>.Failure(message));

        }
    }

    public async Task<ServiceProviderResponseModel> AddServiceProvider(ServiceProviderRequestModel request)
    {
        try
        {
            var serviceProvider = new Databases.AppDbContextModels.ServiceProvider()
            {
                ProviderName = request.ProviderName,
                ServiceId = request.ServiceId,
                ContactNumber = request.ContactNumber,
                Availability = request.Availability,
                TownshipCode = request.TownshipCode,
            };

            _db.ServiceProviders.Add(serviceProvider);
            await _db.SaveChangesAsync();

            return new ServiceProviderResponseModel(
                Result<Databases.AppDbContextModels.ServiceProvider>.Success(serviceProvider));
        }
        catch (Exception ex)
        {
            string message = "An error occurred while adding the service providers: " + ex.Message;
            _logger.LogError(message);
            return new ServiceProviderResponseModel(
                Result<Databases.AppDbContextModels.ServiceProvider>.Failure(message));
        }
    }

    public async Task<ServiceProviderResponseModel> UpdateServiceProvider(int id,ServiceProviderRequestModel request)
    {
        try
        {
            var existingServiceProvider = await _db.ServiceProviders.AsNoTracking().FirstOrDefaultAsync(x => x.ProviderId == id);
            
            if (existingServiceProvider == null) {
                return new ServiceProviderResponseModel(
                          Result<Databases.AppDbContextModels.ServiceProvider>
                          .Success(existingServiceProvider, "Service Provider with Id " + id + " not found."));
            }

            var serviceProvider = new Databases.AppDbContextModels.ServiceProvider()
            {
                ProviderId = id,
                ProviderName = request.ProviderName,
                ServiceId = request.ServiceId,
                ContactNumber = request.ContactNumber,
                Availability = request.Availability,
                TownshipCode = request.TownshipCode,
            };

            _db.ServiceProviders.Update(serviceProvider);
            await _db.SaveChangesAsync();

            return new ServiceProviderResponseModel(
                           Result<Databases.AppDbContextModels.ServiceProvider>
                           .Success(serviceProvider));
        }
        catch (Exception ex)
        {
            string message = "An error occurred while updating the service providers: " + ex.Message;
            _logger.LogError(message);
            return new ServiceProviderResponseModel(
                Result<Databases.AppDbContextModels.ServiceProvider>.Failure(message));
        }
    }
}
