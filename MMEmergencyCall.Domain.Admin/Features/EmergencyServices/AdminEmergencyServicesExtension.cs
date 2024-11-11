using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MMEmergencyCall.Domain.Admin.Features.EmergencyServices;

public static class AdminEmergencyServicesExtension
{
    public static void AddAdminEmergencyServicesService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<AdminEmergencyServicesService>();
    }
}