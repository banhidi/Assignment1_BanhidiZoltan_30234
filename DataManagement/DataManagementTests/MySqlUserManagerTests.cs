using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement.Tests {
    [TestClass()]
    public class MySqlUserManagerTests {
        private static User[] users;

        [ClassInitialize()]
        public static void classInit(TestContext tc) {
            users = new User[3];
            users[0] = new DataManagement.User("zoli", "ruifwfnwivnwinwi", false);
            users[1] = new User("noemi", "njvnwjvnwjv9204092vnjvnw", true);
            users[2] = new User("attila", "cdjvnwovnjvnwv", false);

            IUserManager mgr = new MySqlUserManager();
            mgr.addNewUser(users[0]);
            mgr.addNewUser(users[1]);
            mgr.addNewUser(users[2]);
        }

        [ClassCleanup()]
        public static void classCleanup() {
            IUserManager mgr = new MySqlUserManager();
            mgr.deleteAllUsers();
        }

        [TestMethod()]
        public void getUserTest_1() {
            IUserManager mgr = new MySqlUserManager();
            for (int i = 0; i < users.Length; i++) {
                User? nullableUser = mgr.getUser(users[i].username);
                Assert.IsNotNull(nullableUser);
                User u = (User) nullableUser;
                Assert.AreEqual(u.username, users[i].username);
                Assert.AreEqual(u.hashPassword, users[i].hashPassword);
                Assert.AreEqual(u.isAdmin, users[i].isAdmin);
            }
        }

        [TestMethod()]
        public void getUserTest_2() {
            IUserManager mgr = new MySqlUserManager();
            for(int i = 0; i < users.Length; i++) {
                User usr = users[i];
                User? u = mgr.getUser(usr.username);
                Assert.IsNotNull(u);
                User tmp = (User)u;
                usr.id = tmp.id;
                User? aux = mgr.getUser(((User) u).id);
                Assert.IsNotNull(aux);
                User usrDb = (User)aux;
                Assert.AreEqual<int>(usr.id, usrDb.id);
                Assert.AreEqual<string>(usr.username, usrDb.username);
                Assert.AreEqual<string>(usr.hashPassword, usrDb.hashPassword);
                Assert.AreEqual<bool>(usr.isAdmin, usrDb.isAdmin);
            }
        }


    }
}