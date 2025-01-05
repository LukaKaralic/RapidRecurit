using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RapidRecruit.Data;
using RapidRecruit.Authorization;
using RapidRecruit.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using RapidRecruit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

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

    options.AddPolicy("BusinessOnly", policy =>
        policy.RequireClaim("AccountType", "Business"));
});

var identityBuilder = builder.Services.AddDefaultIdentity<UserAccount>(options =>
{
    options.SignIn.RequireConfirmedAccount = !builder.Environment.IsDevelopment();
});
identityBuilder.AddEntityFrameworkStores<ApplicationDbContext>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddTransient<IEmailSender, NoOpEmailSender>();
}
else
{
    builder.Services.AddTransient<IEmailSender, SendgridSender>();
}

// Claims transformation
builder.Services.AddScoped<IClaimsTransformation, AccountTypeClaimsTransformation>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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