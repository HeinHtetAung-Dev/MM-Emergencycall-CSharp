namespace MMEmergencyCall.Domain.Admin.Features.Users;

public class UserPaginationResponseModel
{
    public int PageNo { get; set; }

    public int PageSize { get; set; }

    public int PageCount { get; set; }

    public bool IsEndOfPage => PageNo == PageCount;

    public List<UserResponseModel> Data { get; set; }
}
