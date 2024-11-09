using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MMEmergencyCall.Databases.AppDbContextModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.Users;

public static class UserServiceExtension
{
    public static void AddUserService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UserService>();
    }
}
