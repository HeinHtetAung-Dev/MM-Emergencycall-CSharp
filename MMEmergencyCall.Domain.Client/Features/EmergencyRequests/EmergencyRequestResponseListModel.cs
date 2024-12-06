using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMEmergencyCall.Domain.Client.Features.EmergencyRequests;

public class EmergencyRequestResponseListModel
{
    public List<EmergencyRequestResponseModel> EmergencyRequestResponseList { get; set; } = new();
}