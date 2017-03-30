using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace DataManagement {
    public class MySqlUserActivityManager : IUserActivityManager {
        private string connString;

        public MySqlUserActivityManager() {
            connString = 
                ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        }

        public void addNewUserActivity(UserActivity u) {
            MySqlConnection conn = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query =
                    "insert into UserActivity(UserId, ItemId, Description, TableName, " +
                    "ActivityDateTime) values (@UserId, @ItemId, @Description, @TableName, " +
                    "@ActivityDateTime);";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@UserId", u.userId);
                cmd.Parameters.AddWithValue("@ItemId", u.itemId);
                cmd.Parameters.AddWithValue("@Description", u.description);
                cmd.Parameters.AddWithValue("@TableName", UserActivity.stringTableName(u.tableName));
                cmd.Parameters.AddWithValue("@ActivityDateTime", u.activityDateTime.ToString("yyyy-MM-dd hh:mm"));
                Console.WriteLine(cmd.ToString());
                cmd.ExecuteNonQuery();
            } catch(Exception ex) {
                throw new Exceptions.MySqlConnectionException();
            } finally {
                if (conn != null)
                    conn.Close();
            }
        }

        public void deleteUserActivity(int id) {
            MySqlConnection conn = null;
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "delete from UserActivity where Id = @Id;";
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

        private UserActivity constructUserActivity(MySqlDataReader reader) {
            UserActivity u = new UserActivity();
            u.id = reader.GetInt32("Id");
            u.userId = reader.GetInt32("UserId");
            u.itemId = reader.GetInt32("ItemId");
            u.description = reader.GetString("Description");
            switch (reader.GetString("TableName")) {
                case "Product":
                    u.tableName = TableNameEnum.Product;
                    IProductManager productMgr = new MySqlProductManager();
                    Product? p = productMgr.getProduct(u.itemId);
                    if (p != null)
                        u.itemTitle = ((Product)p).title;
                    else
                        u.itemTitle = " - ";
                    break;
                case "CustomerOrder":
                    u.tableName = TableNameEnum.CustomerOrder;
                    throw new NotImplementedException();
                    break;
                case "ShoppingCart":
                    u.tableName = TableNameEnum.ShoppingCart;
                    throw new NotImplementedException();
                    break;
            }
            u.activityDateTime = reader.GetDateTime("ActivityDateTime");
            return u;
        }

        public IList<UserActivity> getAllUserActivities() {
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            IList<UserActivity> list = new List<UserActivity>();
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from UserActivity;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(constructUserActivity(reader));
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

        public IList<UserActivity> getUserActivityInterval(DateTime after, DateTime before) {
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            IList<UserActivity> list = new List<UserActivity>();
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from UserActivity where ActivityDateTime >= @After and "+
                    "ActivityDateTime <= @Before;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@After", after);
                cmd.Parameters.AddWithValue("@Before", before);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(constructUserActivity(reader));
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

        public IList<UserActivity> getUserActivityInterval(int userId, DateTime after, DateTime before) {
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            IList<UserActivity> list = new List<UserActivity>();
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from UserActivity where ActivityDateTime >= @After and " +
                    "ActivityDateTime <= @Before and UserId = @UserId;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@After", after);
                cmd.Parameters.AddWithValue("@Before", before);
                cmd.Parameters.AddWithValue("@UserId", userId);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(constructUserActivity(reader));
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

        public IList<UserActivity> getUserActivities(int userId) {
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            IList<UserActivity> list = new List<UserActivity>();
            try {
                conn = new MySqlConnection(connString);
                conn.Open();
                string query = "select * from UserActivity where UserId = @UserId;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@UserId", userId);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(constructUserActivity(reader));
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

    }
}
