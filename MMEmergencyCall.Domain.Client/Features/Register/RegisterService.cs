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

    public async Task<RegisterResponseModel> RegisterUserAsync(RegisterRequestModel request)
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

            var model = new RegisterResponseModel(Result<User>.Success(user));
            return model;
        }
        catch (Exception ex)
        {
            string message = "An error occurred while registering the user: " + ex.ToString();
            _logger.LogError(message);
            return new RegisterResponseModel(Result<User>.Failure(message));
        }
    }
}
