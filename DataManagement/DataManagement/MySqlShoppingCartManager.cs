using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement {
    public class MySqlShoppingCartManager : IShoppingCartManager {
        private string connString;

        public MySqlShoppingCartManager() {
            connString = 
                System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        }

        public void addNewShoppingCartItem(ShoppingCartItem s, int orderId) {
            MySqlConnection conn = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "insert into ShoppingCart(CustomerOrderId, ProductId, Quantity) values " +
                    "(@CustomerOrderId, @ProductId, @Quantity);";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@CustomerOrderId", orderId);
                cmd.Parameters.AddWithValue("@ProductId", s.productId);
                cmd.Parameters.AddWithValue("Quantity", s.Quantity);
                cmd.ExecuteNonQuery();
            } catch(Exception ex) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
            }
        }

        ShoppingCartItem constructShoppingCartItem(MySqlDataReader reader) {
            IProductManager productMgr = new MySqlProductManager();
            Product p = (Product)productMgr.getProduct(reader.GetInt32("ProductId"));
            return new ShoppingCartItem(p, reader.GetInt32("Quantity"));
        }

        public IList<ShoppingCartItem> getShoppingCart(int orderId) {
            IList<ShoppingCartItem> list = new List<ShoppingCartItem>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from ShoppingCart where CustomerOrderId = @CustomerOrderId;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@CustomerOrderId", orderId);
                reader = cmd.ExecuteReader();
                while (reader.Read()) 
                    list.Add(constructShoppingCartItem(reader));
                return list;
            } catch (Exception ex) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public void modifyShoppingCartItem(ShoppingCartItem s, int orderId) {
            MySqlConnection conn = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "update ShoppingCart set Quantity = @Quantity where " +
                    "CustomerOrderId = @CustomerOrderId and ProductId = @ProductId;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@CustomerOrderId", orderId);
                cmd.Parameters.AddWithValue("@ProductId", s.productId);
                cmd.Parameters.AddWithValue("Quantity", s.Quantity);
                cmd.ExecuteNonQuery();
            } catch (Exception ex) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
            }
        }

        public void removeShoppingCartItem(int orderId, int productId) {
            MySqlConnection conn = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "delete from ShoppingCart where " + 
                    "CustomerOrderId = @CustomerOrderId and ProductId = @ProductId;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@CustomerOrderId", orderId);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.ExecuteNonQuery();
            } catch (Exception ex) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
            }
        }
    }
}
