using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data;
using SchoolSystem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorCodesToAdd: null)));

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication("SchoolSystemCookie")
    .AddCookie("SchoolSystemCookie", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

app.UseExceptionHandler("/Home/Error");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

const int maxRetries = 10;
for (var retry = 0; retry < maxRetries; retry++)
{
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
        DbInitializer.Seed(db);
        break;
    }
    catch (Exception ex) when (retry < maxRetries - 1)
    {
        Console.WriteLine($"DB init attempt {retry + 1} failed: {ex.Message}. Retrying...");
        Thread.Sleep(3000);
    }
}

app.Run();
