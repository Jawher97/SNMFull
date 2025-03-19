using Microsoft.AspNetCore.Http.Features;
using Polly.Extensions.Http;
using Polly;
using Serilog;
using Serilog.Formatting.Json;
using SNM.Publishing.Aggregator.Configurations;
using SNM.Publishing.Aggregator.Configurations.Extensions;
using SNM.Publishing.Aggregator.Interfaces;
using SNM.Publishing.Aggregator.Services;
using Microsoft.EntityFrameworkCore;
using SNM.Publishing.Aggregator.DataContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
SerilogConfiguration.AddSerilogApi();
builder.Host.UseSerilog();

//builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();
//builder.Services.AddHealthChecks(builder.Configuration);

builder.Services.AddApplicationServices(builder.Configuration);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.ConfigureCors();
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new JsonFormatter())
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddScoped<IPostInstagramServices, PostInstagramServices>();
builder.Services.AddScoped<IPostTwitterServices, PostTwitterService>();
builder.Services.AddScoped<IPostLinkedInService, PostLinkedInService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
