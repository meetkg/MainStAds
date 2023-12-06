using MainStAds.Core.Interfaces;
using MainStAds.Infrastructure.Data;
using MainStAds.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MainStAds.Application.Mappings;
using Serilog;  // <-- Add this for Serilog
using Serilog.Events;  // <-- Add this to specify log event levels

var builder = WebApplication.CreateBuilder(args);

// Initialize and configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)  // Minimize log verbosity from Microsoft components
    .Enrich.FromLogContext()  // Include contextual information
    .WriteTo.Console()  // Output logs to the console
                        // Add additional sinks if needed, e.g., .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try  // <-- Wrap the code in a try-catch to log any startup failures
{
    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddDbContext<MainStAdsDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("MainStAdsConnection")));
    builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
    builder.Services.AddAutoMapper(typeof(BusinessProfile).Assembly);

    var app = builder.Build();

    
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)  // <-- Catch any exceptions during startup
{
    Log.Fatal(ex, "Application start-up failed");  // <-- Log the failure
    throw;
}
finally
{
    Log.CloseAndFlush();  // <-- Ensure all log entries are flushed before application exit
}
