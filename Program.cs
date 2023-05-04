using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrganicShop2.Data;
using OrganicShop2.Helpers;
using OrganicShop2.Interfaces;
using OrganicShop2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddDbContext<Db>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();///???



var app = builder.Build();

app.UseSession();

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

app.UseEndpoints(endpoints =>
{


    endpoints.MapControllerRoute(
     name: "default",
     pattern: "{controller=UserPages}/{action=Index}/{id?}");


    //endpoints.MapControllerRoute(
    //	 name: "Cart",
    //	 pattern: "Cart/{action}/{id?}",
    //	 defaults: new { controller = "Cart", action = "Index" }
    // );

});

app.Run();
