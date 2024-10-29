using System;
using System.Collections.Generic;

namespace MMEmergencyCall.Database.AppDbContextModels;

public partial class EmergencyRequest
{
    public int RequestId { get; set; }

    public int UserId { get; set; }

    public int ServiceId { get; set; }

    public int? ProviderId { get; set; }

    public DateTime RequestTime { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? ResponseTime { get; set; }

    public string? Notes { get; set; }

    public string? State { get; set; }

    public string? Township { get; set; }

    public virtual ServiceProvider? Provider { get; set; }

    public virtual EmergencyService Service { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
