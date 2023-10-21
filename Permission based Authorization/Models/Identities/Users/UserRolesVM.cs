namespace Permission_based_Authorization.Models.Identities.Users
{
    public class UserRolesVM
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<RoleVM> Roles { get; set; }
    }
}
