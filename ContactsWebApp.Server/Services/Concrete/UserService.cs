using ContactsWebApp.Server.Services.Abstract;
using ContactsWebApp.Shared.Models;

namespace ContactsWebApp.Server.Services.Concrete
{
    public class UserService : IUserService
    {
        public bool CheckPassword(User user, string password)
        {
            return true;
        }

        public User GetUserByEmail(string email)
        {
            return new User
            {
                Id = 1,
                Email = "asd@asd.com",
                Password = "asd",
                Name = "asd",
                Surname = "asd"
            };
        }
    }
}
