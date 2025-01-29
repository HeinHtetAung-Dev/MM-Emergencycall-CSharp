using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MMEmergencyCall.Databases.Dapper;

public static class DapperExtension
{
	public static void AddDapperContext(this WebApplicationBuilder builder)
	{
		builder.Services.AddScoped(
			n => new DapperContext(builder.Configuration.GetConnectionString("DbConnection")!));
	}
}
