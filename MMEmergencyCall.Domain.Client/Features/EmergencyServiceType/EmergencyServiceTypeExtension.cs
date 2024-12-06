using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMEmergencyCall.Domain.Client.Features.EmergencyServiceType;

public static class EmergencyServiceTypeExtension
{
    public static void AddEmergencyServiceType(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<EmergencyServiceTypeService>();
    }
}
