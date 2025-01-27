namespace MMEmergencyCall.Domain.Admin.Features.TownshipTopTenService;

public class TownshipTopTenServiceService
{
	private readonly AppDbContext _db;
	private readonly ILogger<TownshipTopTenServiceService> _logger;
	public TownshipTopTenServiceService(ILogger<TownshipTopTenServiceService> logger, AppDbContext db)
	{
		_logger = logger;
		_db = db;
	}

	public async Task<Result<List<TownshipTopTenServiceResponseModel>>> GetTownshipTopTenService(string townshipCode)
	{
		try
		{

			string query = $@"SET NOCOUNT ON;

    SELECT 
           es.[ServiceId],
		   es.[UserId],
		   es.[ServiceType],
           es.[ServiceName],
		   es.[PhoneNumber],
		   es.[Location],
		   es.[Availability],
		   es.[ServiceStatus],
           COUNT(er.[RequestId]) AS RequestCount
    FROM [dbo].[EmergencyRequests] er
    INNER JOIN [dbo].[EmergencyServices] es
        ON er.[ServiceId] = es.[ServiceId]
    WHERE er.[TownshipCode] = @TownshipCode
    GROUP BY 
	es.[ServiceId], 
	es.[ServiceName], 
	es.[UserId],
	es.[ServiceType],
	es.[PhoneNumber],
	es.[Location],
	es.[ServiceStatus],
	es.[Availability]
    ORDER BY COUNT(er.[RequestId]) DESC;";

			var responseData = await _db.Database
				.SqlQueryRaw<TownshipTopTenServiceResponseModel>(
				query,
				new { TownShipCode = townshipCode }
				)
				.ToListAsync();

			_logger.LogInformation(responseData.ToString());

			return Result<List<TownshipTopTenServiceResponseModel>>.Success(responseData);
		}
		catch (Exception ex)
		{
			return Result<List<TownshipTopTenServiceResponseModel>>.Failure(ex.ToString());
		}

	}
}
