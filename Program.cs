using CareHomeInfoTracker.Data;
using CareHomeInfoTracker.Services.ImageFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// File Upload service.
builder.Services.AddScoped<IFileUploadService, FileUploadService>();

// Authentication Service
builder.Services.AddAuthentication().AddCookie("CookieAuth", options => 
{
    options.Cookie.Name = "CookieAuth";
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
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
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider( Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/Images"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"D:\\MeroBucket"),
    RequestPath = "/Bucket"
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
