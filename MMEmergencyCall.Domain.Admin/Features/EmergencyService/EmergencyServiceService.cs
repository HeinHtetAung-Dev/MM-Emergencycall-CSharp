using MMEmergencyCall.Domain.Admin.Common;
using MMEmergencyCall.Domain.Admin.Features.EmergencyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnumServiceStatus = MMEmergencyCall.Shared.EnumServiceStatus;

namespace MMEmergencyCall.Domain.Admin.Features.EmergencyService;

public class EmergencyServiceService
{
	private readonly ILogger<EmergencyServiceService> _logger;

	private readonly AppDbContext _db;

	public EmergencyServiceService(
		ILogger<EmergencyServiceService> logger,
		AppDbContext db
	)
	{
		_logger = logger;
		_db = db;
	}

	public async Task<
		Result<EmergencyServicesPaginationResponseModel>
	> GetEmergencyServicesByStatusAsync(string? status, int pageNo = 1, int pageSize = 10)
	{
		if (pageNo < 1 || pageSize < 1)
		{
			return Result<EmergencyServicesPaginationResponseModel>.Failure(
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
				return Result<EmergencyServicesPaginationResponseModel>.NotFoundError(
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
				return Result<EmergencyServicesPaginationResponseModel>.Failure(
					"Invalid page number"
				);
			}

			var lst = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

			var data = lst.Select(x => new EmergencyServiceResponseModel
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

			var model = new EmergencyServicesPaginationResponseModel()
			{
				Data = data,
				PageNo = pageNo,
				PageSize = pageSize,
				PageCount = pageCount
			};

			return Result<EmergencyServicesPaginationResponseModel>.Success(model);
		}
		catch (ArgumentException ex)
		{
			_logger.LogError(ex.ToString());
			return Result<EmergencyServicesPaginationResponseModel>.Failure(ex.ToString());
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.ToString());
			return Result<EmergencyServicesPaginationResponseModel>.Failure(ex.ToString());
		}
	}
}
