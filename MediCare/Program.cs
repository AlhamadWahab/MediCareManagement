using BusinessLogicLayer.Profiles;
using BusinessLogicLayer.Service_Pattern.Doctor_Service;
using DomainLayer.Interfaces;
using DomainLayer.Interfaces.Bases_;
using InfrastructureLayer.Data;
using InfrastructureLayer.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/// Add services to the container.
builder.Services.AddControllersWithViews();

#region MediCare DbContext registeration:
string? mediCareConnection = builder.Configuration.GetConnectionString("MediCare_db");
builder.Services.AddDbContext<MediCareDbContext>(op => op.UseInMemoryDatabase(mediCareConnection ?? ""));
#endregion

#region MediCare Services registeration:
builder.Services.AddScoped(typeof(IService<>), typeof(MainService<>));
builder.Services.AddScoped(typeof(IDoctorService), typeof(DoctorService));
#endregion

#region MediCare Repositories (Unit Of Work) registeration;
builder.Services.AddTransient<IRepository, MainRepository>();
#endregion

#region Auto Mapper and DTOs
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
#endregion

/// Add Razor Pages services.
builder.Services.AddRazorPages();

var app = builder.Build();

/// Configure the HTTP request pipeline.
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

app.Run();