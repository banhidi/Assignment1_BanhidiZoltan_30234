using DataManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement {
    public class Order {
        private IList<ShoppingCartItem> shoppingCart;
        public int id { get; }
        public string customerName { get; set; }
        public string deliveryAdress { get; set; }
        public DateTime deliveryDate { get; set; }
        public bool orderStatus { get; set; }
        public int userId { get; set; }  

        public Order(int id) {
            this.id = id;
            shoppingCart = new List<ShoppingCartItem>();
        }

        public override string ToString() {
            string s = "id=" + id.ToString();
            s += ", customerName='" + customerName + "'";
            s += ", deliveryAdress='" + deliveryAdress + "'";
            s += ", deliveryDate=" + deliveryDate.ToString("yyyy-MM-dd");
            s += ", orderStatus=" + orderStatus.ToString();
            s += ", userId=" + userId.ToString();
            return s;
        }

        public Order(int id, IList<ShoppingCartItem> list) {
            this.id = id;
            shoppingCart = list;
        }

        public Order() { }
        
        public void addItem(ShoppingCartItem item) {
            if (shoppingCart.Contains(item))
                throw new ProductAlreadyInShoppingCartException();
            else 
                shoppingCart.Add(item);
        }

        private int getShoppingCartIndex(int productId) {
            int i;
            for (i = 0; i < shoppingCart.Count; i++) {
                if (shoppingCart[i].productId == productId)
                    break;
            }
            if (i > shoppingCart.Count)
                throw new ProductNotInShoppingCartException();
            else
                return i;
        }
        
        public void removeItem(int productId) {
           shoppingCart.RemoveAt(getShoppingCartIndex(productId));
        }
        
        public void modifyItemQuantity(ShoppingCartItem item) {
            int index = shoppingCart.IndexOf(item);
            if (index == -1)
                throw new ProductNotInShoppingCartException();
            else {
                ShoppingCartItem m = shoppingCart[index];
                m.Quantity = item.Quantity;
            }
        }
        
        public IList<ShoppingCartItem> getShoppingCart() {
            return shoppingCart;
        }                   
    }
}
