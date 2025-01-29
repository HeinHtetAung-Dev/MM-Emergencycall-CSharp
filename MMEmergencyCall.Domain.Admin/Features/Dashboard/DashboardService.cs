using Dapper;
using MMEmergencyCall.Databases.AppDbContextModels;
using MMEmergencyCall.Databases.Dapper;
using MMEmergencyCall.Shared;
using System.Data;
using System.Text.Json;

namespace MMEmergencyCall.Domain.Admin.Features.Dashboard;

public class DashboardService
{
	private readonly DapperContext _dapperContext;
	public DashboardService(DapperContext dapperContext)
	{
		_dapperContext = dapperContext;
	}

	public async Task<Result<DashboardModel>> GetDashboard()
	{
		try
		{
			using IDbConnection connection = _dapperContext.CreateConnection();

			using var multi = await connection.QueryMultipleAsync(
					"sp_Dashboard_Process",
					new { UserId = 0 },
					commandType: CommandType.StoredProcedure);

			var dashboardData = new DashboardModel
			{
				RequestSummary = await multi.ReadFirstOrDefaultAsync<RequestSummaryModel>(),
				TopTenServicePerTownship = (await multi.ReadAsync<TopTenServicePerTownshipModel>()).AsList(),
				ServiceProviderActivity = (await multi.ReadAsync<ServiceProviderActivityModel>()).AsList(),
				TopTenRequestPerUser = (await multi.ReadAsync<TopTenRequestPerUserModel>()).AsList()
			};

			return Result<DashboardModel>.Success(dashboardData);

		}
		catch (Exception ex)
		{
			return Result<DashboardModel>.Failure(ex.ToString());
		}
	}
}

