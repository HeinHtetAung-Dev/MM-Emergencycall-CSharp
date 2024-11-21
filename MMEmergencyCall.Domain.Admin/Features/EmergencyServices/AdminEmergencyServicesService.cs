using System;
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
    > GetEmergencyServicesByStatusAsync(int pageNo, int pageSize, string? status)
    {
        if (pageNo < 1 || pageSize < 1)
        {
            return Result<AdminEmergencyServicesPaginationResponseModel>.Failure(
                "Invalid page number or page size"
            );
        }

        var serviceStatus = EnumServiceStatus.None;

        try
        {
            if (!status.IsNullOrEmpty())
            {
                serviceStatus = Enum.Parse<EnumServiceStatus>(status, true);
            }

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
            if (rowCount % pageSize > 0)
            {
                pageCount++;
            }

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
        catch (ArgumentException ex)
        {
            _logger.LogError(ex.ToString());
            return Result<AdminEmergencyServicesPaginationResponseModel>.Failure(ex.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return Result<AdminEmergencyServicesPaginationResponseModel>.Failure(ex.ToString());
        }
    }

    public async Task<
        Result<AdminEmergencyServicesResponseModel>
    > UpdateEmergencyServiceStatusAsync(int id, int userId, string serviceStatus)
    {
        if (!Enum.IsDefined(typeof(EnumServiceStatus), serviceStatus))
        {
            return Result<AdminEmergencyServicesResponseModel>.ValidationError(
                "Invalid Emergency Service Status. Status should be Pending, Approved or Rejected"
            );
        }

        var item = await _db.EmergencyServices.FirstOrDefaultAsync(x => x.ServiceId == id);
        if (item is null)
        {
            return Result<AdminEmergencyServicesResponseModel>.NotFoundError(
                "This is no Emergency Service with Id: " + id
            );
        }

        //// Service should be updated by the created user
        //item = await _db.EmergencyServices.FirstOrDefaultAsync(x =>
        //    x.ServiceId == id && x.UserId == userId
        //);

        //if (item is null)
        //{
        //    return Result<AdminEmergencyServicesResponseModel>.NotFoundError(
        //        "The Emergency Service you are trying to update is created by other user "
        //    );
        //}

        item.ServiceStatus = serviceStatus;
        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        var model = new AdminEmergencyServicesResponseModel()
        {
            ServiceId = id,
            ServiceStatus = serviceStatus
        };

        return Result<AdminEmergencyServicesResponseModel>.Success(model);
    }
}
