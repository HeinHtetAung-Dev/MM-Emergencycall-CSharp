using MMEmergencyCall.Domain.Admin.Common;
using MMEmergencyCall.Domain.Admin.Features.EmergencyRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.UpdateEmergencyRequestStatus
{
	public class UpdateEmergencyRequestStatusService
	{
		private readonly ILogger<UpdateEmergencyRequestStatusService> _logger;
		private readonly AppDbContext _db;

		public UpdateEmergencyRequestStatusService(ILogger<UpdateEmergencyRequestStatusService> logger, AppDbContext db)
		{
			_logger = logger;
			_db = db;
		}

		public async Task<Result<EmergencyRequestResponseModel>> UpdateEmergencyRequestStatus(int id, UpdateEmergencyRequestStatusRequest statusRequest)
		{
			try
			{
				if (!Enum.IsDefined(typeof(EnumEmergencyRequestStatus), statusRequest.Status))
				{
					return Result<EmergencyRequestResponseModel>.ValidationError(
						"Invalid Emergency Request Status. Status should be Cancel, Open or Closed"
					);
				}

				var existingEmergencyRequest = await _db.EmergencyRequests
					.FirstOrDefaultAsync(x => x.RequestId == id);

				if (existingEmergencyRequest is null)
				{
					return Result<EmergencyRequestResponseModel>
						  .NotFoundError("Emergency Request with Id " + id + " not found.");
				}

				existingEmergencyRequest.Status = statusRequest.Status;
				_db.Entry(existingEmergencyRequest).State = EntityState.Modified;
				await _db.SaveChangesAsync();

				var model = new EmergencyRequestResponseModel()
				{
					RequestId = existingEmergencyRequest.RequestId,
					UserId = existingEmergencyRequest.UserId,
					ServiceId = existingEmergencyRequest.ServiceId,
					ProviderId = existingEmergencyRequest.ProviderId,
					RequestTime = existingEmergencyRequest.RequestTime,
					Status = existingEmergencyRequest.Status,
					ResponseTime = existingEmergencyRequest.ResponseTime,
					Notes = existingEmergencyRequest.Notes,
					TownshipCode = existingEmergencyRequest.TownshipCode
				};

				return Result<EmergencyRequestResponseModel>.Success(model);
			}
			catch (Exception ex)
			{
				string message = "An error occurred while updating the status of emergency request with id " + id + " : " +
								 ex.Message;
				_logger.LogError(message);
				return Result<EmergencyRequestResponseModel>.Failure(message);
			}
		}
	}
}
