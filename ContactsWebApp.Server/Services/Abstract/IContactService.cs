using ContactsWebApp.Server.Models;

namespace ContactsWebApp.Server.Services.Abstract
{
    public interface IContactService
    {
        void AddContact(Contact contact);
        void DeleteContact(int id);
        Contact GetContactById(int id);
        List<Contact> GetContacts(int userId);
        void UpdateContact(Contact contact);
    }
}
