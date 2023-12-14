using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PestKitAB104.DAL;
using PestKitAB104.Interfaces;
using PestKitAB104.Middlewares;
using PestKitAB104.Models;
using PestKitAB104.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt =>
   opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);
builder.Services.AddScoped<LayoutService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options =>
options.IdleTimeout = TimeSpan.FromSeconds(50)
);

builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
	options.Password.RequiredLength = 8;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireUppercase = true;

	options.User.RequireUniqueEmail = true;

	options.Lockout.MaxFailedAccessAttempts = 3;
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
}
).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

var app = builder.Build();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllerRoute(
    "Admin",
    "{area:exists}/{controller=home}/{action=index}/{id?}"
    );
app.MapControllerRoute(
    "Default",
    "{controller=home}/{action=index}/{id?}"
    );
app.Run();
