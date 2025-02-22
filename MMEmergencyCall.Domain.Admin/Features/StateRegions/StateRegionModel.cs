﻿namespace MMEmergencyCall.Domain.Admin.Features.StateRegions;

public class StateRegionModel
{
    public int StateRegionId { get; set; }
    public string StateRegionCode { get; set; } = null!;
    public string StateRegionNameEn { get; set; } = null!;
    public string? StateRegionNameMm { get; set; }
}