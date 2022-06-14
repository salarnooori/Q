using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Q.Areas.Identity.Data;
using Q.Data;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("QUserContextConnection") ?? throw new InvalidOperationException("Connection string 'QUserContextConnection' not found.");

builder.Services.AddDbContext<QUserContext>(options =>
    options.UseSqlServer(connectionString));;

builder.Services.AddDefaultIdentity<QUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<QUserContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();
builder.Services.AddDbContext<QTaskContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QTaskContext") ?? throw new InvalidOperationException("Connection string 'QTaskContext' not found.")));

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

app.MapRazorPages();

app.Run();
