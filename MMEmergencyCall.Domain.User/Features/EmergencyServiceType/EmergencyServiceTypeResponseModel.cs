using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Client.Features.EmergencyServiceType;

public class EmergencyServiceTypeResponseModel
{
    public readonly Result<List<string>> Result;

    public EmergencyServiceTypeResponseModel(Result<List<string>> result)
    {
        Result = result;
    }
}
