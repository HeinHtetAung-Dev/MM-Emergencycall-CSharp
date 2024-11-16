using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MMEmergencyCall.Databases.AppDbContextModels;
using MMEmergencyCall.Domain.Client.Features.EmergencyServices;

namespace MMEmergencyCall.Domain.Client.Features.EmergencyRequests;

public class EmergencyRequestService
{
    private readonly ILogger<EmergencyRequestService> _logger;
    private readonly AppDbContext _db;

    public EmergencyRequestService(ILogger<EmergencyRequestService> logger, AppDbContext context)
    {
        _logger = logger;
        _db = context;
    }

    public async Task<Result<EmergencyRequestPaginationResponseModel>> GetEmergencyRequests(int pageNo, int pageSize,
     string? userId = null, string? serviceId = null, string? providerId = null,
     string? status = null, string? townshipCode = null)
    {
        try
        {
            var query = _db.EmergencyRequests.AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(x => x.UserId.ToString() == userId);
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
                    return Result<EmergencyRequestPaginationResponseModel>.ValidationError(
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

            var responseData = emergencyRequests.Select(x => new EmergencyRequestResponseModel
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

            var model = new EmergencyRequestPaginationResponseModel
            {
                PageNo = pageNo,
                PageSize = pageSize,
                PageCount = pageCount,
                Data = responseData
            };

            return Result<EmergencyRequestPaginationResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            string message = "An error occurred while getting the emergency requests: " + ex.Message;
            _logger.LogError(message);
            return Result<EmergencyRequestPaginationResponseModel>.Failure(message);
        }
    }


    public async Task<Result<EmergencyRequestResponseModel>> GetEmergencyRequestById(int id)
    {
        try
        {
            var emergencyRequest = await _db.EmergencyRequests.FirstOrDefaultAsync(x => x.RequestId == id);

            if (emergencyRequest is null)
            {
                return Result<EmergencyRequestResponseModel>
                        .NotFoundError("Emergency Request with Id " + id + " not found.");
            }

            var model = new EmergencyRequestResponseModel()
            {
                RequestId = emergencyRequest.RequestId,
                UserId = emergencyRequest.UserId,
                ServiceId = emergencyRequest.ServiceId,
                ProviderId = emergencyRequest.ProviderId,
                RequestTime = emergencyRequest.RequestTime,
                Status = emergencyRequest.Status,
                ResponseTime = emergencyRequest.ResponseTime,
                Notes = emergencyRequest.Notes,
                TownshipCode = emergencyRequest.TownshipCode
            };

            return Result<EmergencyRequestResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            string message = "An error occurred while getting the emergency request with id " + id + " : " + ex.Message;
            _logger.LogError(message);
            return Result<EmergencyRequestResponseModel>.Failure(message);
        }
    }

    public async Task<Result<EmergencyRequestResponseModel>> AddEmergencyRequest(EmergencyRequestRequestModel request)
    {
        try
        {
            var validateResponse = await ValidateEmergencyRequestRequestModel(request);

            if (validateResponse is not null)
            {
                return validateResponse;
            }

            var emergencyRequest = new EmergencyRequest()
            {
                UserId = request.UserId,
                ServiceId = request.ServiceId,
                ProviderId = request.ProviderId,
                RequestTime = request.RequestTime,
                Status = nameof(EnumEmergencyRequestStatus.Open),
                ResponseTime = request.ResponseTime,
                Notes = request.Notes,
                TownshipCode = request.TownshipCode
            };

            _db.EmergencyRequests.Add(emergencyRequest);
            await _db.SaveChangesAsync();

            var model = new EmergencyRequestResponseModel()
            {
                RequestId = emergencyRequest.RequestId,
                UserId = emergencyRequest.UserId,
                ServiceId = emergencyRequest.ServiceId,
                ProviderId = emergencyRequest.ProviderId,
                RequestTime = emergencyRequest.RequestTime,
                Status = emergencyRequest.Status,
                ResponseTime = emergencyRequest.ResponseTime,
                Notes = emergencyRequest.Notes,
                TownshipCode = emergencyRequest.TownshipCode
            };

            return Result<EmergencyRequestResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            string message = "An error occurred while adding the emergency request : " + ex.Message;
            _logger.LogError(message);
            return Result<EmergencyRequestResponseModel>.Failure(message);
        }
    }

    #region unused update service
    //unused service
    public async Task<Result<EmergencyRequestResponseModel>> UpdateEmergencyRequest(int id,
        EmergencyRequestRequestModel request)
    {
        try
        {
            var existingEmergencyRequest =
                await _db.EmergencyRequests.AsNoTracking().FirstOrDefaultAsync(x => x.RequestId == id);

            if (existingEmergencyRequest is null)
            {
                return Result<EmergencyRequestResponseModel>
                      .NotFoundError("Emergency Request with Id " + id + " not found.");
            }

            var emergencyRequest = new EmergencyRequest()
            {
                RequestId = id,
                UserId = request.UserId,
                ServiceId = request.ServiceId,
                ProviderId = request.ProviderId,
                RequestTime = request.RequestTime,
                //Status = request.Status,
                ResponseTime = request.ResponseTime,
                Notes = request.Notes,
                TownshipCode = request.TownshipCode
            };

            _db.Entry(emergencyRequest).State = EntityState.Modified;
            //_db.EmergencyRequests.Update(emergencyRequest);
            await _db.SaveChangesAsync();

            var response = new EmergencyRequestResponseModel()
            {
                RequestId = emergencyRequest.RequestId,
                UserId = emergencyRequest.UserId,
                ServiceId = emergencyRequest.ServiceId,
                ProviderId = emergencyRequest.ProviderId,
                RequestTime = emergencyRequest.RequestTime,
                Status = emergencyRequest.Status,
                ResponseTime = emergencyRequest.ResponseTime,
                Notes = emergencyRequest.Notes,
                TownshipCode = emergencyRequest.TownshipCode
            };

            return Result<EmergencyRequestResponseModel>.Success(response);
        }
        catch (Exception ex)
        {
            string message = "An error occurred while updating the emergency request with id " + id + " : " +
                             ex.Message;
            _logger.LogError(message);
            return Result<EmergencyRequestResponseModel>.Failure(message);
        }
    }

    #endregion

    public async Task<Result<EmergencyRequestResponseModel>> UpdateEmergencyRequestStatus(int id, UpdateEmergencyRequestStatusRequest statusRequest)
    {
        try
        {
            if (!Enum.IsDefined(typeof(EnumEmergencyRequestStatus), statusRequest.Status))
            {
                return Result<EmergencyRequestResponseModel>.ValidationError(
                    "Invalid Emergency Request Status. Status should be Cancel, Open or Closed"
                );
            }

            var existingEmergencyRequest =
                await _db.EmergencyRequests.FirstOrDefaultAsync(x => x.RequestId == id);

            if (existingEmergencyRequest is null)
            {
                return Result<EmergencyRequestResponseModel>
                      .NotFoundError("Emergency Request with Id " + id + " not found.");
            }

            existingEmergencyRequest.Status = statusRequest.Status;
            _db.Entry(existingEmergencyRequest).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            var model = new EmergencyRequestResponseModel()
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

            return Result<EmergencyRequestResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            string message = "An error occurred while updating the status of emergency request with id " + id + " : " +
                             ex.Message;
            _logger.LogError(message);
            return Result<EmergencyRequestResponseModel>.Failure(message);
        }
    }

    private async Task<Result<EmergencyRequestResponseModel>> ValidateEmergencyRequestRequestModel
        (EmergencyRequestRequestModel? request)
    {
        if (request is null)
        {
            return Result<EmergencyRequestResponseModel>
                .ValidationError("Request model cannot be null.");
        }

        if (request.ProviderId < 1)
        {
            return Result<EmergencyRequestResponseModel>
                .ValidationError("Invalid Provider Id.");
        }

        if (!await IsUserIdExist(request.UserId))
        {
            return Result<EmergencyRequestResponseModel>
                .ValidationError("Invalid User Id.");
        }

        if (!await IsServiceIdExist(request.ServiceId))
        {
            return Result<EmergencyRequestResponseModel>
                .ValidationError("Invalid Service Id.");
        }

        if (!await IsTownshipCodeExist(request.TownshipCode))
        {
            return Result<EmergencyRequestResponseModel>
                .ValidationError("Invalid Township Code.");
        }

        return null;
    }

    private async Task<bool> IsUserIdExist(int userId)
    {
        var isUserIdExist = await _db.Users.AnyAsync(x => x.UserId == userId);
        return isUserIdExist;
    }

    //private async Task<bool> IsProviderIdExist(int providerId)
    //{
    //    var isProviderIdExist = await _db.
    //}

    private async Task<bool> IsServiceIdExist(int serviceId)
    {
        var isServiceIdExist = await _db.EmergencyServices.AnyAsync(x => x.ServiceId == serviceId);
        return isServiceIdExist;
    }

    private async Task<bool> IsTownshipCodeExist(string townshipCode)
    {
        var isTownshipCodeExist = await _db.Townships.AnyAsync(x => x.TownshipCode == townshipCode);
        return isTownshipCodeExist;
    }
}