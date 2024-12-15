namespace MMEmergencyCall.Domain.Client;

public static class HttpContextExtensions
{
    public static int? GetCurrentUserId(this HttpContext httpContext)
    {
        if (httpContext.Items.TryGetValue("UserId", out var userId))
        {
            return userId as int?;
        }

        return null;
    }
}
