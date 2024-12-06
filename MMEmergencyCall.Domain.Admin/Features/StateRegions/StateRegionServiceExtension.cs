namespace MMEmergencyCall.Domain.Admin.Features.StateRegions;

public static class StateRegionServiceExtension
{
    public static void AddStateRegionService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<StateRegionService>();
    }
}