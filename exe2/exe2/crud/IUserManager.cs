using exe2.user;

namespace exe2.crud;

public interface IUserManager
{
    void AddUser(User user);
    IEnumerable<User> GetAllUsers();
}
