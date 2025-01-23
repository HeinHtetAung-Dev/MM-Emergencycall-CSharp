namespace MMEmergencyCall.Domain.Admin.Features.UserList
{
	public class UserListService
	{
		private readonly ILogger<UserListService> _logger;
		private readonly AppDbContext _context;

		public UserListService(ILogger<UserListService> logger, AppDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public async Task<Result<UserListPaginationResponseModel>> GetUsersAsync(int pageNo, int pageSize, string? role, string? userStatus)
		{
			int rowCount = _context.Users.Count();

			int pageCount = rowCount / pageSize;

			if (pageNo < 1)
			{
				return Result<UserListPaginationResponseModel>.ValidationError("Invalid PageNo.");
			}

			if (pageNo > pageCount)
			{
				return Result<UserListPaginationResponseModel>.ValidationError("Invalid PageNo.");
			}

			var query = _context.Users.AsQueryable();

			if (!string.IsNullOrEmpty(role))
			{
				query = query.Where(ur => ur.Role == role);
			}

			if (!string.IsNullOrEmpty(userStatus))
			{
				query = query.Where(us => us.UserStatus == userStatus);
			}

			var user = await query
				.Skip((pageNo - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			var lst = user
				.Select(u => new UserListResponseModel
				{
					UserId = u.UserId,
					Name = u.Name,
					Email = u.Email,
					Password = u.Password,
					PhoneNumber = u.PhoneNumber,
					Address = u.Address,
					EmergencyType = u.EmergencyType,
					EmergencyDetails = u.EmergencyDetails,
					TownshipCode = u.TownshipCode,
					Role = u.Role,
					UserStatus = u.UserStatus
				})
				.ToList();

			UserListPaginationResponseModel model = new ();
			model.Data = lst;
			model.PageNo = pageNo;
			model.PageSize = pageSize;
			model.PageCount = pageCount;

			return Result<UserListPaginationResponseModel>.Success(model);
		}
	}
}
