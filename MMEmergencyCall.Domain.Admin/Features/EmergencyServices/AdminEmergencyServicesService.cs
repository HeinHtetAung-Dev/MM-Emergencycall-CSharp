namespace MMEmergencyCall.Domain.Admin.Features.EmergencyServices;

public class AdminEmergencyServicesService
{
	private readonly ILogger<AdminEmergencyServicesService> _logger;

	private readonly AppDbContext _db;

	public AdminEmergencyServicesService(
		ILogger<AdminEmergencyServicesService> logger,
		AppDbContext db
	)
	{
		_logger = logger;
		_db = db;
	}

	public async Task<
		Result<AdminEmergencyServicesPaginationResponseModel>
	> GetEmergencyServicesByStatusAsync(string? status, int pageNo = 1, int pageSize = 10)
	{
		if (pageNo < 1 || pageSize < 1)
		{
			return Result<AdminEmergencyServicesPaginationResponseModel>.Failure(
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
				return Result<AdminEmergencyServicesPaginationResponseModel>.NotFoundError(
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
				return Result<AdminEmergencyServicesPaginationResponseModel>.Failure(
					"Invalid page number"
				);
			}

			var lst = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

			var data = lst.Select(x => new AdminEmergencyServicesResponseModel
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

			var model = new AdminEmergencyServicesPaginationResponseModel()
			{
				Data = data,
				PageNo = pageNo,
				PageSize = pageSize,
				PageCount = pageCount
			};

			return Result<AdminEmergencyServicesPaginationResponseModel>.Success(model);
		}
		catch (ArgumentException ex)
		{
			_logger.LogError(ex.ToString());
			return Result<AdminEmergencyServicesPaginationResponseModel>.Failure(ex.ToString());
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.ToString());
			return Result<AdminEmergencyServicesPaginationResponseModel>.Failure(ex.ToString());
		}
	}

	public async Task<
		Result<AdminEmergencyServicesResponseModel>
	> UpdateEmergencyServiceStatusAsync(int id, string serviceStatus)
	{
		if (!Enum.IsDefined(typeof(EnumServiceStatus), serviceStatus))
		{
			return Result<AdminEmergencyServicesResponseModel>.ValidationError(
				"Invalid Emergency Service Status. Status should be Pending, Approved or Rejected"
			);
		}

		var item = await _db.EmergencyServices.FirstOrDefaultAsync(x => x.ServiceId == id);
		if (item is null)
		{
			return Result<AdminEmergencyServicesResponseModel>.NotFoundError(
				"This is no Emergency Service with Id: " + id
			);
		}

		item.ServiceStatus = serviceStatus;
		_db.Entry(item).State = EntityState.Modified;
		await _db.SaveChangesAsync();

		var model = new AdminEmergencyServicesResponseModel()
		{
			ServiceId = id,
			ServiceStatus = serviceStatus
		};

		return Result<AdminEmergencyServicesResponseModel>.Success(model);
	}

	public async Task<Result<AdminEmergencyServicesResponseModel>> CreateEmergencyServiceAsync(
		int currentUserId,
		AdminEmergencyServicesRequestModel request
	)
	{
		try
		{
			//TODO Check validation


			var item = new EmergencyService()
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

			var response = new AdminEmergencyServicesResponseModel()
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

			return Result<AdminEmergencyServicesResponseModel>.Success(response);
		}
		catch (Exception ex)
		{
			return Result<AdminEmergencyServicesResponseModel>.Failure(ex.ToString());
		}
	}

	public async Task<
		Result<AdminEmergencyServicesResponseModel>
	> DeleteEmergencyServiceStatusAsync(int id)
	{
		var item = await _db.EmergencyServices.FirstOrDefaultAsync(x => x.ServiceId == id);
		if (item is null)
		{
			return Result<AdminEmergencyServicesResponseModel>.NotFoundError(
				"This is no Emergency Service with Id: " + id
			);
		}

		item.ServiceStatus = EnumServiceStatus.Deleted.ToString();
		_db.Entry(item).State = EntityState.Modified;
		await _db.SaveChangesAsync();

		var model = new AdminEmergencyServicesResponseModel()
		{
			ServiceId = id,
			ServiceStatus = EnumServiceStatus.Deleted.ToString()
		};

		return Result<AdminEmergencyServicesResponseModel>.Success(model);
	}

	public async Task<Result<AdminEmergencyServicesResponseModel>> UpdateEmergencyServiceAsync(
		int id,
		AdminEmergencyServicesRequestModel requestModel
	)
	{
		try
		{
			var item = await _db.EmergencyServices.FirstOrDefaultAsync(x => x.ServiceId == id);
			if (item is null)
			{
				return Result<AdminEmergencyServicesResponseModel>.NotFoundError(
					"This is no Emergency Service with Id: " + id
				);
			}

			item.ServiceGroup = requestModel.ServiceGroup;
			item.ServiceType = requestModel.ServiceType;
			item.ServiceName = requestModel.ServiceName;
			item.PhoneNumber = requestModel.PhoneNumber;
			item.Location = requestModel.Location;
			item.Availability = requestModel.Availability;
			item.TownshipCode = requestModel.TownshipCode;
			_db.Entry(item).State = EntityState.Modified;
			await _db.SaveChangesAsync();

			AdminEmergencyServicesResponseModel responseModel = new()
			{
				ServiceId = item.ServiceId,
				UserId = item.UserId,
				ServiceGroup = requestModel.ServiceGroup,
				ServiceType = requestModel.ServiceType,
				ServiceName = requestModel.ServiceName,
				PhoneNumber = requestModel.PhoneNumber,
				Location = requestModel.Location,
				Availability = requestModel.Availability,
				TownshipCode = requestModel.TownshipCode,
				ServiceStatus = item.ServiceStatus
			};

			return Result<AdminEmergencyServicesResponseModel>.Success(responseModel);
		}
		catch (Exception ex)
		{
			return Result<AdminEmergencyServicesResponseModel>.Failure(ex.ToString());
		}
	}

	public async Task<Result<AdminEmergencyServicesResponseModel>> DeleteEmergencyServiceAsync(
		int id,
		AdminEmergencyServicesRequestModel requestModel
	)
	{
		try
		{
			var item = await _db.EmergencyServices.FirstOrDefaultAsync(x => x.ServiceId == id);
			if (item is null)
			{
				return Result<AdminEmergencyServicesResponseModel>.NotFoundError(
					"This is no Emergency Service with Id: " + id
				);
			}

			_db.Entry(item).State = EntityState.Deleted;
			await _db.SaveChangesAsync();

			AdminEmergencyServicesResponseModel model = new() { ServiceId = item.ServiceId };

			return Result<AdminEmergencyServicesResponseModel>.Success(model);
		}
		catch (Exception ex)
		{
			return Result<AdminEmergencyServicesResponseModel>.Failure(ex.ToString());
		}
	}

}
