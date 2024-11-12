using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Client.Features.EmergencyServices;

public class EmergencyServiceRequestModel
{
    public string ServiceType { get; set; } = null!;
    public string ServiceGroup { get; set; } = null!;

    public string ServiceName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Location { get; set; }

    public string? Availability { get; set; }

    public string? TownshipCode { get; set; }

    public string ServiceStatus { get; set; } = "Pending";
}
