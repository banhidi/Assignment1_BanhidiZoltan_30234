using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace DataManagement {
    public class MySqlProductManager : IProductManager {
        private string connString;

        public MySqlProductManager() {
            connString = 
                ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        }

        public void addNewProduct(Product pWithoutId) {
            MySqlConnection conn = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "insert into Product(Title, Color, Width, Height, Depth, " + 
                    "Price, Stock) values " + 
                    "(@Title, @Color, @Width, @Height, @Depth, @Price, @Stock);";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Title", pWithoutId.title);
                cmd.Parameters.AddWithValue("@Color", pWithoutId.color);
                cmd.Parameters.AddWithValue("@Width", pWithoutId.width);
                cmd.Parameters.AddWithValue("@Height", pWithoutId.height);
                cmd.Parameters.AddWithValue("@Depth", pWithoutId.depth);
                cmd.Parameters.AddWithValue("@Price", pWithoutId.price);
                cmd.Parameters.AddWithValue("@Stock", pWithoutId.stock);
                cmd.ExecuteNonQuery();
            } catch (Exception ex) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
            }
        }

        public void deleteProduct(int id) {
            MySqlConnection conn = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "delete from Product where Id=@Id;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            } catch (Exception ex) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
            }
        }

        public IList<Product> getAllProducts() {
            IList<Product> products = new List<Product>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from Product;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    products.Add(constructProduct(reader));
                return products;
            } catch (Exception ex) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public IList<Product> getAvailableProducts() {
            IList<Product> products = new List<Product>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from Product where Stock > 0;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    products.Add(constructProduct(reader));
                return products;
            } catch (Exception ex) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public Product? getProduct(string title) {
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from Product where Title = @Title;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Title", title);
                reader = cmd.ExecuteReader();
                if (reader.HasRows) {
                    reader.Read();
                    return constructProduct(reader);
                } else
                    return null;
            } catch (Exception ex) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        private Product constructProduct(MySqlDataReader reader) {
            Product p = new Product();
            p.id = reader.GetInt32("Id");
            p.title = reader.GetString("Title");
            p.color = reader.GetString("Color");
            p.width = reader.GetInt32("Width");
            p.height = reader.GetInt32("Height");
            p.depth = reader.GetInt32("Depth");
            p.price = reader.GetFloat("Price");
            p.stock = reader.GetInt32("Stock");
            return p;
        }

        public Product? getProduct(int id) {
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from Product where Id = @Id;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Id", id);
                reader = cmd.ExecuteReader();
                if (reader.HasRows) {
                    reader.Read();
                    return constructProduct(reader);
                } else
                    return null;
            } catch(Exception ex) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public IList<Product> getProducts(string productTitle) {
            IList<Product> products = new List<Product>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from Product where " + 
                    "locate(lower(@Title), lower(Title)) <> 0;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Title", productTitle);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    products.Add(constructProduct(reader));
                return products;
            } catch (Exception ex) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public void modifyProduct(Product pWithValidId) {
            MySqlConnection conn = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "update Product set Title=@Title, Color=@Color, " + 
                    "Width=@Width, Height=@Height, Depth=@Depth, Price=@Price, Stock=@Stock " +
                    "where Id=@Id;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Title", pWithValidId.title);
                cmd.Parameters.AddWithValue("@Color", pWithValidId.color);
                cmd.Parameters.AddWithValue("@Width", pWithValidId.width);
                cmd.Parameters.AddWithValue("@Height", pWithValidId.height);
                cmd.Parameters.AddWithValue("@Depth", pWithValidId.depth);
                cmd.Parameters.AddWithValue("@Price", pWithValidId.price);
                cmd.Parameters.AddWithValue("@Stock", pWithValidId.stock);
                cmd.Parameters.AddWithValue("@Id", pWithValidId.id);
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
