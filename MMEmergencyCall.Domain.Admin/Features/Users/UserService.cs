using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MMEmergencyCall.Databases.AppDbContextModels;
using MMEmergencyCall.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.Users;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<UserListResponseModel>> GetAllAsync()
    {
        var users = await _context.Set<User>().ToListAsync();

        var response = new UserListResponseModel
        {
            UserList = users
        };

        return Result<UserListResponseModel>.Success(response);
    }
}
