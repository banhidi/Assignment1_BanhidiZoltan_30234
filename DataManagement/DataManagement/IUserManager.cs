using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement {
    public interface IUserManager {
        User? getUser(int id);
        User? getUser(string username);
        void deleteUser(int userId);
        void modifyUser(User userWithValidId);
        void addNewUser(User userWithoutId);
        IList<User> getAllUsers();
        IList<User> getAllRegularUsers();
        void deleteAllUsers();
    }
}
