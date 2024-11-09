using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MMEmergencyCall.Domain.Admin.Features.StateRegion;

public static class StateRegionServiceExtension
{
    public static void AddStateRegionService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<StateRegionService>();
    }
}