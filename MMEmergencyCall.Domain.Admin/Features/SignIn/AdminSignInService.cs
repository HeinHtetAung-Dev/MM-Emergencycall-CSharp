using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MMEmergencyCall.Shared;

namespace MMEmergencyCall.Domain.Admin.Features.SignIn;

public class AdminSigninService
{
    private readonly ILogger<User> _logger;

    private readonly AppDbContext _db;

    public AdminSigninService(ILogger<User> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<AdminSignInResponseModel> SigninAsync(AdminSigninRequestModel requestModel)
    {
        var email = requestModel.Email;
        var Password = requestModel.Password;
        

        var user = await _db.Users.AsNoTracking()
            .Where(u => u.Email == requestModel.Email
                    && u.Password == requestModel.Password && u.Role.ToLower() == "admin")
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return new AdminSignInResponseModel(Result<AdminSignInModel>.Failure("Username or Password is incorrect."));
        }

        AdminSignInModel signin = new AdminSignInModel
        {
            Email = user.Email,
            Name = user.Name,
            SessionExpiredTime = DateTime.Now.AddMinutes(5),
            UserId = user.UserId
        };

        var token = signin.ToJson().ToEncrypt();
        signin.Token = token;

        return new AdminSignInResponseModel(Result<AdminSignInModel>.Success(signin));
    }
}
