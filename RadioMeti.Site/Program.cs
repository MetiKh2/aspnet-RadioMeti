using AutoMapper;
using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RadioMeti.Application.AutoMapper;
using RadioMeti.Application.DTOs.Enums;
using RadioMeti.Application.Interfaces;
using RadioMeti.Application.Services;
using RadioMeti.Persistance.context;
using RadioMeti.Persistance.Repository;

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
    option.ValidationInterval = TimeSpan.FromMinutes(5);
});

builder.Services.AddAuthorization(option =>
{
    #region Roles
    option.AddPolicy(PermissionsSite.IndexRoles.ToString().ToUpper(), policy => policy
            .RequireClaim(PermissionsSite.IndexRoles.ToString().ToUpper(), PermissionsSite.IndexRoles.ToString().ToUpper()));

    option.AddPolicy(PermissionsSite.CreateRole.ToString().ToUpper(), policy => policy
            .RequireClaim(PermissionsSite.CreateRole.ToString().ToUpper(), PermissionsSite.CreateRole.ToString().ToUpper()));

    option.AddPolicy(PermissionsSite.EditRole.ToString().ToUpper(), policy => policy
            .RequireClaim(PermissionsSite.EditRole.ToString().ToUpper(), PermissionsSite.EditRole.ToString().ToUpper()));

    option.AddPolicy(PermissionsSite.DeleteRole.ToString().ToUpper(), policy => policy
            .RequireClaim(PermissionsSite.DeleteRole.ToString().ToUpper(), PermissionsSite.DeleteRole.ToString().ToUpper()));
    #endregion  
    #region Users
    option.AddPolicy(PermissionsSite.IndexUsers.ToString().ToUpper(), policy => policy
            .RequireClaim(PermissionsSite.IndexUsers.ToString().ToUpper(), PermissionsSite.IndexUsers.ToString().ToUpper()));

    option.AddPolicy(PermissionsSite.CreateUser.ToString().ToUpper(), policy => policy
            .RequireClaim(PermissionsSite.CreateUser.ToString().ToUpper(), PermissionsSite.CreateUser.ToString().ToUpper()));

    option.AddPolicy(PermissionsSite.EditUser.ToString().ToUpper(), policy => policy
            .RequireClaim(PermissionsSite.EditUser.ToString().ToUpper(), PermissionsSite.EditUser.ToString().ToUpper()));

    option.AddPolicy(PermissionsSite.DeleteUser.ToString().ToUpper(), policy => policy
            .RequireClaim(PermissionsSite.DeleteUser.ToString().ToUpper(), PermissionsSite.DeleteUser.ToString().ToUpper()));
    #endregion
    #region Artists
    option.AddPolicy(PermissionsSite.IndexArtist.ToString().ToUpper(), policy => policy
            .RequireClaim(PermissionsSite.IndexArtist.ToString().ToUpper(), PermissionsSite.IndexArtist.ToString().ToUpper()));

    option.AddPolicy(PermissionsSite.CreateArtist.ToString().ToUpper(), policy => policy
            .RequireClaim(PermissionsSite.CreateArtist.ToString().ToUpper(), PermissionsSite.CreateArtist.ToString().ToUpper()));

    option.AddPolicy(PermissionsSite.EditArtist.ToString().ToUpper(), policy => policy
            .RequireClaim(PermissionsSite.EditArtist.ToString().ToUpper(), PermissionsSite.EditArtist.ToString().ToUpper()));

    option.AddPolicy(PermissionsSite.DeleteArtist.ToString().ToUpper(), policy => policy
            .RequireClaim(PermissionsSite.DeleteArtist.ToString().ToUpper(), PermissionsSite.DeleteArtist.ToString().ToUpper()));
    #endregion
});

#endregion
#region IOC
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IMessageSender,MessageSender>();
builder.Services.AddScoped<IPermissionService,PermissionService>();
builder.Services.AddScoped<IMusicService, MusicService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<IProdcastService, ProdcastService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IVideoService,VideoService>();
builder.Services.AddScoped<IArtistService,ArtistService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
#endregion
#region Auto Mapper
builder.Services.AddAutoMapper(typeof(CustomProfile));

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
   name: "areas",
   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
 );
});
app.Run();
 