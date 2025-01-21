using MMEmergencyCall.Domain.Admin.Common;
using MMEmergencyCall.Domain.Admin.Features.EmergencyServices;
using EnumServiceStatus = MMEmergencyCall.Shared.EnumServiceStatus;

namespace MMEmergencyCall.Domain.Admin.Features.DeleteEmergencyService;

public class DeleteEmergencyServiceService
{
	private readonly ILogger<DeleteEmergencyServiceService> _logger;

	private readonly AppDbContext _db;

	public DeleteEmergencyServiceService(
		ILogger<DeleteEmergencyServiceService> logger,
		AppDbContext db
	)
	{
		_logger = logger;
		_db = db;
	}

	public async Task<Result<EmergencyServiceResponseModel>> DeleteEmergencyServiceAsync(
		int id
	)
	{
		try
		{
			var item = await _db.EmergencyServices.FirstOrDefaultAsync(x => x.ServiceId == id);
			if (item is null)
			{
				return Result<EmergencyServiceResponseModel>.NotFoundError(
					"This is no Emergency Service with Id: " + id
				);
			}

			_db.Entry(item).State = EntityState.Deleted;
			await _db.SaveChangesAsync();

			EmergencyServiceResponseModel model = new() { ServiceId = item.ServiceId };

			return Result<EmergencyServiceResponseModel>.Success(model);
		}
		catch (Exception ex)
		{
			return Result<EmergencyServiceResponseModel>.Failure(ex.ToString());
		}
	}
}
