using Microsoft.Extensions.Logging;

namespace MMEmergencyCall.Domain.Client.Features.SubmitEmergencyRequest;

public class SubmitEmergencyRequestService
{
	private readonly AppDbContext _db;
	private readonly ILogger<SubmitEmergencyRequestService> _logger;
	public SubmitEmergencyRequestService(ILogger<SubmitEmergencyRequestService> logger, AppDbContext db)
	{
		_db = db;
		_logger = logger;
	}

	public async Task<Result<SubmitEmergencyRequestResponseModel>> AddEmergencyRequest(SubmitEmergencyRequestRequestModel request, int currentUserId)
	{
		try
		{
			var validateRequestModelResponse = await ValidateEmergencyRequestRequestModel(request);

			if (validateRequestModelResponse is not null)
			{
				return validateRequestModelResponse;
			}

			if (!await IsUserIdExist(currentUserId))
			{
				return Result<SubmitEmergencyRequestResponseModel>
					.ValidationError("Invalid User");
			}

			var emergencyRequest = new EmergencyRequest()
			{
				UserId = currentUserId,
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

			var model = new SubmitEmergencyRequestResponseModel()
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

			return Result<SubmitEmergencyRequestResponseModel>.Success(model);
		}
		catch (Exception ex)
		{
			string message = "An error occurred while adding the emergency request : " + ex.Message;
			_logger.LogError(message);
			return Result<SubmitEmergencyRequestResponseModel>.Failure(message);
		}
	}
	private async Task<Result<SubmitEmergencyRequestResponseModel>> ValidateEmergencyRequestRequestModel
		(SubmitEmergencyRequestRequestModel? request)
	{
		if (request is null)
		{
			return Result<SubmitEmergencyRequestResponseModel>
				.ValidationError("Request model cannot be null.");
		}

		if (request.ProviderId < 1)
		{
			return Result<SubmitEmergencyRequestResponseModel>
				.ValidationError("Invalid Provider Id.");
		}

		if (!await IsServiceIdExist(request.ServiceId))
		{
			return Result<SubmitEmergencyRequestResponseModel>
				.ValidationError("Invalid Service Id.");
		}

		if (!await IsTownshipCodeExist(request.TownshipCode))
		{
			return Result<SubmitEmergencyRequestResponseModel>
				.ValidationError("Invalid Township Code.");
		}

		return null;
	}

	private async Task<bool> IsUserIdExist(int userId)
	{
		var isUserIdExist = await _db.Users.AnyAsync(x => x.UserId == userId);
		return isUserIdExist;
	}

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
