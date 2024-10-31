namespace MMEmergencyCall.Domain.Features.Register;

public class RegisterService
{
    private readonly AppDbContext _db;

    public RegisterService(AppDbContext context)
    {
        _db = context;
    }

    public async Task<RegisterResponseModel> RegisterUserAsync(RegisterRequestModel request)
    {
        try
        {
            var user = new User
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                TownshipCode = request.TownshipCode,
                EmergencyType = request.EmergencyType,
                EmergencyDetails = request.EmergencyDetails
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // Return a successful result with the created user
            return new RegisterResponseModel(Result<User>.Success(user));
        }
        catch (Exception ex)
        {
            // Log the exception as needed

            // Return a failure result with an error message
            return new RegisterResponseModel(Result<User>.Failure("An error occurred while registering the user: " + ex.Message));
        }
    }
}
