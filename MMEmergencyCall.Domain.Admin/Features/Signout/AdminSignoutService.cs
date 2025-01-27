using MMEmergencyCall.Domain.Admin.Features.RefreshToken;

namespace MMEmergencyCall.Domain.Admin.Features.Signout;

public class AdminSignoutService
{
	private readonly AppDbContext _db;
	public AdminSignoutService(AppDbContext db)
	{
		_db = db;
	}

	public async Task<Result<bool>> Signout(string token)
	{
		try
		{
			var requestTokenModel = token.ToDecrypt().ToObject<TokenModel>();

			var session = await _db.Sessions
				.FirstOrDefaultAsync(x => x.SessionId == requestTokenModel.SessionId);

			session.LogoutTime = DateTime.Now;
			_db.Entry(session).State = EntityState.Modified;
			await _db.SaveChangesAsync();

			return Result<bool>.Success(true);
		}
		catch (Exception ex)
		{
			return Result<bool>.Failure(ex.ToString());
		}
	}
}
