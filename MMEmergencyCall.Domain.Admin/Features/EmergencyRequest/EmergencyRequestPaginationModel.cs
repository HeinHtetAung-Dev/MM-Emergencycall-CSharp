using MMEmergencyCall.Domain.Admin.Common;

namespace MMEmergencyCall.Domain.Admin.Features.EmergencyRequest;

public class EmergencyRequestPaginationResponseModel
{
	public int PageNo { get; set; }
	public int PageSize { get; set; }
	public int PageCount { get; set; }
	public bool IsEndOfPage => PageNo >= PageCount;
	public List<EmergencyRequestResponseModel> Data { get; set; }
}
