using Microsoft.EntityFrameworkCore;
using MMEmergencyCall.Shared;

namespace MMEmergencyCall.Domain.Admin.Features.StateRegion;

public class StateRegionService
{
    private readonly DbContext _context;

    public StateRegionService(DbContext context)
    {
        _context = context;
    }

    public async Task<Result<StateRegionResponseModel>> CreateAsync(StateRegionRequestModel model)
    {
        var stateRegion = new Databases.AppDbContextModels.StateRegion
        {
            StateRegionCode = model.StateRegionCode,
            StateRegionNameEn = model.StateRegionNameEn,
            StateRegionNameMm = model.StateRegionNameMm
        };

        _context.Add(stateRegion);
        await _context.SaveChangesAsync();

        var response = new StateRegionResponseModel
        {
            Data = new StateRegionModel
            {
                StateRegionId = stateRegion.StateRegionId,
                StateRegionCode = stateRegion.StateRegionCode,
                StateRegionNameEn = stateRegion.StateRegionNameEn,
                StateRegionNameMm = stateRegion.StateRegionNameMm
            }
        };

        return Result<StateRegionResponseModel>.Success(response, "State region created successfully.");
    }

    public async Task<Result<StateRegionResponseModel>> GetByIdAsync(int id)
    {
        var stateRegion = await _context.Set<Databases.AppDbContextModels.StateRegion>().FindAsync(id);
        if (stateRegion == null)
            return Result<StateRegionResponseModel>.Failure("State region not found.");

        var response = new StateRegionResponseModel
        {
            Data = new StateRegionModel
            {
                StateRegionId = stateRegion.StateRegionId,
                StateRegionCode = stateRegion.StateRegionCode,
                StateRegionNameEn = stateRegion.StateRegionNameEn,
                StateRegionNameMm = stateRegion.StateRegionNameMm
            }
        };

        return Result<StateRegionResponseModel>.Success(response);
    }

    public async Task<Result<StateRegionResponseModel>> UpdateAsync(int id, StateRegionRequestModel model)
    {
        var stateRegion = await _context.Set<Databases.AppDbContextModels.StateRegion>().FindAsync(id);
        if (stateRegion == null)
            return Result<StateRegionResponseModel>.Failure("State region not found.");

        stateRegion.StateRegionCode = model.StateRegionCode;
        stateRegion.StateRegionNameEn = model.StateRegionNameEn;
        stateRegion.StateRegionNameMm = model.StateRegionNameMm;

        await _context.SaveChangesAsync();

        var response = new StateRegionResponseModel
        {
            Data = new StateRegionModel
            {
                StateRegionId = stateRegion.StateRegionId,
                StateRegionCode = stateRegion.StateRegionCode,
                StateRegionNameEn = stateRegion.StateRegionNameEn,
                StateRegionNameMm = stateRegion.StateRegionNameMm
            }
        };

        return Result<StateRegionResponseModel>.Success(response, "State region updated successfully.");
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var stateRegion = await _context.Set<Databases.AppDbContextModels.StateRegion>().FindAsync(id);
        if (stateRegion == null)
            return Result<bool>.Failure("State region not found.");

        _context.Remove(stateRegion);
        await _context.SaveChangesAsync();

        return Result<bool>.Success(true, "State region deleted successfully.");
    }

    public async Task<Result<StateRegionListResponseModel>> GetAllAsync()
    {
        var stateRegions = await _context.Set<Databases.AppDbContextModels.StateRegion>().ToListAsync();

        var response = new StateRegionListResponseModel
        {
            Data = stateRegions.Select(sr => new StateRegionModel
            {
                StateRegionId = sr.StateRegionId,
                StateRegionCode = sr.StateRegionCode,
                StateRegionNameEn = sr.StateRegionNameEn,
                StateRegionNameMm = sr.StateRegionNameMm
            }).ToList(),
        };

        return Result<StateRegionListResponseModel>.Success(response);
    }
}
