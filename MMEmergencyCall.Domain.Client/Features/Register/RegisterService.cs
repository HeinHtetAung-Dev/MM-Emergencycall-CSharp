using Microsoft.Extensions.Logging;

namespace MMEmergencyCall.Domain.Client.Features.Register;

public class RegisterService
{
    private readonly ILogger<RegisterService> _logger;
    private readonly AppDbContext _db;

    public RegisterService(ILogger<RegisterService> logger, AppDbContext context)
    {
        _logger = logger;
        _db = context;
    }

    public async Task<Result<RegisterModel>> RegisterUserAsync(RegisterRequestModel request)
    {
        try
        {
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Password = request.Password,
                Address = request.Address,
                TownshipCode = request.TownshipCode,
                EmergencyType = request.EmergencyType,
                EmergencyDetails = request.EmergencyDetails,
                Role = "Normal User"
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var model = new RegisterModel()
            {
                UserId = user.UserId,
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                TownshipCode = request.TownshipCode,
                EmergencyType = request.EmergencyType,
                EmergencyDetails = request.EmergencyDetails,
                Role = user.Role,
                UserStatus = user.UserStatus
            };
            return Result<RegisterModel>.Success(model);
        }
        catch (Exception ex)
        {
            string message = "An error occurred while registering the user: " + ex.ToString();
            _logger.LogError(message);
            return Result<RegisterModel>.Failure(message);
        }
    }
}
