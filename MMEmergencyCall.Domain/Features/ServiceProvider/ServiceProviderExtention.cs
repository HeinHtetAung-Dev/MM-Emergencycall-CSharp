namespace MMEmergencyCall.Domain.Features.ServiceProvider;

public static class ServiceProviderExtension
{
    public static void AddServiceProviderType(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ServiceProviderService>();
    }
}
