namespace MMEmergencyCall.Domain.Features.ServiceProvider;

public static class ServiceProviderExtention
{
    public static void AddServiceProviderType(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ServiceProviderService>();
    }
}
