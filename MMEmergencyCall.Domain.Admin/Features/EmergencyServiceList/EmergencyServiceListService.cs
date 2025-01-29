using EnumServiceStatus = MMEmergencyCall.Shared.EnumServiceStatus;

namespace MMEmergencyCall.Domain.Admin.Features.EmergencyServiceList;

public class EmergencyServiceListService
{
	private readonly ILogger<EmergencyServiceListService> _logger;

	private readonly AppDbContext _db;

	public EmergencyServiceListService(
		ILogger<EmergencyServiceListService> logger,
		AppDbContext db
	)
	{
		_logger = logger;
		_db = db;
	}

	public async Task<
		Result<EmergencyServicesListPaginationResponseModel>
	> GetEmergencyServicesByStatusAsync(string? status, int pageNo = 1, int pageSize = 10)
	{
		if (pageNo < 1 || pageSize < 1)
		{
			return Result<EmergencyServicesListPaginationResponseModel>.Failure(
				"Invalid page number or page size"
			);
		}

		var serviceStatus = EnumServiceStatus.None;

		try
		{
			if (!status.IsNullOrEmpty())
			{
				serviceStatus = Enum.Parse<EnumServiceStatus>(status, true);
			}

			var query = _db.EmergencyServices.AsQueryable();

			if (!serviceStatus.Equals(EnumServiceStatus.None))
			{
				query = query.Where(x => x.ServiceStatus == status);
			}

			var servicesByStatus = await query.ToListAsync();

			int rowCount = servicesByStatus.Count();

			if (rowCount is 0)
			{
				return Result<EmergencyServicesListPaginationResponseModel>.NotFoundError(
					$"There is no data with the status: {status}"
				);
			}

			int pageCount = rowCount / pageSize;
			if (rowCount % pageSize > 0)
			{
				pageCount++;
			}

			if (pageNo > pageCount)
			{
				return Result<EmergencyServicesListPaginationResponseModel>.Failure(
					"Invalid page number"
				);
			}

			var lst = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

			var data = lst.Select(x => new EmergencyServiceListResponseModel
			{
				ServiceId = x.ServiceId,
				UserId = x.UserId,
				ServiceGroup = x.ServiceGroup,
				ServiceType = x.ServiceType,
				ServiceName = x.ServiceName,
				PhoneNumber = x.PhoneNumber,
				Location = x.Location,
				Availability = x.Availability,
				TownshipCode = x.TownshipCode,
				ServiceStatus = x.ServiceStatus
			})
				.ToList();

			var model = new EmergencyServicesListPaginationResponseModel()
			{
				Data = data,
				PageNo = pageNo,
				PageSize = pageSize,
				PageCount = pageCount
			};

			return Result<EmergencyServicesListPaginationResponseModel>.Success(model);
		}
		catch (ArgumentException ex)
		{
			_logger.LogError(ex.ToString());
			return Result<EmergencyServicesListPaginationResponseModel>.Failure(ex.ToString());
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.ToString());
			return Result<EmergencyServicesListPaginationResponseModel>.Failure(ex.ToString());
		}
	}
}
