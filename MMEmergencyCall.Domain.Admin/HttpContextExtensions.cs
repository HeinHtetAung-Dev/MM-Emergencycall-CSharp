using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration.UserSecrets;

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
