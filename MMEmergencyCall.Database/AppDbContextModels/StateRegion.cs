using System;
using System.Collections.Generic;

namespace MMEmergencyCall.Database.AppDbContextModels;

public partial class StateRegion
{
    public int StateRegionId { get; set; }

    public string StateRegionCode { get; set; } = null!;

    public string StateRegionNameEn { get; set; } = null!;

    public string? StateRegionNameMm { get; set; }
}
