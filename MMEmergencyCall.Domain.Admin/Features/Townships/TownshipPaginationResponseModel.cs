namespace MMEmergencyCall.Domain.Admin.Features.Townships;

public class TownshipPaginationResponseModel
{
    public int PageNo { get; set; }

    public int PageSize { get; set; }

    public int PageCount { get; set; }

    public bool IsEndOfPage => PageNo == PageCount;

    public List<TownshipResponseModel> Data { get; set; }
}
