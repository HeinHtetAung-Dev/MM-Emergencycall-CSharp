using MMEmergencyCall.Domain.Admin.Common;
using MMEmergencyCall.Domain.Admin.Features.Users;

namespace MMEmergencyCall.Domain.Admin.Features.UserList;

public class UserListResponseModel
{
	public int PageNo { get; set; }

	public int PageSize { get; set; }

	public int PageCount { get; set; }

	public bool IsEndOfPage => PageNo == PageCount;

	public List<UserModel> Data { get; set; }
}
