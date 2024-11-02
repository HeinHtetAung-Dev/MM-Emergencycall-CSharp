using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Features.EmergencyServiceType;

public class EmergencyServiceTypeResponseModel
{
    public Result<List<string>> Result;

    public EmergencyServiceTypeResponseModel(Result<List<string>> result)
    {
        Result = result;
    }
}
