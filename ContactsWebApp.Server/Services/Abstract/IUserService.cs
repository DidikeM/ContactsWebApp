using ContactsWebApp.Shared.Models;

namespace ContactsWebApp.Server.Services.Abstract
{
    public interface IUserService
    {
        bool CheckPassword(User user, string password);
        User GetUserByEmail(string email);
    }
}
