using MMEmergencyCall.Domain.Admin.Common;

using EnumServiceStatus = MMEmergencyCall.Shared.EnumServiceStatus;

namespace MMEmergencyCall.Domain.Admin.Features.UpdateEmergencyServiceStatus;

public class UpdateEmergencyServiceStatusService
{
	private readonly ILogger<UpdateEmergencyServiceStatusService> _logger;

	private readonly AppDbContext _db;

	public UpdateEmergencyServiceStatusService(
		ILogger<UpdateEmergencyServiceStatusService> logger,
		AppDbContext db
	)
	{
		_logger = logger;
		_db = db;

	}
	public async Task<
	Result<EmergencyServiceResponseModel>
> UpdateEmergencyServiceStatusAsync(int id, string serviceStatus)
	{
		if (!Enum.IsDefined(typeof(EnumServiceStatus), serviceStatus))
		{
			return Result<EmergencyServiceResponseModel>.ValidationError(
				"Invalid Emergency Service Status. Status should be Pending, Approved or Rejected"
			);
		}

		var item = await _db.EmergencyServices.FirstOrDefaultAsync(x => x.ServiceId == id);
		if (item is null)
		{
			return Result<EmergencyServiceResponseModel>.NotFoundError(
				"This is no Emergency Service with Id: " + id
			);
		}

		item.ServiceStatus = serviceStatus;
		_db.Entry(item).State = EntityState.Modified;
		await _db.SaveChangesAsync();

		var model = new EmergencyServiceResponseModel()
		{
			ServiceId = id,
			ServiceStatus = serviceStatus
		};

		return Result<EmergencyServiceResponseModel>.Success(model);
	}
}
