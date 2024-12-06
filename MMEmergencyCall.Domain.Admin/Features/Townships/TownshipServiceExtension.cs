namespace MMEmergencyCall.Domain.Admin.Features.Townships;

public static class TownshipServiceExtension
{
    public static void AddTownshipService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<TownshipService>();
    }
}
