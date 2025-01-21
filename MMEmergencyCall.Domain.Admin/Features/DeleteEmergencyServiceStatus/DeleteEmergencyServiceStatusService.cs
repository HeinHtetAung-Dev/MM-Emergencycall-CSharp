﻿using MMEmergencyCall.Domain.Admin.Common;
using EnumServiceStatus = MMEmergencyCall.Shared.EnumServiceStatus;

namespace MMEmergencyCall.Domain.Admin.Features.DeleteEmergencyServiceStatus
{
	public class DeleteEmergencyServiceStatusService
	{
		private readonly ILogger<DeleteEmergencyServiceStatusService> _logger;

		private readonly AppDbContext _db;

		public DeleteEmergencyServiceStatusService(
			ILogger<DeleteEmergencyServiceStatusService> logger,
			AppDbContext db
		)
		{
			_logger = logger;
			_db = db;
		}

		public async Task<
		Result<EmergencyServiceResponseModel>
	> DeleteEmergencyServiceStatusAsync(int id)
		{
			var item = await _db.EmergencyServices.FirstOrDefaultAsync(x => x.ServiceId == id);
			if (item is null)
			{
				return Result<EmergencyServiceResponseModel>.NotFoundError(
					"This is no Emergency Service with Id: " + id
				);
			}

			item.ServiceStatus = EnumServiceStatus.Deleted.ToString();
			_db.Entry(item).State = EntityState.Modified;
			await _db.SaveChangesAsync();

			var model = new EmergencyServiceResponseModel()
			{
				ServiceId = id,
				ServiceStatus = EnumServiceStatus.Deleted.ToString()
			};

			return Result<EmergencyServiceResponseModel>.Success(model);
		}
	}
}
