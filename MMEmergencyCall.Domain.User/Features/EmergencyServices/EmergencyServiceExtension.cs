using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.User.Features.EmergencyServices;

public static class EmergencyServiceExtension
{
    public static void AddEmergencyServiceService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<EmergencyServiceService>();
    }
}
