namespace MMEmergencyCall.Domain.User.Features.EmergencyRequests;

public static class EmergencyRequestExtention
{
    public static void AddEmergencyRequest(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<EmergencyRequestService>();
    }
}
