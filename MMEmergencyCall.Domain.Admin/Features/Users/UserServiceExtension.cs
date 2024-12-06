namespace MMEmergencyCall.Domain.Admin.Features.Users;

public static class UserServiceExtension
{
    public static void AddUserService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UserService>();
    }
}
