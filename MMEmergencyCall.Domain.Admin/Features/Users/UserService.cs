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

    public async Task<Result<List<UserResponseModel>>> GetAllAsync()
    {
        var users = await _context.Set<User>().ToListAsync();

        var response = users.Select(u => new UserResponseModel
        {
            UserId = u.UserId,
            Name = u.Name,
            Email = u.Email,
            Password = u.Password,
            PhoneNumber = u.PhoneNumber,
            Address = u.Address,
            EmergencyType = u.EmergencyType,
            EmergencyDetails = u.EmergencyDetails,
            TownshipCode = u.TownshipCode
        }).ToList();

        return Result<List<UserResponseModel>>.Success(response);
    }
}
