using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Client.Features.EmergencyServices;

public class EmergencyServiceListResponseModel
{
    public Result<List<EmergencyService>> Result { get; set; }

    public EmergencyServiceListResponseModel(Result<List<EmergencyService>> result)
    {
        Result = result;
    }
}
