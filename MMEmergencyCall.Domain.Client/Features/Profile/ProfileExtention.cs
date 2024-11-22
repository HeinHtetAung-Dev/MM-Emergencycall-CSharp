namespace MMEmergencyCall.Domain.Client.Features.Profile;

public static class ProfileExtention
{
    public static void AddProfile(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ProfileService>();
    }
}
