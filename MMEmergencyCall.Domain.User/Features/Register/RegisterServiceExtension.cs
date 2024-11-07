namespace MMEmergencyCall.Domain.User.Features.Register;

public static class RegisterServiceExtension
{
    public static void AddRegisterService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<RegisterService>();
    }
}