using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement {
    public interface IUserActivityManager {
        void addNewUserActivity(UserActivity u);
        IList<UserActivity> getAllUserActivities();
        void deleteUserActivity(int id);
        IList<UserActivity> getUserActivityInterval(DateTime after, DateTime before);
        IList<UserActivity> getUserActivityInterval(int userId, DateTime after, DateTime before);
        IList<UserActivity> getUserActivities(int userId);
    }
}
