using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataManagement;

namespace Controller {
    public class OrderController {
        public IList<Order> getOrders(string username) {
            IUserManager userMgr = new MySqlUserManager();
            User user = (User)userMgr.getUser(username);
            IOrderManager orderMgr = new MySqlOrderManager();
            return orderMgr.getOrders(user.id);
        }

        public void addNewOrder(string customerName, string adress,DateTime date, bool orderStatus, string username) {
            IUserManager userMgr = new MySqlUserManager();
            User user = (User)userMgr.getUser(username);

            Order o = new Order();
            o.customerName = customerName;
            o.deliveryAdress = adress;
            o.deliveryDate = date;
            o.orderStatus = orderStatus;
            o.userId = user.id;

            IOrderManager orderMgr = new MySqlOrderManager();
            orderMgr.addNewOrder(o);
        }

        public void deleteOrder(int orderId) {
            IShoppingCartManager cartMgr = new MySqlShoppingCartManager();
            IList<ShoppingCartItem> list = cartMgr.getShoppingCart(orderId);
            foreach (ShoppingCartItem s in list)
                cartMgr.removeShoppingCartItem(orderId, s.productId);

            IOrderManager orderMgr = new MySqlOrderManager();
            orderMgr.removeOrder(orderId);
        }

        public void modifyOrder(int orderId, string customerName, string adress, DateTime date, bool orderStatus) {
            IOrderManager mgr = new MySqlOrderManager();
            Order o = new Order(orderId);
            o.customerName = customerName;
            o.deliveryAdress = adress;
            o.deliveryDate = date;
            o.orderStatus = orderStatus;
            
            mgr.modifyOrder(o);
        }

        public void addProductToOrder(int productId, int quantity, int orderId) {
            IProductManager productMgr = new MySqlProductManager();
            Product p = (Product)productMgr.getProduct(productId);

            if (quantity > p.stock)
                throw new DataManagement.Exceptions.IllegalQuantityException();

            ShoppingCartItem s = new ShoppingCartItem(p, quantity);

            IOrderManager orderMgr = new MySqlOrderManager();
            Order o = orderMgr.getOrder(orderId);
            o.addItem(s);

            IShoppingCartManager cartMgr = new MySqlShoppingCartManager();
            cartMgr.addNewShoppingCartItem(s, orderId);

            p.stock -= quantity;
            productMgr.modifyProduct(p);
        }

        public void modifyProductInShoppingCart(int productId, int quantity, int orderId) {
            IProductManager productMgr = new MySqlProductManager();
            Product p = (Product)productMgr.getProduct(productId);

            IShoppingCartManager cartMgr = new MySqlShoppingCartManager();
            ShoppingCartItem s = new ShoppingCartItem();
            foreach(ShoppingCartItem item in cartMgr.getShoppingCart(orderId))
                if (item.productId == productId) {
                    s = item;
                    break;
                }

            if (quantity > p.stock + s.Quantity || quantity <= 0)
                throw new DataManagement.Exceptions.IllegalQuantityException();

            p.stock += s.Quantity - quantity;
            s.Quantity = quantity;

            cartMgr.modifyShoppingCartItem(s, orderId);
            productMgr.modifyProduct(p);
        }

        public void removeProductFromShoppingCart(int productId, int orderId) {
            IProductManager productMgr = new MySqlProductManager();
            Product p = (Product)productMgr.getProduct(productId);

            IShoppingCartManager cartMgr = new MySqlShoppingCartManager();
            ShoppingCartItem s = new ShoppingCartItem();
            foreach (ShoppingCartItem item in cartMgr.getShoppingCart(orderId))
                if (item.productId == productId) {
                    s = item;
                    break;
                }

            cartMgr.removeShoppingCartItem(orderId, productId);
            p.stock += s.Quantity;
            productMgr.modifyProduct(p);
        }

        public float calculateTotalPrice(int orderId) {
            IShoppingCartManager cartMgr = new MySqlShoppingCartManager();
            IProductManager productMgr = new MySqlProductManager();
            float price = 0.0f;
            foreach(ShoppingCartItem s in cartMgr.getShoppingCart(orderId)) {
                Product p = (Product) productMgr.getProduct(s.productId);
                price += p.price * s.Quantity;
            }
            return price;
        }
    }
}
