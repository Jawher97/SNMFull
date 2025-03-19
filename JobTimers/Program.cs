using Hangfire;
using Hangfire.MySql;
using JobTimers;
using JobTimers.Services;
using JobTimers.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<ITenantService, TenantService>();
// HttpContext  It contains information about the current request
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddTransient<IHangfireTenantProvider, HangfireTenantProvider>();
// how call somthing exist in appseting.json
builder.Services.Configure<TenantSettings>(builder.Configuration.GetSection("TenantSettings"));

var tenantSettings = builder.Configuration.GetSection("TenantSettings").Get<TenantSettings>();
// configure connexion db
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//var connectionStringHngfire = builder.Configuration.GetConnectionString("HangfireConnection");

//var defaultDbProvider = tenantSettings.Defaults.DBProvider;
//if (defaultDbProvider.ToLower() == "mysql")
//{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseMySql(ServerVersion.AutoDetect(tenantSettings.Defaults.ConnectionString)));
   
//}

//foreach (var tenant in tenantSettings.Tenants)
//{
   
//    var connectionString = tenant.ConnectionString;


//    // Create a new scope for each tenant-specific ApplicationDbContext
//    using var scope = builder.Services.BuildServiceProvider().CreateScope();

//    // Resolve the ApplicationDbContext for the current tenant
//    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

//    // Set the connection string for the DbContext
//    dbContext.Database.SetConnectionString(connectionString);

 

//    if (dbContext.Database.GetPendingMigrations().Any())
//    {
//        dbContext.Database.Migrate();
//    }
//}

builder.Services.AddTransient<IServices, Services>();

// Inside your Startup.cs



builder.Services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseStorage(
                new MySqlStorage(
                   tenantSettings.Defaults.ConnectionString,
                    new MySqlStorageOptions
                    {
                        QueuePollInterval = TimeSpan.FromSeconds(15),
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        PrepareSchemaIfNecessary = true,
                        DashboardJobListLimit = 50000,
                        TransactionTimeout = TimeSpan.FromMinutes(1),
                        TablesPrefix = "Hangfire",                      

                    })
                )
           
             );



builder.Services.AddHangfireServer(  );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHangfireDashboard();
//

//

app.UseHttpsRedirection();

     

app.UseAuthorization();
// TimeZoneInfo.Local
//var manager = new RecurringJobManager();
//// create job per tenant
//foreach (var tenant in tenantSettings.Tenants)
//{

//    manager.AddOrUpdate<IServices>(
//             $"{tenant.Tid}",                // Unique identifier for the job
//          x => x.SchedulePosts(tenant.Tid), // Method representing the job logic
//       Cron.Minutely                        // Schedule: Run daily (you can adjust the schedule as needed)

//     );
//    RecurringJob.TriggerJob($"{tenant.Tid}");
//}

app.MapControllers();

app.Run();
