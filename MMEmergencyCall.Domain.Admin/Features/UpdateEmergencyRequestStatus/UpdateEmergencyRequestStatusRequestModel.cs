using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.UpdateEmergencyRequestStatus;

public class UpdateEmergencyRequestStatusRequestModel
{
	public string Status { get; set; } = null!;
}
