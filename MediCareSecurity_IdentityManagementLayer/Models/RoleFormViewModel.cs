using System.ComponentModel.DataAnnotations;

namespace MediCareSecurity_IdentityManagementLayer.Models
{
    public class RoleFormViewModel
    {
        [Required, StringLength(50)]
        public string RoleName { get; set; }
    }
}
