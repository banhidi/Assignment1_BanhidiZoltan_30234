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

            Order orderWithId = null;
            foreach(Order aux in orderMgr.getOrders(o.userId)) {
                if (aux.customerName.Equals(o.customerName) && aux.deliveryAdress.Equals(o.deliveryAdress) &&
                        aux.deliveryDate.Date.Equals(o.deliveryDate.Date) && aux.orderStatus == o.orderStatus &&
                        aux.userId == o.userId) {
                    orderWithId = aux;
                    break;
                }
            }

            IUserActivityManager activityMgr = new MySqlUserActivityManager();
            UserActivity ua = new UserActivity();
            
            ua.userId = o.userId;
            ua.itemId = (orderWithId == null) ? -1 : orderWithId.id;
            ua.description = "insert new order (" + o.ToString() + ")";
            ua.tableName = TableNameEnum.CustomerOrder;
            ua.activityDateTime = DateTime.Now;
            activityMgr.addNewUserActivity(ua);
        }

        public void deleteOrder(int orderId) {
            IShoppingCartManager cartMgr = new MySqlShoppingCartManager();
            IList<ShoppingCartItem> list = cartMgr.getShoppingCart(orderId);
            foreach (ShoppingCartItem s in list)
                cartMgr.removeShoppingCartItem(orderId, s.productId);

            IOrderManager orderMgr = new MySqlOrderManager();
            Order o = orderMgr.getOrder(orderId);
            orderMgr.removeOrder(orderId);

            IUserActivityManager activityMgr = new MySqlUserActivityManager();
            UserActivity ua = new UserActivity();

            ua.userId = o.userId;
            ua.itemId = o.id;
            ua.description = "delete order (" + o.ToString() + ")";
            ua.tableName = TableNameEnum.CustomerOrder;
            ua.activityDateTime = DateTime.Now;
            activityMgr.addNewUserActivity(ua);
        }

        public void modifyOrder(int orderId, string customerName, string adress, DateTime date, bool orderStatus) {
            IOrderManager mgr = new MySqlOrderManager();
            Order o = new Order(orderId);
            Order lastOrder = mgr.getOrder(orderId);
            o.customerName = customerName;
            o.deliveryAdress = adress;
            o.deliveryDate = date;
            o.orderStatus = orderStatus;
            
            mgr.modifyOrder(o);

            IUserActivityManager activityMgr = new MySqlUserActivityManager();
            UserActivity ua = new UserActivity();

            ua.userId = lastOrder.userId;
            ua.itemId = lastOrder.id;
            ua.description = "modify order from (" + lastOrder.ToString() + ") to (" + o.ToString() + ")";
            ua.tableName = TableNameEnum.CustomerOrder;
            ua.activityDateTime = DateTime.Now;
            activityMgr.addNewUserActivity(ua);
        }

        public void addProductToOrder(int productId, int quantity, int orderId, string username) {
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

            IUserActivityManager activityMgr = new MySqlUserActivityManager();
            UserActivity ua = new UserActivity();
            IUserManager userMgr = new MySqlUserManager();

            ua.userId = ((User)userMgr.getUser(username)).id;
            ua.itemId = -1;
            ua.description = "add to shopping cart (" + s.ToString() + ")";
            ua.tableName = TableNameEnum.ShoppingCart;
            ua.activityDateTime = DateTime.Now;
            activityMgr.addNewUserActivity(ua);
        }

        public void modifyProductInShoppingCart(int productId, int quantity, int orderId, string username) {
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

            int oldQuantity = s.Quantity;
            p.stock += s.Quantity - quantity;
            s.Quantity = quantity;

            cartMgr.modifyShoppingCartItem(s, orderId);
            productMgr.modifyProduct(p);

            IUserActivityManager activityMgr = new MySqlUserActivityManager();
            UserActivity ua = new UserActivity();
            IUserManager userMgr = new MySqlUserManager();

            ua.userId = ((User)userMgr.getUser(username)).id;
            ua.itemId = -1;
            ua.description = "modify shopping cart quantity from " + oldQuantity  + " to " + quantity + " current state is (" + s.ToString() + ")";
            ua.tableName = TableNameEnum.ShoppingCart;
            ua.activityDateTime = DateTime.Now;
            activityMgr.addNewUserActivity(ua);
        }

        public void removeProductFromShoppingCart(int productId, int orderId, string username) {
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

            IUserActivityManager activityMgr = new MySqlUserActivityManager();
            UserActivity ua = new UserActivity();
            IUserManager userMgr = new MySqlUserManager();

            ua.userId = ((User)userMgr.getUser(username)).id;
            ua.itemId = -1;
            ua.description = "remove from shopping cart item " +"(" + s.ToString() + ")";
            ua.tableName = TableNameEnum.ShoppingCart;
            ua.activityDateTime = DateTime.Now;
            activityMgr.addNewUserActivity(ua);
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
