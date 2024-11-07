using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MMEmergencyCall.Domain.Admin.Features.StateRegion;

namespace MMEmergencyCall.Domain.User.Features.Register;

public static class StateRegionServiceExtension
{
    public static void AddStateRegionService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<StateRegionService>();
    }
}