namespace MMEmergencyCall.Domain.Admin.Features.EmergencyRequests;

public class AdminEmergencyRequestPaginationResponseModel
{
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public int PageCount { get; set; }
    public bool IsEndOfPage => PageNo >= PageCount;
    public List<AdminEmergencyRequestResponseModel> Data { get; set; }
}
