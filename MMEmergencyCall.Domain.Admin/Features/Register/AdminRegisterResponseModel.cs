using MMEmergencyCall.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.Register;

public class AdminRegisterResponseModel
{
    public Result<User> Result { get; set; }

    public AdminRegisterResponseModel(Result<User> result)
    {
        Result = result;
    }
}