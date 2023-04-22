using Microsoft.EntityFrameworkCore;
using OrganicShop2.Data;
using OrganicShop2.Helpers;
using OrganicShop2.Interfaces;
using OrganicShop2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddDbContext<Db>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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

app.UseEndpoints(endpoints =>
{
    //endpoints.MapControllerRoute(
    //    name: "default",
    //    pattern: "UserPages/SidebarPartial",
    //    defaults: new { controller = "UserPages", action = "Index" });

    //endpoints.MapControllerRoute(
    //   name: "PagesMenuPartial",
    //   pattern: "UserPages/PagesMenuPartial",
    //   defaults: new { controller = "UserPages", action = "PagesMenuPartial" });

    //endpoints.MapControllerRoute(
    //   name: "pages",
    //   pattern: "{controller=UserPages}/{action=Index}/{id?}");




    endpoints.MapControllerRoute(
      name: "default",
      pattern: "{controller=UserPages}/{action=Index}/{id?}");


    //endpoints.MapControllerRoute(
    //        name: "custom",
    //        pattern: "{controller=UserPages}/{action=Index2}/{id?}");


    //endpoints.MapControllerRoute(
    //  name: "default",
    //  pattern: "{controller=UserPages}/{action=_SidebarPartial}/{id?}");

});

//чат сказал это удалить
//app.MapControllerRoute(
//    //name: "default", pattern: "{controller=Pages}/{action=Index}/{id?}");
//name: "default", pattern: "{controller=UserPages}/{action=SidebarPartial}/{id?}");



app.Run();
