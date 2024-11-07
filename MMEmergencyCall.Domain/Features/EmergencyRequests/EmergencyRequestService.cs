using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MMEmergencyCall.Domain.Features.EmergencyRequests;

public class EmergencyRequestService
{
    private readonly ILogger<EmergencyRequestService> _logger;
    private readonly AppDbContext _db;

    public EmergencyRequestService(ILogger<EmergencyRequestService> logger, AppDbContext context)
    {
        _logger = logger;
        _db = context;
    }

    public async Task<EmergencyRequestListResponseModel> GetEmergencyRequests()
    {
        try
        {
            var emergencyRequests = await _db.EmergencyRequests.AsNoTracking().ToListAsync();
            var response = new EmergencyRequestListResponseModel(
                Result<List<EmergencyRequest>>
                    .Success(emergencyRequests));

            return response;
        }
        catch (Exception ex)
        {
            string message = "An error occurred while getting the emergency requests: " + ex.Message;
            _logger.LogError(message);
            return new EmergencyRequestListResponseModel(
                Result<List<EmergencyRequest>>.Failure(message));
        }
    }

    public async Task<EmergencyRequestResponseModel> GetEmergencyRequestById(int id)
    {
        try
        {
            var emergencyRequest =
                await _db.EmergencyRequests.AsNoTracking().FirstOrDefaultAsync(x => x.RequestId == id);

            if (emergencyRequest is null)
            {
                return new EmergencyRequestResponseModel(
                    Result<EmergencyRequest>
                        .Failure("Emergency Request with Id " + id + " not found."));
            }

            var response = new EmergencyRequestResponseModel(
                Result<EmergencyRequest>
                    .Success(emergencyRequest));
            return response;
        }
        catch (Exception ex)
        {
            string message = "An error occurred while getting the emergency request with id " + id + " : " + ex.Message;
            _logger.LogError(message);
            return new EmergencyRequestResponseModel(
                Result<EmergencyRequest>.Failure(message));
        }
    }

    public async Task<EmergencyRequestResponseModel> AddEmergencyRequest(EmergencyRequestRequestModel request)
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

            return new EmergencyRequestResponseModel(
                Result<EmergencyRequest>.Success(emergencyRequest));
        }
        catch (Exception ex)
        {
            string message = "An error occurred while adding the emergency request : " + ex.Message;
            _logger.LogError(message);
            return new EmergencyRequestResponseModel(
                Result<EmergencyRequest>.Failure(message));
        }
    }

    public async Task<EmergencyRequestResponseModel> UpdateEmergencyRequest(int id,
        EmergencyRequestRequestModel request)
    {
        try
        {
            var existingEmergencyRequest =
                await _db.EmergencyRequests.AsNoTracking().FirstOrDefaultAsync(x => x.RequestId == id);

            if (existingEmergencyRequest is null)
            {
                return new EmergencyRequestResponseModel(
                    Result<EmergencyRequest>
                        .Failure("Emergency Request with Id " + id + " not found."));
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

            return new EmergencyRequestResponseModel(
                Result<EmergencyRequest>.Success(emergencyRequest));
        }
        catch (Exception ex)
        {
            string message = "An error occurred while updating the emergency request with id " + id + " : " +
                             ex.Message;
            _logger.LogError(message);
            return new EmergencyRequestResponseModel(
                Result<EmergencyRequest>.Failure(message));
        }
    }
}