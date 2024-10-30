using System;
using System.Collections.Generic;

namespace MMEmergencyCall.Database.AppDbContextModels;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? EmergencyType { get; set; }

    public string? EmergencyDetails { get; set; }

    public string TownshipCode { get; set; } = null!;
}
