using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement {
    public interface IOrderManager {
        void addNewOrder(Order o);
        void removeOrder(int orderId);
        void modifyOrder(Order o);
        IList<Order> getOrders(int userId);
        Order getOrder(int id);
    }
}
