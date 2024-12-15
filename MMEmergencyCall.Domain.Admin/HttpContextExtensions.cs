namespace MMEmergencyCall.Domain.Admin;

public static class HttpContextExtensions
{
    public static int? GetCurrentAdminId(this HttpContext httpContext)
    {
        if (httpContext.Items.TryGetValue("UserId", out var userId))
        {
            return Convert.ToInt32(userId);
        }

        return null;
    }
}
