using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.Townships;

public class TownshipRequestModel
{
    public string TownshipCode { get; set; } = null!;

    public string TownshipNameEn { get; set; } = null!;

    public string TownshipNameMm { get; set; } = null!;

    public string StateRegionCode { get; set; } = null!;
}
