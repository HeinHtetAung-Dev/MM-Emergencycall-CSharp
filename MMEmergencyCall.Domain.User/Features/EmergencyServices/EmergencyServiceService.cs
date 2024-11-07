using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MMEmergencyCall.Databases.AppDbContextModels;

namespace MMEmergencyCall.Domain.User.Features.EmergencyServices;

public class EmergencyServiceService
{
    private readonly ILogger<EmergencyServiceService> _logger;

    private readonly AppDbContext _db;

    public EmergencyServiceService(ILogger<EmergencyServiceService> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<EmergencyServiceResponseModel> GetEmergencyServiceById(int serviceId)
    {
        try
        {
            var emergencyService = await _db
                .EmergencyServices.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ServiceId == serviceId);
            if (emergencyService == null)
            {
                return new EmergencyServiceResponseModel(
                    Result<EmergencyService>.Failure(
                        "Emergency Service with Id: " + serviceId + " not found."
                    )
                );
            }

            var response = new EmergencyServiceResponseModel(
                Result<EmergencyService>.Success(emergencyService)
            );
            return response;
        }
        catch (Exception ex)
        {
            string message =
                "An error occurred while getting emergency service by ID: " + ex.ToString();
            _logger.LogError(message);
            return new EmergencyServiceResponseModel(Result<EmergencyService>.Failure(message));
        }
    }

    public async Task<EmergencyServiceListResponseModel> GetEmergencyServiceByServiceType(
        string serviceType
    )
    {
        try
        {
            var lst = await _db
                .EmergencyServices.AsNoTracking()
                .Where(x => x.ServiceType == serviceType)
                .ToListAsync();
            if (lst == null)
            {
                return new EmergencyServiceListResponseModel(
                    Result<List<EmergencyService>>.Failure(
                        "Emergency Service with service type: " + serviceType + " not found."
                    )
                );
            }

            var response = new EmergencyServiceListResponseModel(
                Result<List<EmergencyService>>.Success(lst)
            );
            return response;
        }
        catch (Exception ex)
        {
            string message =
                "An error occurred while getting emergency service by service type: "
                + ex.ToString();
            _logger.LogError(message);
            return new EmergencyServiceListResponseModel(
                Result<List<EmergencyService>>.Failure(message)
            );
        }
    }

    public async Task<EmergencyServiceResponseModel> CreateEmergencyServiceAsync(
        EmergencyServiceRequestModel request
    )
    {
        try
        {
            var emergencyService = new EmergencyService
            {
                ServiceType = request.ServiceType,
                ServiceName = request.ServiceName,
                PhoneNumber = request.PhoneNumber,
                Location = request.Location,
                Availability = request.Availability,
                TownshipCode = request.TownshipCode
            };

            _db.EmergencyServices.Add(emergencyService);
            await _db.SaveChangesAsync();
            return new EmergencyServiceResponseModel(
                Result<EmergencyService>.Success(emergencyService)
            );
        }
        catch (Exception ex)
        {
            string message = "An error occurred while creating emergency service: " + ex.ToString();
            _logger.LogError(message);
            return new EmergencyServiceResponseModel(Result<EmergencyService>.Failure(message));
        }
    }
}
