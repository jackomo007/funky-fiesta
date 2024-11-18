using Lil.TimeTracker.Auth;
using Lil.TimeTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication().AddScheme<APIKeyOptions, APIKeyAuthHandler>("APIKEY", o =>o.DisplayMessage = "API Key Authenticator");
builder.Services.AddDbContext<TimeTrackerDbContext>(options=>
    options.UseSqlite(builder.Configuration.GetConnectionString("TrackerDbContext"))
);
builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<TimeTrackerDbContext>();

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

app.MapGroup("identity").MapIdentityApi<IdentityUser>();

app.Run();
