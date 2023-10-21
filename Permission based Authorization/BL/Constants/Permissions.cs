namespace Permission_based_Authorization.BL.Constants
{
    public static class Permissions
    {
        public static async Task<List<string>> GeneratedPermissionList (this string module)
        {
            return new List<string>()
            {
                $"Permission.{module}.View",
                $"Permission.{module}.Create",
                $"Permission.{module}.Edite",
                $"Permission.{module}.Delete"
            };
        } 
    }
}
