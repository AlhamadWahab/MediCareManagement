using BusinessLogicLayer.Profiles;
using DomainLayer.Interfaces.Bases_;
using InfrastructureLayer.Data;
using InfrastructureLayer.Repositories;
using MediCareSecurity_IdentityManagementLayer;
using MediCareSecurity_IdentityManagementLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/// Add services to the container.
builder.Services.AddControllersWithViews();

#region MediCare DbContext registeration
string? mediCareConnection = builder.Configuration.GetConnectionString("MediCareDbContextConnection");
builder.Services.AddDbContext<MediCareDbContext>(op => op.UseSqlServer(mediCareConnection ?? ""));
#endregion

#region Identity registration
builder.Services.AddIdentity<MediCareAppUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;

        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // User settings
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = false;

    }).AddEntityFrameworkStores<MediCareDbContext>().AddDefaultTokenProviders()
    .AddDefaultUI();
builder.Services.ConfigureApplicationCookie(op =>
{
    op.LoginPath = $"/Identity/Account/Login";
    op.LogoutPath = $"/Identity/Account/Logout";
    op.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
#endregion

#region MediCare Services registeration
builder.Services.AddScoped(typeof(IService<>), typeof(MainService<>));
#endregion

#region MediCare Repositories (Unit Of Work) registeration;
builder.Services.AddTransient<IRepository, MainRepository>();
#endregion

#region Auto Mapper and DTOs
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
#endregion

#region Email Sender registration
builder.Services.AddScoped<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, EmailSender>();
#endregion

#region قد يحدث خطأ عند محاولة تسلسل هذه الكائنات عند العمل مع كائنات تحتوي على مراجع دائرية
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });
#endregion


/// Add Razor Pages services.
builder.Services.AddRazorPages();

var app = builder.Build();

#region MediCare Seed registeration
using (var scope = app.Services.CreateScope())
    //{
    //    var services = scope.ServiceProvider;
    //    MediCareSeed.SeedManagerUserAsync(services).Wait();
    //}
    #endregion

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

app.MapRazorPages(); /// with this addition the program will read Register and Login Pages.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();

//// Seed roles asynchronously and user asynchronously
//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    await SeedRoles(roleManager);

//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
//    await SeedUser(userManager); // Seed 

//    /// Method to Seed Roles
//    async Task SeedRoles(RoleManager<IdentityRole> roleManager)
//    {
//        var roles = new[] { UserRole.PatientRole, UserRole.DoctorRole, UserRole.ManagerRole };

//        foreach (var role in roles)
//        {
//            if (!await roleManager.RoleExistsAsync(role))
//            {
//                await roleManager.CreateAsync(new IdentityRole(role));
//            }
//        }
//    }

//    /// Method to Seed User
//    async Task SeedUser(UserManager<IdentityUser> userManager)
//    {
//        string email = "manager.admin@gmx.com";
//        string password = "Tr@RS7!87";

//        var user = await userManager.FindByEmailAsync(email);
//        if (user == null)
//        {
//            user = new IdentityUser()
//            {
//                UserName = email,
//                Email = email,
//                EmailConfirmed = true,// just temperory**********
//            };
//            var result = await userManager.CreateAsync(user, password);
//            if (result.Succeeded)
//            {
//                await userManager.AddToRoleAsync(user, UserRole.ManagerRole);
//            }
//            else
//            {
//                // Log or handle error here if user creation fails
//                foreach (var error in result.Errors)
//                {
//                    Console.WriteLine($"Error: {error.Description}");
//                }
//            }
//        }
//    }




//NAMEQ