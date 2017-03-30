using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Exceptions {
    public class InvalidProductTitleException : Exception {
        public InvalidProductTitleException() :
            base("The title must be unique in the table and must have at least 3 characters") { }

        public InvalidProductTitleException(string message) : base(message) { }
    }

    public class InvalidProductSizeException : Exception {
        public InvalidProductSizeException() :
            base("The size attributes (height, width, depth) of a " +
                "product must be between 0..1000 [cm]") { }

        public InvalidProductSizeException(string message) : base(message) { }
    }

    public class InvalidProductPriceException : Exception {
        public InvalidProductPriceException() : base("The price must be greather or equal than 0") { }

        public InvalidProductPriceException(string message) : base(message) { }
    }

    public class InvalidProductStockException : Exception {
        public InvalidProductStockException() : base("The stock must be greather or equal than 0") { }

        public InvalidProductStockException(string message) : base(message) { }
    }
}
