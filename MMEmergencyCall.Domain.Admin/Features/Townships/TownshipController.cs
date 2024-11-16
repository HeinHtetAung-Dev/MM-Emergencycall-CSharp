using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Shared;

namespace MMEmergencyCall.Domain.Admin.Features.Townships;

[Route("api/[controller]")]
[ApiController]
public class TownshipController : BaseController
{
    private readonly TownshipService _townshipService;

    public TownshipController(TownshipService townshipService)
    {
        _townshipService = townshipService;
    }

    [HttpGet("pageNo/{pageNo}/pageSize/{pageSize}")]
    public async Task<IActionResult> GetAll(int pageNo, int pageSize)
    {
        var model = await _townshipService.GetAllAsync(pageNo, pageSize);
        return Execute(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TownshipRequestModel requestModel)
    {
        Result<TownshipResponseModel> model = null;
        if (string.IsNullOrEmpty(requestModel.TownshipCode))
        {
            model = Result<TownshipResponseModel>.ValidationError("TownshipCode is required.");
        }
        if (string.IsNullOrEmpty(requestModel.TownshipNameEn))
        {
            model = Result<TownshipResponseModel>.ValidationError("TownshipNameEn is required.");
        }
        if (string.IsNullOrEmpty(requestModel.TownshipNameMm))
        {
            model = Result<TownshipResponseModel>.ValidationError("TownshipNameMm is required.");
        }
        if (string.IsNullOrEmpty(requestModel.StateRegionCode))
        {
            model = Result<TownshipResponseModel>.ValidationError("StateRegionCode is required.");
        }

        model = await _townshipService.CreateTownshipAsync(requestModel);

        return Execute(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, TownshipRequestModel requestModel)
    {
        Result<TownshipResponseModel> model = null;
        if (string.IsNullOrEmpty(requestModel.TownshipCode))
        {
            model = Result<TownshipResponseModel>.ValidationError("TownshipCode is required.");
        }
        if (string.IsNullOrEmpty(requestModel.TownshipNameEn))
        {
            model = Result<TownshipResponseModel>.ValidationError("TownshipNameEn is required.");
        }
        if (string.IsNullOrEmpty(requestModel.TownshipNameMm))
        {
            model = Result<TownshipResponseModel>.ValidationError("TownshipNameMm is required.");
        }
        if (string.IsNullOrEmpty(requestModel.StateRegionCode))
        {
            model = Result<TownshipResponseModel>.ValidationError("StateRegionCode is required.");
        }

        model = await _townshipService.UpdateTownshipAsync(id, requestModel);

        return Execute(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var model = await _townshipService.DeleteAsync(id);
        return Execute(model);
    }
}