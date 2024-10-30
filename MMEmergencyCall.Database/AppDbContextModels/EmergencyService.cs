using System;
using System.Collections.Generic;

namespace MMEmergencyCall.Database.AppDbContextModels;

public partial class EmergencyService
{
    public int ServiceId { get; set; }

    public string ServiceType { get; set; } = null!;

    public string ServiceName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Location { get; set; }

    public string? Availability { get; set; }

    public string? TownshipCode { get; set; }
}
