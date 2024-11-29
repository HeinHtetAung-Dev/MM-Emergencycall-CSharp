using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MMEmergencyCall.Databases.AppDbContextModels;
using Geolocation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MMEmergencyCall.Domain.Client.Features.EmergencyServices;

public class EmergencyServiceService
{
    private readonly ILogger<EmergencyServiceService> _logger;

    private readonly AppDbContext _db;

    public EmergencyServiceService(ILogger<EmergencyServiceService> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<Result<EmergencyServicePaginationResponseModel>>
        GetEmergencyServices(int pageNo, int pageSize, string? serviceType)
    {
        if (pageNo < 1)
        {
            return Result<EmergencyServicePaginationResponseModel>.ValidationError("Invalid PageNo.");
        }

        var query = _db.EmergencyServices.AsQueryable();

        if (!string.IsNullOrEmpty(serviceType))
        {
            query = query.Where(x => x.ServiceType == serviceType);
        }

        int totalRecords = await query.CountAsync();

        int pageCount = (int)Math.Ceiling(totalRecords / (double)pageSize);

        if (pageNo > pageCount)
        {
            return Result<EmergencyServicePaginationResponseModel>.ValidationError("Invalid PageNo.");
        }

        query = query.Where(x => x.ServiceStatus == EnumServiceStatus.Approved.ToString());

        var emergencyService = await query
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        var lst = emergencyService
            .Select(sr => new EmergencyServiceResponseModel
            {
                ServiceId = sr.ServiceId,
                UserId = sr.UserId,
                ServiceType = sr.ServiceType,
                ServiceGroup = sr.ServiceGroup,
                ServiceName = sr.ServiceName,
                PhoneNumber = sr.PhoneNumber,
                Location = sr.Location,
                Availability = sr.Availability,
                TownshipCode = sr.TownshipCode,
                ServiceStatus = sr.ServiceStatus
            })
            .ToList();

        EmergencyServicePaginationResponseModel model = new();
        model.Data = lst;
        model.PageSize = pageSize;
        model.PageNo = pageNo;
        model.PageCount = pageCount;

        return Result<EmergencyServicePaginationResponseModel>.Success(model);
    }

    public async Task<Result<EmergencyServiceResponseModel>> GetEmergencyServiceById(int serviceId)
    {
        try
        {
            var emergencyService = await _db.EmergencyServices.FirstOrDefaultAsync(x => x.ServiceId == serviceId);

            if (emergencyService is null)
            {
                return Result<EmergencyServiceResponseModel>
                    .NotFoundError("Emergency Service with Id: " + serviceId + " not found.");
            }

            var model = new EmergencyServiceResponseModel
            {
                ServiceId = emergencyService.ServiceId,
                UserId = emergencyService.UserId,
                ServiceGroup = emergencyService.ServiceGroup,
                ServiceType = emergencyService.ServiceType,
                ServiceName = emergencyService.ServiceName,
                PhoneNumber = emergencyService.PhoneNumber,
                Location = emergencyService.Location,
                Availability = emergencyService.Availability,
                TownshipCode = emergencyService.TownshipCode,
                ServiceStatus = emergencyService.ServiceStatus
            };

            return Result<EmergencyServiceResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            string message = "An error occurred while getting emergency service by ID for id "
                + serviceId + " : " + ex.ToString();
            _logger.LogError(message);
            return Result<EmergencyServiceResponseModel>.Failure(message);
        }
    }

    public async Task<Result<EmergencyServiceResponseModel>> CreateEmergencyServiceAsync(EmergencyServiceRequestModel request)
    {
        try
        {
            var emergencyService = new EmergencyService
            {
                ServiceType = request.ServiceType,
                ServiceGroup = request.ServiceGroup,
                ServiceName = request.ServiceName,
                PhoneNumber = request.PhoneNumber,
                Location = request.Location,
                Availability = request.Availability,
                TownshipCode = request.TownshipCode,
            };

            _db.EmergencyServices.Add(emergencyService);
            await _db.SaveChangesAsync();

            var model = new EmergencyServiceResponseModel
            {
                ServiceId = emergencyService.ServiceId,
                UserId = emergencyService.UserId,
                ServiceGroup = emergencyService.ServiceGroup,
                ServiceType = emergencyService.ServiceType,
                ServiceName = emergencyService.ServiceName,
                PhoneNumber = emergencyService.PhoneNumber,
                Location = emergencyService.Location,
                Availability = emergencyService.Availability,
                TownshipCode = emergencyService.TownshipCode,
                ServiceStatus = emergencyService.ServiceStatus
            };

            return Result<EmergencyServiceResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            string message = "An error occurred while creating emergency service: " + ex.ToString();
            _logger.LogError(message);
            return Result<EmergencyServiceResponseModel>.Failure(message);
        }
    }

    public async Task<Result<EmergencyServiceResponseModel>> UpdateEmergencyService(
        int id,
        EmergencyServiceRequestModel requestModel
    )
    {
        try
        {
            var emergencyService = await _db.Set<EmergencyService>().FindAsync(id);

            if (emergencyService is null)
            {
                return Result<EmergencyServiceResponseModel>
                    .NotFoundError("Emergency Service with id " + id + " not found.");
            }

            var status = emergencyService.ServiceStatus;
            if (status != "Pending")
            {
                return Result<EmergencyServiceResponseModel>
                    .ValidationError("You can edit only Services with Pending status.");
            }

            emergencyService.ServiceType = requestModel.ServiceType;
            emergencyService.ServiceGroup = requestModel.ServiceGroup;
            emergencyService.ServiceName = requestModel.ServiceName;
            emergencyService.PhoneNumber = requestModel.PhoneNumber;
            emergencyService.Location = requestModel.Location;
            emergencyService.Availability = requestModel.Availability;
            emergencyService.TownshipCode = requestModel.TownshipCode;
            emergencyService.ServiceStatus = requestModel.ServiceStatus;

            _db.Entry(emergencyService).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            var model = new EmergencyServiceResponseModel
            {
                ServiceId = emergencyService.ServiceId,
                UserId = emergencyService.UserId,
                ServiceType = emergencyService.ServiceType,
                ServiceGroup = emergencyService.ServiceGroup,
                ServiceName = emergencyService.ServiceName,
                PhoneNumber = emergencyService.PhoneNumber,
                Location = emergencyService.Location,
                Availability = emergencyService.Availability,
                TownshipCode = emergencyService.TownshipCode,
                ServiceStatus = emergencyService.ServiceStatus,
            };

            return Result<EmergencyServiceResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            string message =
                "An error occurred while updating the emergency service for id "
                + id
                + ": "
                + ex.Message;
            _logger.LogError(message);
            return Result<EmergencyServiceResponseModel>.Failure(message);
        }
    }

    public async Task<Result<bool>> DeleteEmergencyService(int id)
    {
        try
        {
            var emergencyService = await _db.EmergencyServices.FindAsync(id);

            if (emergencyService is null)
            {
                return Result<bool>.NotFoundError("Emergency Service not found.");
            }

            var status = emergencyService.ServiceStatus;
            if (status != "Pending")
            {
                return Result<bool>.Failure("You can delete only Services with Pending status.");
            }

            _db.Remove(emergencyService);
            await _db.SaveChangesAsync();

            return Result<bool>.Success(true, "Emergency Service deleted successfully.");
        }
        catch (Exception ex)
        {
            string message =
                "An error occurred while updating the emergency service for id "
                + id
                + ": "
                + ex.Message;
            _logger.LogError(message);
            return Result<bool>.Failure(message);
        }
    }

    public Double ToRadians(decimal? angle)
    {
        var ToRadians = 0.00;
        if (!String.IsNullOrEmpty(angle.ToString()))
        {
            ToRadians = Convert.ToDouble(angle) * Math.PI / 180.0;
        }
        return ToRadians;
    }

    public decimal CalculateDistance(decimal lat1, decimal lon1, decimal? lat2, decimal? lon2)
    {
        var distance = 0.00;
        var location1 = new Coordinate(52.2296756, 21.0122287);
        if (!String.IsNullOrEmpty(lat2.ToString()) && !String.IsNullOrEmpty(lon2.ToString()))
        {
            var R = 6371; // Radius of the Earth in mile
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            distance = R * c; // Distance in kilometers
        }

        return Convert.ToDecimal(distance);
    }

    public decimal CalculateDistanceByUsingLibrary(decimal lat1, decimal lon1, decimal? lat2, decimal? lon2)
    {
        //var distance = 0.00;
        var location1 = new Coordinate(Convert.ToDouble(lat1), Convert.ToDouble(lon1));
        var location2 = new Coordinate(Convert.ToDouble(lat2), Convert.ToDouble(lon2));
        var distance1 = GeoCalculator.GetDistance(location1, location2, 1); // 1 for kilometers
        
        return Convert.ToDecimal(distance1);
    }


    public async Task<Result<EmergencyServicesListWithDistance>> GetEmergencyServiceWithinDistanceAsync(string? townshipCode, string? emergencyType, decimal lat, decimal lng, decimal maxDistanceInKm, int pageNo, int PageSize)
    {

        var query = _db.EmergencyServices.AsQueryable();

        if (!string.IsNullOrEmpty(townshipCode))
        {
            query = query.Where(x => x.TownshipCode.ToUpper() == townshipCode.ToUpper() && x.ServiceStatus == EnumServiceStatus.Approved.ToString());
        }
        if (!string.IsNullOrEmpty(emergencyType))
        {
            query = query.Where(x => x.ServiceType.ToUpper() == emergencyType.ToUpper());
        }

        var emergencyService = await query.Where(x => x.ServiceStatus == EnumServiceStatus.Approved.ToString())
                .Skip((pageNo - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

        List<EmergencyServicesWithDistance> EmergencyServicesWithinDistance = new List<EmergencyServicesWithDistance>();
        if (!string.IsNullOrEmpty(lat.ToString()) && !string.IsNullOrEmpty(lng.ToString())
            && maxDistanceInKm > 0)

        {
            // Calculate distance and filter locations
            EmergencyServicesWithinDistance = emergencyService
               .Select(EmergencyServices => new EmergencyServicesWithDistance
               {
                   ServiceId = EmergencyServices.ServiceId,
                   UserId = EmergencyServices.UserId,
                   ServiceGroup = EmergencyServices.ServiceGroup,
                   ServiceType = EmergencyServices.ServiceType,
                   ServiceName = EmergencyServices.ServiceName,
                   PhoneNumber = EmergencyServices.PhoneNumber,
                   Location = EmergencyServices.Location,
                   Availability = EmergencyServices.Availability,
                   TownshipCode = EmergencyServices.TownshipCode,
                   ServiceStatus = EmergencyServices.ServiceStatus,
                   Ltd = EmergencyServices.Ltd,
                   Lng = EmergencyServices.Lng,
                   Distance = CalculateDistanceByUsingLibrary(lat, lng, EmergencyServices.Ltd, EmergencyServices.Lng)
               })
               .Where(location => location.Distance <= maxDistanceInKm)
               .OrderBy(location => location.Distance).ToList();
        }
        else
        {
            EmergencyServicesWithinDistance = emergencyService
           .Select(EmergencyServices => new EmergencyServicesWithDistance
           {
               ServiceId = EmergencyServices.ServiceId,
               UserId = EmergencyServices.UserId,
               ServiceGroup = EmergencyServices.ServiceGroup,
               ServiceType = EmergencyServices.ServiceType,
               ServiceName = EmergencyServices.ServiceName,
               PhoneNumber = EmergencyServices.PhoneNumber,
               Location = EmergencyServices.Location,
               Availability = EmergencyServices.Availability,
               TownshipCode = EmergencyServices.TownshipCode,
               ServiceStatus = EmergencyServices.ServiceStatus,
               Ltd = EmergencyServices.Ltd,
               Lng = EmergencyServices.Lng,
               Distance = 0
           })
       .ToList();
        }

        EmergencyServicesListWithDistance model = new();
        model.Data = EmergencyServicesWithinDistance;
        if (EmergencyServicesWithinDistance is null || EmergencyServicesWithinDistance.Count <= 0)
        {
            return Result<EmergencyServicesListWithDistance>
                    .NotFoundError("We don't have emergencyservice in this location.");
        }

        return Result<EmergencyServicesListWithDistance>.Success(model);

    }

}
