using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MMEmergencyCall.Databases.AppDbContextModels;
using MMEmergencyCall.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

    public async Task<Result<UserPaginationResponseModel>> GetAllUsersWithPaginationAsync(int pageNo, int pageSize)
    {
        int rowCount = _context.Users.Count();

        int pageCount = rowCount / pageSize;

        if (pageNo < 1)
        {
            return Result<UserPaginationResponseModel>.Failure("Invalid PageNo.");
        }

        if (pageNo > pageCount)
        {
            return Result<UserPaginationResponseModel>.Failure("Invalid PageNo.");
        }

        var user = await _context
            .Users.Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var lst = user
            .Select(u => new UserResponseModel
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

        UserPaginationResponseModel model = new UserPaginationResponseModel();
        model.Data = lst;
        model.PageNo = pageNo;
        model.PageSize = pageSize;
        model.PageCount = pageCount;

        return Result<UserPaginationResponseModel>.Success(model);
    }

    public async Task<Result<UserPaginationResponseModel>> GetUsersByRoleAsync(string role, int pageNo, int pageSize)
    {
        int rowCount = _context.Users.Count();

        int pageCount = rowCount / pageSize;

        if (pageNo < 1)
        {
            return Result<UserPaginationResponseModel>.Failure("Invalid PageNo.");
        }

        if (pageNo > pageCount)
        {
            return Result<UserPaginationResponseModel>.Failure("Invalid PageNo.");
        }

        var user = await _context
            .Users.Where(u => u.Role == role)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var lst = user
            .Select(u => new UserResponseModel
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

        UserPaginationResponseModel model = new UserPaginationResponseModel();
        model.Data = lst;
        model.PageNo = pageNo;
        model.PageSize = pageSize;
        model.PageCount = pageCount;

        return Result<UserPaginationResponseModel>.Success(model);
    }

    public async Task<Result<UserResponseModel>> CreateUserAsync(UserRequestModel request)
    {
        try
        {
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                EmergencyType = request.EmergencyType,
                EmergencyDetails = request.EmergencyDetails,
                TownshipCode = request.TownshipCode,
                Role = request.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var model = new UserResponseModel
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                EmergencyType = user.EmergencyType,
                EmergencyDetails = user.EmergencyDetails,
                TownshipCode = user.TownshipCode,
                Role = user.Role,
                UserStatus = user.UserStatus
            };

            return Result<UserResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            var message = "An error occurred while creating User: " + ex.ToString();
            _logger.LogError(message);
            return Result<UserResponseModel>.Failure(message);
        }
    }
}
