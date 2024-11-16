using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MMEmergencyCall.Domain.Admin.Features.Users;
using MMEmergencyCall.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.Townships;

public class TownshipService
{
    private readonly ILogger<TownshipService> _logger;
    private readonly AppDbContext _context;

    public TownshipService(ILogger<TownshipService> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<TownshipPaginationResponseModel>> GetAllAsync(int pageNo, int pageSize)
    {
        var rowCount = _context.Townships.Count();

        int pageCount = rowCount / pageSize;

        if (pageNo < 1)
        {
            return Result<TownshipPaginationResponseModel>.Failure("Invalid PageNo.");
        }

        if (pageNo > pageCount)
        {
            return Result<TownshipPaginationResponseModel>.Failure("Invalid PageNo.");
        }

        var townships = await _context
            .Townships.Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var lst = townships.Select(ts => new TownshipResponseModel
        {
            TownshipId = ts.TownshipId,
            TownshipCode = ts.TownshipCode,
            TownshipNameEn = ts.TownshipNameEn,
            TownshipNameMm = ts.TownshipNameMm,
            StateRegionCode = ts.StateRegionCode
        }).ToList();

        TownshipPaginationResponseModel model = new TownshipPaginationResponseModel();
        model.Data = lst;
        model.PageNo = pageNo;
        model.PageSize = pageSize;
        model.PageCount = pageCount;

        return Result<TownshipPaginationResponseModel>.Success(model);
    }

    public async Task<Result<TownshipResponseModel>> CreateTownshipAsync(TownshipRequestModel requestModel)
    {
        try
        {
            var township = new Township
            {
                TownshipCode = requestModel.TownshipCode,
                TownshipNameEn = requestModel.TownshipNameEn,
                TownshipNameMm = requestModel.TownshipNameMm,
                StateRegionCode = requestModel.StateRegionCode
            };

            _context.Add(township);
            await _context.SaveChangesAsync();

            var model = new TownshipResponseModel
            {
                TownshipId = township.TownshipId,
                TownshipCode = township.TownshipCode,
                TownshipNameEn = township.TownshipNameEn,
                TownshipNameMm = township.TownshipNameMm,
                StateRegionCode = township.StateRegionCode
            };

            return Result<TownshipResponseModel>.Success(model, "Township is created successfully.");
        }
        catch (Exception ex)
        {
            string message = "An error occurred while creating the township requests: " + ex.Message;
            _logger.LogError(message);
            return Result<TownshipResponseModel>.Failure(message);
        }
    }

    public async Task<Result<TownshipResponseModel>> UpdateTownshipAsync(int id, TownshipRequestModel requestModel)
    {
        try
        {
            var township = await _context.Townships.FindAsync(id);
            if (township is null)
                return Result<TownshipResponseModel>.ValidationError("Township with id " + id + " not found.");

            township.TownshipCode = requestModel.TownshipCode;
            township.TownshipNameEn = requestModel.TownshipNameEn;
            township.TownshipNameMm = requestModel.TownshipNameMm;
            township.StateRegionCode = requestModel.StateRegionCode;

            _context.Entry(township).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var model = new TownshipResponseModel
            {
                TownshipId = township.TownshipId,
                TownshipCode = township.TownshipCode,
                TownshipNameEn = township.TownshipNameEn,
                TownshipNameMm = township.TownshipNameMm,
                StateRegionCode = township.StateRegionCode
            };

            return Result<TownshipResponseModel>.Success(model, "Township is updated successfully.");
        }
        catch (Exception ex)
        {
            string message = "An error occurred while updating the township requests for id " + id + " : " + ex.Message;
            _logger.LogError(message);
            return Result<TownshipResponseModel>.Failure(message);
        }
    }
}
