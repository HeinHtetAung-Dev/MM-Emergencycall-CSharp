using System;
using System.Collections.Generic;

namespace MMEmergencyCall.Databases.AppDbContextModels;

public partial class Session
{
    public Guid SessionId { get; set; }

    public int UserId { get; set; }

    public DateTime ExpireTime { get; set; }

    public DateTime? LogoutTime { get; set; }
}
