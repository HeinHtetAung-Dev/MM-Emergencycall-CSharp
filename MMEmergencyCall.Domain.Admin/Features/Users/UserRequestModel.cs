using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.Users;

public class UserRequestModel
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? EmergencyType { get; set; }

    public string? EmergencyDetails { get; set; }

    public string TownshipCode { get; set; } = null!;

    public string Role { get; set; } = null!;
}
