namespace MMEmergencyCall.Domain.Admin.Features.EmergencyServices;

public class AdminEmergencyServicesPaginationResponseModel
{
    public int PageNo { get; set; }

    public int PageSize { get; set; }

    public int PageCount { get; set; }

    public bool IsEndOfPage => PageNo == PageCount;

    public List<AdminEmergencyServicesResponseModel> Data { get; set; }
}
