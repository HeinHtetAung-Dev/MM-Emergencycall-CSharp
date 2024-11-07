using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Client.Features.EmergencyServices;

public class EmergencyServiceResponseModel
{
    public Result<EmergencyService> Result { get; set; }

    public EmergencyServiceResponseModel(Result<EmergencyService> result)
    {
        Result = result;
    }
}
