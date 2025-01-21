using MMEmergencyCall.Domain.Admin.Common;
using TBLEmergencyService = MMEmergencyCall.Databases.AppDbContextModels.EmergencyService;
using EnumServiceStatus = MMEmergencyCall.Shared.EnumServiceStatus;

namespace MMEmergencyCall.Domain.Admin.Features.CreateEmergencyService;

public class CreateEmergencyServiceService
{
	private readonly ILogger<CreateEmergencyServiceService> _logger;

	private readonly AppDbContext _db;

	public CreateEmergencyServiceService(
		ILogger<CreateEmergencyServiceService> logger,
		AppDbContext db
	)
	{
		_logger = logger;
		_db = db;
	}

	public async Task<Result<EmergencyServiceResponseModel>> CreateEmergencyServiceAsync(
	int currentUserId,
	EmergencyServiceRequestModel request
)
	{
		try
		{
			//TODO Check validation


			var item = new TBLEmergencyService()
			{
				UserId = currentUserId,
				ServiceGroup = request.ServiceGroup,
				ServiceType = request.ServiceType,
				ServiceName = request.ServiceName,
				PhoneNumber = request.PhoneNumber,
				Location = request.Location,
				Availability = request.Availability,
				TownshipCode = request.TownshipCode,
				ServiceStatus = EnumServiceStatus.Pending.ToString()
			};

			_db.EmergencyServices.Add(item);
			await _db.SaveChangesAsync();

			var response = new EmergencyServiceResponseModel()
			{
				ServiceId = item.ServiceId,
				UserId = item.UserId,
				ServiceGroup = item.ServiceGroup,
				ServiceType = item.ServiceType,
				ServiceName = item.ServiceName,
				PhoneNumber = item.PhoneNumber,
				Location = item.Location,
				Availability = item.Availability,
				TownshipCode = item.TownshipCode,
				ServiceStatus = item.ServiceStatus
			};

			return Result<EmergencyServiceResponseModel>.Success(response);
		}
		catch (Exception ex)
		{
			return Result<EmergencyServiceResponseModel>.Failure(ex.ToString());
		}
	}
}
