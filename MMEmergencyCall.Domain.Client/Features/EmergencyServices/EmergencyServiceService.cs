﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MMEmergencyCall.Databases.AppDbContextModels;

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

    public async Task<Result<EmergencyServiceResponseModel>> GetEmergencyServiceById(int serviceId)
    {
        try
        {
            var emergencyService = await _db.EmergencyServices.FirstOrDefaultAsync(x =>
                x.ServiceId == serviceId
            );
            if (emergencyService == null)
            {
                return Result<EmergencyServiceResponseModel>.Failure(
                    "Emergency Service with Id: " + serviceId + " not found."
                );
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
            string message =
                "An error occurred while getting emergency service by ID for id "
                + serviceId
                + " : "
                + ex.ToString();
            _logger.LogError(message);
            return Result<EmergencyServiceResponseModel>.Failure(message);
        }
    }

    public async Task<Result<List<EmergencyServiceResponseModel>>> GetEmergencyServiceByServiceType(
        string serviceType
    )
    {
        try
        {
            var emergencyServices = await _db
                .EmergencyServices.Where(x => x.ServiceType == serviceType)
                .ToListAsync();

            if (emergencyServices == null)
            {
                return Result<List<EmergencyServiceResponseModel>>.Failure(
                    "Emergency Service with service type: " + serviceType + " not found."
                );
            }

            var model = emergencyServices
                .Select(emergencyService => new EmergencyServiceResponseModel
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
                })
                .ToList();

            return Result<List<EmergencyServiceResponseModel>>.Success(model);
        }
        catch (Exception ex)
        {
            string message =
                "An error occurred while getting emergency service by service type: "
                + ex.ToString();
            _logger.LogError(message);
            return Result<List<EmergencyServiceResponseModel>>.Failure(message);
        }
    }

    public async Task<Result<EmergencyServiceResponseModel>> CreateEmergencyServiceAsync(
        EmergencyServiceRequestModel request
    )
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
            if (emergencyService == null)
            {
                return Result<EmergencyServiceResponseModel>.Failure(
                    "Emergency Service with id " + id + " not found."
                );
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
            var result = await _db.SaveChangesAsync();

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
        var emergencyService = await _db.Set<EmergencyService>().FindAsync(id);
        if (emergencyService == null)
            return Result<bool>.Failure("Emergency Service not found.");

        _db.Remove(emergencyService);
        await _db.SaveChangesAsync();
        return Result<bool>.Success(true, "Emergency Service deleted successfully.");
    }

    public async Task<Result<List<EmergencyService>>> GetAllEmergencyService()
    {
        var emergencyService = await _db.Set<EmergencyService>().ToListAsync();

        var model = emergencyService
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

        return Result<List<EmergencyService>>.Success(emergencyService);
    }

    public async Task<
        Result<EmergencyServicePaginationResponseModel>
    > GetAllEmergencyServiceWithPagination(int pageNo, int pageSize)
    {
        int rowCount = _db.EmergencyServices.Count();

        int pageCount = rowCount / pageSize;
        if (rowCount % pageSize > 0)
            pageCount++;

        if (pageNo < 1)
        {
            return Result<EmergencyServicePaginationResponseModel>.Failure("Invalid PageNo.");
        }

        if (pageNo > pageCount)
        {
            return Result<EmergencyServicePaginationResponseModel>.Failure("Invalid PageNo.");
        }

        var emergencyService = await _db
            .EmergencyServices.Skip((pageNo - 1) * pageSize)
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
}
