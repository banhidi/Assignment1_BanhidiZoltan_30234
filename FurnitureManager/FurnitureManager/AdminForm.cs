using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataManagement;
using Controller;

namespace FurnitureManager {
    public partial class AdminForm : Form, IAbstractForm {
        private User selectedUser;
        
        public string AdminUserName {
            set {
                usernameLabel.Text = value;
            }
        }
        public AdminForm() {
            InitializeComponent();            
            reloadUsersTable();
            reloadUserActivities();
        }

        public void setEnabled() {
            this.Enabled = true;
        }

        public void setNewUsername(string username) {
            usernameLabel.Text = username;
        }

        private void addNewUserButton_Click(object sender, EventArgs e) {
            UserForm uf = new FurnitureManager.UserForm(this);
            uf.Operation = UserForm.OperationEnum.Add;
            uf.Show();
            this.Enabled = false;
        }

        public void reloadUsersTable() {
            IUserManager mgr = new MySqlUserManager();           
            userGrid.DataSource = mgr.getAllUsers();
            regularUserGrid.DataSource = mgr.getAllRegularUsers();
        }

        private void modifyUserButton_Click(object sender, EventArgs e) {
            UserForm uf = new UserForm(this);
            uf.Operation = UserForm.OperationEnum.Modify;
            uf.User = selectedUser;
            uf.Show();
            this.Enabled = false;
            
        }

        private void userGrid_SelectionChanged(object sender, EventArgs e) {
            if (userGrid.SelectedRows.Count == 1) {
                selectedUser = (User)userGrid.SelectedRows[0].DataBoundItem;
                modifyUserButton.Enabled = true;
                deleteUserButton.Enabled = true;
            } else {
                modifyUserButton.Enabled = false;
                deleteUserButton.Enabled = false;
            }
        }

        private void showErrorMessage(string text) {
            MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void deleteUserButton_Click(object sender, EventArgs e) {
            UserController uc = new UserController();
            try {
                uc.deleteUser(selectedUser.id);
                reloadUsersTable();
            } catch(DataManagement.Exceptions.MySqlConnectionException ex) {
                showErrorMessage("Error when connecting to database.");
            }
        }

        private void signOutButton_Click(object sender, EventArgs e) {
            this.Owner.Show();
            this.Close();
        }

        private void modifyAccountButton_Click(object sender, EventArgs e) {
            IUserManager mgr = new MySqlUserManager();
            AccountForm af = new AccountForm(this);
            af.User = (User)mgr.getUser(usernameLabel.Text);
            af.Show();
            this.Enabled = false;
        }

        private void reloadUserActivities() {
            IUserActivityManager mgr = new MySqlUserActivityManager();
            activityGrid.DataSource = mgr.getAllUserActivities();
        }

        private void reloadUserActivities(int id) {
            IUserActivityManager mgr = new MySqlUserActivityManager();
            activityGrid.DataSource = mgr.getUserActivities(id);
        }

        private void refreshActivitiesButton_Click(object sender, EventArgs e) {
            reloadUserActivities();
            regularUserGrid.ClearSelection();
        }

        private void userAGrid_SelectionChanged(object sender, EventArgs e) {
            if (regularUserGrid.SelectedRows.Count == 1) {
                User u = (User) regularUserGrid.SelectedRows[0].DataBoundItem;
                reloadUserActivities(u.id);
            }
        }

        private void searchActivitiesButton_Click(object sender, EventArgs e) {
            ActivitiesController c = new ActivitiesController();
            if (regularUserGrid.SelectedRows.Count != 1) {
                regularUserGrid.ClearSelection();
                activityGrid.DataSource = c.search(fromDateTimePicker.Value, toDateTimePicker.Value);
            } else {
                int userId = ((User)regularUserGrid.SelectedRows[0].DataBoundItem).id;
                activityGrid.DataSource = c.search(userId, fromDateTimePicker.Value, toDateTimePicker.Value);
            }
        }

        private void deleteActivitiesButton_Click(object sender, EventArgs e) {
            ActivitiesController ac = new ActivitiesController();
            ac.deleteActivities((IList<UserActivity>)activityGrid.DataSource);
            if (regularUserGrid.SelectedRows.Count == 1) {
                int userId = ((User)regularUserGrid.SelectedRows[0].DataBoundItem).id;
                reloadUserActivities(userId);
            } else
                reloadUserActivities();
        }
    }
}
