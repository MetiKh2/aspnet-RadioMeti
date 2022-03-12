using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RadioMeti.Application.Interfaces;
using RadioMeti.Application.Services;
using RadioMeti.Persistance.context;
using RadioMeti.Site.Logging.Extensions;
var builder = WebApplication.CreateBuilder(args);
#region logging
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();
#endregion
// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
});
#region Database
builder.Services.AddDbContext<RadioMetiDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("RadioMetiConnection"));
});
#endregion
#region Account
builder.Services.AddIdentity<IdentityUser, IdentityRole>(op =>
{
    op.Password.RequireUppercase = false;
    op.Password.RequireNonAlphanumeric = false;
    op.Password.RequireLowercase = false;
    op.Password.RequireDigit = false;
    op.Password.RequiredLength = 6;

    op.User.RequireUniqueEmail = true;

    op.Lockout.MaxFailedAccessAttempts = 6;
    op.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);

}
            )
.AddEntityFrameworkStores<RadioMetiDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/AccessDenied";
    options.Cookie.Name = "RadioMeti";
    options.LoginPath = "/Login";
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
});

builder.Services.Configure<SecurityStampValidatorOptions>(option =>
{
    //option.ValidationInterval = TimeSpan.FromSeconds(5);
});

#endregion
#region IOC
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IMessageSender,MessageSender>();
builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
#endregion
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
