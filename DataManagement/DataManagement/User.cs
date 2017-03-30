using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement {
    public struct User {
        public int id { get; set; }
        public string username { get; set; }
        public string hashPassword;
        public bool isAdmin { get; set; }

        public User(string uname, string hashPass, bool isAdmin) {
            id = -1;
            username = uname;
            hashPassword = hashPass;
            this.isAdmin = isAdmin;
        }
    }
}
