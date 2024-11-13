using Todo.API.Helpers;

namespace Todo.API.Data.Extensions
{
    public class InitialData
    {
        public static IEnumerable<UserModel> GetUsers()
        {
           (string passwordHash, string Salt) = FunctionHelpers.HashPassword(Utils.PasswordByDefault);
            return new List<UserModel>()
            {
                new UserModel () { FullName = "Admin", Role = UserRole.Admin, Email = Utils.EmailByDefault, Password = passwordHash, Salt =Salt}
            }; 
    }
        
    }
}
