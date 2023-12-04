using Microsoft.EntityFrameworkCore;
using PestKitAB104.DAL;
using PestKitAB104.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt =>
   opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);
builder.Services.AddScoped<LayoutService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options =>
options.IdleTimeout = TimeSpan.FromSeconds(50)
);
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    "Admin",
    "{area:exists}/{controller=home}/{action=index}/{id?}"
    );
app.MapControllerRoute(
    "Default",
    "{controller=home}/{action=index}/{id?}"
    );
app.Run();
