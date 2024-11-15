using MMEmergencyCall.Domain.Admin.Features.EmergencyServices;
using MMEmergencyCall.Domain.Admin.Features.Townships;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
},
ServiceLifetime.Transient,
ServiceLifetime.Transient);

builder.AddRegisterService();
builder.AddAdminRegisterService();
builder.AddEmergencyServiceService();
builder.AddEmergencyServiceType();
builder.AddEmergencyRequest();
builder.AddSigninService();
builder.AddAdminSignInService();
builder.AddStateRegionService();
builder.AddUserService();
builder.AddAdminEmergencyServicesService();
builder.AddTownshipService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
