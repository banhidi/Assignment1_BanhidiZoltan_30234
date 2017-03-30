using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement {
    public class MySqlUserManager : IUserManager {
        private string connString;

        public MySqlUserManager() {
            connString =
                System.Configuration.ConfigurationManager.
                ConnectionStrings["MySqlConnectionString"].ConnectionString;
        }

        public Nullable<User> getUser(int id) {
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from User where Id = @Id;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Id", id);
                reader = cmd.ExecuteReader();
                if (reader.HasRows) {
                    reader.Read();
                    User u = new DataManagement.User();
                    u.id = reader.GetInt32("Id");
                    u.username = reader.GetString("Username");
                    u.hashPassword = reader.GetString("Password");
                    u.isAdmin = reader.GetBoolean("Admin");
                    return u;
                } else
                    return null;
            } catch(Exception e) {
                throw new Exceptions.MySqlConnectionException(
                    "Error when connection to the database.");
            } finally {
                if (conn != null) 
                    conn.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public User? getUser(string username) {
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from User where Username = @Username;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Username", username);
                reader = cmd.ExecuteReader();
                if (reader.HasRows) {
                    reader.Read();
                    User u = new User();
                    u.id = reader.GetInt32("Id");
                    u.username = reader.GetString("Username");
                    u.hashPassword = reader.GetString("Password");
                    u.isAdmin = reader.GetBoolean("Admin");
                    return u;
                } else
                    return null;
            } catch(Exception e) {
                throw new Exceptions.MySqlConnectionException("Error when connectiong to database.");
            } finally {
                if (conn != null) 
                    conn.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public void deleteUser(int userId) {
            MySqlConnection conn = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "delete from User where Id = @Id;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Id", userId);
                cmd.ExecuteNonQuery();
            } catch(Exception e) {
                throw new 
                    Exceptions.MySqlConnectionException("Error when connecting to database.");
            } finally {
                if (conn != null)
                    conn.Close();
            }
        }

        public void modifyUser(User userWithValidId) {
            MySqlConnection conn = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "update User set Username=@Username, Password=@Password, " +
                    "Admin=@Admin where Id = @Id;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Id", userWithValidId.id);
                cmd.Parameters.AddWithValue("@Username", userWithValidId.username);
                cmd.Parameters.AddWithValue("@Password", userWithValidId.hashPassword);
                cmd.Parameters.AddWithValue("@Admin", userWithValidId.isAdmin);
                cmd.ExecuteNonQuery();
            } catch(Exception e) {
                System.Console.WriteLine(e.ToString());
                throw new Exceptions.MySqlConnectionException("Error when connecting to the database.");
                
            } finally {
                if (conn != null)
                    conn.Close();
            }
        }

        public void addNewUser(User userWithoutId) {
            MySqlConnection conn = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "insert into User(Username, Password, Admin) values " +
                    "(@Username, @Password, @Admin);";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Username", userWithoutId.username);
                cmd.Parameters.AddWithValue("@Password", userWithoutId.hashPassword);
                cmd.Parameters.AddWithValue("@Admin", userWithoutId.isAdmin);
                cmd.ExecuteNonQuery();
            } catch(Exception e) {
                throw new Exceptions.MySqlConnectionException("Error when connecting to database.");
            } finally {
                if (conn != null)
                    conn.Close();
            }
        }

        public IList<User> getAllUsers() {
            IList<User> list = new List<User>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from User;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    User u = new User();
                    u.id = reader.GetInt32("Id");
                    u.username = reader.GetString("Username");
                    u.hashPassword = reader.GetString("Password");
                    u.isAdmin = reader.GetBoolean("Admin");
                    list.Add(u);
                }
                return list;
            } catch(Exception e) {
                throw new Exceptions.MySqlConnectionException(
                    "Error when connecting to database.");
            } finally {
                if (conn != null)
                    conn.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public IList<User> getAllRegularUsers() {
            IList<User> list = new List<User>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from User where not(Admin);";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    User u = new User();
                    u.id = reader.GetInt32("Id");
                    u.username = reader.GetString("Username");
                    u.hashPassword = reader.GetString("Password");
                    u.isAdmin = reader.GetBoolean("Admin");
                    list.Add(u);
                }
                return list;
            } catch (Exception e) {
                throw new Exceptions.MySqlConnectionException(
                    "Error when connecting to database.");
            } finally {
                if (conn != null)
                    conn.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public void deleteAllUsers() {
            MySqlConnection conn = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "delete from User;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            } catch(Exception e) {
                throw new Exceptions.MySqlConnectionException("Error when connectiong to database.");
            } finally {
                if (conn != null)
                    conn.Close();
            }
        }
    }
}
