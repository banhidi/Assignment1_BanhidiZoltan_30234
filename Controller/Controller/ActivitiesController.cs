using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataManagement;

namespace Controller {
    public class ActivitiesController {

        private void extendUserActivities(IList<UserActivity> list) {
            IUserManager userMgr = new MySqlUserManager();
            IProductManager productMgr = new MySqlProductManager();

            foreach (UserActivity userActivity in list) {
                UserActivity currActivity = userActivity;

                User? u = userMgr.getUser(currActivity.userId);
                currActivity.username = (u == null) ? " - " : ((User)u).username;

                switch (currActivity.tableName) {
                    case TableNameEnum.Product:
                        Product? p = productMgr.getProduct(currActivity.itemId);
                        currActivity.itemTitle = (p == null) ? " - " : ((Product)p).title;
                        break;
                    case TableNameEnum.CustomerOrder:
                        break;
                    case TableNameEnum.ShoppingCart:
                        break;
                }
            }
        }

        public IList<UserActivity> search(DateTime fromDateTime, DateTime toDateTime) {
            IUserActivityManager mgr = new MySqlUserActivityManager();
            DateTime from = DateTime.Parse(fromDateTime.ToString("yyyy-MM-dd") + " 00:00");
            DateTime to = DateTime.Parse(toDateTime.ToString("yyyy-MM-dd") + " 00:00");
            IList<UserActivity> list = mgr.getUserActivityInterval(from, to);
            extendUserActivities(list);
            return list;
        }

        public IList<UserActivity> search(int userId, DateTime fromDateTime, DateTime toDateTime) {
            IUserActivityManager mgr = new MySqlUserActivityManager();
            DateTime from = DateTime.Parse(fromDateTime.ToString("yyyy-MM-dd") + " 00:00:00");
            DateTime to = DateTime.Parse(toDateTime.ToString("yyyy-MM-dd") + " 23:59:59");
            IList<UserActivity> list =  mgr.getUserActivityInterval(userId, from, to);
            extendUserActivities(list);
            return list;
        }

        public void deleteActivities(IList<UserActivity> list) {
            IUserActivityManager mgr = new MySqlUserActivityManager();
            foreach (UserActivity ua in list) {
                mgr.deleteUserActivity(ua.id);
            }
        }

    }
}
