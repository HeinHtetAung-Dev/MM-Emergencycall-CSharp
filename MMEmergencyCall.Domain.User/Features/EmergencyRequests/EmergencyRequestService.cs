using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MMEmergencyCall.Databases.AppDbContextModels;

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

    public async Task<Result<List<EmergencyRequestResponseModel>>> GetEmergencyRequests()
    {
        try
        {
            var emergencyRequests = await _db.EmergencyRequests.AsNoTracking().ToListAsync();
            var response = emergencyRequests.Select(er => new EmergencyRequestResponseModel
            {
                RequestId = er.RequestId,
                UserId = er.UserId,
                ServiceId = er.ServiceId,
                ProviderId = er.ProviderId,
                RequestTime = er.RequestTime,
                Status = er.Status,
                ResponseTime = er.ResponseTime,
                Notes = er.Notes,
                TownshipCode = er.TownshipCode
            }).ToList();

            return Result<List<EmergencyRequestResponseModel>>.Success(response);
        }
        catch (Exception ex)
        {
            string message = "An error occurred while getting the emergency requests: " + ex.Message;
            _logger.LogError(message);
            return Result<List<EmergencyRequestResponseModel>>.Failure(message);
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
                        .Failure("Emergency Request with Id " + id + " not found.");
            }


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
            string message = "An error occurred while getting the emergency request with id " + id + " : " + ex.Message;
            _logger.LogError(message);
            return Result<EmergencyRequestResponseModel>.Failure(message);
        }
    }

    public async Task<Result<EmergencyRequestResponseModel>> AddEmergencyRequest(EmergencyRequestRequestModel request)
    {
        try
        {
            var emergencyRequest = new EmergencyRequest()
            {
                UserId = request.UserId,
                ServiceId = request.ServiceId,
                ProviderId = request.ProviderId,
                RequestTime = request.RequestTime,
                Status = request.Status,
                ResponseTime = request.ResponseTime,
                Notes = request.Notes,
                TownshipCode = request.TownshipCode
            };

            _db.EmergencyRequests.Add(emergencyRequest);
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
            string message = "An error occurred while adding the emergency request : " + ex.Message;
            _logger.LogError(message);
            return Result<EmergencyRequestResponseModel>.Failure(message);
        }
    }

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
                      .Failure("Emergency Request with Id " + id + " not found.");
            }


            var emergencyRequest = new EmergencyRequest()
            {
                RequestId = id,
                UserId = request.UserId,
                ServiceId = request.ServiceId,
                ProviderId = request.ProviderId,
                RequestTime = request.RequestTime,
                Status = request.Status,
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
}