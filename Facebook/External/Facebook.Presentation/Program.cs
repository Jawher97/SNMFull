using SNS.Facebook.Application;
using SNS.Facebook.Infrastructure;
using SNS.Facebook.Infrastructure.DataContext;
using SNS.Facebook.Presentation.Configurations;
using SNS.Facebook.Presentation.Configurations.Extensions;
using SNS.Facebook.Presentation.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
SerilogConfiguration.AddSerilogApi();
builder.Host.UseSerilog();
//builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();
builder.Services.ConfigureCors();
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}
//app.UseStaticFiles(new StaticFileOptions()
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"" + builder.Configuration["UploadsFiles"] + "")),
//    RequestPath = new PathString("/" + builder.Configuration["UploadsFiles"] + "")
//});

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MigrateDatabase<ApplicationDbContext>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
