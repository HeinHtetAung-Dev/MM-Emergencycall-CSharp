﻿namespace MMEmergencyCall.Domain.Client.Features.Register;

public class RegisterResponseModel
{
    public Result<User> Result { get; set; }

    public RegisterResponseModel(Result<User> result)
    {
        Result = result;
    }
}