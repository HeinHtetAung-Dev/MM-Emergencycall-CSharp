﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Client.Features.SubmitEmergencyRequest;

public class SubmitEmergencyRequestRequestModel
{
	public int ServiceId { get; set; }

	public int? ProviderId { get; set; }

	public DateTime RequestTime { get; set; }

	public DateTime? ResponseTime { get; set; }

	public string? Notes { get; set; }

	public string? TownshipCode { get; set; }
}
