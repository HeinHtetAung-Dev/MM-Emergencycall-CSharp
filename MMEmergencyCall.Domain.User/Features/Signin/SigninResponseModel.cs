using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.User.Features.Signin;

public class SigninResponseModel
{
    public Result<SigninModel> Result { get; set; }

    public SigninResponseModel(Result<SigninModel> result)
    {
        Result = result;
    }
}

public class SigninModel
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? SessionExpiredTime { get; set; }

    public string Token { get; set; } = null!;
}
