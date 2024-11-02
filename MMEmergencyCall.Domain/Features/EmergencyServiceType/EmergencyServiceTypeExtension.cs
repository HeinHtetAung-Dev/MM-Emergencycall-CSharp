using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMEmergencyCall.Domain.Features.EmergencyService;

namespace MMEmergencyCall.Domain.Features.EmergencyServiceType;

public static class EmergencyServiceTypeExtension
{
    public static void AddEmergencyServiceType(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<EmergencyServiceTypeService>();
    }
}
