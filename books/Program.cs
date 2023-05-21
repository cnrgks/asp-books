using Microsoft.EntityFrameworkCore;
using books.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies; // Auth için eklendi

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connetionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<KitapDbContext>(options => options.UseMySql(connetionString, ServerVersion.AutoDetect(connetionString)));

//Auth için eklendi
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/Admin/Login");
//

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//Auth için eklendi
app.UseSession();
app.UseCookiePolicy();
app.UseAuthentication();
//

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Admin",
    pattern: "{controller=Admin}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Turler",
    pattern: "{controller=Turler}/{action=Index}/{id?}");

app.Run();
