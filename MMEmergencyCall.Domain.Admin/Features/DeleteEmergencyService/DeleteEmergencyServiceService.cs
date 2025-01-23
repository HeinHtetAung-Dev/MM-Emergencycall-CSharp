﻿namespace MMEmergencyCall.Domain.Admin.Features.DeleteEmergencyService;

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

	public async Task<Result<bool>> DeleteEmergencyServiceAsync(
		int id
	)
	{
		try
		{
			var item = await _db.EmergencyServices.FirstOrDefaultAsync(x => x.ServiceId == id);
			if (item is null)
			{
				return Result<bool>.NotFoundError(
					"This is no Emergency Service with Id: " + id
				);
			}

			_db.Entry(item).State = EntityState.Deleted;
			await _db.SaveChangesAsync();

			return Result<bool>.Success(true);
		}
		catch (Exception ex)
		{
			return Result<bool>.Failure(ex.ToString());
		}
	}
}
