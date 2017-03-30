using System;

namespace DataManagement.Exceptions {
    public class MySqlConnectionException : Exception {
        public MySqlConnectionException() { }

        public MySqlConnectionException(string message) : base(message) { }
        
    }

    public class ProductAlreadyInShoppingCartException : Exception {
        public ProductAlreadyInShoppingCartException() :
            base("This product is already present in the shopping cart") { }

        public ProductAlreadyInShoppingCartException(string msg) : base(msg) { }
    }

    public class IllegalQuantityException : Exception {
        public IllegalQuantityException() :
            base("The quantity must be greater than zero") { }

        public IllegalQuantityException(string msg) : base(msg) { }
    }

    public class ProductNotInShoppingCartException : Exception {
        public ProductNotInShoppingCartException() :
            base("The specified product is not in the shopping cart") { }

        public ProductNotInShoppingCartException(string msg) : base(msg) { }
    }
}
