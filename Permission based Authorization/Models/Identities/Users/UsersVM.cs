namespace Permission_based_Authorization.Models.Identities.Users
{
    public class UsersVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
