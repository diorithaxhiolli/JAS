using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using JAS.Models;
using JAS.Areas.Identity.Data;
using JAS.Controllers;
using System.Drawing.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//DB Connection
builder.Services.AddDbContext<JASContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("JAS")));

builder.Services.AddDefaultIdentity<JASUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<JASContext>();

builder.Services.AddScoped<IRoleStore<IdentityRole>, RoleStore<IdentityRole, JASContext>>();
builder.Services.AddScoped<UserManager<JASUser>>();

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

app.MapRazorPages();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
