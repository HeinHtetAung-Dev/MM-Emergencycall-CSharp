namespace MMEmergencyCall.Domain.Admin.Features.UpdateEmergencyRequestStatus;

public class UpdateEmergencyRequestStatusService
{
	private readonly ILogger<UpdateEmergencyRequestStatusService> _logger;
	private readonly AppDbContext _db;

	public UpdateEmergencyRequestStatusService(ILogger<UpdateEmergencyRequestStatusService> logger, AppDbContext db)
	{
		_logger = logger;
		_db = db;
	}

	public async Task<Result<bool>> UpdateEmergencyRequestStatus(int id, UpdateEmergencyRequestStatusRequestModel statusRequest)
	{
		try
		{
			if (!Enum.IsDefined(typeof(EnumEmergencyRequestStatus), statusRequest.Status))
			{
				return Result<bool>.ValidationError(
					"Invalid Emergency Request Status. Status should be Cancel, Open or Closed"
				);
			}

			var existingEmergencyRequest = await _db.EmergencyRequests
				.FirstOrDefaultAsync(x => x.RequestId == id);

			if (existingEmergencyRequest is null)
			{
				return Result<bool>
					  .NotFoundError("Emergency Request with Id " + id + " not found.");
			}

			existingEmergencyRequest.Status = statusRequest.Status;
			_db.Entry(existingEmergencyRequest).State = EntityState.Modified;
			await _db.SaveChangesAsync();

			//var model = new EmergencyRequestResponseModel()
			//{
			//	RequestId = existingEmergencyRequest.RequestId,
			//	UserId = existingEmergencyRequest.UserId,
			//	ServiceId = existingEmergencyRequest.ServiceId,
			//	ProviderId = existingEmergencyRequest.ProviderId,
			//	RequestTime = existingEmergencyRequest.RequestTime,
			//	Status = existingEmergencyRequest.Status,
			//	ResponseTime = existingEmergencyRequest.ResponseTime,
			//	Notes = existingEmergencyRequest.Notes,
			//	TownshipCode = existingEmergencyRequest.TownshipCode
			//};

			return Result<bool>.Success(true);
		}
		catch (Exception ex)
		{
			string message = "An error occurred while updating the status of emergency request with id " + id + " : " +
							 ex.Message;
			_logger.LogError(message);
			return Result<bool>.Failure(message);
		}
	}
}
