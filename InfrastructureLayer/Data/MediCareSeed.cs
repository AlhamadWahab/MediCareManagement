//using MediCareSecurity_IdentityManagementLayer;
//using MediCareSecurity_IdentityManagementLayer.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.DependencyInjection;

//namespace InfrastructureLayer.Data
//{
//    public class MediCareSeed
//    {
//        public static async Task SeedManagerUserAsync(IServiceProvider serviceProvider)
//        {
//            var userManager = serviceProvider.GetRequiredService<UserManager<MediCareAppUser>>();
//            var user = await userManager.FindByEmailAsync("dev70tec.manager@gmail.com");
//            if (user is not null)
//            {
//                await userManager.AddToRoleAsync(user, UserRole.ManagerRole);
//            }
//        }
//    }
//}
