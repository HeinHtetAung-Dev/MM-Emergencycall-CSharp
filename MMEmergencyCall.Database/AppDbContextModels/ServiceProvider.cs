using System;
using System.Collections.Generic;

namespace MMEmergencyCall.Database.AppDbContextModels;

public partial class ServiceProvider
{
    public int ProviderId { get; set; }

    public string ProviderName { get; set; } = null!;

    public int ServiceId { get; set; }

    public string ContactNumber { get; set; } = null!;

    public string? Availability { get; set; }

    public string? State { get; set; }

    public string? Township { get; set; }

    public virtual ICollection<EmergencyRequest> EmergencyRequests { get; set; } = new List<EmergencyRequest>();

    public virtual EmergencyService Service { get; set; } = null!;
}
