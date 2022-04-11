using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using StarterProject.Context;
using StarterProject.Context.Contexts;
using System.Globalization;

var cultureInfo = new CultureInfo("en-US");
CultureInfo.CurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

//configure entity framework with .Net
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer("Server=.\\MSSQLSERVER01;Database=AmazonasPizza;Trusted_Connection=True;"));

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(options =>
             {
                 options.AccessDeniedPath = "/Account/Denied";
                 options.LoginPath = "/Account/Login";
                 options.SlidingExpiration = true;
             });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization(GetCulture());
app.UseRouting();
app.UseStatusCodePagesWithReExecute("/home/error/{0}");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

DbInitializer.Init();

app.Run();

RequestLocalizationOptions GetCulture()
{
    var defaultCulture = new CultureInfo("en-US");
    return new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture(defaultCulture),
        SupportedCultures = new List<CultureInfo> { defaultCulture },
        SupportedUICultures = new List<CultureInfo> { defaultCulture }
    };
}