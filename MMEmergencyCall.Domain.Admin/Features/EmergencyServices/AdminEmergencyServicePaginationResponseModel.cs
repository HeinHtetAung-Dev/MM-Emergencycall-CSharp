using MMEmergencyCall.Domain.Admin.Features.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.EmergencyServices;

public class AdminEmergencyServicePaginationResponseModel
{

    public int PageNo { get; set; }

    public int PageSize { get; set; }

    public int PageCount { get; set; }

    public bool IsEndOfPage => PageNo == PageCount;

    public List<AdminEmergencyServicesResponseModel> Data { get; set; }
}
