using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MMEmergencyCall.Databases.AppDbContextModels;
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
        GetEmergencyServices(string serviceType,int pageNo, int pageSize)
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

    //public async Task<Result<EmergencyServicePaginationResponseModel>> GetServiceByServiceTypeWithPagination(string serviceType, int pageNo, int pageSize)
    //{
    //    var emergencyServicesQuery = _db.EmergencyServices.Where(x => x.ServiceType == serviceType);
    //    int rowCount = await emergencyServicesQuery.CountAsync();

    //    if (rowCount == 0)
    //    {
    //        return Result<EmergencyServicePaginationResponseModel>
    //            .NotFoundError("Emergency Service with service type : " + serviceType + " not found.");
    //    }

    //    int pageCount = (int)Math.Ceiling((double)rowCount / pageSize);

    //    if (pageNo < 1 || pageNo > pageCount)
    //    {
    //        return Result<EmergencyServicePaginationResponseModel>.ValidationError("Invalid PageNo.");
    //    }

    //    var emergencyServices = await emergencyServicesQuery
    //        .Skip((pageNo - 1) * pageSize)
    //        .Take(pageSize)
    //        .ToListAsync();

    //    var responseList = emergencyServices.Select(sr => new EmergencyServiceResponseModel
    //    {
    //        ServiceId = sr.ServiceId,
    //        ServiceType = sr.ServiceType,
    //        ServiceGroup = sr.ServiceGroup,
    //        ServiceName = sr.ServiceName,
    //        PhoneNumber = sr.PhoneNumber,
    //        Location = sr.Location,
    //        Availability = sr.Availability,
    //        TownshipCode = sr.TownshipCode,
    //        ServiceStatus = sr.ServiceStatus
    //    }).ToList();

    //    var model = new EmergencyServicePaginationResponseModel
    //    {
    //        Data = responseList,
    //        PageSize = pageSize,
    //        PageNo = pageNo,
    //        PageCount = pageCount
    //    };

    //    return Result<EmergencyServicePaginationResponseModel>.Success(model);
    //}

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
}
