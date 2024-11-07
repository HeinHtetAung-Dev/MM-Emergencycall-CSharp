using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Client.Features.Signin;

public class SigninRequestModel
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
