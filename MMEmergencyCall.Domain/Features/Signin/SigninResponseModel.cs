using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Features.Signin;

public class SigninResponseModel
{
    public Result<User> Result { get; set; }

    public SigninResponseModel(Result<User> result)
    {
        Result = result;
    }
}
