using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement {
    public interface IShoppingCartManager {
        void addNewShoppingCartItem(ShoppingCartItem s, int orderId);
        void removeShoppingCartItem(int orderId, int productId);
        void modifyShoppingCartItem(ShoppingCartItem s, int orderId);
        IList<ShoppingCartItem> getShoppingCart(int orderId);
    }
}
