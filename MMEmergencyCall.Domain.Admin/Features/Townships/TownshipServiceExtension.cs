using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.Townships;

public static class TownshipServiceExtension
{
    public static void AddTownshipService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<TownshipService>();
    }
}
