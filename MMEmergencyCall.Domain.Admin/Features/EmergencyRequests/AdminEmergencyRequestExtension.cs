using Microsoft.AspNetCore.Builder;

namespace MMEmergencyCall.Domain.Admin.Features.EmergencyRequests;

public static class AdminEmergencyRequestExtension
{
    public static void AddAdminEmergencyRequest(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<AdminEmergencyRequestService>();
    }
}
