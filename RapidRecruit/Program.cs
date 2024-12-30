using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RapidRecruit.Data;
using RapidRecruit.Authorization;
using RapidRecruit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
));

// Authorization
builder.Services.AddScoped<IAuthorizationHandler, OwnerHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OwnerPolicy", policy =>
        policy.Requirements.Add(new OwnerRequirement()));
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BusinessOnly", policy =>
        policy.RequireClaim("AccountType", "Business"));
});

builder.Services.AddDefaultIdentity<UserAccount>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

// After your AddDefaultIdentity call, add:
builder.Services.AddScoped<IClaimsTransformation, AccountTypeClaimsTransformation>(); 
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();