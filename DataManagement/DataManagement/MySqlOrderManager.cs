using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement {
    public class MySqlOrderManager : IOrderManager {
        private string connString;

        public MySqlOrderManager() {
            connString =
                System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        }

        public void addNewOrder(Order o) {
            MySqlConnection conn = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "insert into " +
                    "CustomerOrder(CustomerName, DeliveryAdress, DeliveryDate, OrderStatus, UserId) values " +
                    "(@CustomerName, @DeliveryAdress, @DeliveryDate, @OrderStatus, @UserId);";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@CustomerName", o.customerName);
                cmd.Parameters.AddWithValue("@DeliveryAdress", o.deliveryAdress);
                cmd.Parameters.AddWithValue("@DeliveryDate", o.deliveryDate);
                cmd.Parameters.AddWithValue("@OrderStatus", o.orderStatus);
                cmd.Parameters.AddWithValue("@UserId", o.userId);
                cmd.ExecuteNonQuery();
            } catch(Exception e) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
            }
        }

        private Order constructOrder(MySqlDataReader reader) {
            IShoppingCartManager shoppingCartMgr = new MySqlShoppingCartManager();
            int id = reader.GetInt32("Id");
            Order o = new Order(id, shoppingCartMgr.getShoppingCart(id));
            o.customerName = reader.GetString("CustomerName");
            o.deliveryAdress = reader.GetString("DeliveryAdress");
            o.deliveryDate = reader.GetDateTime("DeliveryDate");
            o.orderStatus = reader.GetBoolean("OrderStatus");
            o.userId = reader.GetInt32("UserId");
            return o;
        }

        public IList<Order> getOrders(int userId) {
            IList<Order> list = new List<Order>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from CustomerOrder where UserId = @UserId;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@UserId", userId);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(constructOrder(reader));
                return list;
            } catch (Exception e) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public Order getOrder(int id) {
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from CustomerOrder where Id = @Id;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Id", id);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                    return constructOrder(reader);
                else
                    return null;
            } catch (Exception e) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public void modifyOrder(Order o) {
            MySqlConnection conn = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "update CustomerOrder " +
                    "set CustomerName=@CustomerName, DeliveryAdress=@DeliveryAdress, " +
                    "DeliveryDate=@DeliveryDate, OrderStatus=@OrderStatus where Id=@Id;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@CustomerName", o.customerName);
                cmd.Parameters.AddWithValue("@DeliveryAdress", o.deliveryAdress);
                cmd.Parameters.AddWithValue("@DeliveryDate", o.deliveryDate);
                cmd.Parameters.AddWithValue("@OrderStatus", o.orderStatus);
                cmd.Parameters.AddWithValue("@Id", o.id);
                cmd.ExecuteNonQuery();
            } catch (Exception e) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
            }
        }

        public void removeOrder(int orderId) {
            MySqlConnection conn = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "delete from CustomerOrder where Id = @Id;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Id", orderId);
                cmd.ExecuteNonQuery();
            } catch (Exception e) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
            }
        }
    }
}
