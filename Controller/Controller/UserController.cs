using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataManagement;

namespace Controller {
    public class UserController {

        public void addNewUser(string username, string password1, string password2, bool isAdmin) {
            if (!password1.Equals(password2))
                throw new Exceptions.DifferentPasswordsException();
            IUserManager mgr = new MySqlUserManager();
            if (username.Length < 6 || mgr.getUser(username) != null)
                throw new Exceptions.InvalidUsernameException();
            ISecurity s = new Security();
            string hashPassword = s.applyHashFunction(password1);
            mgr.addNewUser(new User(username, hashPassword, isAdmin));
        }
        
        public void modifyUser(int id, string username, bool isAdmin) {
            IUserManager mgr = new MySqlUserManager();
            User? usr = mgr.getUser(username);
            if (usr == null) {
                if (username.Length >= 6) {
                    usr = mgr.getUser(id);
                    User u = (User)usr;
                    u.username = username;
                    u.isAdmin = isAdmin;
                    mgr.modifyUser(u);
                } else
                    throw new Exceptions.InvalidUsernameException();
            } else {
                User u = (User)usr;
                if (u.id != id)
                    throw new Exceptions.InvalidUsernameException();
                else {
                    u.isAdmin = isAdmin;
                    mgr.modifyUser(u);
                }
            }
        } 

        public void deleteUser(int userId) {
            IUserManager mgr = new MySqlUserManager();
            mgr.deleteUser(userId);
        }

        public bool isValidAdmin(string username, string password) {
            IUserManager mgr = new MySqlUserManager();
            User? u = mgr.getUser(username);
            if (u == null)
                return false;
            else {
                ISecurity s = new Security();
                string hashPassword = s.applyHashFunction(password);
                User usr = (User)u;
                return usr.hashPassword.Equals(hashPassword) && usr.isAdmin;
            }
        }

        public bool isValidRegularUser(string username, string password) {
            IUserManager mgr = new MySqlUserManager();
            User? u = mgr.getUser(username);
            if (u == null)
                return false;
            else {
                User usr = (User)u;
                ISecurity s = new Security();
                string hashPassword = s.applyHashFunction(password);
                return usr.hashPassword.Equals(hashPassword) && !usr.isAdmin;
            }
        }

        public void modifyAccount(string oldPassword, int id, string username, 
                string password1, string password2) {
            IUserManager mgr = new MySqlUserManager();
            User u = (User)mgr.getUser(id);
            ISecurity s = new Security();
            if (s.applyHashFunction(oldPassword).Equals(u.hashPassword)) {
                User? t = mgr.getUser(username);
                if (t == null) {
                    if (password1.Equals(password2)) {
                        if (username.Length >= 6) {
                            User tmp = new User(username, s.applyHashFunction(password1), u.isAdmin);
                            tmp.id = id;
                            mgr.modifyUser(tmp);
                        } else
                            throw new Exceptions.InvalidUsernameException();
                    } else
                        throw new Exceptions.DifferentPasswordsException();
                } else if (((User)t).id == id) {
                    if (password1.Equals(password2)) {
                        User tmp = new User(username, s.applyHashFunction(password1), u.isAdmin);
                        tmp.id = id;
                        mgr.modifyUser(tmp);
                    } else
                        throw new Exceptions.DifferentPasswordsException();
                } else
                    throw new Exceptions.InvalidUsernameException();
            } else
                throw new Exceptions.InvalidPasswordException();
        }

    }    
}
