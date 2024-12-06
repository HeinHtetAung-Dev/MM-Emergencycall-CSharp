namespace MMEmergencyCall.Domain.Admin.Features.Register;

public class AdminRegisterService
{
    private readonly ILogger<AdminRegisterService> _logger;
    private readonly AppDbContext _db;

    public AdminRegisterService(ILogger<AdminRegisterService> logger, AppDbContext context)
    {
        _logger = logger;
        _db = context;
    }
    public async Task<AdminRegisterResponseModel> RegisterAdminAsync(AdminRegisterRequestModel request)
    {
        try
        {
            var ExistUser = await _db.Users.AnyAsync(x => x.Email.ToLower() == request.Email.ToLower() ||
                            x.PhoneNumber.ToLower() == request.PhoneNumber.ToLower());
            if (!ExistUser)
            {
                var user = new User
                {
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    Password = request.Password,
                    Address = request.Address,
                    TownshipCode = request.TownshipCode,
                    EmergencyType = request.EmergencyType,
                    EmergencyDetails = request.EmergencyDetails,
                    UserStatus = "Approved",
                    Role = "admin"
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                var model = new AdminRegisterResponseModel(Result<User>.Success(user));
                return model;
            }
            else
            {
                var model = new AdminRegisterResponseModel(Result<User>.Failure("This user already exists."));
                return model;
            }
        }
        catch (Exception ex)
        {
            string message = "An error occurred while registering the admin: " + ex.ToString();
            _logger.LogError(message);
            return new AdminRegisterResponseModel(Result<User>.Failure(message));
        }
    }
}

