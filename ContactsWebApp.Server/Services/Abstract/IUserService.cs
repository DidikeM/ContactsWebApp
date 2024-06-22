using ContactsWebApp.Server.Models;

namespace ContactsWebApp.Server.Services.Abstract
{
    public interface IUserService
    {
        bool CheckPassword(User user, string password);
        bool CreateUser(User user);
        User GetUserByEmail(string email);
    }
}
