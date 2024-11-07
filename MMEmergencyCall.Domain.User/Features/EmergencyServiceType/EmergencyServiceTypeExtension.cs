using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Client.Features.EmergencyServiceType;

public static class EmergencyServiceTypeExtension
{
    public static void AddEmergencyServiceType(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<EmergencyServiceTypeService>();
    }
}
