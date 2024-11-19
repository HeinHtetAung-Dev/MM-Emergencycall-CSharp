﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
        Result<AdminEmergencyServicesPaginationResponseModel>
    > GetEmergencyServicesByStatusAsync(string status, int pageNo = 1, int pageSize = 10)
    {
        if (pageNo < 1 || pageSize < 1)
        {
            return Result<AdminEmergencyServicesPaginationResponseModel>.Failure(
                "Invalid page number or page size"
            );
        }

        var serviceStatus = EnumServiceStatus.None;

        if (!status.IsNullOrEmpty())
        {
            serviceStatus = Enum.Parse<EnumServiceStatus>(status, true);
        }

        try
        {
            var query = _db.EmergencyServices.AsQueryable();

            if (!serviceStatus.Equals(EnumServiceStatus.None))
            {
                query = query.Where(x => x.ServiceStatus == status);
            }

            var servicesByStatus = await query.ToListAsync();

            int rowCount = servicesByStatus.Count();

            if (rowCount is 0)
            {
                return Result<AdminEmergencyServicesPaginationResponseModel>.NotFoundError(
                    $"There is no data with the status: {status}"
                );
            }

            int pageCount = rowCount / pageSize;

            if (pageNo > pageCount)
            {
                return Result<AdminEmergencyServicesPaginationResponseModel>.Failure(
                    "Invalid page number"
                );
            }

            var lst = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

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

            var model = new AdminEmergencyServicesPaginationResponseModel()
            {
                Data = data,
                PageNo = pageNo,
                PageSize = pageSize,
                PageCount = pageCount
            };

            return Result<AdminEmergencyServicesPaginationResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return Result<AdminEmergencyServicesPaginationResponseModel>.Failure(ex.ToString());
        }
    }
}
