using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MMEmergencyCall.Domain.Admin.Features.EmergencyRequests;

public class AdminEmergencyRequestService
{
    private readonly ILogger<AdminEmergencyRequestService> _logger;
    private readonly AppDbContext _db;

    public AdminEmergencyRequestService(ILogger<AdminEmergencyRequestService> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<Result<AdminEmergencyRequestPaginationResponseModel>> GetEmergencyRequests(int pageNo, int pageSize,
    int? userId = null, string? serviceId = null, string? providerId = null,
    string? status = null, string? townshipCode = null)
    {
        try
        {
            var query = _db.EmergencyRequests.AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(x => x.UserId == userId.Value);
            }
            if (!string.IsNullOrEmpty(serviceId))
            {
                query = query.Where(x => x.ServiceId.ToString() == serviceId);
            }
            if (!string.IsNullOrEmpty(providerId))
            {
                query = query.Where(x => x.ProviderId.ToString() == providerId);
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (!Enum.IsDefined(typeof(EnumEmergencyRequestStatus), status))
                {
                    return Result<AdminEmergencyRequestPaginationResponseModel>.ValidationError(
                        "Invalid Emergency Request Status. Status should be Cancel, Open or Closed"
                    );
                }

                query = query.Where(x => x.Status == status);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x => x.Status == status);
            }
            if (!string.IsNullOrEmpty(townshipCode))
            {
                query = query.Where(x => x.TownshipCode == townshipCode);
            }

            int totalRecords = await query.CountAsync();

            int pageCount = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var emergencyRequests = await query
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var responseData = emergencyRequests.Select(x => new AdminEmergencyRequestResponseModel
            {
                RequestId = x.RequestId,
                UserId = x.UserId,
                ServiceId = x.ServiceId,
                ProviderId = x.ProviderId,
                RequestTime = x.RequestTime,
                Status = x.Status,
                ResponseTime = x.ResponseTime,
                Notes = x.Notes,
                TownshipCode = x.TownshipCode
            }).ToList();

            var model = new AdminEmergencyRequestPaginationResponseModel
            {
                PageNo = pageNo,
                PageSize = pageSize,
                PageCount = pageCount,
                Data = responseData
            };

            return Result<AdminEmergencyRequestPaginationResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            string message = "An error occurred while getting the emergency requests: " + ex.Message;
            _logger.LogError(message);
            return Result<AdminEmergencyRequestPaginationResponseModel>.Failure(message);
        }
    }

    public async Task<Result<AdminEmergencyRequestResponseModel>> UpdateEmergencyRequestStatus(int id, UpdateEmergencyRequestStatusRequest statusRequest)
    {
        try
        {
            if (!Enum.IsDefined(typeof(EnumEmergencyRequestStatus), statusRequest.Status))
            {
                return Result<AdminEmergencyRequestResponseModel>.ValidationError(
                    "Invalid Emergency Request Status. Status should be Cancel, Open or Closed"
                );
            }

            var existingEmergencyRequest = await _db.EmergencyRequests
                .FirstOrDefaultAsync(x => x.RequestId == id );

            if (existingEmergencyRequest is null)
            {
                return Result<AdminEmergencyRequestResponseModel>
                      .NotFoundError("Emergency Request with Id " + id + " not found.");
            }

            existingEmergencyRequest.Status = statusRequest.Status;
            _db.Entry(existingEmergencyRequest).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            var model = new AdminEmergencyRequestResponseModel()
            {
                RequestId = existingEmergencyRequest.RequestId,
                UserId = existingEmergencyRequest.UserId,
                ServiceId = existingEmergencyRequest.ServiceId,
                ProviderId = existingEmergencyRequest.ProviderId,
                RequestTime = existingEmergencyRequest.RequestTime,
                Status = existingEmergencyRequest.Status,
                ResponseTime = existingEmergencyRequest.ResponseTime,
                Notes = existingEmergencyRequest.Notes,
                TownshipCode = existingEmergencyRequest.TownshipCode
            };

            return Result<AdminEmergencyRequestResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            string message = "An error occurred while updating the status of emergency request with id " + id + " : " +
                             ex.Message;
            _logger.LogError(message);
            return Result<AdminEmergencyRequestResponseModel>.Failure(message);
        }
    }

}
