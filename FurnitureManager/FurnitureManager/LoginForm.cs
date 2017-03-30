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

namespace FurnitureManager {
    public partial class LoginForm : Form {
        public LoginForm() {
            InitializeComponent();
        }

        private void signInButton_Click(object sender, EventArgs e) {
            Controller.UserController uc = new Controller.UserController();
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            if (uc.isValidAdmin(username, password)) {
                AdminForm af = new AdminForm();
                af.Owner = this;
                af.AdminUserName = username;
                af.Show();
                this.usernameTextBox.Clear();
                this.passwordTextBox.Clear();
                this.errorLabel.Visible = false;
                this.Hide();
            } else if (uc.isValidRegularUser(username, password)) {
                RegularUserForm ruf = new RegularUserForm(username);
                ruf.Owner = this;
                ruf.Show();
                this.usernameTextBox.Clear();
                this.passwordTextBox.Clear();
                this.errorLabel.Visible = false;
                this.Hide();
            } else
                errorLabel.Visible = true;            
        }
    }
}
