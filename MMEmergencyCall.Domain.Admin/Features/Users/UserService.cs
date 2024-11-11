using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<UserService> _logger;
    private readonly AppDbContext _context;

    public UserService(ILogger<UserService> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<UserResponseModel>>> GetAllAsync()
    {
        var users = await _context.Users.ToListAsync();

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

    public async Task<Result<UserResponseModel>> GetByIdAsync(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return Result<UserResponseModel>.Failure("User not found.");

            var model = new UserResponseModel()
            {

                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                EmergencyType = user.EmergencyType,
                EmergencyDetails = user.EmergencyDetails,
                TownshipCode = user.TownshipCode
            };

            return Result<UserResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            string message = "An error occurred while getting the user requests for id " + id + " : " + ex.Message;
            _logger.LogError(message);
            return Result<UserResponseModel>.Failure(message);
        }
    }

    public async Task<Result<bool>> IsExistUser(int id)
    {
        try
        {
            var isExist = await _context.Users.AnyAsync(x => x.UserId == id);
            return Result<bool>.Success(isExist);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure("User doesn't exist.");
        }
    }
}
