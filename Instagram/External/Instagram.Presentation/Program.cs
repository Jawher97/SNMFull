using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SNM.Instagram.Application;
using SNM.Instagram.Infrastructure;
using SNM.Instagram.Infrastructure.DataContext;
using SNM.Instagram.Presentation.Configurations;
using SNM.Instagram.Presentation.Configurations.Extensions;
using SNM.Instagram.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
SerilogConfiguration.AddSerilogApi();
builder.Host.UseSerilog();
builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();
builder.Services.ConfigureCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MigrateDatabase<ApplicationDbContext>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
