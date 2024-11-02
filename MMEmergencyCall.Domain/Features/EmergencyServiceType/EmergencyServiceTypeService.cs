using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MMEmergencyCall.Domain.Features.EmergencyServiceType;

namespace MMEmergencyCall.Domain.Features.EmergencyService;

public class EmergencyServiceTypeService
{
    private readonly ILogger<EmergencyServiceTypeService> _logger;
    private readonly AppDbContext _db;

    public EmergencyServiceTypeService(
        ILogger<EmergencyServiceTypeService> logger,
        AppDbContext context
    )
    {
        _logger = logger;
        _db = context;
    }

    public EmergencyServiceTypeResponseModel GetServiceTypes()
    {
        try
        {
            List<string> lst = _db.EmergencyServices.Select(s => s.ServiceType).Distinct().ToList();

            return new EmergencyServiceTypeResponseModel(Result<List<string>>.Success(lst));
        }
        catch (Exception ex)
        {
            string message =
                "An error occurred while getting emergency service types." + ex.ToString();
            _logger.LogError(message);
            return new EmergencyServiceTypeResponseModel(Result<List<string>>.Failure(message));
        }
    }
}
