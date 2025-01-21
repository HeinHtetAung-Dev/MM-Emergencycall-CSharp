using MMEmergencyCall.Domain.Admin.Common;
using MMEmergencyCall.Domain.Admin.Features.EmergencyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.EmergencyService;

public class EmergencyServicesPaginationResponseModel
{
	public int PageNo { get; set; }

	public int PageSize { get; set; }

	public int PageCount { get; set; }

	public bool IsEndOfPage => PageNo == PageCount;

	public List<EmergencyServiceResponseModel> Data { get; set; }
}
