namespace MMEmergencyCall.Domain.Admin.Features.EmergencyServices;

public static class AdminEmergencyServicesExtension
{
    public static void AddAdminEmergencyServicesService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<AdminEmergencyServicesService>();
    }
}