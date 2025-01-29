﻿using MMEmergencyCall.Domain.Admin.Features.EmergencyRequest;

namespace MMEmergencyCall.Domain.Admin.Features.EmergencyRequestList;

public class EmergencyRequestListService
{
	private readonly ILogger<EmergencyRequestListService> _logger;
	private readonly AppDbContext _db;

	public EmergencyRequestListService(ILogger<EmergencyRequestListService> logger, AppDbContext db)
	{
		_logger = logger;
		_db = db;
	}

	public async Task<Result<EmergencyRequestListPaginationResponseModel>> GetEmergencyRequests(int pageNo, int pageSize,
int? userId = null, string? serviceId = null, string? providerId = null,
string? status = null, string? townshipCode = null)
	{
		try
		{
			var query = _db.EmergencyRequests.AsQueryable();

			if (userId.HasValue)
			{
				query = query.Where(x => x.UserId == userId.Value);
			}
			if (!string.IsNullOrEmpty(serviceId))
			{
				query = query.Where(x => x.ServiceId.ToString() == serviceId);
			}
			if (!string.IsNullOrEmpty(providerId))
			{
				query = query.Where(x => x.ProviderId.ToString() == providerId);
			}

			if (!string.IsNullOrEmpty(status))
			{
				if (!Enum.IsDefined(typeof(EnumEmergencyRequestStatus), status))
				{
					return Result<EmergencyRequestListPaginationResponseModel>.ValidationError(
						"Invalid Emergency Request Status. Status should be Cancel, Open or Closed"
					);
				}

				query = query.Where(x => x.Status == status);
			}

			if (!string.IsNullOrEmpty(status))
			{
				query = query.Where(x => x.Status == status);
			}
			if (!string.IsNullOrEmpty(townshipCode))
			{
				query = query.Where(x => x.TownshipCode == townshipCode);
			}

			int totalRecords = await query.CountAsync();

			int pageCount = (int)Math.Ceiling(totalRecords / (double)pageSize);

			var emergencyRequests = await query
				.Skip((pageNo - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			var responseData = emergencyRequests.Select(x => new EmergencyRequestListModel
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

			var model = new EmergencyRequestListPaginationResponseModel
			{
				PageNo = pageNo,
				PageSize = pageSize,
				PageCount = pageCount,
				Data = responseData
			};

			return Result<EmergencyRequestListPaginationResponseModel>.Success(model);
		}
		catch (Exception ex)
		{
			string message = "An error occurred while getting the emergency requests: " + ex.Message;
			_logger.LogError(message);
			return Result<EmergencyRequestListPaginationResponseModel>.Failure(message);
		}
	}
}
