using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMEmergencyCall.Domain.Client.Features.Signin;

public static class SigninExtension
{
    public static void AddSigninService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<SigninService>();
    }
}
