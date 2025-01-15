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

    public async Task<Result<AdminSignInModel>> SigninAsync(AdminSigninRequestModel requestModel)
    {
        var email = requestModel.Email;
        var password = requestModel.Password;
        

        var user = await _db.Users
                   .Where(u => u.Email == requestModel.Email
                   && u.Password == requestModel.Password && u.Role.ToLower() == "admin" && u.IsVerified == EnumVerify.Y.ToString())
                   .FirstOrDefaultAsync();

        if (user is null)
        {
            return Result<AdminSignInModel>.ValidationError("Username or Password is incorrect.");
        }

        AdminSignInModel signin = new()
        {
            Email = user.Email,
            Name = user.Name,
            SessionExpiredTime = DateTime.Now.AddMinutes(5),
            Role = user.Role,
            UserId = user.UserId
        };

        var token = signin.ToJson().ToEncrypt();
        signin.Token = token;

        return Result<AdminSignInModel>.Success(signin);
    }
}
