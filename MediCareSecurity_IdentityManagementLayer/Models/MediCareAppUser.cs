using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MediCareSecurity_IdentityManagementLayer.Models
{
    public class MediCareAppUser : IdentityUser
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        public byte[]? ProfilPicture { get; set; } = null;
    }
}
