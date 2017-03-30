using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Exceptions {
    public class DifferentPasswordsException : System.Exception {
        public DifferentPasswordsException() : base("Different passwords were given") { }

        public DifferentPasswordsException(string msg) : base(msg) { }
    }

    public class InvalidUsernameException : System.Exception {
        public InvalidUsernameException() : 
            base("The username must have at least 6 characters, and not used by another user") { }

        public InvalidUsernameException(string msg) : base(msg) { }
    }

    public class InvalidPasswordException : System.Exception {
        public InvalidPasswordException() : base("The given password is not correct.") { }

        public InvalidPasswordException(string message) : base(message) { }
    }
}
