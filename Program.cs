using CareHomeInfoTracker.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Authentication Service
builder.Services.AddAuthentication().AddCookie("CookieAuth", options => 
{
    options.Cookie.Name = "CookieAuth";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin"));
});

//Database Connection
var connectionString = builder.Configuration["ConnectionStrings:CareHomeInfoContext"];
builder.Services.AddDbContext<CareHomeInfoContext>(
        options => options.UseNpgsql(connectionString)
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
