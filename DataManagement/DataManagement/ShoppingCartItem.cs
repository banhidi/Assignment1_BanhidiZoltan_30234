using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement {
    public struct ShoppingCartItem {
        private Product product;
        private int quantity;
        public int Quantity {
            get { return quantity; }

            set {
                if (quantity <= 0)
                    throw new Exceptions.IllegalQuantityException();
                else
                    quantity = value;
            }
        }

        public float unitPrice {
            get { return product.price; }
        }

        public string Title {
            get { return product.title; }
        }

        public int productId {
            get {
                return product.id;
            }
        }

        public ShoppingCartItem(Product p, int quantity) {
            product = p;
            if (quantity <= 0)
                throw new Exceptions.IllegalQuantityException();
            else
                this.quantity = quantity;
        }

        public override bool Equals(object obj) {
            if (obj is ShoppingCartItem) {
                ShoppingCartItem s = (ShoppingCartItem)obj;
                return s.productId == productId;
            } else
                return false;
        }

    }
}
