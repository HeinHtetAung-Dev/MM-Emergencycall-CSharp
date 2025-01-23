using MMEmergencyCall.Domain.Admin.Common;

namespace MMEmergencyCall.Domain.Admin.Features.TownshipTopTenRequest;

public class TownshipTopTenRequestService
{
	private readonly ILogger<TownshipTopTenRequestService> _logger;

	private readonly AppDbContext _db;

	public TownshipTopTenRequestService(ILogger<TownshipTopTenRequestService> logger, AppDbContext db)
	{
		_logger = logger;
		_db = db;
	}

	public async Task<Result<List<EmergencyRequestResponseModel>>> GetTopTenRequestPerUser(string townshipCode)
	{
		try
		{
			var emergencyRequests = await _db.EmergencyRequests
				.Where(x => x.TownshipCode == townshipCode)
				.OrderByDescending(x => x.RequestTime)
				.Take(10)
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

			return Result<List<EmergencyRequestResponseModel>>.Success(responseData);

		}
		catch (Exception ex)
		{
			return Result<List<EmergencyRequestResponseModel>>.Failure(ex.ToString());
		}
	}
}
