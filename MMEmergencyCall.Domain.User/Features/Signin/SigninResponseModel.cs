﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Client.Features.Signin;

public class SigninResponseModel
{
    public Result<SigninModel> Result { get; set; }

    public SigninResponseModel(Result<SigninModel> result)
    {
        Result = result;
    }
}