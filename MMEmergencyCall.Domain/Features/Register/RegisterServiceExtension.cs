using Microsoft.Extensions.DependencyInjection;

namespace MMEmergencyCall.Domain.Features.Register
{
    public static class RegisterServiceExtension
    {
        public static void AddRegisterService(this IServiceCollection services)
        {
            services.AddScoped<RegisterService>();
        }
    }
}
