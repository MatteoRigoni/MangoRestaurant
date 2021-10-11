using Duende.IdentityServer.Services;
using Mango.Services.Identity;
using Mango.Services.Identity.Data;
using Mango.Services.Identity.Models;
using Mango.Services.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuration EF
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IProfileService, ProfileService>();

// Configuration IdentityServer
var builderIdentity = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
})
    .AddInMemoryIdentityResources(Consts.IdentityResources)
    .AddInMemoryApiScopes(Consts.ApiScopes)
    .AddInMemoryClients(Consts.Clients)
    .AddAspNetIdentity<ApplicationUser>();

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
    builderIdentity.AddDeveloperSigningCredential();
else
    throw new Exception("Need to configure signing credentials");

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
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

 using (var scope = app.Services.CreateScope())
{
    // automatically apply migrations
    var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
    db.Database.Migrate();

    // seed roles and admin user
    var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();
    dbInitializer.Initialize();
}

app.Run();
