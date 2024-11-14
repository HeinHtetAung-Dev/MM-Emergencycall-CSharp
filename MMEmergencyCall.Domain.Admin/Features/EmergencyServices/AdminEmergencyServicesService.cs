﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MMEmergencyCall.Shared;

namespace MMEmergencyCall.Domain.Admin.Features.EmergencyServices;

public class AdminEmergencyServicesService
{
    private readonly ILogger<AdminEmergencyServicesService> _logger;

    private readonly AppDbContext _db;

    public AdminEmergencyServicesService(
        ILogger<AdminEmergencyServicesService> logger,
        AppDbContext db
    )
    {
        _logger = logger;
        _db = db;
    }

    public async Task<
        Result<List<AdminEmergencyServicesResponseModel>>
    > GetEmergencyServicesByStatus(string status)
    {
        try
        {
            var emergencyServices = await _db
                .EmergencyServices.Where(x => x.ServiceStatus == status)
                .ToListAsync();

            if (emergencyServices == null)
            {
                return Result<List<AdminEmergencyServicesResponseModel>>.Failure(
                    "Emergency Service with service status: " + status + " not found."
                );
            }

            var model = emergencyServices
                .Select(emergencyService => new AdminEmergencyServicesResponseModel
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
                    ServiceStatus = emergencyService.ServiceStatus,
                })
                .ToList();

            return Result<List<AdminEmergencyServicesResponseModel>>.Success(model);
        }
        catch (Exception ex)
        {
            string message =
                "An error occurred while getting emergency service by service status: "
                + ex.ToString();
            _logger.LogError(message);

            return Result<List<AdminEmergencyServicesResponseModel>>.Failure(message);
        }
    }

    public async Task<
        Result<AdminEmergencyServicePaginationResponseModel>
    > GetEmergencyServicesByStatusPaginationAsync(string status, int pageNo, int pageSize)
    {
        if (pageNo < 1 || pageSize < 1)
        {
            return Result<AdminEmergencyServicePaginationResponseModel>.Failure(
                "Invalid page number or page size"
            );
        }

        try
        {
            var servicesByStatus = await _db
                .EmergencyServices.Where(x => x.ServiceStatus == status)
                .ToListAsync();

            int rowCount = servicesByStatus.Count();

            int pageCount = rowCount / pageSize;

            if (pageNo > pageCount)
            {
                return Result<AdminEmergencyServicePaginationResponseModel>.Failure(
                    "Invalid page number"
                );
            }

            var lst = await _db
                .EmergencyServices.Where(x => x.ServiceStatus == status)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var data = lst.Select(x => new AdminEmergencyServicesResponseModel
                {
                    ServiceId = x.ServiceId,
                    UserId = x.UserId,
                    ServiceGroup = x.ServiceGroup,
                    ServiceType = x.ServiceType,
                    ServiceName = x.ServiceName,
                    PhoneNumber = x.PhoneNumber,
                    Location = x.Location,
                    Availability = x.Availability,
                    TownshipCode = x.TownshipCode,
                    ServiceStatus = x.ServiceStatus
                })
                .ToList();

            var model = new AdminEmergencyServicePaginationResponseModel()
            {
                Data = data,
                PageNo = pageNo,
                PageSize = pageSize,
                PageCount = pageCount
            };

            return Result<AdminEmergencyServicePaginationResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return Result<AdminEmergencyServicePaginationResponseModel>.Failure(ex.ToString());
        }
    }
}
