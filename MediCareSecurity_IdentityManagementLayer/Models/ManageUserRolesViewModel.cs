namespace MediCareSecurity_IdentityManagementLayer.Models
{
    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<CheckboxRolesOfUserViewModel> Roles { get; set; }
    }
}
