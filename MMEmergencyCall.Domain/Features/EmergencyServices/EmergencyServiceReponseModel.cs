using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Features.EmergencyServices;

public class EmergencyServiceReponseModel
{
    public Result<EmergencyService> Result { get; set; }

    public EmergencyServiceReponseModel(Result<EmergencyService> result)
    {
        Result = result;
    }
}
