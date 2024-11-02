using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MMEmergencyCall.Databases.AppDbContextModels;

namespace MMEmergencyCall.Domain.Features.EmergencyServices;

public class EmergencyServiceService
{
    private readonly ILogger<EmergencyServiceService> _logger;

    private readonly AppDbContext _db;

    public EmergencyServiceService(ILogger<EmergencyServiceService> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<EmergencyServiceReponseModel> CreateEmergencyServiceAsync(
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
            return new EmergencyServiceReponseModel(
                Result<EmergencyService>.Success(emergencyService)
            );
        }
        catch (Exception ex)
        {
            string message = "An error occurred while creating emergency service: " + ex.ToString();
            _logger.LogError(message);
            return new EmergencyServiceReponseModel(Result<EmergencyService>.Failure(message));
        }
    }
}
