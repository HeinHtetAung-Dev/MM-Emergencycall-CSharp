using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMEmergencyCall.Domain.Client.Features.EmergencyServices;

public class EmergencyServicePaginationResponseModel
{
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public int PageCount { get; set; }
    public bool IsEndOfPage => PageNo >= PageCount;
    public List<EmergencyServiceResponseModel> Data { get; set; }
}
